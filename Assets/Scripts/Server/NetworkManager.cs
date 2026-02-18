using Shared;
using Shared.Network;
using Shared.Packets;
using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using System.Collections.Concurrent;
using UnityEngine;

//컨텐츠단에게 패킷 송수신 기능을 제공하는 어댑터
//쓰레드 안정성이 보장됨
public class NetworkManager : MonoBehaviour
{
    //싱글턴
    private static NetworkManager _instance;

    //등록된 패킷 수신 콜백
    private Dictionary<PacketID, Action<IPacket>> _onRecievePacket = new();
    //쓰레드로부터 안전한 읽기/쓰기가 가능한 큐 타입, 실행할 콜백 임시 저장용
    private ConcurrentQueue<KeyValuePair<PacketID, IPacket>> _callBackQueue = new();

    private ConcurrentQueue<Action> _onConnectCallBackQueue = new();

    //서버에 연결 되었는가?
    public static bool IsConnected => ServerSession.Instance != null;


    private void Update()
    {
        //매 프레임마다 실행할 콜백이 있는지 체크하고 실행
        FlushAndRunCallbacks();
    }

    //패킷을 전송
    public static void Send(IPacket packet)
    {
        ArraySegment<byte> data = packet.Write();
        ServerSession.Instance.Send(data);
    }

    //세션이 데이터를 넘겨주기 위해 사용하는 함수
    //다른 클래스는 사용을 지양해야함
    public static void RecieveData(PacketID packetID, ArraySegment<byte> buffer)
    {
        //수신한 패킷ID에 대응하는 패킷 구조체를 만들어 buffer의 값을 해독해 채워넣음
        IPacket packet = PacketFactory.GeneratePacket(packetID, buffer);
        //쓰레드 안정성을 위해 바로 콜백을 호출하지 않고 큐에 저장
        _instance._callBackQueue.Enqueue(new(packetID, packet));
    }

    //패킷 수신시 작동할 콜백 등록
    //콜백에서 IPacet타입 매개변수를 적절한 패킷 타입으로 형변환하여 사용 바람
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

    //등록되어있는 패킷 수신 콜백을 등록 해제함
    //등록되지 않은 콜백을 등록 해제하려고 해도 에러는 안남
    public static bool UnregistRecievePacketCallBack(PacketID packetId, Action<IPacket> callBack)
    {
        bool isContained = _instance._onRecievePacket.ContainsKey(packetId);
        if (!isContained) return false;

        _instance._onRecievePacket[packetId] -= callBack;
        return true;
    }

    //비동기적으로 서버에 연결을 시도함
#warning 서버 연결 실패시 예외처리 필요
    public static void ConnectServerAsync(Action<bool> onConnectCallBack)
    {
        static void Logic(Action<bool> onConnectCallBack)
        {
            //DNS (Domain Name System)
            string host = Dns.GetHostName();
            IPHostEntry ipHost = Dns.GetHostEntry(host);
            //IPAddress ipAddr = ipHost.AddressList[0];
            IPAddress ipAddr = IPAddress.Parse("여기에 ip 입력");
            IPEndPoint endPoint = new IPEndPoint(ipAddr, 7777);

            Connector connector = new Connector(
                (isConnected) =>
                {
                    _instance._onConnectCallBackQueue.Enqueue(() => onConnectCallBack(isConnected));
                }
                );

            connector.Connect(endPoint, () => { return new ServerSession(); });
        }

        Task t = new Task(() => Logic(onConnectCallBack));
        t.Start();
    }

    //서버와 연결 해제
    public static void DisconnectServer()
    {
        ServerSession.Instance.Disconnect();
    }

    //게임이 시작되고, 첫 씬이 로딩되기 전에 자동으로 호출되는 함수
    //NetworkManager 객체를 만들어서 씬에 배치해줌
    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    private static void Init()
    {
        GameObject g = new GameObject(nameof(NetworkManager));
        DontDestroyOnLoad(g);
        _instance = g.AddComponent<NetworkManager>();
    }

    //실행할 콜백들을 큐에서 전부 꺼내면서 실행
    private void FlushAndRunCallbacks()
    {
        while (_callBackQueue.Count > 0)
        {
            KeyValuePair<PacketID, IPacket> data;
            bool dequeueSuccess = _callBackQueue.TryDequeue(out data);

            if (!dequeueSuccess) break;

            if (_instance._onRecievePacket.ContainsKey(data.Key))
            {
                _instance._onRecievePacket[data.Key]?.Invoke(data.Value);
            }
        }

        while(_onConnectCallBackQueue.Count > 0)
        {
            Action onConnectCallBack;
            bool dequeueSuccess = _onConnectCallBackQueue.TryDequeue(out onConnectCallBack);

            if (!dequeueSuccess) break;

            onConnectCallBack?.Invoke();
        }
    }
}