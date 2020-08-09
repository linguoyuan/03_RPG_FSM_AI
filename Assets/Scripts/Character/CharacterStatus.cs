using UnityEngine;
using System.Collections;


namespace ARPGSimpleDemo.Character
{
    /// <summary>
    /// 角色状态 （表达所有角色共同的状态信息）
    /// </summary>
    public abstract class CharacterStatus : MonoBehaviour
    {
        /// <summary>生命 </summary>
        public int HP = 100;
        /// <summary>生命 </summary>
        public int MaxHP=100;
        /// <summary>当前魔法 </summary>
        public int SP = 100;
        /// <summary>最大魔法 </summary>
        public int MaxSP =100;
        /// <summary>伤害基数</summary>
        public int damage = 100;
         /// <summary>攻击速度 </summary>
        public int attackSpeed = 5;
        /// <summary>主技能攻击距离 ,用于设置AI的攻击范围，与目标距离此范围内发起攻击</summary>
        public float attackDistance = 2;
        
        /// <summary>受击特效挂点 挂点名为HitFxPos </summary>
        [HideInInspector]
        public Transform HitFxPos;

        #region HUD Text

        /// <summary>HUD Prefab</summary>
        public GameObject hudGo;
        /// <summary>HUDText对象 用于显示HUD减血效果</summary>
        //private HUDText hudText;

		/// <summary>HUD挂点 判断HUD挂点是否存在 </summary>
		private bool isBlood;

		/// <summary>动态为角色增加HUD组件  Hud挂点为HudTarget</summary>
		IEnumerator InitHUD()
		{
			//while(!isBlood)
			//{
			//	if(HUDRoot.go == null)
			//	{
			//		HUDRoot.go = GameObject.FindGameObjectWithTag("HUD");
					yield return new WaitForSeconds(0.1f);
			//	}
			//	else
			//	{
			//		isBlood = true;
			//		GameObject child = NGUITools.AddChild(HUDRoot.go, hudGo);
			//		hudText = child.GetComponentInChildren<HUDText>();
			//		child.AddComponent<UIFollowTarget>().target = TransformHelper.FindChild(transform ,"HudTarget");
			//	}
			//}
		}

        #endregion

        public void Start()
        {
            StartCoroutine(InitHUD());
            HitFxPos = TransformHelper.FindChild(transform, "HitFxPos");
        }

        #region 行为

        /// <summary>受击 模板方法</summary>
        public virtual void OnDamage(int damage, GameObject killer)
        {
            //应用伤害
            var damageVal = ApplyDamage(damage, killer);
            //应用HUD 
            ApplyHUD(damageVal);
            //应用死亡
            if (HP <= 0)
                Dead(killer);
        }

        /// <summary>应用伤害</summary>
        public virtual int ApplyDamage(int damage, GameObject killer)
        {
            HP -= damage;
            return damage;
        }

        /// <summary>显示HUD 减血效果</summary>
        public virtual void ApplyHUD(int damageVal)
        {
            //hudText.Add(-damageVal, Color.red, 2);
            Debug.Log("效果-减血：" + damageVal);
        }

        /// <summary>
        /// 死亡
        /// </summary>
        /// <param name="killer">杀手</param>
        public virtual void Dead(GameObject killer)
        {

        }

        #endregion

    }

}