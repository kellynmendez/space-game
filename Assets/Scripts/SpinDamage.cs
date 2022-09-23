using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpinDamage : MonoBehaviour
{
    [SerializeField] float _damageDuration = 1;
    [SerializeField] float _movementSpeed = 20f;
    [SerializeField] float _targetOffsetX = -210f;
    [SerializeField] float _targetOffsetZ = -200f;

    [Header("Setup ")]
    [SerializeField] GameObject _visualsToDeactivate = null;

    [Header("Feedback")]
    [SerializeField] AudioClip _damageSFX = null;
    [SerializeField] AudioClip _mushroomSFX = null;
    [SerializeField] ParticleSystem _explodeParticle = null;

    AudioSource _audioSource = null;
    Collider _colliderToDeactivate = null;
    private Vector3 _targetPosition;
    bool _damaged = false;
    int _mushroomScoreIncr = 100;
    int _damageDebrisScoreDecr = -20;

    private void Awake()
    {
        _audioSource = GetComponent<AudioSource>();
        _colliderToDeactivate = GetComponent<Collider>();
        _targetPosition = new Vector3(
            transform.position.x + _targetOffsetX,
            0,
            transform.position.z + _targetOffsetZ);
    }

    private void Update()
    {
        // move position a step closer to the target.
        var step = _movementSpeed * Time.deltaTime;
        transform.position = Vector3.MoveTowards(transform.position, _targetPosition, step);

        // check if the position of the debris and the target point are approximately equal.
        if (Vector3.Distance(transform.position, _targetPosition) < 0.001f)
        {
            Destroy(this.gameObject);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("ship is damaged");
        if (other.gameObject.tag == "Player")
        {
            PlayerShip playerShip = other.gameObject.GetComponent<PlayerShip>();
            bool _mushroomMode = playerShip.InMushroomMode();
            if (_mushroomMode)
            {
                // Disabling debris
                DisableObject(_mushroomSFX);
                // adding to score
                playerShip.UpdateScore(_mushroomScoreIncr);
            }
            // if we have a valid player and not currently damaged
            else if (playerShip != null && _damaged == false)
            {
                // start powerup timer; restart if it's already started
                StartCoroutine(DamageSpinSequence(playerShip));
                // subtracting from score
                playerShip.UpdateScore(_damageDebrisScoreDecr);
            }
        }
            
    }

    IEnumerator DamageSpinSequence(PlayerShip playerShip)
    {
        // set boolean for detecting lockout
        _damaged = true;

        ActivateDamageSpin(playerShip);
        // Disabling object
        DisableObject(_damageSFX);

        // wait for the required duration
        yield return new WaitForSeconds(_damageDuration);
        // reset
        DeactivateDamageSpin(playerShip);

        // set boolean to release lockout
        _damaged = false;
    }

    void ActivateDamageSpin(PlayerShip playerShip)
    {
        // start spin damage sequence
        playerShip?.ShipDamaged();
        // visuals
        playerShip?.SetBoosters(true);
    }

    void DeactivateDamageSpin(PlayerShip playerShip)
    {
        // end spin damage sequence
        playerShip?.ShipDamageDone();
        // visuals
        playerShip?.SetBoosters(false);
    }

    public void DisableObject(AudioClip _sfx)
    {
        // disable collider so it can't be retriggered
        _colliderToDeactivate.enabled = false;
        // disable visuals to simulate deactivated
        _visualsToDeactivate.SetActive(false);
        // deactivate particle flash/audio
        PlayFX(_sfx);
    }


    private void PlayFX(AudioClip _sfx)
    {
        // play gfx
        if (_explodeParticle != null)
        {
            _explodeParticle.Play();
        }
        // play sfx
        if (_audioSource != null && _sfx != null)
        {
            _audioSource.PlayOneShot(_sfx, _audioSource.volume);
        }
    }

}