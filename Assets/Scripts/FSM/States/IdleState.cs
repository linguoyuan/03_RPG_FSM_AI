using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AI.FSM
{
    /// <summary>
    /// 待机状态
    /// </summary>
	public class IdleState : FSMState 
	{
        private bool animationIsRun = false;
        public override void Init()
        {
            stateId = FSMStateID.Idle;
        }

        public override void EnterState(BaseFSM fsm)
        {
            animationIsRun = false;
        }

        public override void ExitState(BaseFSM fsm)
        {
            animationIsRun = false;
        }

        public override void Action(BaseFSM fsm)
        {
            //播待机动画
            if (animationIsRun == false)
            {
                //fsm.PlayeAnimation(fsm.animParams.Idle);
                fsm.PlayeAnimation(fsm.animNames.Idle);
                animationIsRun = true;
            }
        }

    }
}
