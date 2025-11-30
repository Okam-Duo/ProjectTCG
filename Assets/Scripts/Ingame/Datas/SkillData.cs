using UnityEngine;

//스킬의 외형 데이터 저장용 스크립터블 오브젝트
//데이터베이스 적용 전 임시로 사용
[CreateAssetMenu(fileName = "NewSkillData", menuName = "Custom/영웅 스킬 데이터", order = int.MinValue)]
public class SkillData : ScriptableObject
{
    public int SkillID;

    public Sprite Sprite;     //스킬 일러스트
    public string Name;     //스킬 이름
    public string Description;     //스킬 설명
}