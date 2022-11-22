//==============================================================
//
//   Title:  CameraController
//   Writer: Ryuidhi Saito
//   Date:   10/14
//
//   Overview: カメラの機能を記述するクラスです。
//             
//==============================================================

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    private int CameraID;                 // カメラ管理番号
    private CameraManager cameraManager;  // カメラマネージャ

    // Start is called before the first frame update
    void Start()
    {
        // カメラマネージャにカメラを登録
        cameraManager = GameObject.Find("CameraManager").GetComponent<CameraManager>();
        CameraID = cameraManager.AddCameras(this.gameObject);
    }

    // Update is called once per frame
    void Update()
    {

    }

    // カメラをアクティブにする
    //
    // カメラが切り替わる時に呼ぶ
    public void CameraActive()
    {
        cameraManager.CameraActive(CameraID);
    }
}
