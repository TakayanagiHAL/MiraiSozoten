using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TitleVersionText : MonoBehaviour
{
    [SerializeField]
    Text TeamProduct = null;

    [SerializeField]
    [Header("リリース年")]
    string release_year = string.Empty;

    // Start is called before the first frame update
    void Start()
    {
        // バージョン
        var version = Application.version;
        // チーム名
        var teamName = Application.companyName;
        //  エディターバージョン
        var editorVersion = Application.unityVersion;

        // バージョン情報表示
        TeamProduct.text = $"Ver.{version}  ©{release_year}  {teamName}\nU.E.{editorVersion}";

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
