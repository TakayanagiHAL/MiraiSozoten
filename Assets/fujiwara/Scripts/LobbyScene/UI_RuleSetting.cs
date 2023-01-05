using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SoftGear.Strix.Unity.Runtime;
using UnityEngine.UI;

public class UI_RuleSetting : StrixBehaviour
{

    [SerializeField] private GameObject[] PlayerIcons = new GameObject[4];

    [SerializeField] private GameObject RoomID;

    // Start is called before the first frame update
    void Start()
    {
        //ルームIDの表示
        long id = StrixNetwork.instance.selfRoomMember.GetRoomId();
        string idText = id.ToString();

        RoomID.GetComponent<Text>().text = idText;



    }

    // Update is called once per frame
    void Update()
    {

        //プレイヤーリストの表示更新
        int memberCount = StrixNetwork.instance.room.GetMemberCount();

        int count = 0;
        foreach(var roomMember in StrixNetwork.instance.sortedRoomMembers)
        {
            Text name = PlayerIcons[count].transform.Find("PlayerNameBoard").gameObject.transform.Find("PlayerNameText").GetComponent<Text>();

            name.text = roomMember.GetName();
            PlayerIcons[count].transform.Find("PlayerLobbyStatus").gameObject.SetActive(true);
            count++;
        }

        //不足分（NPC）
        if(memberCount<4)
        {
            while(count<4)
            {
                Text name = PlayerIcons[count].transform.Find("PlayerNameBoard").gameObject.transform.Find("PlayerNameText").GetComponent<Text>();
                name.text = "NPC";
                PlayerIcons[count].transform.Find("PlayerLobbyStatus").gameObject.SetActive(false);
                count++;
               
            }
            
        }
    }

    

}
