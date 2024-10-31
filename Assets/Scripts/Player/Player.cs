
using UnityEngine;

public class Player : MonoBehaviour
{
    private static Player instance;
    public static Player Instance
    { 
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<Player>();

                if (instance == null)
                {
                    instance = new GameObject("Player").AddComponent<Player>();
                }
            }

            return instance;
        }
    }

    [HideInInspector] public PlayerInputController inputController;
    [HideInInspector] public PlayerStatus status;
    [HideInInspector] public Rigidbody rigidBody;

    void Awake()
    {
        inputController = GetComponent<PlayerInputController>();
        status = GetComponent<PlayerStatus>();

        rigidBody = GetComponent<Rigidbody>();
    }

}