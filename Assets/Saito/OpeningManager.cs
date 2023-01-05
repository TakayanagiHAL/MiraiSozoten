using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpeningManager : MonoBehaviour
{
    [SerializeField] TextController text;

    [SerializeField] ResourceEffectUI player1UI;
    [SerializeField] ResourceEffectUI player2UI;
    [SerializeField] ResourceEffectUI player3UI;
    [SerializeField] ResourceEffectUI player4UI;

    [SerializeField] SeaResource distResource;

    int EventNum;       // テキスト終了フラグを取得した数
    bool ResourceEvent; // リソース取得イベントの通過フラグ
    bool EventClear;    // リソース取得イベントが終了して次の文章を始める

    float EfectTime;    // 次の文章を始めるまでの時間

    bool GetEnd;        // textEndフラグのお取得を1回に制限するフラグ


    // Start is called before the first frame update
    void Start()
    {
        EventNum = 0;
        ResourceEvent = false;
        EventClear = false;

        EfectTime = 0.0f;

        GetEnd = false;
    }

    // Update is called once per frame
    void Update()
    {
        // テキスト終了フラグが立ったらEventNumを加算
        if (text.GetTextEnd() == true)
        {
            if (GetEnd == false)
            {
                EventNum++;
                Debug.Log("EventNum:" + EventNum);

                GetEnd = true;
            }            
        }

        // 最初の文章が終了し、資源獲得演出が入る
        if (EventNum == 1)
        {
            if (ResourceEvent == false)
            {
                // リソース取得エフェクトを動かす
                player1UI.SetAction(distResource);
                player2UI.SetAction(distResource);
                player3UI.SetAction(distResource);
                player4UI.SetAction(distResource);

                ResourceEvent = true;
            }


            if (EfectTime > 1.0f)
            {

                if (EventClear == false)
                {
                    text.SetText("それではいよいよスタートです！。他の船長に負けないように、頑張りましょうね！");
                    EventClear = true;

                    Debug.Log("EventClear:" + EventClear);

                    GetEnd = false;
                }
            }
            else
            {
                EfectTime += Time.deltaTime;
            }

        }

        // テキストが終了し、シーンが変わる
        if (EventNum == 2)
        {
            Debug.Log("シーンチェンジ");
            OpeningEnd();
        }
    }

    // シーン変更用の関数
    void OpeningEnd()
    {

    }
}
