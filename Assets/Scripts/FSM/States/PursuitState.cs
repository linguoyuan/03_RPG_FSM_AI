using System;
using System.Collections.Generic;
using UnityEngine;

namespace AI.FSM
{
	public class PursuitState : FSMState 
	{
        private bool animationIsRun = false;
        public override void Init()
        {
            stateId = FSMStateID.Pursuit;
        }

        public override void EnterState(BaseFSM fsm)
        {
            animationIsRun = false;
        }

        public override void ExitState(BaseFSM fsm)
        {
            animationIsRun = false;
            //停止移动 
            fsm.StopMove();
        }

        public override void Action(BaseFSM fsm)
        {
            if (animationIsRun == false)
            { 
                fsm.PlayeAnimation(fsm.animNames.Run);
                animationIsRun = true;
            }
            //播放跑动画
            if (fsm.targetPlayer != null && fsm.targetPlayer.gameObject != null)
            {
                fsm.MoveToTarget(fsm.targetPlayer.position, fsm.MoveSpeed, fsm.RotationSpeed);
            }
        }
    }
}
