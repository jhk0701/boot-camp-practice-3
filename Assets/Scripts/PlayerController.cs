using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    private Rigidbody rb;

    [Header("Movement")]
    [SerializeField] private float moveSpeed;
    [SerializeField] private float jumpPower = 80f;
    private Vector2 curMovementInput;
    public LayerMask groundLayerMask;

    [Header("Look")]
    [SerializeField] private Transform cameraContainer;
    public float minXLook;
    public float maxXLook;
    private float camCurXRot;
    private float camCurYRot;
    public float lookSensitivity;
    Vector2 mouseDelta;
    public bool canLook = true;

    public Action Inventory;


    void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    void Start()
    {
        // 게임에서는 커서 안 보임
        Cursor.lockState = CursorLockMode.Locked;
    }

    void CameraLook()
    {
        camCurXRot += mouseDelta.y * lookSensitivity;
        camCurXRot = Mathf.Clamp(camCurXRot, minXLook, maxXLook);
        cameraContainer.localEulerAngles = new Vector3(-camCurXRot, 0, 0);

        transform.eulerAngles += new Vector3(0, mouseDelta.x * lookSensitivity, 0);
    }

    // 물리 연산을 하는 함수는 되도록 FixedUpdate에서
    void FixedUpdate()
    {
        Move();
    }

    void LateUpdate()
    {
        if (canLook)
            CameraLook();    
    }

    public void OnMove(InputAction.CallbackContext value)
    {
        // if (value.phase == InputActionPhase.Started) // 키를 누를 떼, 한 번만
        if (value.phase == InputActionPhase.Performed) // 키가 눌리는 동안
        {
            curMovementInput = value.ReadValue<Vector2>();
        }
        else if (value.phase == InputActionPhase.Canceled) // 키를 뗄 때, 한 번만
        {
            curMovementInput = Vector2.zero;
        }
    }    
    
    void Move()
    {
        Vector3 dir = transform.forward * curMovementInput.y + transform.right * curMovementInput.x;
        dir *= moveSpeed;
        dir.y = rb.velocity.y;

        rb.velocity = dir;
    }
    
    public void OnLook(InputAction.CallbackContext value)
    {
        mouseDelta = value.ReadValue<Vector2>();
    }

    public void OnJump(InputAction.CallbackContext value)
    {

        if (value.phase == InputActionPhase.Started && IsGrounded()) // 키를 누를 떼, 한 번만
        {
            rb.AddForce(Vector3.up * jumpPower, ForceMode.Impulse);
        }
    }

    bool IsGrounded()
    {
        Ray[] rays = new Ray[4]
        {
            new Ray(transform.position + transform.forward * 0.2f + transform.up * 0.01f, Vector3.down),
            new Ray(transform.position + -transform.forward * 0.2f + transform.up * 0.01f, Vector3.down),
            new Ray(transform.position + transform.right * 0.2f + transform.up * 0.01f, Vector3.down),
            new Ray(transform.position + -transform.right * 0.2f + transform.up * 0.01f, Vector3.down),
        };

        for (int i = 0; i < rays.Length; i++)
        {
            if(Physics.Raycast(rays[i], 0.1f, groundLayerMask))
            {
                return true;
            }
        }
        
        return false;
    }   
    
    public void OnInventory(InputAction.CallbackContext context)
    {
        if(context.phase == InputActionPhase.Started)
        {
            Inventory?.Invoke();
            ToggleCursor();
        }
    }

    void ToggleCursor()
    {
        bool toggle = Cursor.lockState == CursorLockMode.Locked;
        Cursor.lockState = toggle ? CursorLockMode.None : CursorLockMode.Locked;
        canLook = !toggle;
    }
}
