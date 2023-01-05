using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public enum UpgradeSubject
{
    SUBJECT_DIEZELENGINE = 0,
    SUBJECT_SHIPBODY,
    SUBJECT_WHALEMOUSE,
    SUBJECT_CRANE,
    SUBJECT_RADER
};

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


public class CraftUI_SubjectIconController : MonoBehaviour
{
    Player player;

    // ステータスアップグレード用パラメータリスト 
    [SerializeField] List<Paramater> UpgradePalamater;
    [SerializeField] List<SeaResource> UseResourceList;
    [SerializeField] UpgradeSubject IconNum;

    CraftUI craftUI;
    CraftUI_MainSubjectController MSC;

    int ContenaNum;

    bool CursolNotice;// カーソルが当たっているかのフラグ

    // Start is called before the first frame update
    void Start()
    {
        // CraftUIをCraftCanvasから取得
        craftUI = GameObject.Find("CraftCanvas").gameObject.GetComponent<CraftUI>();
        CursolNotice = false;

        // PlayerをCraftUI_MainSubjectControllerから取得
        GameObject SubjectPanel = this.gameObject.transform.parent.gameObject;
        MSC = SubjectPanel.GetComponent<CraftUI_MainSubjectController>();
        player = MSC.GetPlayer();

        // テスト用プレイヤーリソース
        player.seaResource.ePlastic = 50000;
        player.seaResource.plastic = 50000;
        player.seaResource.seaFood = 50000;
        player.seaResource.steel = 50000;
        player.seaResource.wood = 50000;

        ContenaNum = 0;

        // IconNumがSUBJECT_WHALEMOUSEだった場合回収量の強化パラメータを送る
        if (IconNum == UpgradeSubject.SUBJECT_WHALEMOUSE)
        {
            MSC.SetMouseGetPower(1.0f);
        }
        // IconNumがSUBJECT_CRANEだった場合回収量の強化パラメータを送る
        if (IconNum == UpgradeSubject.SUBJECT_CRANE)
        {
            MSC.SetCraneGetPower(1.0f);
        }
    }

    // Update is called once per frame
    void Update()
    {
        // カーソルが当たっている時にEnterで強化
        if (CursolNotice == true)
        {
            if (Input.GetKeyDown(KeyCode.Return))
            {
                // 素材足りてるか判定
                if (UpgradeDecision())
                {
                    if (IconNum == UpgradeSubject.SUBJECT_DIEZELENGINE)
                    {
                        craftUI.DieselEngineUpgrade(UseResourceList[ContenaNum],UpgradePalamater[ContenaNum], UpgradePalamater.Count);                        
                    }
                    else if (IconNum == UpgradeSubject.SUBJECT_SHIPBODY)
                    {
                        craftUI.ShipBodyUpgrade(UseResourceList[ContenaNum], UpgradePalamater[ContenaNum], UpgradePalamater.Count);                        
                    }
                    else if (IconNum == UpgradeSubject.SUBJECT_WHALEMOUSE)
                    {
                        MSC.AddRate(UpgradePalamater[ContenaNum].Rate);

                        craftUI.WhaleMouseUpgrade(UseResourceList[ContenaNum], UpgradePalamater[ContenaNum], UpgradePalamater.Count);
                        MSC.SetMouseGetPower(UpgradePalamater[ContenaNum].GetPower);
                    }
                    else if (IconNum == UpgradeSubject.SUBJECT_CRANE)
                    {
                        craftUI.CraneUpgrade(UseResourceList[ContenaNum], UpgradePalamater[ContenaNum], UpgradePalamater.Count);
                        MSC.SetCraneGetPower(UpgradePalamater[ContenaNum].GetPower);
                    }
                    else if (IconNum == UpgradeSubject.SUBJECT_RADER)
                    {
                        craftUI.SonarUpgrade(UseResourceList[ContenaNum], UpgradePalamater[ContenaNum], UpgradePalamater.Count);                        
                    }

                    ContenaNumGet();
                    MSC.SetPlayerText(UseResourceList[ContenaNum], UpgradePalamater[ContenaNum], IconNum);
                }
            }           
        }
    }

    /* =========ステータスの強化レベルからListの要素番号を取得========== */
    void ContenaNumGet()
    {
        if (IconNum == UpgradeSubject.SUBJECT_DIEZELENGINE)//ディーゼルの強化レベルを取得
        {
            ContenaNum = player.dieselEngine;
        }
        else if (IconNum == UpgradeSubject.SUBJECT_SHIPBODY)// 船体の強化レベルを取得
        {
            ContenaNum = player.shipBody;
        }
        else if (IconNum == UpgradeSubject.SUBJECT_WHALEMOUSE)// WhaleMouseの強化レベルを取得
        {
            ContenaNum = player.whaleMouse;
        }
        else if (IconNum == UpgradeSubject.SUBJECT_CRANE)// クレーンの強化レベルを取得
        {
            ContenaNum = player.crane;
        }
        else if (IconNum == UpgradeSubject.SUBJECT_RADER)// レーダーの強化レベルを取得
        {
            ContenaNum = player.sonar;
        }

        if (ContenaNum>= UpgradePalamater.Count)
        {
            ContenaNum = 0;
        }
    }

    /* ==========アップグレード出来るか判定する========== */
    bool UpgradeDecision()
    {
        // 要素番号を取得する
        ContenaNumGet();

        // プレイヤーが持っている素材と必要素材数を比較する
        // プラスチック
        if (player.seaResource.plastic < UseResourceList[ContenaNum].plastic)
        {
            return false;
        }

        // エンプラ
        if(player.seaResource.ePlastic< UseResourceList[ContenaNum].ePlastic)
        {
            return false;
        }

        // 木材
        if(player.seaResource.wood< UseResourceList[ContenaNum].wood)
        {
            return false;
        }

        // 鋼材
        if(player.seaResource.steel< UseResourceList[ContenaNum].steel)
        {
            return false;
        }

        // 海鮮
        if(player.seaResource.seaFood < UseResourceList[ContenaNum].seaFood)
        {
            return false;
        }

        return true;
    }

    // 上昇するパラメータを取得する
    public Paramater GetParam()
    {
        ContenaNumGet();

        return UpgradePalamater[ContenaNum];
    }

    // 強化に使うリソース量のデータを取得する
    public SeaResource GetUseResource()
    {
        ContenaNumGet();

        return UseResourceList[ContenaNum];
    }

    // カーソルがあった時にCursolNoticeをtrueにする
    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.name == "SubjectCursol")
        {
            // テキスト表示を変更
            ContenaNumGet();
            MSC.SetPlayerText(UseResourceList[ContenaNum], UpgradePalamater[ContenaNum],IconNum);            

            CursolNotice = true;
        }
    }

    // カーソルが離れた時にCursolNoticeをfalseにする
    public void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.name == "SubjectCursol")
        {
            CursolNotice = false;
        }
    }
}
