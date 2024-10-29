using UnityEngine;

public class Player : MonoBehaviour
{
    // 분석 문제 2 : 
    // CharacterManager와 Player의 역할
    // CharacterManager : 
    /*
        관리해야할 캐릭터가 지금은 플레이어 하나이기 때문에
        CharacterManager의 역할이 단지 Player을 캐싱하는 것에서 멈춰있지만,
        멀티 플레이 등등의 기능이 추가된다면 이 CharacterManager를 통해서 다른 플레이어를 찾는 등의
        관리자 역할을 더 많이 수행할 수 있을 것으로 보인다.

        이번 강의 프로젝트에서는 Player만 캐싱하고 있으므로
        CharacterManager을 거치는 과정이 번거롭다면 CharacterManager의 싱글톤 패턴을 Player에게 옮기는 것도 괜찮을 것 같다.

        단, 이때 Player는 DontDestroy 해줄 필요가 없을테니 유의해야한다.
    */
    // Player : 
    /*
        플레이어 관련된 컴포넌트들을 캐싱하고 서로의 접근을 중재해주는 역할을 하고 있다.
        UI 등에서 현재 CharacterManager를 통해서 접근하고 있는데
        캐릭터 관리의 기능이 Player에게만 제한되어 있다면 Player에게 싱글톤을 옮겨 접근을 원활하게 해줄 수 있을 것 같다.
    */
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

}