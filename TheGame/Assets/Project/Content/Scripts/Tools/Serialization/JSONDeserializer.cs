
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityTools.MiniJSON;

namespace UnityTools.Serialization {
	public class JSONDeserializer : Serializer {
		private Dictionary<string, object> hash;
		public JSONDeserializer( string serializedValue) {
			hash = (Dictionary<string, object>)Json.Deserialize( serializedValue );
		}
		public JSONDeserializer( Dictionary<string, object> hash) {
			this.hash = hash;
		}
		
		public override void SerializeMember<T>( string key, ref T val ) {
			if (!hash.ContainsKey(key))
				return;
			if (val==null)
				val = new T();
			
			JSONDeserializer serializer = new JSONDeserializer( (Dictionary<string, object>)hash[key] );
			val.Serialize( serializer );
		}
		public override void SerializeMember<T>( string key, ref List<T> val ) {
			if (!hash.ContainsKey(key))
				return;
			if (val==null)
				val = new List<T>();
			val.Clear();			
			List<object> list = (List<object>)hash[key];
			foreach (object item in list) {
				if (typeof(ISerializable).IsAssignableFrom(typeof(T))) {
					T deserializedItem = new T();
					JSONDeserializer serializer = new JSONDeserializer((Dictionary<string, object>)item);
					(deserializedItem as ISerializable).Serialize( serializer );
					val.Add( deserializedItem );
				} else if (typeof(T)==typeof(int))
					AddToList<T>(val, (int)(long)item); //val.Add( (int)(long)item );
				else if (typeof(T)==typeof(float))
					AddToList<T>(val, (float)(double)item);
				else
					val.Add( (T)item );
			}
		}
		public override void SerializeStringList (string key, ref List<string> val)
		{
			if (!hash.ContainsKey(key))
				return;
			if (val==null)
				val = new List<string>();
			val.Clear();			
			List<object> list = (List<object>)hash[key];
			foreach (object item in list)
				val.Add( (string)item );
		}
		private void AddToList<T>(List<T> list, object val) {
			list.Add( (T)val );
		}
		public override void SerializeMember( string key, ref int val ){
			if (!hash.ContainsKey(key))
				return;
			val = (int)(long)hash[key];
		}
		public override void SerializeMember( string key, ref long val ){
			if (!hash.ContainsKey(key))
				return;
			val = (long)hash[key];
		}
		public override void SerializeMember( string key, ref float val ){
			if (!hash.ContainsKey(key))
				return;
			object obj = hash[key];
			if (obj.GetType()==typeof(long))
				val = (float)(long)obj;
			else
				val = (float)(double)hash[key];
		}
		public override void SerializeMember( string key, ref string val ){
			if (!hash.ContainsKey(key))
				return;
			val = (string)hash[key];
		}
		public override void SerializeMember( string key, ref bool val ){
			if (!hash.ContainsKey(key))
				return;
			val = (bool)hash[key];
		}
		
		public override object GetSerializedValue ()
		{
			return Json.Serialize( hash );
		}
	}
}