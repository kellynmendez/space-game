using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WinVolume : MonoBehaviour
{
    [SerializeField] string winText = "You win!";
    UIController _uiController = null;

    private void Awake()
    {
        // Searching objects in scene for script of type UIController
        _uiController = FindObjectOfType<UIController>();
    }

    private void OnTriggerEnter(Collider other)
    {
        // Detect if it's the player
        PlayerShip playerShip = other.gameObject.GetComponent<PlayerShip>();
        // If we found something valid, continue
        if (playerShip != null)
        {
            _uiController.ShowText(winText);
            playerShip.Win();
        }
    }
}
