using System;
using System.Collections.Generic;
//using LinqTools;
using System.Text;

namespace UnityTools.AI {
	public enum TaskManagerState {
		None, 
		Ready, 
		Working, 
		Finished,
 		Pause,
		Error
	}
}
