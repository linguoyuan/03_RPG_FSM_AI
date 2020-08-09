using System;
using System.Collections.Generic;
using ARPGSimpleDemo.Character;

namespace AI.FSM
{
    public class KilledPlayerTrigger : FSMTrigger
    {
        public override void Init()
        {
            triggerId = FSMTriggerID.KilledPlayer;
        }

        protected override bool Evaluate(BaseFSM fsm)
        {
            if (fsm.targetPlayer == null) return false;
            //玩家被打死        
            if (fsm.targetPlayer.GetComponent<CharacterStatus>().HP <= 0)
                return true;
            return false;
        }
    }
}
