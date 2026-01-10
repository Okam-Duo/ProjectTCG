using Shared;
using Shared.Network;
using Shared.Packets;
using System;
using System.Collections.Generic;

#warning 클라이언트 개발용 임시 어댑터 클래스, 추후 기능 연결 필요
//추가 데이터는 전부 long타입으로 때워놨으니 나중에 변경하거나 로직 추가 필요
//커플링을 신경쓸 필요가 없다고 판단될 시 패킷 수신 이벤트를 콜백에서 함수로 변경 바람


//패킷 송수신 어댑터
public class ServerEventManager
{
    //싱글턴
    public static ServerEventManager Instance { get; private set; } = new ServerEventManager();

    public Dictionary<PacketID, Action<IPacket>> OnRecievePacket { get; private set; }

    private ServerEventManager() { }

    public void Send(IPacket packet)
    {
        ArraySegment<byte> data = packet.Write();
        ServerSession.Instance.Send(data);
    }

    public void RecieveData(PacketID packetID, ArraySegment<byte> buffer)
    {
        IPacket packet = PacketFactory.GeneratePacket(packetID, buffer);

        if (OnRecievePacket.ContainsKey(packetID))
        {
            OnRecievePacket[packetID]?.Invoke(packet);
        }
    }
}