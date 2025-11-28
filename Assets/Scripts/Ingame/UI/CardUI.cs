using UnityEngine;
using UnityEngine.InputSystem;

public class CardUI : MonoBehaviour
{
    private enum ECardState
    {
        Idle = 0,
        OnMouse = 1,
        OnDrag = 2,
        OnUse = 3
    }

    [SerializeField] private float _idleScale;
    [SerializeField] private float _onMouseScale;
    [SerializeField] private float _onDragScale;
    [SerializeField] private float _onUseScale;

    [SerializeField] private ECardState _state;

    private float _currentTargetScale = 1;

    private void Start()
    {
        SetState(ECardState.Idle);
    }

    private void Update()
    {
        transform.localScale = new Vector3(_currentTargetScale, _currentTargetScale, _currentTargetScale);
        if (_state != ECardState.Idle)
        {
        }

        if (_state == ECardState.OnDrag)
        {
            transform.position = Mouse.current.position.ReadValue();
        }
    }

    #region 마우스 이벤트

    public void OnMouseEnterEvent()
    {
        if (_state == ECardState.Idle)
        {
            SetState(ECardState.OnMouse);
        }
    }

    public void OnMouseExitEvent()
    {
        if (_state == ECardState.OnMouse)
        {
            SetState(ECardState.Idle);
        }
    }

    public void OnMouseBeginDragEvent()
    {
        if (_state == ECardState.OnMouse)
        {
            SetState(ECardState.OnDrag);
        }
    }

    public void OnMouseEndDragEvent()
    {
        if (_state == ECardState.OnDrag)
        {
            SetState(ECardState.OnUse);
            TryUseCard();
        }
    }

    private void SetState(ECardState state)
    {
        _state = state;
        _currentTargetScale = GetTargetScale(state);
    }

    #endregion

    private void TryUseCard()
    {
    }

    private float GetTargetScale(ECardState state)
    {
        switch (state)
        {
            case ECardState.Idle:
                return _idleScale;
            case ECardState.OnMouse:
                return _onMouseScale;
            case ECardState.OnDrag:
                return _onDragScale;
            case ECardState.OnUse:
                return _onUseScale;
            default:
                Debug.Assert(false, "CardUI 클래스에서 새로운 ECardState 타입에 대한 카드 사이즈 할당 필요");
                return -1;
        }
    }
}
