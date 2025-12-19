using System;
using System.Threading;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

public class StaticDataManager
{

    #region 테이블 파일 경로

    private const string _cardTableAddress = "StaticDataManager/CardTable";
    private const string _heroTableAddress = "StaticDataManager/HeroTable";

    #endregion

    public static bool IsReady { get; private set; } = false;

    #region private 필드

    private static StaticDataManager _instance;

    private Action _onLoaded;

    private CardData[] _cardDatas;
    private HeroData[] _heroDatas;
    private int _loadingCounter = 0;

    #endregion

    public static void LoadTableAsync(Action onLoadedTable)
    {
        _instance = new();
        _instance._onLoaded = onLoadedTable;
        _instance.GetAddressedDatasAsync();
    }

    public static CardData GetCardData(int id)
    {
        Debug.Assert(id >= 0 && id < _instance._cardDatas.Length, "참조하려는 카드의 id가 범위를 벗어났습니다.");
        Debug.Assert(_instance._cardDatas[id] != null, "참조하려는 id의 카드가 할당되지 않았습니다.");
        Debug.Assert(IsReady, "테이블이 준비되지 않았습니다.");

        return _instance._cardDatas[id];
    }

    public static HeroData GetHeroData(int id)
    {
        Debug.Assert(id >= 0 && id < _instance._cardDatas.Length, "참조하려는 영웅의 id가 범위를 벗어났습니다.");
        Debug.Assert(_instance._cardDatas[id] != null, "참조하려는 id의 영웅이 할당되지 않았습니다.");
        Debug.Assert(IsReady, "테이블이 준비되지 않았습니다.");

        return _instance._heroDatas[id];
    }

    #region 파일 읽기

    private void GetAddressedDatasAsync()
    {
        Addressables.LoadAssetAsync<CardTable>(_cardTableAddress).Completed += OnLoadedCardTable;
        Addressables.LoadAssetAsync<HeroTable>(_heroTableAddress).Completed += OnLoadedHeroTable;
    }

    private void OnLoadedCardTable(AsyncOperationHandle<CardTable> table)
    {
        CardTable cardTable = table.Result;

        _cardDatas = new CardData[cardTable.cards.Length];

        for (int i = 0; i < _cardDatas.Length; i++)
        {
            _cardDatas[i] = new CardData(i, cardTable.cards[i]);
        }

        CheckLoadingEnd();
    }

    private void OnLoadedHeroTable(AsyncOperationHandle<HeroTable> table)
    {
        HeroTable heroTable = table.Result;

        _heroDatas = new HeroData[heroTable.heroes.Length];

        for (int i = 0; i < _heroDatas.Length; i++)
        {
            _heroDatas[i] = new HeroData(i, heroTable.heroes[i]);
        }

        CheckLoadingEnd();
    }

    private void CheckLoadingEnd()
    {
        int result = Interlocked.Increment(ref _loadingCounter);

        if (result == 2)
        {
            IsReady = true;
            _onLoaded?.Invoke();
        }
    }

    #endregion
}