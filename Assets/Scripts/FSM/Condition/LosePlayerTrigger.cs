using System;
using System.Collections.Generic;
using UnityEngine;
//using ARPGSimpleDemo.Character;

namespace AI.FSM
{
    /// <summary>离开视野范围 </summary>
	public class LosePlayerTrigger : FSMTrigger
	{
        public override void Init()
        {
            this.triggerId = FSMTriggerID.LosePlayer;
        }

        protected override bool Evaluate(BaseFSM fsm)
        {
            if (fsm.targetPlayer == null)
                return false;
              
            //玩家离开攻击范围
            if (Vector3.Distance(fsm.transform.position, fsm.targetPlayer.transform.position) > fsm.sightDistance )
                return true;

            return false;
        }
    }
}
