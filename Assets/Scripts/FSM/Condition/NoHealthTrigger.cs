using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AI.FSM
{
	public class NoHealthTrigger : FSMTrigger
	{
        public override void Init()
        {
            triggerId = FSMTriggerID.NoHealth;
        }

        protected override bool Evaluate(BaseFSM fsm)
        {
            return fsm.chStatus.HP <= 0;
        }
    }
}
