# [스탠다드반] 꾸준 실습 3

## Q1 분석 문제
### 1. 입문 강의에서 입력 받는 방식의 차이와 공통점
> <b>공통점 : </b><br>
> Input Action Asset과 Player Input를 이용한다.<br>
> Player Input에게 입력에 대한 정보를 받는다.

> <b>차이점 :</b><br>
> 1. 사용하는 매개변수의 자료형이 다르다.
> #### Send Messages : InputValue <br><br>
>> InputValue 주요 기능
>> * Get : 입력 값을 받는 메서드
>> * Get<T\> : 입력값을 받는 메서드로 원하는 자료형을 제너릭란이 입력하여 받을 수 있다.<br>
단, Input Action Asset에서 설정한 입력 방식이 지원하는 자료형인 경우에 한정된다.
>> * isPressed : 버튼이 눌렸는지 아닌지 여부 - 버튼 방식이 아니라면 오류가 뜸
> 
> #### Invoke Unity Events : InputAction.CallbackContext <br>
> CallbackContext를 통해서 더 세세한 입력 시점을 이용할 수 있다.<br><br>
>> CallbackContext 주요 기능
>> * phase : 입력의 시점, 입력 상태를 받아올 수 있다.<br>

  |상태 이름|설명|값|
  |-|-|-|
  |Disabled| 액션이 활성화되지 않은 상태 |0|
  |Waiting| 액션이 활성화되어 있고 관련된 인풋을 받도록 기다리는 상태|1|
  |Started| 연동한 컨트롤러가 이제 막 액션을 시작하는 상태, 액션이 시작할 때 |2|
  |Performed| 액션이 진행 중인 상태, 누르고 있는 중 |3|
  |Canceled| 액션이 끝난 상태, 손을 뗀 상태 |4|
>> * ReadValue : 입력값을 받아올 수 있다.
>> * ReadValue<T\> : 입력값을 받는 메서드로 원하는 자료형을 제너릭란이 입력하여 받을 수 있다.<br>
단, Input Action Asset에서 설정한 입력 방식이 지원하는 자료형인 경우에 한정된다.
> ----
> 2. 성능 차이
> #### Send Messages<br><br>
> * 성능적인 측면에서 **매우 비싼 방식**이다.
> * Send Messages 방식이 Player Input 컴포넌트와 같이 있는 **다른 컴포넌트들 전부**에게 양식에 맞는 메서드에 찾고 호출을 보낸다.<br>이 과정에서 **비용이 매우 많이 소모**된다.
> #### Invoke Unity Events<br><br>
> * 성능 측면에서 Send Messages보다 **더 저렴하다.**
> * **등록된 컴포넌트의 메서드만 호출**하므로 비용이 상당히 절감된다.
> ----
> +. 각 방식별 성능 비교 (오른쪽이 가장 성능이 좋다는 의미)
>> Broadcast Message < Send Message < Invoke Unity Event <= Invoke C# Event

### 2. CharacterManager와 Player의 역할
> #### CharacterManager의 역할
> 지금은 관리해야할 캐릭터가 지금은 플레이어 하나이기 때문에<br>
> CharacterManager의 역할이 단지 Player을 캐싱하는 것에서 멈춰있지만<br>
> 기능이 많아지면서 캐릭터가 다양해질수록 그리고 특정한 캐릭터를 찾는 등의 작업이 필요해 질수록 CharacterManager의 역할은 더 중요해질 것이다.<br><br>
> 또한, 개별 객체에 직접 접근하는 방식이 권장하지 않는 방식이기에<br>
> Player에 직접 접근하기 보다는 CharacterManager를 통한 접근이 추후 확장성을 위해서라도 필요하다고 생각한다.
>
> #### Player의 역할
> 플레이어를 이루는 기능들은 매우 다양하다.<br>
> 그러다보니 Player라는 클래스에 이 모든 기능을 구현하면<br>
> 한 클래스가 너무 많은 책임을 갖게되면서 SPR를 위반할 것이다.<br>
>
> 그래서 현재 플레이어에게 필요한 여러 기능(책임)들은 각각의 클래스들로 위임되었고<br>
> 결과적으로 Player는 플레이어와 관련된 여러 기능을 가진 복합객체의 성격을 띄고 있다.<br>
> 현재 프로젝트에서 Player는 하나만 존재하고 거의 모든 기능이 Player에 접근하다보니 일종의 매니저 성격을 띄게되었다.<br><br>
> 하지만 플레이어는 여전히 하나의 지역적인 객체이며<br>
> 추후의 다양한 플레이어 캐릭터의 추가 등을 고려한다면<br>
> 확장성을 위해서 CharacterManager와 Player를 구분하는 것이 바람직할 것이다.

### 3. 핵심 로직 분석
> #### Move
> 플레이어에게 W,A,S,D로 입력받는 값은 Vector2 형태로 넘어온다.<br>
> 이때 입력값은 다음과 같다.
>> Up (W) : (0, 1)<br>
>> Down (S) : (0, -1)<br>
>> Left (A) : (-1, 0)<br>
>> Right (D) : (1, 0)<br><br>
>
> 이렇게 입력받은 Vector2 값을 movement 변수에 캐싱해두었다가<br>
> FixedUpdate에서 Move 메서드를 호출하여 플레이어를 움직인다.<br>
> 캐싱된 movement을 캐릭터 transform의 전면, 측면에 해당하는 값과 곱해줌으로써 <br>
> 입력된 값으로 캐릭터의 움직임의 방향을 구현할 수 있다.<br>
```cs
Vector2 movement;
float speed = 10f;

void FixedUpdate()
{
    Move();
}

void Move()
{
    // 캐릭터의 앞, 뒤에 대한 입력 : movement.y
    // 캐릭터의 좌, 우에 대한 입력 : movement.x
    Vector3 move = transform.forward * movement.y + transform.right * movement.x;
    move *= speed;
    move.y += rb.velocity.y;

    rb.velocity = move;
}
```
> #### CameraLook
> Move때와 비슷하게 CameraLook도 Vector2의 자료형을 입력받아 사용한다.<br>
> 하지만 차이점은 이번에 입력받는 것은 mouseDelta 값이다.<br>
> mouseDelta는 마우스 위치의 변화량을 나타낸다.<br><br>
> mouseDelta.x는 마우스의 좌,우 이동량을 나타내고<br>
> mouseDelta.y는 마우스의 상,하 이동량을 나타낸다.<br><br>
> 그리고 적용할 때 유의할 점은 mouseDelta.x는 캐릭터의 좌우 회전에 사용되고<br>
> mouseDelta.y는 캐릭터 카메라의 상하 회전에 사용된다.<br>
```cs
void CameraLook()
{
    float speed = rotateSensitive * Time.deltaTime;

    transform.Rotate(Vector3.up * direction.x * speed);

    camRotateX += speed * -direction.y;
    camRotateX = Mathf.Clamp(camRotateX, clampForFirstPerson.x, clampForFirstPerson.y);

    cameraAxis.localEulerAngles = new Vector3(camRotateX, 0f, 0f);
}   
```
> #### IsGrounded
> 캐릭터가 점프 입력을 받아 점프를 했는데 이때 또 다시 점프를 입력받으면 공중에서 한 번 더 점프를 할 수 있다.<br>
> 이것을 방지하기 위해서 캐릭터가 현재 땅에 있는지 검사하는 메서드를 만들었다.<br>
> 방식은 캐릭터의 아래로 짧은 레이캐스트를 쏘아서 맞았는지 검사하는 것이다.<br><br>
> 단, 이때 한 지점에서만 레이캐스트를 쏘면<br>
> 캐릭터가 경사진 곳에 있을 때나 아슬아슬하게 절벽 같은 곳에 서 있을 때,<br>
> 땅에 있지 않는 것으로 판단할 수 있어서 캐릭터의 앞,뒤,좌,우 네방향에서 쏘도록 구현되었다.<br><br>
> 그리고 레이캐스트가 캐릭터를 땅이라고 인식하지 않도록 LayerMask에서 자기 자신을 제외한 오브젝트들을 감지하도록했다.<br>
```cs
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
```

### 4. FixedUpdate와 LateUpdate
> #### Move를 FixedUpdate에서 호출하는 이유
> Move 메서드의 구현 방식을 보면 rigidbody에 접근하여 속도값을 직접 조절하고 있다.<br>
> 이는 물리 연산을 통한 캐릭터 움직임 구현 방법이다.<br><br>
> FixedUpdate는 Update보다 빨리 호출되며, Update보다 다르게 일정한 간격으로 호출된다.<br>
> 또한 공식문서에 따르면 Unity 엔진에서 물리 연산은 FixedUpdate 바로 뒤에 진행된다고 한다.<br>
> 물리 연산 관련 작업을 FixedUpdate에서 호출하면 바로 직후에 Unity 엔진단에서 물리 연산을 처리할 수 있는 것이다.<br>
> 이 부분이 Move를 FixedUpdate에서 호출한 이유이다.
> #### CameraLook을 LateUpdate에서 호출하는 이유
> 우선 Update 별로 실행 순서는 다음과 같다.<br>
>> FixedUpdate -> Update -> LateUpdate
>
> FixedUpdate는 대체로 물리 관련 연산에서 오브젝트의 움직임을 반영할 것이다.<br>
> Update에서는 물리적 움직임 외에 다른 다양한 연산에서 오브젝트들이 상호작용할 것이다.<br>
>
> 카메라는 플레이어가 보는 화면을 그려주는 역할을 수행한다.<br>
> 만약 오브젝트들의 움직임이 끝나지 않은 도중에 카메라가 이 화면을 담아낸다면<br>
> 움직임이 완성되지 않은 그림이 카메라에 찍힐 것이다.
>
> 이 지점에서 의도하지 않는 문제가 일어날 수 있는데<br>
> 카메라가 다른 오브젝트에 종속되어 움직이는 경우<br>
> 카메라는 종속된 오브젝트의 움직임과 동시에 화각을 움직여주어야하는 상황이 생긴다.<br><br>
> 이런 동기화된 움직임으로 인해서 원치 않게 이상한 방향을 바라보고 있다거나 보여져선 안 될 오브젝트를 바라볼 수도 있다.<br>
> 이런 상황을 최소화하기 위해서 카메라와 같이 렌더링, 플레이어에게 보여지기 위한 작업은 LateUpdate로 분리하여 작업하도록한다. <br><br>
> 정리하자면<br>
> 카메라가 캐릭터를 비추기위해 따라다니거나 시점을 움직이는 동작은<br>
> 캐릭터의 움직임에 대한 변경사항 이후에 따라와야할 추가적인 로직이다.
> 캐릭터 움직임에 대한 계산이 완전히 종료되어 최종값이 적용된 후<br>
> 카메라를 추가적으로 움직여서 일관된 결과를 보장할 수 있다.<br>

## Q2 분석 문제
### 1. 별도의 UI 스크립트를 만드는 이유에 대해 객체지향적 관점에서 생각
> UI는 정보를 보여주는 방식에 따라 처리하는 방법도 다양하다.<br>
> 만약에 이 UI들을 하나의 스크립트에서 관리한다면<br>
> 서로 다른 방식을 보여주는 UI 기능들로 인해서 한 스크립트에 너무 많은 책임이 모일 것이다.<br><br>
> 이는 곧 단일 책임 원칙을 위배하는 작업이다.<br>
> 지나치게 많은 기능을 가진 클래스는 내부적으로 유지보수와 관리가 힘들어지고<br>
> 수정이 생긴다면 최악의 경우 클래스를 전반적으로 고쳐야할 상황이 올 수도 있다.<br><br>
> 그렇기에 UI관련된 작업을 한다면 데이터 정의, 데이터 처리, UI 표현<br>
> 이렇게 3개의 역할로 나누어서 구현하는 것을 권장하는 것이 MVC 패턴이다.

### 2. 인터페이스의 특징에 대해 정리해보고 구현된 로직을 분석
> #### 인터페이스의 특징
> * 추상화<br>
> 구체적인 클래스를 직접 찾아 호출하지 않고<br>
> 인터페이스를 상속한 클래스에게서 인터페이스를 찾아 명시한 메서드를 호출하여<br>
> 하나의 방식으로 다양한 구현을 사용할 수 있다.<br>
> * 강제적 구현의 보장<br>
> 인터페이스를 상속한 클래스는 명시된 메서드를 반드시 구현해야하므로<br>
> 인터페이스에 접근하여 원하는 메서드를 호출하여 다양한 구현을 사용할 수 있다.
> * 다중 상속<br>
> 인터페이스를 다중 상속받아 여러 기능을 동시에 구현할 수 있다.<br>
>
> #### 구현된 로직
> 씬에는 접근한 대상에게 주기적으로 물리적 피해를 주는 CampFire 오브젝트가 있다.<br>
> 그리고 체력의 개념과 데미지를 입는 기능을 가진 클래스들은 프로젝트가 진행됨에 따라 매우 다양해질 것이다.<br>
> CampFire는 이런 클래스들이 추가될 때마다 해당 구체적인 클래스에 직접 접근하여<br>
> 데미지를 주도록 한다면 매우 비효율적인 작업이 될 것이다.<br><br>
> 이를 위해서 프로젝트에서는 데미지를 입을 수 있는 대상에 IDamagable 인터페이스를 상속하고<br>
> IDamagable 내에 TakeDamage(float amount)라는 메서드를 반드시 구현하도록 했다.<br><br>
> 이렇게 한다면 CampFire는 앞으로 새로운 클래스들이 추가되더라도<br>
> IDamagable을 상속하는지만 확인하고 상속했다면 TakeDamage 메서드를 호출하는 것으로<br>
> 수 많은 앞으로 추가될 클래스에 대응할 수 있다.

### 3. 핵심 로직 분석
> #### UI 스크립트
> * Prompt<br>
> 플레이어의 화면 정중앙에서 레이캐스트를 쏘아 아이템을 검사하고<br>
> 검사한 아이템의 데이터를 Prompt Text에 텍스트로 넣어준다.
> * UICondition<br>
> 플레이어의 체력, 배고픔, 스태미너를 나타내는 Condition 클래스를<br>
> 묶어주고 이를 CharacterManager에 있는 Player에 접근하여 PlayerCondition에 전달해준다.<br><br>
> PlayerCondition에서 플레이어의 상태에 따른 변화가 발생하면<br>
> 전달받은 각 Condition들에 변화를 반영한다.<br><br>
> Condition 클래스는 가지고 있는 value 값을 할당된 bar 이미지에 반영한다.<br>
> #### CampFire
> CampFire 오브젝트의 Trigger로 IDamagable을 상속한 클래스가 들어온다면<br>
> List에 넣어 주기적으로 데미지를 입힌다.
> #### DamageIndicator
> PlayerCondition은 IDamagable을 상속하여 TakeDamage를 구현하고있다.<br>
> 그리고 외부에서 TakeDamage가 호출될 때 OnTakeDamage 이벤트를 발생시킨다.<br><br>
> DamageIndicator는 OnTakeDamage 이벤트를 구독하고 있어<br>
> 플레이어가 데미지를 입는다는 것을 알 수 있다.<br>
> OnTakeDamage 이벤트가 발생하면 화면을 붉게 표시하는 Flash 메서드를 호출하여<br>
> 플레이어의 피해 표시를 출력한다.<br>

## Q3 분석 문제
> ### Interaction 기능의 구조와 핵심 로직
> Interaction 기능의 핵심은 IInteractable 인터페이스를 이용하여<br>
> 다양한 아이템들을 일관된 대응으로 처리했다는 것이다.<br><br>
> 플레이어는 화면 정중앙의 레이캐스트를 통해서 Interaction이 가능한 오브젝트를 찾는다면 IInteractable 자료형의 변수에 오브젝트를 저장하고<br>
> 이 오브젝트의 정보를 Prompt Text에 띄워준다.<br><br>
> 플레이어가 E키를 눌러 상호작용하면<br>
> 저장된 IInteractable에 OnInteract() 메서드를 호출하여<br>
> 자식에서 구현된 상호작용 내용을 사용한다.
>
> ### Inventory 기능의 구조와 핵심 로직
> 아이템과 상호작용을 했다면 아이템은 UIInventory의 AddItem 메서드를 호출한다.<br><br>
> 축적가능한 아이템이면서 UIInventory에 이미 있던 아이템이라면 14개의 ItemSlot 중에서<br>
> 일치하는 아이템 정보를 가진 슬롯을 찾고 수량을 1개 늘린다.<br><br>
> 새로 얻은 아이템이라면 빈 슬롯을 찾는다.<br>
> 빈 슬롯을 찾았다면 그곳에 아이템의 정보를 넣고 ItemSlot의 UI에 정보를 표시한다.<br><br>
> 만약 축적 가능하지 않고, 빈 슬롯이 없는 상태라면<br>
> 습득한 아이템은 다시 아이템 프리팹을 생성하여 버린다.<br>
>
> ItemSlot은 버튼으로 구성되어있고 클릭한다면<br>
> 그 아이템의 정보를 출력하고 아이템에 대한 상호작용 버튼을 표시한다.