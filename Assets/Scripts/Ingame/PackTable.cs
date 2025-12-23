using UnityEngine;

[CreateAssetMenu(fileName = "PackTable", menuName = "Custom/팩 테이블", order = int.MinValue)]
public class PackTable : ScriptableObject
{
    public PackDataHolder packs;
}
