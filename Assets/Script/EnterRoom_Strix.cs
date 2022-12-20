using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SoftGear.Strix.Unity.Runtime;
using UnityEngine.Events;

public class EnterRoom_Strix : MonoBehaviour
{
    /// <summary>
    /// ルームに参加可能な最大人数
    /// </summary>
    public int capacity = 4;

    /// <summary>
    /// ルーム名
    /// </summary>
    public string roomName = "New Room";

    /// <summary>
    /// ルーム入室完了時イベント
    /// </summary>
    public UnityEvent onRoomEntered;

    /// <summary>
    /// ルーム入室失敗時イベント
    /// </summary>
    public UnityEvent onRoomEnterFailed;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    //部屋にエントリー
    public void EnterRoom()
    {
        StrixNetwork.instance.JoinRandomRoom(StrixNetwork.instance.playerName,
            args => {
                onRoomEntered.Invoke();
            }, args => {
                CreateRoom();
            });
    }


    public void CreateRoom()
    {
        RoomProperties roomProperties = new RoomProperties
        {
            name = roomName,
            capacity = 4,
            key1 = 0
        };

        RoomMemberProperties memberProperties = new RoomMemberProperties
        {
            name = StrixNetwork.instance.playerName,
            properties = new Dictionary<string, object>(){
                    {"nowScene",0 }
                }
        };

        StrixNetwork.instance.CreateRoom(
          roomProperties,
           memberProperties,
            args =>
            {
                onRoomEntered.Invoke();
                Debug.Log("部屋の作成に成功しました。");
            },
            args =>
            {
                Debug.Log("部屋の作成に失敗しました。error=" + args.cause);
                onRoomEnterFailed.Invoke();
            }

            );


    }

    public void RoomStatusInit()
    {
        StrixNetwork.instance.SetRoomMember(
          StrixNetwork.instance.selfRoomMember.GetPrimaryKey(),
          new Dictionary<string, object>(){
                {"properties",new Dictionary<string,object>(){
                    {"nowScene",1 },
                    {"state",0 }
                } }
              },
          args =>
          {
          },
          args =>
          {
          }
          );
    }

}
