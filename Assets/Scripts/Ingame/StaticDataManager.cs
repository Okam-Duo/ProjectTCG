using System;
using System.Threading;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

public class StaticDataManager
{

    #region 데이터 입력용 스크립터블 오브젝트

    [CreateAssetMenu(fileName = "CardTable", menuName = "Custom/카드 테이블", order = int.MinValue)]
    public class CardTable : ScriptableObject
    {
        public CardDataHolder[] cards;
    }

    [CreateAssetMenu(fileName = "HeroList", menuName = "Custom/영웅 테이블", order = int.MinValue)]
    public class HeroTable : ScriptableObject
    {
        public HeroDataHolder[] heroes;
    }

    [CreateAssetMenu(fileName = "NewHeroData", menuName = "Custom/영웅 데이터", order = int.MinValue)]
    public class HeroDataHolder : ScriptableObject
    {
        [Header("영웅 일러스트")]
        public Sprite Sprite;
        [Header("영웅 이름")]
        public string Name;
        [Header("최대 체력")]
        public int MaxHealth;
    }

    [CreateAssetMenu(fileName = "NewCardData", menuName = "Custom/서폿카드 데이터", order = int.MinValue)]
    public class CardDataHolder : ScriptableObject
    {
        [Header("카드 일러스트")]
        public Sprite Sprite;
        [Header("카드 이름")]
        public string Name;
        [Header("카드 설명")]
        public string Description;
        [Header("카드 희귀도")]
        public CardRarity Rarity;
        [Header("토큰카드 여부")]
        public bool IsToken;
    }

    private const string _cardTableAddress = "CardTable";
    private const string _heroTableAddress = "HeroTable";

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