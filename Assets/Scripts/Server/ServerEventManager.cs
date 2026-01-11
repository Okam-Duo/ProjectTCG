using Shared;
using Shared.Network;
using Shared.Packets;
using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;

#warning 클라이언트 개발용 임시 어댑터 클래스, 추후 기능 연결 필요
//추가 데이터는 전부 long타입으로 때워놨으니 나중에 변경하거나 로직 추가 필요
//커플링을 신경쓸 필요가 없다고 판단될 시 패킷 수신 이벤트를 콜백에서 함수로 변경 바람


//패킷 송수신 어댑터
public class ServerEventManager
{
    //싱글턴
    private static ServerEventManager _instance = new ServerEventManager();

    private Dictionary<PacketID, Action<IPacket>> _onRecievePacket = new();

    private ServerEventManager() { }

    public static void Send(IPacket packet)
    {
        ArraySegment<byte> data = packet.Write();
        ServerSession.Instance.Send(data);
    }

    public static void RecieveData(PacketID packetID, ArraySegment<byte> buffer)
    {
        IPacket packet = PacketFactory.GeneratePacket(packetID, buffer);

        if (_instance._onRecievePacket.ContainsKey(packetID))
        {
            _instance._onRecievePacket[packetID]?.Invoke(packet);
        }
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
}