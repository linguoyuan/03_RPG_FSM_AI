using System;
using System.Collections.Generic;
using UnityEngine;

namespace AI.FSM
{
	public class ReachPlayerTrigger:FSMTrigger
	{
        public override void Init()
        {
            triggerId = FSMTriggerID.ReachPlayer;
        }

        /// <summary>
        /// 目标是否在攻击范围内
        /// </summary>
        protected override bool Evaluate(BaseFSM fsm)
        {
            return Vector3.Distance(fsm.transform.position, fsm.targetPlayer.position) <= fsm.chStatus.attackDistance;
        }
    }
}
