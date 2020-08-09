using UnityEngine;
using System.Collections;
using System.Collections.Generic ;

namespace AI.FSM
{
    //触发条件
    public abstract class FSMTrigger
    {
        //子条件
        public List<FSMTrigger> subTriggers = new List<FSMTrigger>();
        //自我描述 
        public FSMTriggerID triggerId;

        public FSMTrigger()
        {
            Init();
        }

        //初始化
        public abstract void Init();
        //检查条件是否满足
        protected abstract bool Evaluate(BaseFSM fsm);

        //检查子条件
        protected bool CheckSubTriggers(BaseFSM fsm)
        {
            if (subTriggers.Count == 0) return true;

            for (int i = 0; i < subTriggers.Count ; i++)
            {
                if (subTriggers[i].Evaluate(fsm))
                {
                    return true;
                }
            }
            return false;
        }

        //处理条件检查，检查本条件及子条件
        public bool HandleEvaluate(BaseFSM fsm)
        {
            bool result = Evaluate(fsm) && CheckSubTriggers(fsm);

            if (result)
            {
                //调用状态机切换状态
                return true;
            }
            return result;

        }

    }



}