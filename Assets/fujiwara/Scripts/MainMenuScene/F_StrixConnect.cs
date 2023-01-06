using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

using System.Linq;

using SoftGear.Strix.Client.Core.Auth.Message;
using SoftGear.Strix.Client.Core.Error;
using SoftGear.Strix.Client.Core.Model.Manager.Filter.Builder;
using SoftGear.Strix.Unity.Runtime;
using SoftGear.Strix.Net.Logging;
using SoftGear.Strix.Unity.Runtime.Event;


public class F_StrixConnect : MonoBehaviour
{
    public string host = "127.0.0.1";
    public int port = 9122;
    public string applicationId = "00000000-0000-0000-0000-000000000000";
    public Level logLevel = Level.INFO;
    public UnityEvent OnConnect;
    public string playerName = "プレイヤー";

    private bool isRoomCreate;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //サーバー接続開始
    public void Connect(bool mode)
    {
        LogManager.Instance.Filter = logLevel;

        StrixNetwork.instance.applicationId = applicationId;
        StrixNetwork.instance.playerName = playerName;
        StrixNetwork.instance.ConnectMasterServer(host, port, OnConnectCallback, OnConnectFailedCallback);

        isRoomCreate = mode;
    }

    //成功ハンドラー
    private void OnConnectCallback(StrixNetworkConnectEventArgs args)
    {

        OnConnect.Invoke();

        //連続して押させないようにUIを非表示
        //gameObject.SetActive(false);
        Debug.Log("接続に成功しました");



        //部屋の作成、検索で処理が分かれる
        if (isRoomCreate)
        {
            Debug.Log("部屋を作成します");
            GetComponent<F_StrixEntreRoom>().CreateRoom();
        }
        else
        {

            Debug.Log("部屋を検索します");
            //ここで部屋検索を呼ばなければならない

            double a = GetComponent<MainMenuManager>().GetRoomID();
            RoomAccessWithID(a);
        }
    }

    //失敗ハンドラー
    private void OnConnectFailedCallback(StrixNetworkConnectFailedEventArgs args)
    {
        string error = "接続できませんでした";

        if (args.cause != null)
        {
            error = args.cause.Message;
        }
    }



    //IDを持って部屋にアクセスする
    public void RoomAccessWithID(double roomid)
    {
        var strixNetwork = StrixNetwork.instance;

        Debug.Log("検索部屋番号" + roomid);


        //※ルームのカスタムプロパティ（key1～8）はdouble型なので、検索対象の変数はdouble型でないといけない
        //部屋の検索
        strixNetwork.SearchJoinableRoom(
                           condition: ConditionBuilder.Builder().Field("key3").EqualTo(roomid).Build(),  //key3(ルーム番号)がroomidと一致する部屋のみ出力
                            order: null,
                           limit: 1,                                                                            // 結果を10件のみ取得します
                           offset: 0,                                                                            // 結果を先頭から取得します
                           handler: searchResults => {


                               //ヒットしたルーム情報がリストとして返される
                               //※どの部屋がヒットしなかったとしてもこの成功ハンドラーが呼ばれるが、roomInfoCollectionは空である

                               
                               var foundRooms = searchResults.roomInfoCollection;

                               if(foundRooms.Count>0)
                               {
                                   var roomInfo = foundRooms.First();

                                   GetComponent<F_StrixEntreRoom>().IDHitRoom(roomInfo);
                               }
                               else
                               {
                                   Debug.Log("部屋をサーチできませんでした");
                                   GameObject.Find("UI_Canvas").GetComponent<MenuSceneUI>().RoomAccessError();

                               }

                              
                               
                           },
                           failureHandler: searchError => Debug.LogError("部屋をサーチできませんでした"));
    }
}
