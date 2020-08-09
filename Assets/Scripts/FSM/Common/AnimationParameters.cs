using System;
using System.Collections.Generic;
using UnityEngine;

namespace AI.FSM
{
    /// <summary>为动画状态机提供的参数</summary>
    [Serializable]
	public class AnimationParameters
	{
        public string Idle = "idle";
        public string Dead = "dead";
        public string Run = "run";
        public string FightIdle = "fightIdle";
        public string Attack = "attack";
        public string Walk = "walk";
	}

    [Serializable]
    public class AnimationNames
    {
        public string Idle = "Idle";
        public string Dead = "Death";
        public string Run = "Run";
        //public string FightIdle = "Attack";
        public string Skill = "Skill";
        public string Attack = "Attack";
        public string Stand = "Stand";
        public string Walk = "Walk";
    }
}
