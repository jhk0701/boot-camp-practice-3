using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    PlayerInputController inputController => Player.Instance.inputController;
    Rigidbody rb => Player.Instance.rigidBody;

    Vector3 movement;


    
}
