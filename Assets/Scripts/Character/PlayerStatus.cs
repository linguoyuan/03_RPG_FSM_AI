using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace ARPGSimpleDemo.Character
{
    public class PlayerStatus : CharacterStatus
    {
        /// <summary>防御</summary>  
        public int defence = 1;
        /// <summary>等级</summary> 
        public int level = 1;
        /// <summary>经验</summary>
		public int exp = 1;
        /// <summary>最大经验</summary>
        public int maxExp = 10;
		/// <summary>力量</summary>
		public int force = 10;
		/// <summary>智力</summary>
		public int intellect = 10;
        /// <summary>角色动画组件</summary>
        public CharacterAnimation chAnim = null;

        public new void Start()
        {
            base.Start();
            chAnim = GetComponent<CharacterAnimation>();
        }

        public override int ApplyDamage(int damage, GameObject killer)
        {
            var damageVal = damage - defence;

            if (damageVal > 0)
            {
                HP -= damageVal;
				//UIBattleHead uiBattleHead = UIManager.Instance.GetUI<UIBattleHead>(UIName.UIBattleHead);
				//uiBattleHead.SetHp(HP, MaxHP);	//add更新血条	
                return damageVal;
            }
            return 0;
        }

        public void LevelUp()
        {

        }

        public void CollectExp(int exp)
        {
            this.exp += exp;
        }

        public override void Dead(GameObject killer)
        {
            chAnim.PlayAnimation("death");
            Debug.Log("英雄死亡");
			//UIManager.Instance.SetVisible(UIName.UIBattleOver, true);	////add显示失败面板
        }
    }
}
