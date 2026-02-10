using Shared.Network;
using System.Net;
using System.Text;
using System;
using UnityEngine;
using Shared.Packets;

public class ServerSession : Session
{
    public static ServerSession Instance { get; private set; }

    public override void OnConnected(EndPoint endPoint)
    {
        DebugUtil.Log($"OnConnected : {endPoint}");

        Instance = this;
    }

    public override void OnDisconnected(EndPoint endPoint)
    {
        DebugUtil.Log($"OnDisconnected : {endPoint}");

        Instance = null;
    }

    //Session의 로직으로 인해 패킷의 유효성이 보장된 상태임
    public override int OnRecv(ArraySegment<byte> buffer)
    {
        //수신된 데이터에서 패킷의 전체 사이즈와 패킷ID를 뽑아옴
        ushort packetSize = BitConverter.ToUInt16(buffer.Array, buffer.Offset);
        int c = sizeof(ushort);
        PacketID packetId = (PacketID)BitConverter.ToUInt16(buffer.Array, buffer.Offset + c);
        c += sizeof(ushort);

        //컨텐츠단으로 전달
        NetworkManager.RecieveData(packetId, new ArraySegment<byte>(buffer.Array, buffer.Offset + c, packetSize - c));

        //버퍼에서 몇바이트나 사용했는지 반환
        return buffer.Count;
    }

    public override void OnSend(int numOfByte)
    {
        DebugUtil.Log($"Transferred byte : {numOfByte}");
    }
}