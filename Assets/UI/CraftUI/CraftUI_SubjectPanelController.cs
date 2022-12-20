using System.Collections;
using System.Collections.Generic;
using UnityEngine;

enum UpdateSubjectState
{
    MAIN_SUBJECT=0,
    SPECIALIZE_SUBJECT,
    MAX_SUBJECT
}

public class CraftUI_SubjectPanelController : MonoBehaviour
{
    // 表示パネルの変数
    GameObject MainSubject;
    GameObject SpecializePanel;

    RectTransform MainRect;
    RectTransform SpecializeRect;

    CraftUI_MainSubjectController MainSubjectController;

    UpdateSubjectState state;

    // パネル切り替え時の動きに関する変数
    bool PanelChange;
    float ChangeTime;
    float StartTime;

    // Start is called before the first frame update
    void Start()
    {
        MainSubject = this.gameObject.transform.Find("MainSubjectPanel").gameObject;
        SpecializePanel = this.gameObject.transform.Find("SpecializationPanel").gameObject;

        MainRect = MainSubject.GetComponent<RectTransform>();
        SpecializeRect = SpecializePanel.GetComponent<RectTransform>();

        MainSubjectController = MainSubject.GetComponent<CraftUI_MainSubjectController>();

        MainSubject.SetActive(true);
        SpecializePanel.SetActive(false);

        state = UpdateSubjectState.MAIN_SUBJECT;
    }

    // Update is called once per frame
    void Update()
    {
        if (PanelChange == false)
        {
            // 表示パネル切り替え
            if (Input.GetKeyDown(KeyCode.RightArrow)
                && MainSubjectController.GetSubjectNum() == 3
                && state == UpdateSubjectState.MAIN_SUBJECT)
            {
                SpecializePanel.SetActive(true);

                PanelChange = true;
            }
            else if (Input.GetKeyDown(KeyCode.LeftArrow) && state == UpdateSubjectState.SPECIALIZE_SUBJECT)
            {
                MainSubject.SetActive(true);
                
                PanelChange = true;
            }
        }        

        PanelUpdate();
    }

    // パネルが切り替わる時の動きの処理
    void PanelUpdate()
    {
        if (PanelChange == false) return;

        // 経過時間を計測

        // パネルを移動(と回転)
        if(state == UpdateSubjectState.MAIN_SUBJECT) // メイン➡特化
        {
            // パネルを回転
            MainRect.transform.Rotate(Vector3.up, 2.0f);
            SpecializeRect.transform.Rotate(Vector3.up, 2.0f);

            // パネルを移動
            Vector3 MainPos = MainRect.anchoredPosition;
            MainPos.x -= 4.0f;
            MainRect.anchoredPosition = MainPos;

            Vector3 SpecializePos = SpecializeRect.anchoredPosition;
            SpecializePos.x -= 6.0f;
            SpecializeRect.anchoredPosition = SpecializePos;

            if (MainRect.transform.rotation.y > 0.67f)
            {
                Quaternion MainQ = Quaternion.AngleAxis(90.0f, Vector3.up);
                MainRect.transform.rotation = MainQ;

                Quaternion SpeQ = Quaternion.AngleAxis(0.0f, Vector3.up);
                SpecializeRect.transform.rotation = SpeQ;

                MainRect.anchoredPosition = new Vector3(0.0f, -10.0f, 0.0f);
                SpecializeRect.anchoredPosition = new Vector3(-5.0f, -10.0f, 0.0f);

                state = UpdateSubjectState.SPECIALIZE_SUBJECT;

                MainSubject.SetActive(false);

                PanelChange = false;
            }
        }
        else if(state == UpdateSubjectState.SPECIALIZE_SUBJECT) // 特化➡メイン
        {
            MainRect.transform.Rotate(Vector3.up, -2.0f);
            SpecializeRect.transform.Rotate(Vector3.up, -2.0f);

            Vector3 MainPos = MainRect.anchoredPosition;
            MainPos.x += 3.5f;
            MainRect.anchoredPosition = MainPos;

            Vector3 SpecializePos = SpecializeRect.anchoredPosition;
            SpecializePos.x += 6.0f;
            SpecializeRect.anchoredPosition = SpecializePos;

            if (MainRect.transform.rotation.y < 0.0f)
            {
                Quaternion MainQ = Quaternion.AngleAxis(0.0f, Vector3.up);
                MainRect.transform.rotation = MainQ;

                Quaternion SpeQ = Quaternion.AngleAxis(-90.0f, Vector3.up);
                SpecializeRect.transform.rotation = SpeQ;

                MainRect.anchoredPosition = new Vector3(150.0f, -10.0f, 0.0f);
                SpecializeRect.anchoredPosition = new Vector3(275.0f, -10.0f, 0.0f);

                state = UpdateSubjectState.MAIN_SUBJECT;

                SpecializePanel.SetActive(false);

                PanelChange = false;
            }
        }
    }
}
