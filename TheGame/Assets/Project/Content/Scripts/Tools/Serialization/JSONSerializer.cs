using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityTools.MiniJSON;

namespace UnityTools.Serialization {
	public class JSONSerializer : Serializer {
		private Dictionary<string, object> hash;
		public JSONSerializer() {
			hash = new Dictionary<string, object>();
		}
		
		public override void SerializeMember<T>( string key, ref T val ) {
			if (val==null)
				return;
			JSONSerializer serializer = new JSONSerializer();
			val.Serialize( serializer );
			hash.Add( key, serializer.hash );
		}
		public override void SerializeMember<T>( string key, ref List<T> val ) {
			if (val==null)
				return;
			List<object> list = new List<object>();
			foreach (T item in val) {
				if (item is ISerializable) {
					JSONSerializer serializer = new JSONSerializer();
					(item as ISerializable).Serialize( serializer );
					list.Add( serializer.hash );
				} else
					list.Add( item );
			}
			hash.Add( key, list );
		}
		public override void SerializeStringList (string key, ref List<string> val)
		{
			if (val==null)
				return;
			List<object> list = new List<object>();
			foreach (string item in val)
				list.Add( item );
			hash.Add( key, val );
		}
		public override void SerializeMember( string key, ref int val ){
			hash.Add( key, val );
		}
		public override void SerializeMember( string key, ref long val ){
			hash.Add( key, val );
		}
		public override void SerializeMember( string key, ref float val ){
			hash.Add( key, val );
		}
		public override void SerializeMember( string key, ref string val ){
			hash.Add( key, val );
		}
		public override void SerializeMember( string key, ref bool val ){
			hash.Add( key, val );
		}
		
		public override object GetSerializedValue ()
		{
			return Json.Serialize( hash );
		}
	}
}