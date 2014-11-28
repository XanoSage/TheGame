using UnityEngine;
using System.Collections;

namespace UnityTools.Other {
	public interface ISerializable {
		void Serialize (Tools.DataBuffer buffer);
		void Deserialize (Tools.DataBuffer buffer);
	}
}
