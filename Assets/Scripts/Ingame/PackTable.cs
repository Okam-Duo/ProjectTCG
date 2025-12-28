using UnityEngine;

[CreateAssetMenu(fileName = "PackTable", menuName = "Custom/데이터 테이블/팩 테이블", order = int.MinValue)]
public class PackTable : ScriptableObject, StaticDataManager.IStaticDataTable<PackDataHolder>
{
    public PackDataHolder[] packs;

    public PackDataHolder[] Holders => packs;
}
