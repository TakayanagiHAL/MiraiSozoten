using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CraftUI_MainSubjectController : MonoBehaviour
{
    GameObject SubjectCursol;
    GameObject IconFragment;

    RectTransform CursolTransform;
    int SubjectNum;

    CraftUI craftUI;
    Player player;

    // 現在のステータス表示テキスト
    Text NowShipLevelText;

    Text NowSpeedText;
    Text NowLadingText;
    Text NowArmerText;
    Text NowSalvageText;
    Text NowRateText;
    Text NowRaderText;

    // 強化後のステータス表示テキスト
    Text AfterShipLevelText;

    Text AfterSpeedText;
    Text AfterLadingText;
    Text AfterArmerText;
    Text AfterSalvageText;
    Text AfterRateText;
    Text AfterRaderText;

    // 必要素材テキスト
    Text UsePlasticText;
    Text UseEnplaText;
    Text UseWoodText;
    Text UseSteelText;
    Text UseSeafoodText;

    // 所持素材表示テキスト
    Text HavePlasticText;
    Text HaveEnplaText;
    Text HaveWoodText;
    Text HaveSteelText;
    Text HaveSeafoodText;

    Paramater NextUpParamater;      // 強化後のパラメータを一時的に保持する変数
    SeaResource UpgradeUseResource; // 強化に必要な素材数を一時的に保持する変数

    // 回収量の計算のために必要
    float craneGetPower;
    float mouseGetPower;

    float returnabilityRate;

    // Start is called before the first frame update
    void Start()
    {
        IconFragment = this.gameObject.transform.Find("IconFragment").gameObject;
        IconFragment.SetActive(false);

        SubjectCursol = this.gameObject.transform.Find("SubjectCursol").gameObject;
        CursolTransform = SubjectCursol.GetComponent<RectTransform>();
        SubjectNum = 0;

        /* ==========Textを親オブジェクトから取得==========*/
        GameObject CraftUIPanel= this.gameObject.transform.parent.transform.parent.transform.parent.gameObject;
        GameObject StatusBackground = CraftUIPanel.transform.Find("StatusBackground").gameObject;
        GameObject UpdateView = StatusBackground.transform.Find("UpdateView").gameObject;

        // ステータス表示オブジェクトから各種Textを取得
        GameObject UnderLine_ShipLevel = UpdateView.transform.Find("UnderLine_ShipLV").gameObject;
        NowShipLevelText = UnderLine_ShipLevel.transform.Find("Ship_NowLevel").gameObject.GetComponent<Text>();
        AfterShipLevelText = UnderLine_ShipLevel.transform.Find("Ship_NextLevel").gameObject.GetComponent<Text>();

        GameObject UnderLine_Speed = UpdateView.transform.Find("UnderLine_Speed").gameObject;
        NowSpeedText = UnderLine_Speed.transform.Find("Speed_NowStatus").gameObject.GetComponent<Text>();
        AfterSpeedText = UnderLine_Speed.transform.Find("Speed_NextStatus").gameObject.GetComponent<Text>();

        GameObject UnderLine_Lading = UpdateView.transform.Find("UnderLine_Lading").gameObject;
        NowLadingText = UnderLine_Lading.transform.Find("Lading_NowStatus").gameObject.GetComponent<Text>();
        AfterLadingText = UnderLine_Lading.transform.Find("Lading_NextStatus").gameObject.GetComponent<Text>();

        GameObject UnderLine_Armer = UpdateView.transform.Find("UnderLine_Armer").gameObject;
        NowArmerText = UnderLine_Armer.transform.Find("Armer_NowStatus").gameObject.GetComponent<Text>();
        AfterArmerText = UnderLine_Armer.transform.Find("Armer_NextStatus").gameObject.GetComponent<Text>();

        GameObject UnderLine_Salvage = UpdateView.transform.Find("UnderLine_Salvage").gameObject;
        NowSalvageText = UnderLine_Salvage.transform.Find("Salvage_NowStatus").gameObject.GetComponent<Text>();
        AfterSalvageText = UnderLine_Salvage.transform.Find("Salvage_NextStatus").gameObject.GetComponent<Text>();

        // 還元率テキスト
        GameObject UnderLine_Rate = UpdateView.transform.Find("UnderLine_Rate").gameObject;
        NowRateText = UnderLine_Rate.transform.Find("Rate_NowStatus").gameObject.GetComponent<Text>();
        AfterRateText = UnderLine_Rate.transform.Find("Rate_NextStatus").gameObject.GetComponent<Text>();

        GameObject UnderLine_Rader = UpdateView.transform.Find("UnderLine_Rader").gameObject;
        NowRaderText = UnderLine_Rader.transform.Find("Rader_NowStatus").gameObject.GetComponent<Text>();
        AfterRaderText = UnderLine_Rader.transform.Find("Rader_NextStatus").gameObject.GetComponent<Text>();

        // 素材テキスト表示オブジェクトから各種Textを取得
        GameObject ItemBackground = StatusBackground.transform.Find("ItemBackground").gameObject;

        GameObject UnderLine_Plastic= ItemBackground.transform.Find("UnderLine_Plastic").gameObject;
        UsePlasticText = UnderLine_Plastic.transform.Find("Plastic_NeedNum").gameObject.GetComponent<Text>();
        HavePlasticText = UnderLine_Plastic.transform.Find("Plastic_HaveNum").gameObject.GetComponent<Text>();

        GameObject UnderLine_EnPla = ItemBackground.transform.Find("UnderLine_EnPla").gameObject;
        UseEnplaText = UnderLine_EnPla.transform.Find("EnPla_NeedNum").gameObject.GetComponent<Text>();
        HaveEnplaText = UnderLine_EnPla.transform.Find("EnPla_HaveNum").gameObject.GetComponent<Text>();

        GameObject UnderLine_Wood = ItemBackground.transform.Find("UnderLine_Wood").gameObject;
        UseWoodText = UnderLine_Wood.transform.Find("Wood_NeedNum").gameObject.GetComponent<Text>();
        HaveWoodText = UnderLine_Wood.transform.Find("Wood_HaveNum").gameObject.GetComponent<Text>();

        GameObject UnderLine_Steel = ItemBackground.transform.Find("UnderLine_Steel").gameObject;
        UseSteelText = UnderLine_Steel.transform.Find("Steel_NeedNum").gameObject.GetComponent<Text>();
        HaveSteelText = UnderLine_Steel.transform.Find("Steel_HaveNum").gameObject.GetComponent<Text>();

        GameObject UnderLine_Seefood = ItemBackground.transform.Find("UnderLine_Seefood").gameObject;
        UseSeafoodText = UnderLine_Seefood.transform.Find("Seafood_NeedNum").gameObject.GetComponent<Text>();
        HaveSeafoodText = UnderLine_Seefood.transform.Find("Seafood_HaveNum").gameObject.GetComponent<Text>();


        // craftUIとplayerを取得
        craftUI = CraftUIPanel.transform.parent.transform.parent.gameObject.GetComponent<CraftUI>();

        player = craftUI.GetPlayer();


        // 所持素材表示テキストにテキストを入力
        HavePlasticText.text = player.seaResource.plastic.ToString();
        HaveEnplaText.text = player.seaResource.ePlastic.ToString();
        HaveWoodText.text = player.seaResource.wood.ToString();
        HaveSteelText.text = player.seaResource.steel.ToString();
        HaveSeafoodText.text = player.seaResource.seaFood.ToString();

        returnabilityRate = 1.0f;

        craneGetPower = craftUI.GetCraneGetPower();
        mouseGetPower = craftUI.GetMouseGetPower();
        
        NextUpParamater = new Paramater();
        UpgradeUseResource = new SeaResource();
    }

    // Update is called once per frame
    void Update()
    {        
        // 上下入力で項目を選ぶ
        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            SubjectNum++;
            if (SubjectNum > 4)
            {
                SubjectNum = 4;
            }

            if (SubjectNum == 3)
            {
                IconFragment.SetActive(true);
            }
            else if (SubjectNum != 3)
            {
                IconFragment.SetActive(false);
            }
            
            CursolPositionCalculation();            
        }
        else if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            SubjectNum--;
            if (SubjectNum < 0)
            {
                SubjectNum = 0;
            }

            if (SubjectNum == 3)
            {
                IconFragment.SetActive(true);
            }
            else if (SubjectNum != 3)
            {
                IconFragment.SetActive(false);
            }
            
            CursolPositionCalculation();
        }

        // Enterキーで強化
        if (Input.GetKeyDown(KeyCode.Return))
        {
            // 素材足りてるか判定
            if (SubjectNum == 0)
            {
                craftUI.DieselEngineUpgrade();
            }
            else if (SubjectNum == 1)
            {
                craftUI.ShipBodyUpgrade();
            }
            else if (SubjectNum == 2)
            {
                //AddRate(UpgradePalamater[ContenaNum].Rate);

                craftUI.WhaleMouseUpgrade();
            }
            else if (SubjectNum == 3)
            {
                craftUI.CraneUpgrade();
            }
            else if (SubjectNum == 4)
            {
                craftUI.SonarUpgrade();
            }

            TextUpdate();
        }
    }
    
    // カーソルのポジションを計算してセットする
    void CursolPositionCalculation()
    {
        // y=-5+160*x
        CursolTransform.anchoredPosition = new Vector2(-45.0f, -5.0f + 160.0f * (2 - SubjectNum));
    }

    public int GetSubjectNum()
    {
        return SubjectNum;
    }

    // テキスト内容を更新する
    public void TextUpdate()
    {
        NextUpParamater = craftUI.GetNextParamater(SubjectNum);
        UpgradeUseResource = craftUI.GetNextUseResource(SubjectNum);

        craneGetPower = craftUI.GetCraneGetPower();
        mouseGetPower = craftUI.GetMouseGetPower();

        SetPlayerText(NextUpParamater);
    }

    /* ===========CraftUI_SubjectIconControllerからパラメータと素材数を取得=========== */
    public void SetPlayerText(Paramater upParamater)
    {
        // ステータス表示テキストを更新
        NowShipLevelText.text = player.shipLevel.ToString();
        AfterShipLevelText.text = (player.shipLevel + 1).ToString();

        NowSpeedText.text = player.speed.ToString();
        int speed = NextUpParamater.Speed + player.speed;
        AfterSpeedText.text = speed.ToString();

        NowLadingText.text = player.resourceStack.ToString();
        int lading = NextUpParamater.Lading + player.resourceStack;
        AfterLadingText.text = lading.ToString();

        NowArmerText.text = player.shipArmer.ToString();
        AfterArmerText.text = (player.shipArmer + upParamater.Armer).ToString();

        // 回収量テキスト(クレーン&マウスかそれ以外かで処理を分ける)
        NowSalvageText.text = player.getPower.ToString();
        if (SubjectNum== 2)
        {
            if (upParamater.GetPower > 0.0f)
            {
                AfterSalvageText.text = ((int)(100.0f * (upParamater.GetPower * craneGetPower))).ToString();
            }
            else
            {
                AfterSalvageText.text = player.getPower.ToString();
            }
        }
        else if(SubjectNum == 3)
        {
            if (upParamater.GetPower > 0.0f)
            {
                AfterSalvageText.text = ((int)(100.0f * (upParamater.GetPower * mouseGetPower))).ToString();
            }
            else
            {
                AfterSalvageText.text = player.getPower.ToString();
            }
        }
        else
        {
            AfterSalvageText.text= player.getPower.ToString();
        }

        // 還元率テキスト
        NowRateText.text = (returnabilityRate * 100.0f).ToString() + "%";
        AfterRateText.text = ((returnabilityRate + upParamater.Rate) * 100.0f).ToString() + "%";


        // レーダーステータステキスト
        NowRaderText.text = player.searchPower.ToString();
        int rader = upParamater.SearchPower + player.searchPower;
        AfterRaderText.text = rader.ToString();

        // 所持素材テキストを更新
        HavePlasticText.text = player.seaResource.plastic.ToString();
        HaveEnplaText.text = player.seaResource.ePlastic.ToString();
        HaveWoodText.text = player.seaResource.wood.ToString();
        HaveSteelText.text = player.seaResource.steel.ToString();
        HaveSeafoodText.text = player.seaResource.seaFood.ToString();

        // 必要素材テキストに数値を入力
        UsePlasticText.text = UpgradeUseResource.plastic.ToString();
        UseEnplaText.text = UpgradeUseResource.ePlastic.ToString();
        UseWoodText.text = UpgradeUseResource.wood.ToString();
        UseSteelText.text = UpgradeUseResource.steel.ToString();
        UseSeafoodText.text = UpgradeUseResource.seaFood.ToString();
    }
    
    // マウス強化時に呼ぶ
    public void AddRate(float rate)
    {
        returnabilityRate += rate;
    }
}
