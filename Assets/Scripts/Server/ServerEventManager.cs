using Shared.Network;
using System;
using System.Collections.Generic;
using Shared;
using System.Diagnostics;

#warning 클라이언트 개발용 임시 어댑터 클래스, 추후 기능 연결 필요

//패킷 송수신 어댑터
public class ServerEventManager
{
    //싱글턴
    public static ServerEventManager Instance { get; private set; } = new ServerEventManager();

    //콜백은 메인스레드에서 작동함이 보장됨
    private Dictionary<PacketID, Action<IPacket>> _onRecievePacket = new();

    private ServerEventManager() { }

    public void Send(IPacket packet)
    {
        //TODO
    }

    public void RegistCallback(PacketID id, Action<IPacket> callback)
    {
        if (_onRecievePacket.ContainsKey(id))
        {
            _onRecievePacket[id] += callback;
        }
        else
        {
            _onRecievePacket[id] = callback;
        }
    }

    public void UnregistCallback(PacketID id, Action<IPacket> callback)
    {
        Debug.Assert(_onRecievePacket.ContainsKey(id), "콜백이 없는 패킷ID의 콜백을 등록해제하려고 시도했습니다.");

        if (_onRecievePacket.ContainsKey(id))
        {
            _onRecievePacket[id] -= callback;
        }
    }
}