using UnityEngine;
using System.Collections.Generic;
using System;

namespace AI.FSM
{
    //状态类
    public abstract class FSMState
    {
        public FSMState()
        {
            Init();
        }
        //数据:
        public FSMStateID stateId;
        //触发条件的列表
        protected List<FSMTrigger> triggers = new List<FSMTrigger>();
      

        //条件->输出状态的映射关系
        protected Dictionary<FSMTriggerID, FSMStateID> map = new Dictionary<FSMTriggerID, FSMStateID>();

        //行为：
        //维护列表(增加，删除，查找(根据转换条件查输出状态)) 
        public void AddTrigger(FSMTriggerID triggerId, FSMStateID stateId)
        {
            if (!map.ContainsKey(triggerId))
            {   
                map.Add(triggerId, stateId);
                var type = Type.GetType("AI.FSM." + triggerId.ToString() + "Trigger");
                if (type != null) //有条件触发器的话，就创建条件触发器
                {
                    var trigger = Activator.CreateInstance(type) as FSMTrigger;
                    triggers.Add(trigger);
                }
            }
            else
                map[triggerId] = stateId;
        }
        //删除
        public void DeleteTrigger(FSMTriggerID triggerId)
        {
            if (map.ContainsKey(triggerId))
            {
                map.Remove(triggerId);
                triggers.RemoveAll(p => p.triggerId == triggerId);
            }
        }
        //查找
        public FSMStateID GetOutPutState(FSMTriggerID triggerId)
        {
            if (map.ContainsKey(triggerId))
                return map[triggerId];
            return FSMStateID.None;
        }
      
        /// <summary>初始化状态</summary>
        public abstract void Init();
        //1.进入状态时
        public abstract void EnterState(BaseFSM fsm);
        //2.离开状态时
        public abstract void ExitState(BaseFSM fsm);
        /// <summary>持续状态时，执行的主要行为</summary>
        public abstract void Action(BaseFSM fsm);
        //4.检查转换条件
        public virtual void Reason(BaseFSM fsm)
        {
            //检查每一个条件，如果有满足的，则转换状态
            for (int i = 0; i < triggers.Count; i++)
            {
                if (triggers[i].HandleEvaluate(fsm))
                {
                    fsm.ChangeActiveState(triggers[i].triggerId);
                }
            }
        }

    }
}
