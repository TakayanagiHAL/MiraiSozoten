using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class CraftUI_SubjectIconController : MonoBehaviour
{
    CraftUI_MainSubjectController MSC;

    bool CursolNotice;// カーソルが当たっているかのフラグ

    // Start is called before the first frame update
    void Start()
    {        
        CursolNotice = false;

        // PlayerをCraftUI_MainSubjectControllerから取得
        GameObject SubjectPanel = this.gameObject.transform.parent.gameObject;
        MSC = SubjectPanel.GetComponent<CraftUI_MainSubjectController>();
    }

    // Update is called once per frame
    void Update()
    {
        // カーソルが当たっている時にEnterで強化
        if (CursolNotice == true)
        {
            
        }
    }
    
    // カーソルがあった時にCursolNoticeをtrueにする
    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.name == "SubjectCursol")
        {
            // MeinSubjectControllerのテキストを更新
            MSC.TextUpdate();

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
