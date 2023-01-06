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
        //この処理は、シーン移動直後かつ、部屋に接続されている状況であれば、他のスクリプトからでも処理可能
        StrixNetwork.instance.SetRoomMember(
          StrixNetwork.instance.selfRoomMember.GetPrimaryKey(),
          new Dictionary<string, object>(){
                {"properties",new Dictionary<string,object>(){
                    {"nowScene",2 }
                } }
              },
          args =>
          {
              Debug.Log("メンバープロパティ：シーン番号を変更しました。");
          },
          args =>
          {
              Debug.Log("メンバープロパティ：シーン番号の変更に失敗しました。error = " + args.cause);
          }
          );
    }

    // Update is called once per frame
    void Update()
    {        
        //全員がシーンジャンプできたかチェック

        //※デバッグする際、ゲームシーンに飛んでから、全クライアントのウィンドウを一瞬でもアクティブにしてないとそのクライアントのStart()が呼ばれないので注意
        if (!isCheck)
        {
            if (CheckAllMemberinGameScene(SceneNum))
            {
                Debug.Log("全プレイヤーがゲームシーンに移動しました。");
                isCheck = true;
            }
        }

        //ホストだけで（または、ルーム内で1回だけ）関数を呼びたい時は、以下のif文を使って
        //if(StrixNetwork.instance.isRoomOwner)
        //{

        //}
    }

    private bool CheckAllMemberinGameScene(int num)
    {
        //現在の全ルームメンバーを参照
        var A = StrixNetwork.instance.roomMembers;

        int membercount = 0;

        foreach (var roomMember in A)
        {
            membercount++;
            //そもそもそのプロパティがなかったら失敗
            if (!roomMember.Value.GetProperties().TryGetValue("nowScene", out object value))
            {
                Debug.Log("プロパティ「nowScene」がありませんでした。");
                return false;
            }

            if ((int)value != num)
            {
                Debug.Log(membercount+"番プレイヤーの値が" + (int)value);
                return false;
            }
        }
        return true;
    }
}