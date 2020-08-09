using System;
using System.Collections.Generic;
using UnityEngine;

namespace AI.FSM
{
    public class AttackingState : FSMState
    {
        //两次攻击计时
        private float attackTime;

        public override void Init()
        {
            stateId = FSMStateID.Attacking;
        }

        public override void EnterState(BaseFSM fsm)
        {
            if (attackTime == 0)
                attackTime = fsm.chStatus.attackSpeed;
            fsm.PlayeAnimation(fsm.animNames.Stand);
        }

        public override void ExitState(BaseFSM fsm)
        {
        }

        public override void Action(BaseFSM fsm)
        {
            //盯住玩家
            TransformHelper.LookAtTarget(fsm.targetPlayer.position - fsm.transform.position, fsm.transform, fsm.RotationSpeed);

            if (attackTime <= 0)
            {
                attackTime = fsm.chStatus.attackSpeed;
                fsm.PlayeAnimation(fsm.animNames.Attack);
                fsm.RandomAttack();
            }
            attackTime -= Time.deltaTime;
        }
    }
}
