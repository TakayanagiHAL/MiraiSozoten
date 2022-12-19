using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CraftUI : MonoBehaviour
{
    Player player;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void DieselEngineUpgrade()
    {
        //成功時
        if (true)
        {
            player.dieselEngine++;
            //資源処理
            //パラメータ処理
        }
        else//失敗時
        {

        }
    }

    public void ShipBodyUpgrade()
    {
        //成功時
        if (true)
        {
            player.shipBody++;
            //資源処理
            //パラメータ処理
        }
        else//失敗時
        {

        }
    }

    public void WhaleMouseUpgrade()
    {
        //成功時
        if (true)
        {
            player.whaleMouse++;
            //資源処理
            //パラメータ処理
        }
        else//失敗時
        {

        }
    }

    public void CraneUpgrade()
    {
        //成功時
        if (true)
        {
            player.crane++;
            //資源処理
            //パラメータ処理
        }
        else//失敗時
        {

        }
    }

    public void SonarUpgrade()
    {
        //成功時
        if (true)
        {
            player.sonar++;
            //資源処理
            //パラメータ処理
        }
        else//失敗時
        {

        }
    }
}
