using System;
using System.Threading;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

/// <summary>
/// ScriptableObject 데이터 id기반 참조용 매니저
/// </summary>
public sealed class StaticDataManager
{

    #region 테이블 주소

    private const string _addressPrefix = "StaticDataManager/";

    private const string _cardTableAddress = _addressPrefix + "CardTable";
    private const string _heroTableAddress = _addressPrefix + "HeroTable";
    private const string _packTableAddress = _addressPrefix + "PackTable";

    private const int _loadDataTypeCount = 3;

    #endregion

    #region private 필드

    private static StaticDataManager _instance;

    private Action _onLoaded;
    private int _loadingCounter = 0;

    private StaticDataLoader<HeroData, HeroTable, HeroDataHolder> _heroDataLoader;
    private StaticDataLoader<CardData, CardTable, CardDataHolder> _cardDataLoader;
    private StaticDataLoader<PackData, PackTable, PackDataHolder> _packDataLoader;

    #endregion

    /// <summary>
    /// 데이터를 불러올 준비가 되었는지 확인합니다.
    /// </summary>
    public static bool IsReady { get; private set; } = false;

    private StaticDataManager(Action onLoaded)
    {
        _heroDataLoader = new(_heroTableAddress, CheckLoadingEnd);
        _cardDataLoader = new(_cardTableAddress, CheckLoadingEnd);
        _packDataLoader = new(_packTableAddress, CheckLoadingEnd);

        _onLoaded = onLoaded;
    }

    /// <summary>
    /// 데이터를 비동기로 불러옵니다, 데이터가 불러와진 뒤 부터 GetData류 함수를 사용할 수 있습니다.
    /// </summary>
    public static void LoadTableAsync(Action onLoaded)
    {
        _instance = new(onLoaded);
    }

    public static CardData GetCardData(int id)
    {
        Debug.Assert(IsReady, "테이블이 준비되지 않았습니다, LoadTableAsync함수를 호출하고 기다려주세요.");

        return _instance._cardDataLoader.GetData(id);
    }

    public static HeroData GetHeroData(int id)
    {
        Debug.Assert(IsReady, "테이블이 준비되지 않았습니다, LoadTableAsync함수를 호출하고 기다려주세요.");

        return _instance._heroDataLoader.GetData(id);
    }

    public static PackData GetPackData(int id)
    {
        Debug.Assert(IsReady, "테이블이 준비되지 않았습니다, LoadTableAsync함수를 호출하고 기다려주세요.");

        return _instance._packDataLoader.GetData(id);
    }

    #region 기능 구현용 private요소들

    private void CheckLoadingEnd()
    {
        int result = Interlocked.Increment(ref _loadingCounter);

        if (result == _loadDataTypeCount)
        {
            IsReady = true;
            _onLoaded?.Invoke();
        }
    }

    //데이터 종류에 따라 불러오는 로직 일반화
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

    #region 데이터 홀더, 테이블이 상속하는 인터페이스

    /// <summary>
    /// 데이터 홀더들을 배열로 저장해 ID를 배정하는 ScriptableObject
    /// </summary>
    /// <typeparam name="DataHolderT">테이블이 참조할 데이터 홀더</typeparam>
    public interface IStaticDataTable<DataHolderT>
    {
        DataHolderT[] Holders { get; }
    }

    /// <summary>
    /// 데이터를 입력하기 위한 ScriptableObject
    /// </summary>
    /// <typeparam name="DataT">홀더가 만들어낼 데이터 반환형</typeparam>
    public interface IStaticDataHolder<DataT>
    {
        DataT GetData();
    }

    #endregion
}