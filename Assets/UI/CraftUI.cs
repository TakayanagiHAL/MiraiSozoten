using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CraftUI : MonoBehaviour
{
    [SerializeField] Player player;

    // 回収量の計算のために必要
    float craneGetPower;
    float mouseGetPower;

    // Start is called before the first frame update
    void Start()
    {
        if (player == null)
        {
            Debug.Log("MainPlayer NULL");
        }
        
        Init();
    }

    // Update is called once per frame
    void Update()
    {

    }

    /* ==========ディーゼルエンジンの強化========== */
    public void DieselEngineUpgrade(SeaResource resource, Paramater param,int maxLevel)
    {
        if (player.dieselEngine < maxLevel)
        {
            // 強化レベルアップ
            player.dieselEngine++;

            //資源処理
            player.seaResource = resource - player.seaResource;


            //パラメータ処理
            player.speed += param.Speed;

            Debug.Log("エンジン強化");
        }
    }

    /* ==========船体の強化========== */
    public void ShipBodyUpgrade(SeaResource resource, Paramater param, int maxLevel)
    {
        if (player.shipBody < maxLevel)
        {
            // 強化レベルアップ
            player.shipBody++;

            //資源処理
            player.seaResource = resource - player.seaResource;

            //パラメータ処理
            // スピード
            player.speed += param.Speed;

            // 積載量
            player.resourceStack += param.Lading;

            // 装甲厚
            player.shipArmer += param.Armer;

            Debug.Log("船体強化");
        }
    }

    /* ==========SWWマウスの強化========== */
    public void WhaleMouseUpgrade(SeaResource resource, Paramater param, int maxLevel)
    {

        if (player.whaleMouse < maxLevel)
        {
            // 強化レベルアップ
            player.whaleMouse++;

            //資源処理
            player.seaResource = resource - player.seaResource;

            //パラメータ処理
            // スピード
            player.speed += param.Speed;

            // 回収量(100*(クレーンの回収力*マウスの回収力))       
            if (param.GetPower > 0.0f)
            {
                player.getPower = (int)(100.0f * (craneGetPower * param.GetPower));
                mouseGetPower = param.GetPower;
            }                

            Debug.Log("引き揚げ量強化");
        }
    }

    /* ==========クレーンの強化========== */
    public void CraneUpgrade(SeaResource resource, Paramater param, int maxLevel)
    {

        if (player.crane < maxLevel)
        {
            // 強化レベルアップ
            player.crane++;

            //資源処理
            player.seaResource = resource - player.seaResource;

            //パラメータ処理
            if (param.GetPower > 0.0f)
            {
                player.getPower = (int)(100.0f * (mouseGetPower * param.GetPower));
                craneGetPower = param.GetPower;
            }            

            Debug.Log("クレーン強化");
        }
    }

    /* ==========レーダーの強化========== */
    public void SonarUpgrade(SeaResource resource, Paramater param, int maxLevel)
    {

        if (player.sonar < maxLevel)
        {
            // 強化レベルアップ
            player.sonar++;

            //資源処理
            player.seaResource = resource - player.seaResource;

            //パラメータ処理
            player.searchPower += param.SearchPower;

            Debug.Log("ソナー強化");
        }
    }

    // クラフトが始まる時に呼ぶ初期化処理
    void Init()
    {
        // スピードのステータス表示
        //NowSpeedText.text = player.speed.ToString();

        //float afterSpeedStatus = player.speed + DieselPalamUpList[player.dieselEngine];
        //AfterSpeedText.text = afterSpeedStatus.ToString();

        // 積載量のステータス表示
        //NowLadingText.text = player.resourceStack.ToString();

        //float afterStackText = player.resourceStack + BodyPalamUpList[player.shipBody].y;
        //AfterLadingText.text = afterStackText.ToString();

        // 回収量のステータス表示
        //NowSalvageText.text = player.getPower.ToString();

        //float afterSalvageText = player.getPower + CranePalamUpList[player.crane];
        //AfterSalvageText.text = afterSalvageText.ToString();

        // 探知力のステータス表示
        //NowRaderText.text = player.searchPower.ToString();

        //float AfterText = player.searchPower + SonarPalamUpList[player.sonar];
        //AfterRaderText.text = AfterText.ToString();
    }

    public void SetCraneGetower(float power)
    {
        craneGetPower = power;
    }

    public void SetMouseGetower(float power)
    {
        mouseGetPower = power;
    }
}
