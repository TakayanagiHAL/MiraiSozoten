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
    [SerializeField] CraftUI craftUI;
    [SerializeField] Player player;

    // ステータステキスト表示オブジェクト
    [SerializeField] GameObject SpeedTextUnit;
    [SerializeField] GameObject LasingTextUnit;
    [SerializeField] GameObject ArmerTextUnit;
    [SerializeField] GameObject SalvageTextUnit;
    [SerializeField] GameObject RateTextUnit;
    [SerializeField] GameObject RaderTextUnit;

    // 現在のステータス表示テキスト
    Text NowSpeedText;
    Text NowLadingText;
    Text NowArmerText;
    Text NowSalvageText;
    Text NowRateText;
    Text NowRaderText;

    // 強化後のステータス表示テキスト
    Text AfterSpeedText;
    Text AfterLadingText;
    Text AfterArmerText;
    Text AfterSalvageText;
    Text AfterRateText;
    Text AfterRaderText;

    // 素材テキスト表示オブジェクト
    [SerializeField] GameObject PlasticTextUnit;
    [SerializeField] GameObject EnplaTextUnit;
    [SerializeField] GameObject WoodTextUnit;
    [SerializeField] GameObject SteelTextUnit;
    [SerializeField] GameObject SeaFoodTextUnit;

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

    CraftUI_SubjectIconController NowSubject;

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

        // ステータス表示オブジェクトから各種Textを取得
        NowSpeedText = SpeedTextUnit.transform.Find("Speed_NowStatus").gameObject.GetComponent<Text>();
        AfterSpeedText = SpeedTextUnit.transform.Find("Speed_NextStatus").gameObject.GetComponent<Text>();

        NowLadingText = LasingTextUnit.transform.Find("Lading_NowStatus").gameObject.GetComponent<Text>();
        AfterLadingText = LasingTextUnit.transform.Find("Lading_NextStatus").gameObject.GetComponent<Text>();

        NowArmerText = ArmerTextUnit.transform.Find("Armer_NowStatus").gameObject.GetComponent<Text>();
        AfterArmerText = ArmerTextUnit.transform.Find("Armer_NextStatus").gameObject.GetComponent<Text>();

        NowSalvageText = SalvageTextUnit.transform.Find("Salvage_NowStatus").gameObject.GetComponent<Text>();
        AfterSalvageText = SalvageTextUnit.transform.Find("Salvage_NextStatus").gameObject.GetComponent<Text>();

        // 還元率テキスト
        NowRateText = RateTextUnit.transform.Find("Rate_NowStatus").gameObject.GetComponent<Text>();
        AfterRateText = RateTextUnit.transform.Find("Rate_NextStatus").gameObject.GetComponent<Text>();

        NowRaderText = RaderTextUnit.transform.Find("Rader_NowStatus").gameObject.GetComponent<Text>();
        AfterRaderText = RaderTextUnit.transform.Find("Rader_NextStatus").gameObject.GetComponent<Text>();

        // 素材テキスト表示オブジェクトから各種Textを取得
        UsePlasticText = PlasticTextUnit.transform.Find("Plastic_NeedNum").gameObject.GetComponent<Text>();
        HavePlasticText = PlasticTextUnit.transform.Find("Plastic_HaveNum").gameObject.GetComponent<Text>();

        UseEnplaText = EnplaTextUnit.transform.Find("EnPla_NeedNum").gameObject.GetComponent<Text>();
        HaveEnplaText = EnplaTextUnit.transform.Find("EnPla_HaveNum").gameObject.GetComponent<Text>();

        UseWoodText = WoodTextUnit.transform.Find("Wood_NeedNum").gameObject.GetComponent<Text>();
        HaveWoodText = WoodTextUnit.transform.Find("Wood_HaveNum").gameObject.GetComponent<Text>();

        UseSteelText = SteelTextUnit.transform.Find("Steel_NeedNum").gameObject.GetComponent<Text>();
        HaveSteelText = SteelTextUnit.transform.Find("Steel_HaveNum").gameObject.GetComponent<Text>();

        UseSeafoodText = SeaFoodTextUnit.transform.Find("Seafood_NeedNum").gameObject.GetComponent<Text>();
        HaveSeafoodText = SeaFoodTextUnit.transform.Find("Seafood_HaveNum").gameObject.GetComponent<Text>();

        // 所持素材表示テキストにテキストを入力
        HavePlasticText.text = player.seaResource.plastic.ToString();
        HaveEnplaText.text = player.seaResource.ePlastic.ToString();
        HaveWoodText.text = player.seaResource.wood.ToString();
        HaveSteelText.text = player.seaResource.steel.ToString();
        HaveSeafoodText.text = player.seaResource.seaFood.ToString();

        returnabilityRate = 1.0f;
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

    public Player GetPlayer()
    {
        return player;
    }

    /* ===========CraftUI_SubjectIconControllerからパラメータと素材数を取得=========== */
    public void SetPlayerText(SeaResource resource, Paramater upParamater, UpgradeSubject subject)
    {
        // ステータス表示テキストを更新
        NowSpeedText.text = player.speed.ToString();
        int speed = upParamater.Speed + player.speed;
        AfterSpeedText.text = speed.ToString();

        NowLadingText.text = player.resourceStack.ToString();
        int lading = upParamater.Lading + player.resourceStack;
        AfterLadingText.text = lading.ToString();

        NowArmerText.text = player.shipArmer.ToString();
        //int lading = upParamater.Lading + player.resourceStack;
        AfterArmerText.text = (player.shipArmer + upParamater.Armer).ToString(); //lading.ToString();

        // 回収量テキスト(クレーン&マウスかそれ以外かで処理を分ける)
        NowSalvageText.text = player.getPower.ToString();
        if (subject== UpgradeSubject.SUBJECT_WHALEMOUSE)
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
        else if(subject == UpgradeSubject.SUBJECT_CRANE)
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
        UsePlasticText.text = resource.plastic.ToString();
        UseEnplaText.text = resource.ePlastic.ToString();
        UseWoodText.text = resource.wood.ToString();
        UseSteelText.text = resource.steel.ToString();
        UseSeafoodText.text = resource.seaFood.ToString();
    }

    // マウスかクレーンの強化の時だけ追加で呼ぶ
    public void SetGetPowerText(float state, UpgradeSubject subject)
    {
        if (subject == UpgradeSubject.SUBJECT_WHALEMOUSE)
        {
            mouseGetPower = state;
        }
        else if(subject == UpgradeSubject.SUBJECT_CRANE)
        {            
            craneGetPower = state;
        }
    }

    // マウス強化時に呼ぶ
    public void AddRate(float rate)
    {
        returnabilityRate += rate;
    }

    public void SetCraneGetPower(float power)
    {
        if (power > 0.0f)
        {
            craneGetPower = power;
            craftUI.SetCraneGetower(power);
        }
    }

    public void SetMouseGetPower(float power)
    {
        if (power > 0.0f)
        {
            mouseGetPower = power;
            craftUI.SetMouseGetower(power);
        }
    }
}
