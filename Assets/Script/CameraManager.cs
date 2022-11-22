//==============================================================
//
//   Title:  CameraManager
//   Writer: Ryuidhi Saito
//   Date:   10/14
//
//   Overview: Cameracontrollerをコンポーネントに持っている
//             カメラを管理するクラスです。
//==============================================================


using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    private List<GameObject> Cameras = new List<GameObject>();  // カメラのリスト
    private int NowCamera;                                      // 現在写しているカメラの番号

    // Start is called before the first frame update
    void Start()
    {
        // とりあえず0で初期化
        NowCamera = 0;
    }

    // Update is called once per frame
    void Update()
    {

    }

    // カメラ全停止
    public void CameraAllInactive()
    {
        for (int i = 0; i < Cameras.Count; i++)
        {
            Cameras[i].SetActive(false);
        }
    }

    // カメラをリストにセット
    // 
    // 引数 :camera  カメラのGameObject
    //
    public int AddCameras(GameObject camera)
    {
        // カメラを非アクティブ化してリストに追加
        camera.SetActive(false);
        Cameras.Add(camera);

        // リストのカメラ数-1の数値を管理番号として返す
        return Cameras.Count - 1;
    }

    // 写すカメラをセット
    // 
    // 引数 :num  CameraControllerクラスが持っているCameraID
    //
    public void CameraActive(int num)
    {
        // カメラが1つもない場合中に入らない
        if (Cameras.Count <= 0) return;

        // 現在動いているカメラを非アクティブ化
        Cameras[NowCamera].SetActive(false);

        // 新たに写すカメラをアクティブ化
        Cameras[num].SetActive(true);
        NowCamera = num;
    }
}
