using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AI.FSM
{

    public enum FSMTriggerID
    {
        SawPlayer,          //发现玩家	
        NoHealth,            //生命为0	
        ReachPlayer,       //玩家进入攻击范围	
        KilledPlayer,       //打死玩家	
        LosePlayer,         //玩家离开视野范围	
        WithOutAttack,    //玩家离开攻击范围
        PatrolComplete
    }
}
