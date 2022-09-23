using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class PlayerShip : MonoBehaviour
{
    [Header("Speed variables")]
    [SerializeField] float _moveSpeed = 14f;
    [SerializeField] float _turnSpeed = 3f;

    [Header("Damage spin")]
    [SerializeField] float _rotationSpeed = 3f;

    [Header("Setup")]
    [SerializeField] GameObject _visualsToDeactivate = null;

    [Header("Feedback")]
    [SerializeField] TrailRenderer _trail = null;
    [SerializeField] AudioClip _deathSFX = null;
    [SerializeField] ParticleSystem _deathParticles = null;
    [SerializeField] AudioClip _winSFX = null;

    bool _playerIsDead = false;
    bool _damaged = false;
    bool _mushroomMode = false;
    AudioSource _audioSource = null;
    Rigidbody _rb = null;
    UIController _uiController = null;
    TrailRenderer _trailRend;
    int _collectibleCount = 0;
    int _collectScoreIncr = 100;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody>();
        _trail.enabled = false;
        _trailRend = _trail.GetComponent<TrailRenderer>();
        _uiController = FindObjectOfType<UIController>();
        _audioSource = GetComponent<AudioSource>();
    }

    private void FixedUpdate()
    {
        if (_playerIsDead)
            return;

        MoveShip();
        TurnShip();
    }

    private void Update()
    {
        if (_damaged)
        {
            Debug.Log("spinning!");
            SpinDamage();
        }
    }

    // Uses forces to build momentum forward/backward
    void MoveShip()
    {
        // S/Down = -1, W/Up = 1, None = 0. Scale by moveSpeed
        float moveAmountThisFrame = Input.GetAxisRaw("Vertical") * _moveSpeed;
        // Combine direction with calculated amount
        Vector3 moveDirection = transform.forward * moveAmountThisFrame;
        // Apply the movement to the physics object
        _rb.AddForce(moveDirection);
    }

    // Don't use forces for this; we want rotations to be precise
    void TurnShip()
    {
        // A/Left = -1, D/Right = 1, None = 0. Scale by turnSpeed
        float turnAmountThisFrame = Input.GetAxisRaw("Horizontal") * _turnSpeed;
        // Specify an axis to apply our turn amount (x,y,z) as a rotation
        Quaternion turnOffset = Quaternion.Euler(0, turnAmountThisFrame, 0);
        // Spin the rigidbody
        _rb.MoveRotation(_rb.rotation * turnOffset);
    }

    void SpinDamage()
    {
        transform.Rotate(0f, _rotationSpeed, 0f, Space.Self);
    }

    public void AddCollectible()
    {
        if (_uiController == null)
        {
            Debug.LogError("No UIController prefab in the scene. " +
                "UIController is needed to display collectible count!");
            return;
        }

        // update the count
        _collectibleCount = _collectibleCount + _collectScoreIncr;
        _uiController.UpdateCollectibleCount(_collectibleCount);
    }

    public void Kill()
    {
        Debug.Log("Player has been killed!");
        DisableDeathObjects();
        PlayDeathFX();
        _playerIsDead = true;
    }

    public void Win()
    {
        Debug.Log("Player has won the game!");
        _playerIsDead = true;
        DisableDeathObjects();
        PlayWinFX();
    }

    public bool InMushroomMode()
    {
        return _mushroomMode;
    }

    public void SetMushroomMode(bool mushroomy)
    {
        _mushroomMode = mushroomy;
    }


    public void SetSpeed(float speedChange)
    {
        _moveSpeed += speedChange;
    }

    public void SetScale(float scaleChange)
    {
        transform.localScale *= scaleChange;
        _trailRend.time *= scaleChange;
    }

    public void SetBoosters(bool activeState)
    {
        _trail.enabled = activeState;
    }

    public void ShipDamaged()
    {
        _damaged = true;
    }

    public void ShipDamageDone()
    {
        _damaged = false;
        transform.Rotate(0, 0, 0);
    }

    private void PlayDeathFX()
    {
        Debug.Log("playing death fx");
        if (_deathParticles != null)
        {
            _deathParticles.Play();
        }
        if (_audioSource != null && _deathSFX != null)
        {
            _audioSource.volume = 150;
            _audioSource.PlayOneShot(_deathSFX, _audioSource.volume);
        }
    }
    private void PlayWinFX()
    {
        if (_audioSource != null && _winSFX != null)
        {
            _audioSource.PlayOneShot(_winSFX, _audioSource.volume);
        }
    }

    private void DisableDeathObjects()
    {
        _visualsToDeactivate.SetActive(false);
        _rb.detectCollisions = false;
        _rb.velocity = Vector3.zero;
        _rb.constraints = RigidbodyConstraints.FreezeAll;
    }
}
