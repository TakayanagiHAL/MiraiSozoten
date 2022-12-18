using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using SoftGear.Strix.Unity.Runtime;


public class TemporaryConnectManager : StrixBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        GetComponent<Connect_Strix>().Connect();
    }

    // Update is called once per frame
    void Update()
    {
        if (StrixNetwork.instance.room != null)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                RpcToAll(nameof(SceneJumpAllMember));
            }
        }
    }

    [StrixRpc]
    public void SceneJumpAllMember()
    {
        SceneManager.LoadScene(2);
    }
}
