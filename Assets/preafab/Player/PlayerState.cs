using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerState
{
    public virtual void TurnInit(Player player) { }

    public virtual void TurnUpdate(Player player) { }
}