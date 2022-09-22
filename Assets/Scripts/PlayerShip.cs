using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class PlayerShip : MonoBehaviour
{
    [Header("Speed variables")]
    [SerializeField] float _moveSpeed = 14f;
    [SerializeField] float _turnSpeed = 3f;

    [Header("Setup")]
    [SerializeField] GameObject _visualsToDeactivate = null;

    [Header("Feedback")]
    [SerializeField] TrailRenderer _trail = null;
    [SerializeField] AudioClip _deathSFX = null;
    [SerializeField] ParticleSystem _deathParticles = null;
    [SerializeField] AudioClip _winSFX = null;

    bool _playerIsDead = false;
    AudioSource _audioSource = null;
    Rigidbody _rb = null;
    UIController _uiController = null;
    int _collectibleCount = 0;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody>();
        _trail.enabled = false;
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

    public void AddCollectible()
    {
        if (_uiController == null)
        {
            Debug.LogError("No UIController prefab in the scene. " +
                "UIController is needed to display collectible count!");
            return;
        }

        // update the count
        _collectibleCount = _collectibleCount + 1;
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

    public void SetSpeed(float speedChange)
    {
        _moveSpeed += speedChange;
    }

    public void SetBoosters(bool activeState)
    {
        _trail.enabled = activeState;
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
