using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SunDayCycle : MonoBehaviour
{
    [SerializeField]
    private float RotationSpeed;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        // x軸を軸にして毎秒2度、回転させるQuaternionを作成（変数をrotとする）
        Quaternion rot = Quaternion.AngleAxis(-RotationSpeed, Vector3.up);
        // 現在の自信の回転の情報を取得する。
        Quaternion q = this.transform.rotation;
        // 合成して、自身に設定
        this.transform.rotation = q * rot;
    }
}
