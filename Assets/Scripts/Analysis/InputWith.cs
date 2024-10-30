using UnityEngine;
using UnityEngine.InputSystem;

namespace Analysis
{
    public class InputWithSendMessage : MonoBehaviour
    {
        // 분석문제 1
        // 공통점 
        // Input Action Asset과 Player Input를 이용한다
        // Player Input에게 인풋에 대한 정보를 받는다.

        // SendMessage : PlayerInput의 이벤트 호출을 받는 방식
        // 1. 사용하는 매개변수의 자료형이 다름 : InputValue
        /*
            그에 따라 입력을 이용하는 방식이 달라짐

            InputValue에서 사용하는 메서드는
            Get : 입력 값을 받는 메서드 
            isPressed : 버튼이 눌렸는지 아닌지 여부 - 버튼 방식이 아니라면 오류가 뜸
        */
        // 2. 성능 차이
        /*
            Player Input에 직접 등록하지 않아도 이 클래스에서 매개변수와 이름의 양식에 맞게 구현만 해놓으면
            Player Input이 호출시켜 준다.
            
            메세지를 보내는 방식 자체가 성능에 안좋다.
            이 게임 오브젝트에 붙어있는 컴포넌트에 메서드를 다 찾고 
            맞는 메서드에 보내는 과정에서 비용이 상당히 많이 소모된다.
        */

        // 정리
        // 공통적으로 InputActionAssets을 이용하는 PlayerInput으로 작동시킨다.
        // 단, 차이점은
        // Sendmessage의 경우 직접적인 할당의 과정없이 메서드 구현만 해두어도 입력을 처리할 수 있다
        // 하지만, 이 방법으로는 세세한 시점별 조정은 하기 위해선 별도의 구현이 필요하다.
        // 그리고 성능적인 측면에서 매우 비싼 방식이다.

        // vector2 
        public void OnMove(InputValue value)
        {
            Debug.Log(value.Get<Vector2>());
            // Debug.Log(value.isPressed);
            
        }

        // button
        public void OnJump(InputValue value)
        {
            Debug.Log(value.isPressed);
        }

        public void OnLook(InputValue value)
        {
            Debug.Log(value.Get<Vector2>());
        }
    }

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
}

