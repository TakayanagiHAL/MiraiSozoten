using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SoftGear.Strix.Unity.Runtime;

public class RoomMatch : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public static bool CheckAllRoomMembersState(int desiredState)//メンバー全員の状態を確認する関数     ＜Strixのサンプルコード＞
    {
        foreach (var roomMember in StrixNetwork.instance.roomMembers)
        {
            if (!roomMember.Value.GetProperties().TryGetValue("state", out object value))
            {
                return false;
            }

            if ((int)value != desiredState)
            {
                return false;
            }
        }

        Debug.Log("CheckAllRoomMembersState OK");
        return true;
    }

    public void CheckMenberState()
    {
        bool state = CheckAllRoomMembersState(0);

        Debug.Log("state:"+state);
    }

    public void CheckChangeState()
    {
        var StrixSelf = StrixNetwork.instance.selfRoomMember;
        
        var dic = new Dictionary<string,object>();
        dic.Add("state", 1);
        StrixSelf.SetProperties(dic);//stateの上書き

        StrixSelf.GetProperties().TryGetValue("state", out object value);

      
        Debug.Log("Now State :" +value);
    }

    //*******************ここから下*******************

    private void testEnterRoom_self()//ルームに入室した際に呼び出す      新たにステートをプロパティ内に作り、そのステートで準備完了か判断する
    {
        // ルームメンバーの状態を変更します: 0は「準備中」、1は「準備完了」
        StrixNetwork.instance.SetRoomMember(
            StrixNetwork.instance.selfRoomMember.GetPrimaryKey(),//自分自身の状態を変更
            new Dictionary<string, object>() {
                { "properties", new Dictionary<string, object>()
                    {
                        { "state", 0 }
                    }
                }
            },
            args => {
                Debug.Log("SetRoomMember succeeded");
            },
            args => {
                Debug.Log("SetRoomMember failed. error = " + args.cause);
            }
        );
    }

    private void testChangeState_self()//ボタンか何かで準備完了状態に変更する関数
    {
        var dic = new Dictionary<string, object>();
        dic.Add("state", 1);
        StrixNetwork.instance.selfRoomMember.SetProperties(dic);//stateの上書き    準備完了
    }

    private void testAllCheckState_Master()//ルームマスターがメンバー全員のステートを確認する関数     割と適当
    {
        var StrixMember = StrixNetwork.instance;

        bool gameStart = CheckAllRoomMembersState(StrixMember.room.GetMemberCount());
        if (gameStart == false)
            return;

        Debug.Log("メンバー全員が準備完了");
    }
}
