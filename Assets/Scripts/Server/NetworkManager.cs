using Shared;
using Shared.Network;
using Shared.Packets;
using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using System.Collections.Concurrent;
using UnityEngine;

//패킷 송수신 어댑터
public class NetworkManager : MonoBehaviour
{
    //싱글턴
    private static NetworkManager _instance;

    private Dictionary<PacketID, Action<IPacket>> _onRecievePacket = new();
    private ConcurrentQueue<KeyValuePair<PacketID, IPacket>> _callBackQueue = new();

    private void Update()
    {
        //Flush _callBackQueue
        while (_callBackQueue.Count > 0)
        {
            KeyValuePair<PacketID, IPacket> data;
            bool success = _callBackQueue.TryDequeue(out data);

            if (!success) break;

            if (_instance._onRecievePacket.ContainsKey(data.Key))
            {
                _instance._onRecievePacket[data.Key]?.Invoke(data.Value);
            }
        }
    }

    public static void Send(IPacket packet)
    {
        ArraySegment<byte> data = packet.Write();
        ServerSession.Instance.Send(data);
    }

    public static void RecieveData(PacketID packetID, ArraySegment<byte> buffer)
    {
        IPacket packet = PacketFactory.GeneratePacket(packetID, buffer);

        _instance._callBackQueue.Enqueue(new(packetID, packet));
    }

    public static void RegistRecievePacketCallBack(PacketID packetId, Action<IPacket> callBack)
    {
        if (_instance._onRecievePacket.ContainsKey(packetId))
        {
            _instance._onRecievePacket[packetId] += callBack;
        }
        else
        {
            _instance._onRecievePacket[packetId] = callBack;
        }
    }

    public static bool UnregistRecievePacketCallBack(PacketID packetId, Action<IPacket> callBack)
    {
        bool isContained = _instance._onRecievePacket.ContainsKey(packetId);
        if (!isContained) return false;

        _instance._onRecievePacket[packetId] -= callBack;
        return true;
    }

#warning 서버 연결 실패시 예외처리 필요
    public static void ConnectServerAsync()
    {
        static void Logic()
        {
            //DNS (Domain Name System)
            string host = Dns.GetHostName();
            IPHostEntry ipHost = Dns.GetHostEntry(host);
            IPAddress ipAddr = ipHost.AddressList[0];
            IPEndPoint endPoint = new IPEndPoint(ipAddr, 7777);

            Connector connector = new Connector();

            connector.Connect(endPoint, () => { return new ServerSession(); });
        }

        Task t = new Task(Logic);
        t.Start();
    }

    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    private static void Init()
    {
        GameObject g = new GameObject(nameof(NetworkManager));
        DontDestroyOnLoad(g);
        _instance = g.AddComponent<NetworkManager>();
    }
}