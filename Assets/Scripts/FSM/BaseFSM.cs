using UnityEngine;
using System.Collections.Generic;
using ARPGSimpleDemo.Character;
using System;
using System.IO;

namespace AI.FSM
{
    /// <summary>
    /// 有限状态机
    /// </summary>
    public class BaseFSM : MonoBehaviour
    {
        #region 关联的组件
        /// <summary>角色状态数据</summary>
        [HideInInspector]
        public CharacterStatus chStatus = null;
        /// <summary>动画参数</summary> 
        //[HideInInspector]
        //public AnimationParameters animParams = null;

        /// <summary>动画名字</summary> 
        //[HideInInspector]
        public AnimationNames animNames = null;

        /// <summary> 角色系统(技能系统的外观类)</summary>
        //public CharacterSkillSystem chSystem = null;
        //寻路组件
        public UnityEngine.AI.NavMeshAgent navMeshAgent;
        //动画组件
        private CharacterAnimation anim;

        #endregion

        #region 数据(AI系统需要的所有数据)

        /// <summary>状态对象库,保存AI所有状态</summary>
        private List<FSMState> allState;
        /// <summary>AI 关注的目标</summary>
        public Transform targetPlayer = null;
        /// <summary>AI的视距</summary>
        public float sightDistance = 20f;
        /// <summary>可视角度</summary>
        public int sightPOV = 160;
        /// <summary>跑动速度</summary>
        public float MoveSpeed = 1f;
        /// <summary>行走速度</summary>
        public float walkSpeed = 0.2f;
        /// <summary>旋转速度</summary>
        public float RotationSpeed = 0.5f;
        /// <summary>巡逻的路点</summary>
        public Transform[] PatrolWayPoints;
        /// <summary>巡逻模式</summary>
        public PatrolMode patrolMode = PatrolMode.PingPong;
        /// <summary>状态机配置文件名称</summary>
        public string AIConfigFile;

        #endregion


        #region 如果用户有自己的运动方法，请将动动方法关联以下委托
        //行为代理
        public delegate void MovementHandler(Vector3 targetPos, float speed, float rotSpeed);
        public MovementHandler CustomeMovement = null;

        #endregion

        #region 初始状态，当前状态

        //当前状态
        private FSMState currentState;
        public FSMStateID currentSateId;
        //默认状态
        private FSMState defaultState;
        public FSMStateID defaultStateId = FSMStateID.Idle;


        /// <summary>初始化起始状态</summary>
        private void InitDefaultState()
        {
            defaultState = allState.Find(p => p.stateId == defaultStateId);
            Debug.Log("defaultState = " + defaultState);
            currentState = defaultState;
            currentSateId = currentState.stateId;
            Debug.Log("currentSateId = " + currentSateId);
            currentState.EnterState(this);
        }

        #endregion

        #region 初始化

        public void Awake()
        {
            allState = new List<FSMState>();

        }

        public void Start()
        {
            targetPlayer = GameObject.FindGameObjectWithTag("Player").transform;
            navMeshAgent = GetComponent<UnityEngine.AI.NavMeshAgent>();
            anim = GetComponent<CharacterAnimation>();
            chStatus = GetComponent<CharacterStatus>();
            //chSystem = GetComponent<CharacterSkillSystem>();
            ConfigFSM();
            //初始化起始状态
            InitDefaultState();
        }

        #endregion

        //string sourcePath = "";
        //string aiConfigFile = "BaseAI";
        //private IEnumerator LoadText(string url)
        //{
        //    sourcePath = Path.Combine(Application.streamingAssetsPath, aiConfigFile);
        //    UnityWebRequest request = new UnityWebRequest(sourcePath);
        //    yield return request;
        //    if (request.isDone)
        //    {
        //        temp = request.downloadHandler.text;
        //        Debug.Log(temp);
        //    }

        //    yield return null;
        //}





        #region 状态机实时工作 当前状态Action,Reason

        public void Update()
        {
            if (currentState != null)
            {
                //当前状态下的行为
                currentState.Action(this);
                //当前状态下转换条件的检测
                currentState.Reason(this);
            }
        }

        #endregion

        #region 状态机管理状态行为 添加 删除 状态切换 配置状态机

        /// <summary>添加状态</summary>
        public void AddState(FSMState state)
        {
            if (!allState.Exists(p => p.stateId == state.stateId))
            {
                allState.Add(state);
            }
        }

        /// <summary>删除状态</summary> 
        public void DeleteState(FSMState state)
        {
            if (allState.Exists(p => p.stateId == state.stateId))
            {
                allState.Remove(state);
            }
        }

        /// <summary>状态切换</summary> 
        public void ChangeActiveState(FSMTriggerID triggerId)
        {
            if (currentState == null) return;
            //根据条件编号，在当前状态查找输出状态
            var stateId = currentState.GetOutPutState(triggerId);
            //可能得到的3个结果  
            //1.None : 不处理
            if (stateId == FSMStateID.None) return;
            //退出当前状态
            currentState.ExitState(this);
            //2.默认状态: 将原默认状态设为当前状态
            if (stateId == FSMStateID.Default)
                currentState = defaultState;
            else //3.具体状态: 将具体状态设为当前状态
                currentState = allState.Find(p => p.stateId == stateId);

            currentSateId = currentState.stateId;
            //进入新状态
            currentState.EnterState(this);
        }

        ///// <summary>配置状态机 硬编码</summary>
        //public virtual void configfsm()
        //{
        //    //创建状态对象
        //    //待机
        //    idlestate idle = new idlestate();
        //    //设置转换
        //    idle.addtrigger(fsmtriggerid.nohealth, fsmstateid.dead);
        //    idle.addtrigger(fsmtriggerid.sawplayer, fsmstateid.pursuit);
        //    //死亡状态
        //    deadstate dead = new deadstate();
        //    //追逐
        //    pursuitstate pursuit = new pursuitstate();
        //    pursuit.addtrigger(fsmtriggerid.reachplayer, fsmstateid.attacking);
        //    pursuit.addtrigger(fsmtriggerid.nohealth, fsmstateid.dead);
        //    pursuit.addtrigger(fsmtriggerid.loseplayer, fsmstateid.default);

        //    //攻击
        //    attackingstate attack = new attackingstate();
        //    attack.addtrigger(fsmtriggerid.nohealth, fsmstateid.dead);
        //    attack.addtrigger(fsmtriggerid.withoutattack, fsmstateid.pursuit);
        //    attack.addtrigger(fsmtriggerid.killedplayer, fsmstateid.default);

        //    patrollingstate patrol = new patrollingstate();
        //    patrol.addtrigger(fsmtriggerid.patrolcomplete, fsmstateid.idle);
        //    patrol.addtrigger(fsmtriggerid.nohealth, fsmstateid.dead);
        //    patrol.addtrigger(fsmtriggerid.sawplayer, fsmstateid.pursuit);

        //    //加入状态机
        //    addstate(idle);
        //    addstate(dead);
        //    addstate(pursuit);
        //    addstate(attack);
        //    addstate(patrol);
        //}


        /// <summary>配置状态机，通过AI配置文件配置状态机</summary>
        public void ConfigFSM()
        {

            if (string.IsNullOrEmpty(AIConfigFile))
            {
                Debug.Log("没找到AI配置文件");
                return;
            }
            else
            {
                Debug.Log("加载AI配置文件......");
                //加载配置文件
                //var dic = AIConfigLoader.GetInstance().Load(AIConfigFile);
                var dic = AIConfigLoader.Load(AIConfigFile);
                Debug.Log("dic.Count = " + dic.Count);
                if (dic.Count == 0) return;
                //遍历所有的主键
                foreach (var mainKey in dic.Keys)
                {
                    //主键名即是状态类的名称，
                    var stateClassName = "AI.FSM." + mainKey;
                    
                    if ("AI.FSM.[IdleState" == stateClassName)
                    {
                        stateClassName = "AI.FSM.IdleState";
                    }
                    Debug.Log("stateClassName = " + stateClassName);
                    //反射创建状态对象
                    var stateObj = Activator.CreateInstance(Type.GetType(stateClassName)) as FSMState;
                    //再遍历主键下所有的子键
                    foreach (var subKey in dic[mainKey].Keys)
                    {
                        //子键名即是TriggerID，将字符串的TriggerID转为枚举
                        var triggerId = (FSMTriggerID)Enum.Parse(typeof(FSMTriggerID), subKey);
                        //取得子键对应的子值，子值即是StateId，//将字符串的StateId转为枚举，
                        var stateId = (FSMStateID)Enum.Parse(typeof(FSMStateID), dic[mainKey][subKey]);
                        //调用创建好的状态对象的AddTriggers方法加入triggerid,Stateid
                        stateObj.AddTrigger(triggerId, stateId);
                    }
                    //将创建好的状态对象放入状态机
                    AddState(stateObj);
                }
            }
        }

        #endregion

        #region //辅助行为 寻路  动画

        ///<summary>寻路</summary>
        public void MoveToTarget(Vector3 pos, float moveSpeed, float rotationSpeed)
        {
            MoveToTarget(pos, moveSpeed, rotationSpeed, chStatus.attackDistance);
        }
        ///<summary>寻路</summary>
        public void MoveToTarget(Vector3 pos, float moveSpeed, float rotationSpeed, float stoppingDistance)
        {

            if (navMeshAgent != null) //网格寻格
            {
                pos.y = transform.position.y;
                //面向目标
                TransformHelper.LookAtTarget(pos - transform.position, transform, rotationSpeed);
                navMeshAgent.speed = moveSpeed;
                navMeshAgent.stoppingDistance = stoppingDistance;
                navMeshAgent.SetDestination(pos);
            }
            else if (CustomeMovement != null) //自定义运动方式 
            {
                Debug.Log("111111");
                CustomeMovement(pos, moveSpeed, rotationSpeed);
            }
                
        }
        ///<summary>寻路</summary>
        public void MoveToTarget(Vector3 pos)
        {
            MoveToTarget(pos, MoveSpeed, RotationSpeed);
        }

        /// <summary>停止寻路</summary>
        public void StopMove()
        {
            navMeshAgent.SetDestination(transform.position);
        }

        /// <summary>动画</summary>
        public void PlayeAnimation(string paramName)
        {
            //anim.PlayAnimation(paramName);
            anim.PlayAnimationByName(paramName);
        }

        /// <summary>随机技能攻击</summary>
        public void RandomAttack()
        {
            //chSystem.RandomSelectSkill();
            Debug.Log("随机释放了一个技能");
        }

        #endregion

        /// <summary>停止状态机</summary>
        public void Stop()
        {
            this.enabled = false;
        }
    }
}
