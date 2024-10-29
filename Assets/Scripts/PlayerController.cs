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

    // 분석 문제 4 : Move를 FixedUpdate에서 호출하는 이유
    /*
        Move 메서드의 구현 방식을 보면 rigidbody에 접근하여 속도값을 직접 조절하고 있다.
        이는 전형적인 물리 연산을 통한 캐릭터 움직임 구현 방법이다.

        FixedUpdate는 update보다 빨리 호출되며, update와 다르게 일정한 간격으로 호출된다.
        또한 공식문서에 따르면 Unity 엔진은 물리 연산이 FixedUpdate 호출 바로 뒤에 진행된다고 한다.
        물리 연산 관련 작업을 FixedUpdate에서 수행하면 바로 직후에 Unity 엔진단에서 물리 연산을 처리할 수 있는 것이다.

        이 부분이 Move를 FixedUpdate에서 호출한 이유이다.
    */
    // 분석 문제 4 : CameraLook을 LateUpdate에서 호출하는 이유
    /*
        우선 Update 별로 실행 순서는 다음과 같다.
        FixedUpdate -> Update -> LateUpdate

        FixedUpdate는 대체로 물리 관련 연산에서 오브젝트의 움직임 등이 변화할 것이다.
        Update에서는 물리적 움직임 외에 다른 다양한 연산에서 오브젝트들이 상호작용할 것이다.

        그렇다면 카메라의 렌더링은 언제 수행하는 것이 가장 바람직할까.
        카메라는 플레이어가 보는 화면을 그려주는 역할을 수행한다.

        만약 오브젝트들의 움직임이 끝나지 않은 도중에 카메라가 이 화면을 담아낸다면
        움직임이 완성되지 않은 그림이 카메라에 찍힐 것이다.

        이 지점에서 의도하지 않는 문제가 일어날 수 있는데
        카메라가 다른 오브젝트에 종속되어 움직이는 경우,
        카메라는 종속된 오브젝트의 움직임과 동시에 화각을 움직여주어야하는 상황이 생긴다.

        이런 동기화된 움직임으로 인해서 원치 않게 이상한 방향을 바라보고 있다거나
        보여져선 안될 오브젝트를 바라볼 수도 있다.

        이런 상황을 최소화하기 위해서
        카메라와 같이 렌더링, 플레이어에게 보여지기 위한 작업은 LateUpdate로 분리하여
        작업하도록한다.

        정리하자면
        카메라가 캐릭터를 비추기위해 따라다니거나 시점을 움직이는 동작은
        캐릭터의 움직임에 대한 변경사항 이후에 따라와야할 추가적인 로직이다.

        캐릭터 움직임에 대한 계산이 완전히 종료되어 최종 값이 적용된 후
        카메라를 추가적으로 움직여서 일관된 결과를 보장할 수 있다.
    */
    void FixedUpdate()
    {
        Move();
    }

    void LateUpdate()
    {
        if (canLook)
            CameraLook();    
    }

    // PlayerInput의 Unity Event에 할당되어 입력을 받는다. : W,A,S,D의 입력을 통한 방향값
    public void OnMove(InputAction.CallbackContext value)
    {
        // if (value.phase == InputActionPhase.Started) // 키를 누를 떼, 한 번만
        if (value.phase == InputActionPhase.Performed) // 키가 눌리는 동안
        {
            // 입력받은 방향값을 캐싱해둔다.
            curMovementInput = value.ReadValue<Vector2>();
        }
        else if (value.phase == InputActionPhase.Canceled) // 키를 뗄 때, 한 번만
        {
            curMovementInput = Vector2.zero;
        }
    }    
    
    void Move()
    {
        // FixedUpdate에서 호출되어 캐싱한 방향값을 적용한다.
        Vector3 dir = transform.forward * curMovementInput.y + transform.right * curMovementInput.x;
        dir *= moveSpeed;      // 방향에 지정한 속도를 반영한다.
        dir.y = rb.velocity.y; // 이때, 점프와 같이 값을 덮어씌우지 말아야할 경우는 예외처리한다.

        rb.velocity = dir;
    }
    
    // PlayerInput의 Unity Event에 할당되어 입력을 받는다. : 마우스의 움직임
    public void OnLook(InputAction.CallbackContext value)
    {
        // 입력받은 마우스의 움직임을 캐싱해둔다.
        mouseDelta = value.ReadValue<Vector2>();
    }
    void CameraLook()
    {
        // LateUpdate에서 호출되어 캐싱한 마우스 움직임을 적용한다.
        // 이때, 횡방향 이동(마우스 움직임의 X축)은 캐릭터가 좌우로 회전해야 하므로 Y축에 적용한다.
        // 종방향 이동(마우스 움직임의 Y축)은 캐릭터의 시야와 관련해서 상하로 회전해야 하므로 카메라 회전값의 X축에 적용한다.

        camCurXRot += mouseDelta.y * lookSensitivity;  // 사전에 지정한 민감도를 반영한다.
        camCurXRot = Mathf.Clamp(camCurXRot, minXLook, maxXLook); // 이때, 종방향 이동은 지나치게 회전하지 않도록 하한값과 상한값을 적용한다.
        cameraContainer.localEulerAngles = new Vector3(-camCurXRot, 0, 0);

        transform.eulerAngles += new Vector3(0, mouseDelta.x * lookSensitivity, 0);
    }

    public void OnJump(InputAction.CallbackContext value)
    {

        if (value.phase == InputActionPhase.Started && IsGrounded()) // 키를 누를 떼, 한 번만
        {
            rb.AddForce(Vector3.up * jumpPower, ForceMode.Impulse);
        }
    }

    // 캐릭터가 점프 중에 또 점프하지 않도록하기 위한 메서드이다.
    // 해결 아이디어는 캐릭터가 점프하려면 땅에 발을 딛고 있어야 한다에서 착안한 것 같다.
    bool IsGrounded()
    {
        // 캐릭터가 경사진 곳에 있을 수 있으므로 캐릭터 주변 4개 방향(앞,뒤,좌,우)에서 아래를 보는 레이캐스트를 투사하여 땅에 있는지 확인한다.
        // 레이캐스트가 닿는 범위는 땅이 충분히 가까워야하는 상황이므로 0.2f가 적용되었다.
        Ray[] rays = new Ray[4]
        {
            new Ray(transform.position + transform.forward * 0.2f + transform.up * 0.01f, Vector3.down),
            new Ray(transform.position + -transform.forward * 0.2f + transform.up * 0.01f, Vector3.down),
            new Ray(transform.position + transform.right * 0.2f + transform.up * 0.01f, Vector3.down),
            new Ray(transform.position + -transform.right * 0.2f + transform.up * 0.01f, Vector3.down),
        };

        // 앞에서 지정한 4개 방향을 레이캐스트로 조사한다.
        // 하나라도 땅에 닿은 결과가 있다면 true를 반환하고, 그렇지 않다면 false를 반환한다.
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
