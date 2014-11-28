using UnityEngine;
using System.Collections.Generic;

namespace UnityTools.Serialization {
	public interface ISerializable {
		void Serialize( Serializer serializer );
	}
	public abstract class Serializer {
		public abstract void SerializeMember<T>( string key, ref T val ) where T : ISerializable, new();
		public abstract void SerializeMember<T>( string key, ref List<T> val ) where T : new();
		public abstract void SerializeMember( string key, ref int val );
		public abstract void SerializeMember( string key, ref long val );
		public abstract void SerializeMember( string key, ref float val );
		public abstract void SerializeMember( string key, ref string val );
		public abstract void SerializeMember( string key, ref bool val );
		public abstract void SerializeStringList( string key, ref List<string> val );
		
		public abstract object GetSerializedValue();
		
		
	}
}