using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SoftGear.Strix.Unity.Runtime;
using SoftGear.Strix.Client.Core;

public class RoomNumberChecker : StrixBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        var Number = StrixNetwork.instance.sortedRoomMembers;

        Debug.Log("Number(たぶん４)："+ Number.Count);
       
        
        int count = 1;
        foreach (var RoomMember in StrixNetwork.instance.sortedRoomMembers)
        {//                                           メンバー

            if (StrixNetwork.instance.selfRoomMember.GetUid() != RoomMember.GetUid())
            {//                     自分の番号                 Room内のメンバーの番号（昇順）

                Debug.Log(count+"人目ちがう");
                count++;
            }
            else//  selfRoomMember.GetUid() = RoomMember.GetUid()のとき
            {
                Debug.Log("あなたは" + count + "人目です");
            }
          
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
