using System;
using System.Collections.Generic;
using UnityEngine;

namespace ARPGSimpleDemo.Character
{
    /// <summary>
    /// 小怪基础状态信息
    /// </summary>
    public class MonsterStatus : CharacterStatus
    {
        /// <summary>防御</summary>
        public int defence;
        /// <summary>躲避</summary>
        public int miss;
        
        /// <summary>死后贡献的经验值</summary>
        public int giveExp=100;

		/// <summary>小怪动画组件</summary>
		public CharacterAnimation chAnim = null;

		///add <summary>小怪血条组件</summary>
		//private Blood blood;

		public new void Start()
		{
			base.Start();
			chAnim = GetComponent<CharacterAnimation>();
			//add初始化小怪血条
			//blood = GetComponent<Blood>();
		}
		
		/// <summary>
		/// 重写父类的死亡方法 
        /// </summary>
        /// <param name="killer">杀手</param>
        public override void Dead(GameObject killer)
        {
            if (HP <= 0)
            {
				var status =killer.GetComponent<PlayerStatus>();
                if(status!= null)
                    status.CollectExp(this.giveExp);
				chAnim.PlayAnimation("death");
				//FightingStatistics.Instance.DeleteEnemy(gameObject);	//add怪物死亡后清除列表
                //销毁
                GameObject.Destroy(gameObject, 3);
            }
        }


		/// <summary>
		///add 重写父类的受击方法 
		/// </summary>
		/// <param name="killer">杀手</param>
		public override void OnDamage(int damage, GameObject killer)
		{
			base.OnDamage(damage,killer);
			//blood.SetBlood(HP, MaxHP);
		}

    }
}
