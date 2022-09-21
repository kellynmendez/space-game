using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collectible : MonoBehaviour
{
    [SerializeField] Collider _triggerToDisable = null;
    [SerializeField] GameObject _artToDisable = null;

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("Collectible has been entered!");
        PlayerShip _playerShip = other.attachedRigidbody.GetComponent<PlayerShip>();
        if (_playerShip != null)
        {
            _playerShip.AddCollectible();
        }

        // disable relevant components for "Collection"
        Debug.Log("Disabling...");
        _triggerToDisable.enabled = false;
        _artToDisable.SetActive(false);
    }
}
