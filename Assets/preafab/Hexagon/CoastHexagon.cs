using System.Collections;
using System.Collections.Generic;
using UnityEngine;


   class CoastHexagon : HexagonMethod
   {
    public override void OnPassage(Player player){
        player.getRVol++;
    }

    public override void OnReach(Player player) {
    }
}

