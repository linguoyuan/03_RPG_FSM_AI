using System;
using System.Collections.Generic;
using UnityEngine;

namespace AI.FSM
{
    /// <summary>巡逻状态</summary>
    public class PatrollingState : FSMState
    {
        /// <summary>当前路点</summary>
        private Transform currentWayPoint;
        /// <summary>当前路点索引</summary>
        private int currentWayPointIndex = 0;
        public float arrivalDistance = 1f;
        /// <summary>巡逻是否完成</summary>
        private bool patrolComplete = false;
        private bool animationIsRun = false;


        public override void Init()
        {
            stateId = FSMStateID.Patrolling;
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
            //判断是否到达路点
            if (Vector3.Distance(fsm.transform.position, fsm.PatrolWayPoints[currentWayPointIndex].position) < arrivalDistance)
            {

                if (currentWayPointIndex == fsm.PatrolWayPoints.Length - 1)
                {
                    if (fsm.patrolMode == PatrolMode.Once)//单程
                    {
                        patrolComplete = true;
                        currentWayPointIndex = 0;
                        return;
                    }
                    //结束巡逻
                    else if (fsm.patrolMode == PatrolMode.PingPong)
                    {
                        Array.Reverse(fsm.PatrolWayPoints);
                    }     
                }
                //到 : 换下一个路点
                currentWayPointIndex +=1;
                currentWayPointIndex = currentWayPointIndex % fsm.PatrolWayPoints.Length;
            }
            currentWayPoint = fsm.PatrolWayPoints[currentWayPointIndex];
            fsm.MoveToTarget(currentWayPoint.position, fsm.walkSpeed, fsm.RotationSpeed,arrivalDistance);
            //Debug.Log("fsm.walkSpeed = " + fsm.walkSpeed);

            //播待行走动画
            if (animationIsRun == false)
            {
                //fsm.PlayeAnimation(fsm.animParams.Walk);
                fsm.PlayeAnimation(fsm.animNames.Walk);
                animationIsRun = true;
            }

        }

        /// <summary>检查条件是否达到</summary>
        public override void Reason(BaseFSM fsm)
        {
            base.Reason(fsm);
            if (patrolComplete)
            {
                fsm.ChangeActiveState(FSMTriggerID.PatrolComplete);
            }       
        }
    }
}
