/*角色动画控制类
 *职责：负责动画切换，动画参数的调整，事画事件的处理
 */
using System;
using System.Collections.Generic;
using UnityEngine;

namespace ARPGSimpleDemo.Character
{
    /// <summary>
    /// 角色动画控制类
    /// </summary>
	public class CharacterAnimation : MonoBehaviour 
	{
        /// <summary>引用动画组件</summary>
        private Animator anim;
        /// <summary>引用技能系统</summary>
        //private CharacterSkillSystem chSystem;
        /// <summary>当前动画参数</summary>
        public string currentAnim = "idle";
        /// <summary>是否在播放攻击类动画</summary>
        //public bool isAttack = false;

        private AnimationClip clip;

        public void Start()
        {
            anim = GetComponent<Animator>();
        }
     
        /// <summary>
        /// 播放动画
        /// </summary>
        /// <param name="paramName">动画状态机Bool型参数名</param>
        public  void PlayAnimation(string paramName)
        {
            anim.SetBool(paramName, true);
            anim.SetBool(currentAnim, false);
            currentAnim = paramName;
        }

        //根据动画clip名字播放
        public void PlayAnimationByName(string aniName)
        {
            Debug.Log("播放动画："+ aniName);           
            anim.Play(aniName);
        }
      
        //根据动画状态名称播放
        public void PlayAniByNameLoop(string state)
        {
            //if (anim.GetCurrentAnimatorClipInfo(1)[0].clip.name == aniName)
            if (anim.GetCurrentAnimatorStateInfo(0).IsName(state) == false)
            {
                Debug.Log(state);
                anim.CrossFade(state, 0f);
            }
        }
    }
}
