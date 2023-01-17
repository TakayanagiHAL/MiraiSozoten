using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;

public class TextController : MonoBehaviour
{
    // テキストオブジェクト
    Text textline1; // ※1行26文字まで(全角)
    Text textline2;
    Text textline3;
    Text textline4;

    RectTransform textRect1;
    RectTransform textRect2;
    RectTransform textRect3;
    RectTransform textRect4;

    [SerializeField , TextArea(1, 6)] string EventText; // イベントの文章の全て

    string[] DisplayText;                   // EventTextを「。」で区切ったもの
                                            
    List<string> ProcessedDispText;         // DisplayTextを更に分割したもの(最終的に表示する文章)
                                            
    int EventCarryOut;                      // EventTextの文字数の進み具合
                                            
    int NowProcessedNum;                    // ProcessedDispTextの現在表示している数

    bool TextDispFrag;                      // テキストを表示するかのフラグ
    float DispTime;                         // 次の行を表示する経過時間
    [SerializeField] float NextDispTime;    // 次の行を表示するまでの時間
                                            
    bool MoveTextFlag;                      // テキスト移動フラグ
    float TextMoveTime;                     // テキスト移動の経過時間
    [SerializeField] float TextMoveTimeMax; // テキストの移動時間


    // 各テキストの移動前ポジションを一時的に保存する変数
    float BeforeTextPosY1;
    float BeforeTextPosY2;
    float BeforeTextPosY3;
    float BeforeTextPosY4;

    bool TextEnd;
    
    // Start is called before the first frame update
    void Start()
    {
        // テキストを取得
        GameObject MaskPanek = this.gameObject.transform.Find("TextFrame").gameObject.transform.Find("MaskPanel").gameObject;

        textline1 = MaskPanek.transform.Find("TextLine1").gameObject.GetComponent<Text>();
        textline2 = MaskPanek.transform.Find("TextLine2").gameObject.GetComponent<Text>();
        textline3 = MaskPanek.transform.Find("TextLine3").gameObject.GetComponent<Text>();
        textline4 = MaskPanek.transform.Find("TextLine4").gameObject.GetComponent<Text>();

        textRect1 = MaskPanek.transform.Find("TextLine1").gameObject.GetComponent<RectTransform>();
        textRect2 = MaskPanek.transform.Find("TextLine2").gameObject.GetComponent<RectTransform>();
        textRect3 = MaskPanek.transform.Find("TextLine3").gameObject.GetComponent<RectTransform>();
        textRect4 = MaskPanek.transform.Find("TextLine4").gameObject.GetComponent<RectTransform>();

        // EventTextを「。」で区切る
        DisplayText = EventText.Split("。");

        EventCarryOut = 0;

        ProcessedDispText = new List<string>();
        CreateDisplayText();// 最初に表示する文字列を取得

        NowProcessedNum = 0;

        TextDispFrag = false;
        DispTime = NextDispTime + 0.1f;

        MoveTextFlag = false;
        TextMoveTime = 0.0f;
        

        BeforeTextPosY1 = 0.0f;
        BeforeTextPosY2 = 0.0f;
        BeforeTextPosY3 = 0.0f;
        BeforeTextPosY4 = 0.0f;

        TextEnd = false;
    }

    // Update is called once per frame
    void Update()
    {
        // Enterキーで文章を進ませる
        if (Input.GetKeyDown(KeyCode.Return) /*|| Gamepad.current.bButton.wasPressedThisFrame*/)
        {
            // 表示内容を初期化
            TextClear();

            // 次のDisplayTextを作成
            if (EventCarryOut < DisplayText.Length - 1)
            {
                EventCarryOut++;
                CreateDisplayText();
                DispTime = NextDispTime + 0.1f;

                //Debug.Log("DisplayText.Length:" + DisplayText.Length);
                //Debug.Log("EventCarryOut:" + EventCarryOut);
            }
            else
            {
                TextEnd = true;
                Debug.Log("textEnd:" + TextEnd);
            }
        }

        // 時間経過でテキストを表示するフラグを立てる
        TimeDispFrag();

        // テキストを表示する
        ExecutionTextDisp();
    }

    /* ==========表示する文章を作成========== */
    void CreateDisplayText()
    {        
        // イベントの進み数がDisplayTextの要素数以内かどうか判定
        if (EventCarryOut < DisplayText.Length)
        {
            // 先頭に"\n"が入っていたら削除する
            int HeadBreakSymbol = DisplayText[EventCarryOut].IndexOf("\n");
            if (HeadBreakSymbol == 0)
            {
                DisplayText[EventCarryOut] = DisplayText[EventCarryOut].Remove(0, 1);
            }

            // リストの中身を全て削除
            ProcessedDispText.Clear();

            int StringNum = 0;
            int LoopNum = 0;
            while (true)
            {
                // 配列の長さを取得
                StringNum = DisplayText[EventCarryOut].Length;

                //Debug.Log("StringNum:" + StringNum);

                // 文章の長さが0だった場合ループを抜ける
                if (StringNum == 0)
                {
                    break;
                }

                string provisionalText; // 一時的に26文字入れるテキスト
                int actualTextNum;      // 先頭からの文字数
                if (StringNum >= 26)
                {
                    actualTextNum = 26;
                }
                else
                {
                    actualTextNum = StringNum;
                }

                // Substring(抜き出し始める文字数,抜き出す文字数)
                provisionalText = DisplayText[EventCarryOut].Substring(0, actualTextNum);

                // 取得した26文字に"\n"が含まれているか調べる
                // 入っていたら先頭からの文字数を取得し、無かったら26文字
                int partCharNum = provisionalText.IndexOf("\n") + 1;
                if (partCharNum != 0) // 含まれている
                {
                    actualTextNum = partCharNum;
                }

                // 取得した文章をProcessedDispTextに追加
                // "\n"が含まれていた場合は更に最後尾から"\n"を抜く
                ProcessedDispText.Add(provisionalText.Substring(0, actualTextNum)); //= provisionalText.Substring(0, actualTextNum);
                ProcessedDispText[LoopNum].Replace("\n", "");
                //Debug.Log("ProcessedDispText[i]:" + ProcessedDispText[LoopNum]);


                // 抜き出した文字数だけDisplayText[EventCarryOut]の先頭から文字を削除する
                DisplayText[EventCarryOut] = DisplayText[EventCarryOut].Remove(0, actualTextNum);

                LoopNum++;
            }

            // 現在の表示行数を0にする
            NowProcessedNum = 0;
        }
    }

    /* ==========時間経過でテキストを表示するフラグを立てる========== */
    void TimeDispFrag()
    {
        // 表示している番号がProcessedDispTextの要素数を越えていない時
        if (NowProcessedNum< ProcessedDispText.Count)
        {
            // テキスト移動フラグが立っていない時
            if (MoveTextFlag == false)
            {
                DispTime += Time.deltaTime;

                // DispTimeが1.5f以上
                if (DispTime > NextDispTime)
                {
                    // NowProcessedNumが3以上の場合はtextlineを上にずらして一番上の物を一番下に移動させる
                    // 移動させるフラグを立てる
                    if (NowProcessedNum >= 3)
                    {
                        MoveTextFlag = true;

                        // 各テキストの移動前ポジションを一時的に保存する
                        BeforeTextPosY1 = textRect1.anchoredPosition.y;
                        BeforeTextPosY2 = textRect2.anchoredPosition.y;
                        BeforeTextPosY3 = textRect3.anchoredPosition.y;
                        BeforeTextPosY4 = textRect4.anchoredPosition.y;
                    }
                    else
                    {
                        TextDispFrag = true;
                    }                    
                }
            }            
        }        
    }

    /* ==========実際にテキストを表示する========== */
    void ExecutionTextDisp()
    {
        // テキストを上に移動させる
        MoveTextLine();

        // 移動し終わったらテキストを表示する
        if (TextDispFrag == true)
        {
            TextSet();
            NowProcessedNum++;
            DispTime = 0.0f;

            TextDispFrag = false;
        }
    }

    /* ==========3行以上表示する場合に表示する行をずらす========== */
    void MoveTextLine()
    {
        if (NowProcessedNum >= 3)
        {
            if (MoveTextFlag == true)  // 移動フラグが立ったら
            {
                // 時間計測
                TextMoveTime += Time.deltaTime;

                // 
                if (TextMoveTime < TextMoveTimeMax)  // 移動中
                {
                    // 経過時間から移動量を計算する
                    float MovePosition = 65.0f * (TextMoveTime / TextMoveTimeMax);

                    // 移動前ポジション+MovePosition
                    textRect1.anchoredPosition = new Vector3(0.0f, BeforeTextPosY1 + MovePosition, 0.0f);
                    textRect2.anchoredPosition = new Vector3(0.0f, BeforeTextPosY2 + MovePosition, 0.0f);
                    textRect3.anchoredPosition = new Vector3(0.0f, BeforeTextPosY3 + MovePosition, 0.0f);
                    textRect4.anchoredPosition = new Vector3(0.0f, BeforeTextPosY4 + MovePosition, 0.0f);
                }
                else // 移動終了
                {
                    // それぞれ移動前から65上に移動した値を代入する
                    textRect1.anchoredPosition = new Vector3(0.0f, BeforeTextPosY1 + 65.0f, 0.0f);

                    textRect2.anchoredPosition = new Vector3(0.0f, BeforeTextPosY2 + 65.0f, 0.0f);
                    textRect3.anchoredPosition = new Vector3(0.0f, BeforeTextPosY3 + 65.0f, 0.0f);
                    textRect4.anchoredPosition = new Vector3(0.0f, BeforeTextPosY4 + 65.0f, 0.0f);

                    // yが130を超えていたら一番下(-130)に移動
                    if (textRect1.anchoredPosition.y > 129.9f)
                    {
                        textRect1.anchoredPosition = new Vector3(0.0f, -130.0f, 0.0f);
                        textline1.text = "";
                    }
                    else if (textRect2.anchoredPosition.y > 129.9f)
                    {
                        textRect2.anchoredPosition = new Vector3(0.0f, -130.0f, 0.0f);
                        textline2.text = "";
                    }
                    else if (textRect3.anchoredPosition.y > 129.9f)
                    {
                        textRect3.anchoredPosition = new Vector3(0.0f, -130.0f, 0.0f);
                        textline3.text = "";
                    }
                    else if (textRect4.anchoredPosition.y > 129.9f)
                    {
                        textRect4.anchoredPosition = new Vector3(0.0f, -130.0f, 0.0f);
                        textline4.text = "";
                    }


                    TextMoveTime = 0.0f;

                    MoveTextFlag = false;
                    TextDispFrag = true;
                }
            }
        }
    }
    

    /* ==========テキストの文章をセット========== */
    void TextSet()
    {
        if(NowProcessedNum< ProcessedDispText.Count)
        {
            // 現在の表示番号を０～３に変換
            int num = NowProcessedNum % 4;

            if (num == 0)
            {
                textline1.text = ProcessedDispText[NowProcessedNum].ToString();
            }
            else if (num == 1)
            {
                textline2.text = ProcessedDispText[NowProcessedNum].ToString();
            }
            else if (num == 2)
            {
                textline3.text = ProcessedDispText[NowProcessedNum].ToString();
            }
            else if (num == 3)
            {
                textline4.text = ProcessedDispText[NowProcessedNum].ToString();
            }
        }
    }

    /* ==========テキストの文章とポジションをクリア========== */
    void TextClear()
    {
        // Textのポジションリセット
        textRect1.anchoredPosition = new Vector3(0.0f, 65.0f, 0.0f);
        textRect2.anchoredPosition = new Vector3(0.0f, 0.0f, 0.0f);
        textRect3.anchoredPosition = new Vector3(0.0f, -65.0f, 0.0f);
        textRect4.anchoredPosition = new Vector3(0.0f, -130.0f, 0.0f);



        // Textの中身クリア
        textline1.text = "";
        textline2.text = "";
        textline3.text = "";
        textline4.text = "";

        // ProcessedDispTextの中身を全てクリア
        ProcessedDispText.Clear();


        // 各種フラグと時間計測用の変数を初期化
        NowProcessedNum = 0;   // 現在の表示テキスト数
        TextDispFrag = false;  // テキスト表示フラグ
        DispTime = 0.0f;       // 次の行を表示する経過時間

        MoveTextFlag = false;  // テキスト移動のフラグ
        TextMoveTime = 0.0f;   // テキスト移動の経過時間
    }

    /* ==========テキストの読み上げ終了判定を取得する========== */
    public bool GetTextEnd()
    {
        return TextEnd;
    }

    /* ==========テキストを追加で入力する========== */
    // ※内容を上書きするので、上書きしても問題無い時か全部表示しきってから追加で入れること
    public void SetText(string text)
    {
        TextClear();                     // テキストを全て初期化

        DisplayText = text.Split("。");  // 表示テキスト内容を上書き

        EventCarryOut = 0;               // テキストの進み具合を初期化

        TextEnd = false;                 // テキスト終了フラグをfalseにする

        CreateDisplayText();             // テキストを加工

        //Debug.Log("DisplayText.Length:" + DisplayText.Length);
    }



    /* ==========string型テキストから特定の文字を探して数を調べる ========== */
    int SpecificCharNumSearch(string Text, string search)
    {
        int NowPoint = 0;
        int PartNum = 0;// "\n"の数

        // Textnumから"\n"の数を割り出す
        while (true)
        {
            // 先頭からの文字数を取得
            int PartStrNum = Text.IndexOf(search, NowPoint);

            // "\n"が無い場合はループを出る
            if (PartStrNum == -1)
            {
                break;
            }

            NowPoint = PartStrNum + 1;

            PartNum++;
        }

        return PartNum;
    }

}
