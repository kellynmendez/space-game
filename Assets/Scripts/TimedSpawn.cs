using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimedSpawn : MonoBehaviour
{
    [SerializeField] GameObject _objectToSpawn;
    [SerializeField] float _spawnTime;
    [SerializeField] float _spawnDelay;

    private bool _stopSpawning = false;

    void Start()
    {
        InvokeRepeating("SpawnObject", _spawnTime, _spawnDelay);
    }

    private void SpawnObject()
    {
        Instantiate(_objectToSpawn, transform.position, transform.rotation);
        if (_stopSpawning)
        {
            CancelInvoke("SpawnObject");
        }
    }
}
