using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CraftUI : MonoBehaviour
{
    Player player;

    // ステータス表示用テキスト
    Text NowSpeedText;
    Text AfterSpeedText;

    Text NowLadingText;
    Text AfterLadingText;

    Text NowSalvageText;
    Text AfterSalvageText;

    Text NowSalvageDepthText;
    Text AfterSalvageDepthText;

    Text NowRaderText;
    Text AfterRaderText;

    // DieselEngineUpgradeのアップデートのリスト
    [SerializeField] List<int> DieselPalamUpList;// = new List<int>() { 0, 2, 3, 5, 6, 7, 9, 11, 13, 15 };
    [SerializeField] List<Vector3Int> BodyPalamUpList;
    [SerializeField] List<Vector3> WhaleMousePalamUpList;
    [SerializeField] List<float> CranePalamUpList;
    [SerializeField] List<int> SonarPalamUpList;



    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.Find("MainPlayer").GetComponent<Player>();

        if (player == null)
        {
            Debug.Log("MainPlayer NULL");
        }

        GameObject Centerui = this.gameObject.transform.Find("CenterUIPanel").gameObject;
        GameObject UpdateView = Centerui.transform.Find("CraftUIPanel").gameObject.transform.Find("StatusBackground").gameObject.transform.Find("UpdateView").gameObject;

        if (UpdateView == null)
        {
            Debug.Log("UpdateView NULL");
        }

        GameObject SpeedText = UpdateView.gameObject.transform.Find("UnderLine_Speed").gameObject;
        NowSpeedText = SpeedText.transform.Find("Speed_NowStatus").GetComponent<Text>();
        AfterSpeedText = SpeedText.transform.Find("Speed_NextStatus").GetComponent<Text>();

        if (SpeedText == null)
        {
            Debug.Log("SpeedText NULL");
        }
        //NowSpeedText.text = player.speed.ToString();

        GameObject LadingText = UpdateView.gameObject.transform.Find("UnderLine_Lading").gameObject;
        NowLadingText = LadingText.transform.Find("Lading_NowStatus").GetComponent<Text>();
        AfterLadingText = LadingText.transform.Find("Lading_NextStatus").GetComponent<Text>();

        if (LadingText == null)
        {
            Debug.Log("LadingText NULL");
        }

        GameObject SalvageText = UpdateView.gameObject.transform.Find("UnderLine_Salvage").gameObject;
        NowSalvageText = SalvageText.transform.Find("Salvage_NowStatus").GetComponent<Text>();
        AfterSalvageText = SalvageText.transform.Find("Salvage_NextStatus").GetComponent<Text>();

        if (SalvageText == null)
        {
            Debug.Log("SalvageText NULL");
        }

        GameObject SalvageDepthText = UpdateView.gameObject.transform.Find("UnderLine_SalvageDepth").gameObject;
        NowSalvageDepthText = SalvageDepthText.transform.Find("SalvageDepth_NowStatus").GetComponent<Text>();
        AfterSalvageDepthText = SalvageDepthText.transform.Find("SalvageDepth_NextStatus").GetComponent<Text>();

        if (SalvageDepthText == null)
        {
            Debug.Log("SalvageDepthText NULL");
        }

        GameObject RaderText = UpdateView.gameObject.transform.Find("UnderLine_Rader").gameObject;
        NowRaderText = RaderText.transform.Find("Rader_NowStatus").GetComponent<Text>();
        AfterRaderText = RaderText.transform.Find("Rader_NextStatus").GetComponent<Text>();

        if (RaderText == null)
        {
            Debug.Log("RaderText NULL");
        }

        Init();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void DieselEngineUpgrade()
    {
        //成功時
        if (true)
        {
            if (player.dieselEngine < DieselPalamUpList.Count-1)
            {
                // 強化レベルアップ
                player.dieselEngine++;

                //資源処理

                //パラメータ処理
                player.speed += DieselPalamUpList[player.dieselEngine - 1];

                // UI表示(仮)
                NowSpeedText.text = player.speed.ToString();

                float afterSpeedStatus = player.speed + DieselPalamUpList[player.dieselEngine];
                AfterSpeedText.text = afterSpeedStatus.ToString();


                Debug.Log("エンジン強化");
            }

        }
        else//失敗時
        {

        }
    }

    public void ShipBodyUpgrade()
    {
        //成功時
        if (true)
        {
            if(player.shipBody< BodyPalamUpList.Count-1)
            {
                // 強化レベルアップ
                player.shipBody++;

                //資源処理
                //パラメータ処理
                // スピード
                player.speed += BodyPalamUpList[player.dieselEngine - 1].x;

                // 積載量
                player.resourceStack += BodyPalamUpList[player.dieselEngine - 1].y;

                // UI表示更新
                NowSpeedText.text = player.speed.ToString();

                float afterSpeedStatus = player.speed + DieselPalamUpList[player.dieselEngine];
                AfterSpeedText.text = afterSpeedStatus.ToString();

                NowLadingText.text = player.resourceStack.ToString();
                float afterStackText = player.resourceStack + BodyPalamUpList[player.dieselEngine].y;
                AfterLadingText.text = afterStackText.ToString();
            }            
        }
        else//失敗時
        {

        }
        Debug.Log("船体強化");
    }

    public void WhaleMouseUpgrade()
    {
        //成功時
        if (true)
        {
            if(player.whaleMouse< WhaleMousePalamUpList.Count-1)
            {
                // 強化レベルアップ
                player.whaleMouse++;

                //資源処理
                //パラメータ処理
                // スピード
                player.speed += (int)WhaleMousePalamUpList[player.whaleMouse - 1].x;

                // 回収量
                float power = player.getPower;
                player.getPower = (int)(power * WhaleMousePalamUpList[player.whaleMouse - 1].y);

                // UI表示(仮)
                // スピード
                NowSpeedText.text = player.speed.ToString();

                float afterSpeedStatus = player.speed + WhaleMousePalamUpList[player.whaleMouse].x;
                AfterSpeedText.text = afterSpeedStatus.ToString();

                // 回収量
                NowSalvageText.text = player.getPower.ToString();

                float AfterText = player.getPower + WhaleMousePalamUpList[player.whaleMouse].y;
                AfterSalvageText.text = AfterText.ToString();
            }            
        }
        else//失敗時
        {

        }
        Debug.Log("引き揚げ量強化");
    }

    public void CraneUpgrade()
    {
        //成功時
        if (true)
        {
            if(player.crane< CranePalamUpList.Count-1)
            {
                // 強化レベルアップ
                player.crane++;

                //資源処理
                //パラメータ処理
                float power = player.getPower;
                player.getPower = (int)(power * CranePalamUpList[player.crane - 1]);

                // UI表示更新
                NowSalvageText.text = player.getPower.ToString();

                float AfterText = player.getPower + CranePalamUpList[player.crane];
                AfterSalvageText.text = AfterText.ToString();
            }            
        }
        else//失敗時
        {

        }
        Debug.Log("クレーン強化");
    }

    public void SonarUpgrade()
    {
        //成功時
        if (true)
        {
            if(player.sonar< SonarPalamUpList.Count-1)
            {
                // 強化レベルアップ
                player.sonar++;

                //資源処理
                //パラメータ処理
                player.searchPower += SonarPalamUpList[player.sonar - 1];

                // UI表示(仮)
                NowRaderText.text = player.searchPower.ToString();

                float AfterText = player.searchPower + SonarPalamUpList[player.sonar];
                AfterRaderText.text = AfterText.ToString();
            }            
        }
        else//失敗時
        {

        }
        Debug.Log("ソナー強化");
    }

    // クラフトが始まる時に呼ぶ初期化処理
    void Init()
    {
        // スピードのステータス表示
        NowSpeedText.text = player.speed.ToString();

        float afterSpeedStatus = player.speed + DieselPalamUpList[player.dieselEngine];
        AfterSpeedText.text = afterSpeedStatus.ToString();

        // 積載量のステータス表示
        NowLadingText.text = player.resourceStack.ToString();

        float afterStackText = player.resourceStack + BodyPalamUpList[player.dieselEngine].y;
        AfterLadingText.text = afterStackText.ToString();

        // 回収量のステータス表示
        NowSalvageText.text = player.getPower.ToString();

        float afterSalvageText = player.getPower + CranePalamUpList[player.crane];
        AfterSalvageText.text = afterSalvageText.ToString();

        // 探知力のステータス表示
        NowRaderText.text = player.searchPower.ToString();

        float AfterText = player.searchPower + SonarPalamUpList[player.sonar];
        AfterRaderText.text = AfterText.ToString();
    }
}
