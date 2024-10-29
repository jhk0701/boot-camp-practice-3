using UnityEngine;
using UnityEngine.InputSystem;

public class InputWithUnityEvent : MonoBehaviour
{
    // 공통점
    // Input Action Asset과 Player Input를 이용한다
    // Player Input에게 인풋에 대한 정보를 받는다.

    // Unity Event : PlayerInput의 이벤트에 Unity Event로 등록하는 방식
    // 1. 사용하는 매개변수의 자료형이 다름 : InputAction.CallbackContext
    /*
        context에서는 sendmessage 방식과 다르게 더 세세한 입력 시점을 이용할 수 있다.

        phase에 따라 아래와 같이 시점별로 다른 사용을 할 수 있다.
        started : 누른 첫 순간
        performed: 누르는 중일 때
        canceled : 뗏을 때

        ReadValue : 입력값을 받는 메서드
    */
    // 2. 성능차이
    /*
        Player Input의 Unity Event란에 직접 할당해야 작동시킬 수 있다.
        Unity Event란에 직접 할당해서 쓰는 만큼
        Player Input은 할당된 오브젝트만 호출한다.

        이 과정에서 성능적으로 SendMessage보다 매우 효율적인 작동이 가능하다.
        
    */

    // 정리
    // 공통적으로 InputActionAssets을 이용하는 PlayerInput으로 작동시킨다.
    // 단, 차이점은
    // Unity Event의 경우 직접적인 할당해야만 메서드를 호출시킬 수 있다.
    // 성능 측면에서 더 저렴하다.

    // 방식별 성능 비용 비교
    // BroadCast Message < Send Message < Invoke Unity Event <= Invoke C# Event
    // 성능 측면에서 Invoke Unity Event, Invoke C# Event 두 방식을 추천해주심

    public void OnMove(InputAction.CallbackContext context)
    {
        if(context.phase == InputActionPhase.Started)
        {
            Debug.Log("Move Started");
        }
        else if(context.phase == InputActionPhase.Performed)
        {
            Debug.Log(context.ReadValue<Vector2>());

        }
        else if(context.phase == InputActionPhase.Canceled)
        {
            Debug.Log("Move Canceled");
        }
    }

    public void OnJump(InputAction.CallbackContext context)
    {

    }

    public void OnLook(InputAction.CallbackContext context)
    {

    }
}