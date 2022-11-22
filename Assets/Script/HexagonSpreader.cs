using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SoftGear.Strix.Unity.Runtime;

public class HexagonSpreader : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        OwnerCheck();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OwnerCheck()
    {
        //ルームオーナー以外はこのオブジェクトを破壊
        var strixNetwork = StrixNetwork.instance;
        if (!strixNetwork.isRoomOwner)
        {
            Destroy(gameObject);
            Debug.Log("ルームオーナー以外のHexagonSpreaderを消去しました。");
        }
    }
}