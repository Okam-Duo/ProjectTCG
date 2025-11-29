using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

//서폿 카드 UI 클래스
public class CardUI : MonoBehaviour
{
    //카드의 애니메이션을 위한 상태
    private enum CardState
    {
        Idle = 0,     //플레이어와 상호작용하지 않는 기본 상태
        OnMouse = 1,     //카드 위에 마우스가 올려짐
        OnDrag = 2,     //카드가 마우스로 인해 드래그되어 들려있음
        OnUse = 3     //카드가 사용됨
    }

    //이 카드를 소유한 손패
    public HandUI Hand { get; private set; }

    [Header("컴포넌트 참조")]
    [SerializeField] private Image _image;
    [SerializeField] private TextMeshProUGUI _nameText;
    [SerializeField] private TextMeshProUGUI _descriptionText;
    [Header("애니메이션 관련 변수")]
    [SerializeField] private float _idleScale;
    [SerializeField] private float _onMouseScale;
    [SerializeField] private float _onDragScale;
    [SerializeField] private float _onUseScale;

    [SerializeField] private CardState _state;

    //이 카드의 크기는 해당 변수의 크기에 점점 가까워짐(애니메이션)
    private float _currentTargetScale = 1;
    //카드를 사용하지 않았을 때 돌아갈 원래 좌표
    //손패에서 카드가 들려있던 좌표임
    private Vector3 _originPosition;

    private void Start()
    {
        SetState(CardState.Idle);
    }

    private void Update()
    {
        //카드의 크기를 변경
        transform.localScale = new Vector3(_currentTargetScale, _currentTargetScale, _currentTargetScale);
        
        //카드가 기본상태라면 원래 위치로 돌아감
        if (_state == CardState.Idle)
        {
            transform.position = _originPosition;
        }

        //카드를 들었다면 마우스 위치를 따라감
        if (_state == CardState.OnDrag)
        {
            transform.position = Mouse.current.position.ReadValue();
        }
    }

    #region 마우스 이벤트

    //마우스가 UI위에 올려졌을 때
    public void OnMouseEnterEvent()
    {
        if (_state == CardState.Idle)
        {
            SetState(CardState.OnMouse);
        }
    }

    //마우스가 UI위에서 나갔을 때
    public void OnMouseExitEvent()
    {
        //OnUse등의 상태에서 나가는건 상관없으니까 OnMouse상태가 맞는지 확인해줌
        if (_state == CardState.OnMouse)
        {
            SetState(CardState.Idle);
        }
    }

    //UI를 드래그하기 시작했을 때
    public void OnMouseBeginDragEvent()
    {
        if (_state == CardState.OnMouse)
        {
            SetState(CardState.OnDrag);
        }
    }

    //UI 드래그가 끝났을 때
    public void OnMouseEndDragEvent()
    {
        if (_state == CardState.OnDrag)
        {
            SetState(CardState.OnUse);
            TryUseCard();
        }
    }

    //애니메이션 상태 설정
    private void SetState(CardState state)
    {
        _state = state;
        _currentTargetScale = GetTargetScale(state);
    }

    #endregion

    //카드UI 데이터 초기화
    //카드UI 생성시 호출해야됨
    public void SetData(HandUI owner, CardData cardData)
    {
        Hand = owner;
        _image.sprite = cardData.sprite;
        _nameText.text = cardData.name;
        _descriptionText.text = cardData.description;
        #warning UI에 희귀도 반영해야함
    }

    //손패에서 카드 위치를 지정해주는 용도의 함수
    public void SetOriginPosition(Vector3 position)
    {
        _originPosition = position;
    }

    //카드 사용 시도
    private void TryUseCard()
    {
        //UI측면에서 확인할 조건
        if (Hand.CheckConditionToUseCard(this))
        {
#warning 카드사용 로직 작성해야함
        }
    }

    //애니메이션 타입에 따른 스케일 받아오기
    private float GetTargetScale(CardState state)
    {
        switch (state)
        {
            case CardState.Idle:
                return _idleScale;
            case CardState.OnMouse:
                return _onMouseScale;
            case CardState.OnDrag:
                return _onDragScale;
            case CardState.OnUse:
                return _onUseScale;
            default:
                Debug.Assert(false, "CardUI 클래스에서 새로운 ECardState 타입에 대한 카드 사이즈 할당 필요");
                return -1;
        }
    }
}
