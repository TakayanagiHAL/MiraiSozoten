using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SoftGear.Strix.Unity.Runtime;

public class AllMemberSceneChecker : MonoBehaviour
{
    [SerializeField] private int SceneNum;
    private bool isCheck;

    // Start is called before the first frame update
    void Start()
    {
        isCheck = false;


        //現在のシーン番号をStrixNetwork.instanceの追加プロパティ「nowScene」に保存（前提として、ルーム作成時に追加プロパティを設定している）
        //この処理は、シーン移動直後であれば、他のスクリプトからでも処理可能
        StrixNetwork.instance.SetRoomMember(
          StrixNetwork.instance.selfRoomMember.GetPrimaryKey(),
          new Dictionary<string, object>(){
                {"properties",new Dictionary<string,object>(){
                    {"nowScene",2 }
                } }
              },
          args =>
          {
              Debug.Log("準備状態を変更しました");
          },
          args =>
          {
              Debug.Log("準備状態の変更に失敗しました。error = " + args.cause);
          }
          );

    }

    // Update is called once per frame
    void Update()
    {
        if (CheckAllMemberinGameScene(SceneNum) && !isCheck)
        {
            Debug.Log("全プレイヤーがゲームシーンに移動しました。");
            isCheck = true;
        }
    }

    private bool CheckAllMemberinGameScene(int num)
    {
        //現在の全ルームメンバーを参照

        var A = StrixNetwork.instance.roomMembers;

        int membercount = 1;

        foreach (var roomMember in A)
        {
            membercount++;
            //そもそもそのプロパティがなかったら失敗
            if (!roomMember.Value.GetProperties().TryGetValue("nowScene", out object value))
            {
                Debug.Log("プロパティ「nowScene」がありませんでした。");
                return false;

            }

            //そのプロパティが期待値じゃなかったら失敗
            if ((int)value != num)
            {
                Debug.Log("値が" + (int)value);
                return false;
            }



        }

        return true;
    }

}
