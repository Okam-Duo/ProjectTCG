using UnityEngine;

//들고있는 카드 관리용 손패 클래스
public class HandUI : MonoBehaviour
{
    //플레이어가 드래그한 카드를 사용 가능한지 확인
    //올바른 범위에 드래그 했는지 등
    public bool CheckConditionToUseCard(CardUI card)
    {
#warning 카드 드래그 범위 지정할 것
        return false;
    }
}
