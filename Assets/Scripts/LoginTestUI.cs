using Shared.Packets;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LoginTestUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _logText;
    [SerializeField] private TMP_InputField _loginIdField;
    [SerializeField] private TMP_InputField _passwordField;
    [SerializeField] private TMP_InputField _nickNameField;
    [SerializeField] private Button _idCheckButton;
    [SerializeField] private Button _loginButton;
    [SerializeField] private Button _logoutButton;
    [SerializeField] private Button _SignInButton;

    private void Awake()
    {
        Shared.Logger.OnAddLogData += Log;
        NetworkManager.RegistRecievePacketCallBack(PacketID.S_CheckIdAvailableRes, OnRecieve_S_CheckIdAvailableRes);
        NetworkManager.RegistRecievePacketCallBack(PacketID.S_SignInRes, OnRecieve_S_SignInRes);
        NetworkManager.RegistRecievePacketCallBack(PacketID.S_LoginRes, OnRecieve_S_LoginRes);

        _idCheckButton.onClick.AddListener(OnClickIdCheckButton);
        _loginButton.onClick.AddListener(OnClickLoginButton);
        _logoutButton.onClick.AddListener(OnClickLogoutButton);
        _SignInButton.onClick.AddListener(OnClickSignInButton);
    }

    private void Start()
    {
        Log($"{nameof(LoginTestUI)}.{nameof(Start)}() 호출됨");
        NetworkManager.ConnectServerAsync((b) => Log("서버 연결 성공 여부 : " + b.ToString()));
    }

    private void OnDestroy()
    {
        Shared.Logger.OnAddLogData -= Log;
        NetworkManager.UnregistRecievePacketCallBack(PacketID.S_CheckIdAvailableRes, OnRecieve_S_CheckIdAvailableRes);
        NetworkManager.UnregistRecievePacketCallBack(PacketID.S_SignInRes, OnRecieve_S_SignInRes);
        NetworkManager.UnregistRecievePacketCallBack(PacketID.S_LoginRes, OnRecieve_S_LoginRes);
    }

    public void Log(string s)
    {
        Debug.Log(s);
        _logText.text = s;
    }

    private void OnClickIdCheckButton()
    {
        string id = _loginIdField.text;

        NetworkManager.Send(new C_CheckIdAvailableReq(id));
    }

    private void OnClickLoginButton()
    {
        string id = _loginIdField.text;
        string password = _passwordField.text;

        NetworkManager.Send(new C_LoginReq(id, password.GetHashCode()));
    }

    private void OnClickLogoutButton()
    {
        NetworkManager.Send(new C_LogoutReq());
    }

    private void OnClickSignInButton()
    {
        string id = _loginIdField.text;
        string password = _passwordField.text;
        string nickName = _nickNameField.text;

        NetworkManager.Send(new C_SignInReq(id, password.GetHashCode(), nickName));
    }

    private void OnRecieve_S_CheckIdAvailableRes(IPacket p)
    {
        S_CheckIdAvailableRes res = (S_CheckIdAvailableRes)p;

        Log($"id 유효성 여부 : {res.isIdAvailable}");
    }

    private void OnRecieve_S_SignInRes(IPacket p)
    {
        S_SignInRes res = (S_SignInRes)p;

        Log($"회원가입 성공 여부 : {res.isSuccess}");
    }

    private void OnRecieve_S_LoginRes(IPacket p)
    {
        S_LoginRes res = (S_LoginRes)p;
        Log($"로그인 성공 여부 : {res.isSuccess}\n닉네임 : {res.nickName}\nuserId : {res.uid}");
    }

    private void OnApplicationQuit()
    {
        NetworkManager.Send(new C_LogoutReq());
    }
}
