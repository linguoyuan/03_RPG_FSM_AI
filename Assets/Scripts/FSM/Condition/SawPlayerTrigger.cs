using System;
using System.Collections.Generic;
using UnityEngine;
using ARPGSimpleDemo.Character;

namespace AI.FSM
{
    /// <summary>
    /// 发现玩家条件触发器
    /// </summary>
	public class SawPlayerTrigger : FSMTrigger 
	{
        public override void Init()
        {
            triggerId = FSMTriggerID.SawPlayer;
        }

        protected override bool Evaluate(BaseFSM fsm)
        {
            //检查玩家与AI的距离小于视距
            return 
                Vector3.Distance(fsm.transform.position ,fsm.targetPlayer.position ) <= fsm.sightDistance 
                && fsm.targetPlayer.GetComponent<CharacterStatus>().HP > 0
                && Vector3.Angle(fsm.transform.forward,fsm.targetPlayer.position - fsm.transform.position) <= fsm.sightPOV/2 ;
        }
    }
}
