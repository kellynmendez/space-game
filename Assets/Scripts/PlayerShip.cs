using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class PlayerShip : MonoBehaviour
{
    [SerializeField] float _moveSpeed = 14f;
    [SerializeField] float _turnSpeed = 3f;

    [Header("Feedback")]
    [SerializeField] TrailRenderer _trail = null;

    Rigidbody _rb = null;
    UIController _uiController = null;
    int _collectibleCount = 0;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody>();
        _trail.enabled = false;
        _uiController = FindObjectOfType<UIController>();
    }

    private void FixedUpdate()
    {
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
        this.gameObject.SetActive(false);
    }

    public void Win()
    {
        Debug.Log("Player has won the game!");
        this.gameObject.SetActive(false);
    }

    public void SetSpeed(float speedChange)
    {
        _moveSpeed += speedChange;
        // TODO audio/visuals

    }

    public void SetBoosters(bool activeState)
    {
        _trail.enabled = activeState;
    }
}
