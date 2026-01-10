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
        //DebugUtil.Log($"[From Server] {}");

        int c = sizeof(ushort);
        PacketID packetId = (PacketID)BitConverter.ToUInt16(buffer.Array, c);
        ServerEventManager.Instance.RecieveData(packetId, new ArraySegment<byte>(buffer.Array, c, buffer.Count - c));

        return buffer.Count;
    }

    public override void OnSend(int numOfByte)
    {
        DebugUtil.Log($"Transferred byte : {numOfByte}");
    }
}