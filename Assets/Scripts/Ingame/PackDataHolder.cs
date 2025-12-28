using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewPackData", menuName = "Custom/팩 데이터", order = int.MinValue)]
public class PackDataHolder : ScriptableObject, StaticDataManager.IStaticDataHolder<PackData>
{
    [Header("팩 일러스트")]
    public Sprite Sprite;
    [Header("팩 이름")]
    public string Name;
    [Header("팩 설명")]
    public string Description;
    [Header("보상 리스트(보상 형식과 id 입력)")]
    public KeyValuePair<ResourceType, int>[] Rewards;     //<보상 타입, id> 배열

    public PackData GetData() => new PackData(Name, Description, Sprite, Rewards);
}
