using System;

#warning 클라이언트 개발용 임시 어댑터 클래스, 추후 기능 연결 필요
//추가 데이터는 전부 long타입으로 때워놨으니 나중에 변경하거나 로직 추가 필요
//커플링을 신경쓸 필요가 없다고 판단될 시 패킷 수신 이벤트를 콜백에서 함수로 변경 바람


//패킷 송수신 어댑터
public class PacketManager
{
    //싱글턴
    public static PacketManager Instance { get; private set; } = new PacketManager();

    public enum GameResult
    {
        Win,     //내가 승리
        Lose,     //내가 패배
        Draw     //무승부
    }

    #region 서버의 패킷 수신

    public event Action<GameResult> OnGameOver;
    //플레이어 인덱스
    public event Action<byte> OnChangeTure;

    //플레이어 인덱스, 카드 ID
    public event Action<byte, int> OnDrawCard;
    public event Action<byte, int> OnUseCard;
    //플레이어 인덱스, 카드 ID, 추가 데이터
    public event Action<byte, int, long> OnRunCardEffect;

    //플레이어 인덱스, 영웅 인덱스, 스킬 인덱스, 추가 데이터
    public event Action<byte, int, int, long> OnHeroUseSkill;
    //플레이어 인덱스, 영웅 인덱스
    public event Action<byte, int> OnHeroDead;
    //플레이어 인덱스, 영웅 인덱스, 변경 전 체력, 변경 후 체력
    public event Action<byte, int, int, int> OnHeroChangeHelath;

    #endregion

    private PacketManager() { }

    #region 패킷 송신 함수

    public void SendUseCardPacket(byte cardIndex, Action<bool> isSuccessCallback)
    {
        SendUseCardPacket(cardIndex, 0, isSuccessCallback);

        isSuccessCallback?.Invoke(true);
    }

    public void SendUseCardPacket(byte cardIndex, long additionalData, Action<bool> isSuccessCallback)
    {
        isSuccessCallback?.Invoke(true);
    }

    public void SendHeroUseSkill(int heroIndex, int skillIndex, long additionalData, Action<bool> isSuccessCallback)
    {
        isSuccessCallback?.Invoke(true);
    }

    public void SendEndTurn()
    {

    }

    public void SendSurrender()
    {

    }

    #endregion
}