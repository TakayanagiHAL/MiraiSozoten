using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public struct Paramater
{
    public static Paramater operator +(Paramater a, Paramater b)
    {
        b.Speed += a.Speed;
        b.Lading += a.Lading;
        b.Armer += a.Armer;
        b.GetPower += a.GetPower;
        b.Rate += a.Rate;
        b.SearchPower += a.SearchPower;

        return b;
    }

    public static Paramater operator -(Paramater a, Paramater b)
    {
        b.Speed -= a.Speed;
        b.Lading -= a.Lading;
        b.Armer -= a.Armer;
        b.GetPower -= a.GetPower;
        b.Rate -= a.Rate;
        b.SearchPower -= a.SearchPower;

        return b;
    }


    public int Speed;
    public int Lading;
    public int Armer;
    public float GetPower;
    public float Rate;
    public int SearchPower;
}


public class CraftUI : MonoBehaviour
{
    [SerializeField] Player player;

    // 上昇するパラメータリスト
    [SerializeField] List<Paramater> DeaselUpParamater;
    [SerializeField] List<Paramater> BodyUpParamater;
    [SerializeField] List<Paramater> MouseUpParamater;
    [SerializeField] List<Paramater> CraneUpParamater;
    [SerializeField] List<Paramater> RaderUpParamater;

    // 必要素材リスト
    [SerializeField] List<SeaResource> DeaselUseResource;
    [SerializeField] List<SeaResource> BodyUseResource;
    [SerializeField] List<SeaResource> MouseUseResource;
    [SerializeField] List<SeaResource> CraneUseResource;
    [SerializeField] List<SeaResource> RaderUseResource;

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


        craneGetPower = CraneUpParamater[1].GetPower;
        mouseGetPower = MouseUpParamater[1].GetPower;

        Init();
    }

    // Update is called once per frame
    void Update()
    {

    }

    /* ==========ディーゼルエンジンの強化========== */
    public void DieselEngineUpgrade()
    {
        if (player.dieselEngine < DeaselUseResource.Count)
        {
            // 素材が足りているか判定する
            if (UpgradeDecision(DeaselUseResource[player.dieselEngine]))
            {
                Debug.Log("player.seaResource:" + player.seaResource.plastic + ", " + player.seaResource.ePlastic + ", " + player.seaResource.wood + ", " + player.seaResource.steel + ", " + player.seaResource.seaFood);
                //資源処理
                player.seaResource = DeaselUseResource[player.dieselEngine] - player.seaResource;

                Debug.Log("DeaselUseResource[player.dieselEngine] :" + DeaselUseResource[player.dieselEngine].plastic + ", " + DeaselUseResource[player.dieselEngine].ePlastic 
                    + ", " + DeaselUseResource[player.dieselEngine].wood + ", " + DeaselUseResource[player.dieselEngine].steel + ", " + DeaselUseResource[player.dieselEngine].seaFood);
                Debug.Log("player.seaResource:" + player.seaResource.plastic + ", " + player.seaResource.ePlastic + ", " + player.seaResource.wood + ", " + player.seaResource.steel + ", " + player.seaResource.seaFood);

                //パラメータ処理
                player.speed += DeaselUpParamater[player.dieselEngine].Speed;

                // 強化レベルアップ
                player.dieselEngine++;

                Debug.Log("エンジン強化");
            }            
        }
    }

    /* ==========船体の強化========== */
    public void ShipBodyUpgrade()
    {
        if (player.shipBody < BodyUseResource.Count)
        {
            // 素材が足りているか判定する
            if (UpgradeDecision(BodyUseResource[player.shipBody]))
            {
                //資源処理
                player.seaResource = BodyUseResource[player.shipBody] - player.seaResource;

                //パラメータ処理
                // スピード
                player.speed += BodyUpParamater[player.shipBody].Speed;

                // 積載量
                player.resourceStack += BodyUpParamater[player.shipBody].Lading;

                // 装甲厚
                player.shipArmer += BodyUpParamater[player.shipBody].Armer;

                // 強化レベルアップ
                player.shipBody++;


                Debug.Log("船体強化");
            }           
        }
    }

    /* ==========SWWマウスの強化========== */
    public void WhaleMouseUpgrade()
    {

        if (player.whaleMouse < MouseUseResource.Count)
        {
            // 素材が足りているか判定する
            if (UpgradeDecision(MouseUseResource[player.whaleMouse]))
            {               
                //資源処理
                player.seaResource = MouseUseResource[player.dieselEngine] - player.seaResource;

                //パラメータ処理
                // スピード
                player.speed += MouseUpParamater[player.whaleMouse].Speed;

                // 回収量(100*(クレーンの回収力*マウスの回収力))       
                if (MouseUpParamater[player.whaleMouse].GetPower > 0.0f)
                {
                    player.getPower = (int)(100.0f * (craneGetPower * MouseUpParamater[player.whaleMouse].GetPower));
                    mouseGetPower = MouseUpParamater[player.whaleMouse].GetPower;
                }

                // 強化レベルアップ
                player.whaleMouse++;

                Debug.Log("引き揚げ量強化");
            }            
        }
    }

    /* ==========クレーンの強化========== */
    public void CraneUpgrade()
    {
        // 最大強化かどうかを判定する
        if (player.crane < CraneUseResource.Count)
        {
            // 素材が足りているか判定する
            if (UpgradeDecision(CraneUseResource[player.crane]))
            {               
                //資源処理
                player.seaResource = CraneUseResource[player.crane] - player.seaResource;

                //パラメータ処理
                if (CraneUpParamater[player.crane].GetPower > 0.0f)
                {
                    player.getPower = (int)(100.0f * (CraneUpParamater[player.crane].GetPower * mouseGetPower));

                    craneGetPower = CraneUpParamater[player.crane].GetPower;
                }

                // 強化レベルアップ
                player.crane++;

                Debug.Log("クレーン強化");
            }
        }
    }

    /* ==========レーダーの強化========== */
    public void SonarUpgrade()
    {

        if (player.sonar < RaderUseResource.Count)
        {
            // 素材が足りているか判定する
            if (UpgradeDecision(RaderUseResource[player.sonar]))
            {                
                //資源処理
                player.seaResource = RaderUseResource[player.sonar] - player.seaResource;

                //パラメータ処理
                player.searchPower += RaderUpParamater[player.sonar].SearchPower;

                // 強化レベルアップ
                player.sonar++;

                Debug.Log("ソナー強化");
            }
        }
    }


    /* ==========アップグレード出来るか判定する========== */
    bool UpgradeDecision(SeaResource UseResource)
    {      
        // プレイヤーが持っている素材と必要素材数を比較する
        // プラスチック
        if (player.seaResource.plastic < UseResource.plastic)
        {
            return false;
        }

        // エンプラ
        if (player.seaResource.ePlastic < UseResource.ePlastic)
        {
            return false;
        }

        // 木材
        if (player.seaResource.wood < UseResource.wood)
        {
            return false;
        }

        // 鋼材
        if (player.seaResource.steel < UseResource.steel)
        {
            return false;
        }

        // 海鮮
        if (player.seaResource.seaFood < UseResource.seaFood)
        {
            return false;
        }

        return true;
    }

    /* ==========パラメータの加算量を取得========== */
    public Paramater GetNextParamater(int cursolNum)
    {
        // カーソルがどこにあるかを判定する
        if (cursolNum == 0)
        {
            int ContenaNum = player.dieselEngine;
            if(DeaselUpParamater.Count<= player.dieselEngine)
            {
                ContenaNum = 0;
            }
            return DeaselUpParamater[ContenaNum];
        }
        else if(cursolNum == 1)
        {
            int ContenaNum = player.shipBody;
            if (BodyUpParamater.Count <= player.shipBody)
            {
                ContenaNum = 0;
            }

            return BodyUpParamater[ContenaNum];
        }
        else if(cursolNum == 2)
        {
            int ContenaNum = player.whaleMouse;
            if (MouseUpParamater.Count <= player.whaleMouse)
            {
                ContenaNum = 0;
            }

            return MouseUpParamater[ContenaNum];
        }
        else if (cursolNum == 3)
        {
            int ContenaNum = player.crane;
            if (CraneUpParamater.Count <= player.crane)
            {
                ContenaNum = 0;
            }

            return CraneUpParamater[ContenaNum];
        }
        else if(cursolNum == 4)
        {
            int ContenaNum = player.sonar;
            if (RaderUpParamater.Count <= player.sonar)
            {
                ContenaNum = 0;
            }

            return RaderUpParamater[ContenaNum];
        }

        // カーソルが０～４以外だった場合
        Paramater param;
        param.Speed = -1;
        param.Lading = -1;
        param.Armer = -1;
        param.GetPower = -1.0f;
        param.Rate = -1.0f;
        param.SearchPower = -1;

        return param;
    }

    public SeaResource GetNextUseResource(int cursolNum)
    {        
        // カーソルがどこにあるかを判定する
        if (cursolNum == 0)
        {
            int ContenaNum = player.dieselEngine;
            if (DeaselUpParamater.Count <= player.dieselEngine)
            {
                ContenaNum = 0;
            }
            return DeaselUseResource[ContenaNum];
        }
        else if (cursolNum == 1)
        {
            int ContenaNum = player.shipBody;
            if (BodyUpParamater.Count <= player.shipBody)
            {
                ContenaNum = 0;
            }

            return BodyUseResource[ContenaNum];
        }
        else if (cursolNum == 2)
        {
            int ContenaNum = player.whaleMouse;
            if (MouseUpParamater.Count <= player.whaleMouse)
            {
                ContenaNum = 0;
            }

            return MouseUseResource[ContenaNum];
        }
        else if (cursolNum == 3)
        {
            int ContenaNum = player.crane;
            if (CraneUpParamater.Count <= player.crane)
            {
                ContenaNum = 0;
            }

            return CraneUseResource[ContenaNum];
        }
        else if (cursolNum == 4)
        {
            int ContenaNum = player.sonar;
            if (RaderUpParamater.Count <= player.sonar)
            {
                ContenaNum = 0;
            }

            return RaderUseResource[ContenaNum];
        }

        // カーソルが０～４以外だった場合
        SeaResource resource;
        resource.plastic = -1;
        resource.ePlastic = -1;
        resource.wood = -1;
        resource.steel = -1;
        resource.seaFood = -1;

        return resource;
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

    // クレーンのの回収力を取得する
    public float GetCraneGetPower()
    {
        return craneGetPower;
    }

    // マウスの回収力を取得
    public float GetMouseGetPower()
    {
        return mouseGetPower;
    }

    public Player GetPlayer()
    {
        if (player != null)
        {
            return player;
        }
        else
        {
            return null;
        }
    }
}
