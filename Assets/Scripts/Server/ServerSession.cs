using Shared.Network;
using System.Net;
using System.Text;
using System;
using UnityEngine;

public class ServerSession : Session
{
    public override void OnConnected(EndPoint endPoint)
    {
        DebugUtil.Log($"OnConnected : {endPoint}");

        ArraySegment<byte> s = BitConverter.GetBytes(124);
        Send(s);
    }

    public override void OnDisconnected(EndPoint endPoint)
    {
        DebugUtil.Log($"OnDisconnected : {endPoint}");
    }

    public override int OnRecv(ArraySegment<byte> buffer)
    {
        DebugUtil.Log($"[From Server] {Encoding.UTF8.GetString(buffer)}");
        return buffer.Count;
    }

    public override void OnSend(int numOfByte)
    {
        DebugUtil.Log($"Transferred byte : {numOfByte}");
    }
}