using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SoftGear.Strix.Unity.Runtime;
using UnityEngine.UI;

public class UI_RuleSetting : StrixBehaviour
{
    [SerializeField] private GameObject RoomID;

    // Start is called before the first frame update
    void Start()
    {
        //ÉãÅ[ÉÄIDÇÃï\é¶
        long id = StrixNetwork.instance.selfRoomMember.GetRoomId();
        string idText = id.ToString();

        RoomID.GetComponent<Text>().text = idText;

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
