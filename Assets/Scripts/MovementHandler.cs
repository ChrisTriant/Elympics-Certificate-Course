using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementHandler : MonoBehaviour
{
    [SerializeField] private Rigidbody _rigidbody;
    [SerializeField] private float _speed;

    public void HandleMovement(Vector2 movementInput, Vector3 mouseWorldPosition)
    {
        movementInput.Normalize();
        movementInput *= _speed;

        _rigidbody.velocity = new Vector3(movementInput.x, 0, movementInput.y);

        mouseWorldPosition.y = _rigidbody.position.y;

        _rigidbody.MoveRotation(Quaternion.LookRotation(mouseWorldPosition -  _rigidbody.position));
    }
}
