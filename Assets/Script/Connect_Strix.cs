using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SoftGear.Strix.Client.Core.Auth.Message;
using SoftGear.Strix.Client.Core.Error;
using SoftGear.Strix.Unity.Runtime;
using SoftGear.Strix.Net.Logging;
using SoftGear.Strix.Unity.Runtime.Event;
using UnityEngine.UI;
using UnityEngine.Events;


public class Connect_Strix : MonoBehaviour
{
    public string host = "127.0.0.1";
    public int port = 9122;
    public string applicationId = "00000000-0000-0000-0000-000000000000";
    public Level logLevel = Level.INFO;
    public UnityEvent OnConnect;

    private bool isRoomCreate;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    //接続
    public void Connect()
    {
        LogManager.Instance.Filter = logLevel;

        StrixNetwork.instance.applicationId = applicationId;
        StrixNetwork.instance.playerName = "Player Name";
        StrixNetwork.instance.ConnectMasterServer(host, port, OnConnectCallback, OnConnectFailedCallback);

    }

    //成功ハンドラー
    private void OnConnectCallback(StrixNetworkConnectEventArgs args)
    {
        OnConnect.Invoke();
    }

    //失敗ハンドラー
    private void OnConnectFailedCallback(StrixNetworkConnectFailedEventArgs args)
    {
        string error = "";

        if (args.cause != null)
        {
            error = args.cause.Message;
        }
    }
}
