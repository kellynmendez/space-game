using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HazardVolume : MonoBehaviour
{
    [SerializeField] string loseText = "You lost!";

    [Header("Feedback")]
    [SerializeField] AudioClip _mushroomSFX = null;
    [SerializeField] ParticleSystem _explodeParticle = null;

    [Header("Setup")]
    [SerializeField] GameObject _visualsToDeactivate = null;

    AudioSource _audioSource = null;
    Collider _colliderToDeactivate = null;
    UIController _uiController = null;

    private void Awake()
    {
        // Searching objects in scene for script of type UIController
        _uiController = FindObjectOfType<UIController>();
        // getting audio source
        _audioSource = GetComponent<AudioSource>();
        // get collider
        _colliderToDeactivate = GetComponent<Collider>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            PlayerShip playerShip = other.gameObject.GetComponent<PlayerShip>();
            bool _mushroomMode = playerShip.InMushroomMode();
            if (_mushroomMode)
            {
                // Disabling debris
                DisableObject();
            }
            // If we found something valid, continue
            else if (playerShip != null)
            {
                _uiController.ShowText(loseText);
                playerShip.Kill();
            }
        }
    }

    public void DisableObject()
    {
        // disable collider so it can't be retriggered
        _colliderToDeactivate.enabled = false;
        // disable visuals to simulate deactivated
        _visualsToDeactivate.SetActive(false);
        // deactivate particle flash/audio
        PlayFX();
    }

    private void PlayFX()
    {
        // play gfx
        if (_explodeParticle != null)
        {
            _explodeParticle.Play();
        }
        // play sfx
        if (_audioSource != null && _mushroomSFX != null)
        {
            _audioSource.PlayOneShot(_mushroomSFX, _audioSource.volume);
        }
    }
}
