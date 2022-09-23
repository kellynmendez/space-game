using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerupDestroy : MonoBehaviour
{
    [Header("Powerup Settings")]
    [SerializeField] float _sizeIncreaseAmount = 2;
    [SerializeField] float _powerupDuration = 5;

    [Header("Setup ")]
    [SerializeField] GameObject _visualsToDeactivate = null;

    [Header("Feedback")]
    [SerializeField] AudioClip _puSpeedSFX = null;
    [SerializeField] ParticleSystem _puSpeedParticle = null;

    AudioSource _audioSource = null;
    Collider _colliderToDeactivate = null;
    bool _poweredUp = false;

    private void Awake()
    {
        _audioSource = GetComponent<AudioSource>();
        _colliderToDeactivate = GetComponent<Collider>();
        EnableObject();
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("collected mushroom powerup");
        PlayerShip playerShip = other.gameObject.GetComponent<PlayerShip>();
        // if we have a valid player and not already powered up
        if (playerShip != null && _poweredUp == false)
        {
            // start powerup timer; restart if it's already started
            StartCoroutine(PowerupSequence(playerShip));
        }
    }

    IEnumerator PowerupSequence(PlayerShip playerShip)
    {
        // set boolean for detecting lockout
        _poweredUp = true;

        ActivatePowerup(playerShip);
        // simulate this object being disabled. We don't REALLY want
        // to disable it, because we still need script behavior to
        // continue functioning
        DisableObject();

        // wait for the required duration
        yield return new WaitForSeconds(_powerupDuration);
        // reset
        Debug.Log("resetting size!");
        DeactivatePowerup(playerShip);
        EnableObject();

        // set boolean to release lockout
        _poweredUp = false;
    }

    void ActivatePowerup(PlayerShip playerShip)
    {
        if (playerShip != null)
        {
            // powerup player
            playerShip.SetScale(_sizeIncreaseAmount);
            // visuals
            playerShip.SetBoosters(true);
        }
    }

    void DeactivatePowerup(PlayerShip playerShip)
    {
        // revert player powerup - will subtract
        playerShip?.SetScale(_sizeIncreaseAmount * (1 / _sizeIncreaseAmount));
        // visuals
        playerShip?.SetBoosters(false);
    }

    public void DisableObject()
    {
        // disable collider so it can't be retriggered
        _colliderToDeactivate.enabled = false;
        // disable visuals to simulate deactivated
        _visualsToDeactivate.SetActive(false);
        // TODO deactivate particle flash/audio
        PlayFX();
    }

    public void EnableObject()
    {
        // enable collider so it can be retriggered
        _colliderToDeactivate.enabled = true;
        // enable visuals again to draw player attention
        _visualsToDeactivate.SetActive(true);
        // TODO reactivate particle flash/audio

    }


    private void PlayFX()
    {
        // play gfx
        if (_puSpeedParticle != null)
        {
            _puSpeedParticle.Play();
        }
        // play sfx
        if (_audioSource != null && _puSpeedSFX != null)
        {
            _audioSource.PlayOneShot(_puSpeedSFX, _audioSource.volume);
        }
    }

}
