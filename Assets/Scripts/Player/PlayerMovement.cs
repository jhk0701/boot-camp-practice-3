using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    PlayerInputController inputController => Player.Instance.inputController;
    Rigidbody rb => Player.Instance.rigidBody;

    Vector2 movement;

    [SerializeField] float speed = 10f;

    void Start()
    {
        inputController.OnMoveEvent += OnMove;
    }
    
    void FixedUpdate()
    {
        // rb.velocity = 
        Vector3 move = transform.forward * movement.y + transform.right * movement.x;
        move *= speed;
        move.y += rb.velocity.y;

        rb.velocity = move;
    }

    void OnMove(Vector2 dir)
    {
        movement = dir;
    }

}
