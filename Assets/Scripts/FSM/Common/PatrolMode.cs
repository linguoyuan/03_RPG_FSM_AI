using System;

namespace AI.FSM
{
    /// <summary>巡逻模式</summary>
	public enum PatrolMode
	{
        /// <summary>单程</summary>
        Once, 
        /// <summary>循环</summary>
        Loop,
        /// <summary>往返</summary>
        PingPong,
	}
}
