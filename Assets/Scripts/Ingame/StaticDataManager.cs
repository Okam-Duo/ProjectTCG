using System;
using System.Threading;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using static UnityEngine.Rendering.DebugUI;

//id기반 정적 데이터 관리자
public sealed class StaticDataManager
{

    #region 테이블 주소

    private const string _addressPrefix = "StaticDataManager/";

    private const string _cardTableAddress = _addressPrefix + "CardTable";
    private const string _heroTableAddress = _addressPrefix + "HeroTable";
    private const string _packTableAddress = _addressPrefix + "PackTable";


    #endregion

    #region private 필드

    private static StaticDataManager _instance;

    private Action _onLoaded;
    private int _loadingCounter = 0;

    private StaticDataLoader<HeroData, HeroTable, HeroDataHolder> _heroDataLoader;
    private StaticDataLoader<CardData, CardTable, CardDataHolder> _cardDataLoader;
    private StaticDataLoader<PackData, PackTable, PackDataHolder> _packDataLoader;

    #endregion

    public static bool IsReady { get; private set; } = false;

    private StaticDataManager(Action onLoaded)
    {
        _heroDataLoader = new(_heroTableAddress, CheckLoadingEnd);
        _cardDataLoader = new(_cardTableAddress, CheckLoadingEnd);
        _packDataLoader = new(_packTableAddress, CheckLoadingEnd);

        _onLoaded = onLoaded;
    }

    public static void LoadTableAsync(Action onLoadedTable)
    {
        _instance = new(onLoadedTable);
    }

    public static CardData GetCardData(int id)
    {
        Debug.Assert(IsReady, "테이블이 준비되지 않았습니다.");

        return _instance._cardDataLoader.GetData(id);
    }

    public static HeroData GetHeroData(int id)
    {
        Debug.Assert(IsReady, "테이블이 준비되지 않았습니다.");

        return _instance._heroDataLoader.GetData(id);
    }

    public static PackData GetPackData(int id)
    {
        Debug.Assert(IsReady, "테이블이 준비되지 않았습니다.");

        return _instance._packDataLoader.GetData(id);
    }

    #region 기능 구현용 private요소들

    private void CheckLoadingEnd()
    {
        int result = Interlocked.Increment(ref _loadingCounter);

        if (result == 2)
        {
            IsReady = true;
            _onLoaded?.Invoke();
        }
    }

    private class StaticDataLoader<DataT, TableT, HolderT>
        where TableT : IStaticDataTable<HolderT>
        where HolderT : IStaticDataHolder<DataT>
    {
        private HolderT[] _holders;
        private Action _onLoaded;

        public StaticDataLoader(string tableAddress, Action OnLoaded)
        {
            Addressables.LoadAssetAsync<TableT>(tableAddress).Completed += OnLoadedTable;
            _onLoaded = OnLoaded;
        }

        public DataT GetData(int id)
        {
            Debug.Assert(id >= 0 && id < _holders.Length, "참조하려는 데이터의 id가 범위를 벗어났습니다.");
            Debug.Assert(_holders[id] != null, "참조하려는 데이터의 영웅이 할당되지 않았습니다.");

            return _holders[id].GetData();
        }

        private void OnLoadedTable(AsyncOperationHandle<TableT> tableHandle)
        {
            _holders = tableHandle.Result.Holders;

            _onLoaded?.Invoke();
        }
    }

    #endregion
}

public interface IStaticDataTable<DataHolderT>
{
    DataHolderT[] Holders { get; }
}

public interface IStaticDataHolder<DataT>
{
    DataT GetData();
}