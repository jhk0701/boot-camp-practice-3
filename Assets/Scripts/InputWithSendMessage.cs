using UnityEngine;
using UnityEngine.InputSystem;

public class InputWithSendMessage : MonoBehaviour
{
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