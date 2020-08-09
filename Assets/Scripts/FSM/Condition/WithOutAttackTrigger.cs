using System;
using System.Collections.Generic;
using UnityEngine;


namespace AI.FSM
{
    /// <summary>目标离开攻击范围</summary>
	public class WithOutAttackTrigger : FSMTrigger 
	{
        public override void Init()
        {
            triggerId = FSMTriggerID.WithOutAttack;
        }

        protected override bool Evaluate(BaseFSM fsm)
        {
            if (fsm.targetPlayer == null) return false;
            
            //玩家离开攻击范围
            if (Vector3.Distance(fsm.transform.position, fsm.targetPlayer.transform.position) > fsm.chStatus.attackDistance)
                return true;

            return false;
        }
    }
}
