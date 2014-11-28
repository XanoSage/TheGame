
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityTools.MiniJSON;
using System.IO;

namespace UnityTools.Serialization {
	public class JSONSerializationToFile {
		public static void SaveToFile( string path, ISerializable val ) {
			JSONSerializer serializer = new JSONSerializer();
			val.Serialize( serializer );
			//TextWriter tw = File.CreateText(path);
			//tw.WriteLine( (string)serializer.GetSerializedValue() );
			//tw.Close();
		}
		public static T LoadFromFile<T>( string path ) where T : ISerializable, new() {
			T rezult = new T();
			LoadFromFile( path, rezult );
			return rezult;
		}
		public static void LoadFromFile( string path, ISerializable rezult ) {
			string jsonText = null;
			try
			{
				using (StreamReader sr = new StreamReader( path ))
				{
					jsonText = sr.ReadToEnd();
				}
			}
			catch
			{
				Debug.Log("The file could not be read:"+path);
				return;
			}
			JSONDeserializer serializer = new JSONDeserializer( jsonText );
			rezult.Serialize( serializer );
		}

		public static void SaveListToFile<T>( string path, List<T> val ) where T : ISerializable, new() {
			JSONSerializer serializer = new JSONSerializer();
			serializer.SerializeMember<T>( "list", ref val );
			//TextWriter tw = File.CreateText(path);
			//tw.WriteLine( (string)serializer.GetSerializedValue() );
			//tw.Close();
		}
		public static List<T> LoadListFromFile<T>( string path ) where T : ISerializable, new() {
			string jsonText = null;
			StreamReader sr = null;
			try
			{
				sr = new StreamReader( path );
				jsonText = sr.ReadToEnd();
			}
			catch
			{
				Debug.Log("The file could not be read:"+path);
				if (sr!=null)
					sr.Close();
				return new List<T>();
			}
			JSONDeserializer serializer = new JSONDeserializer( jsonText );
			List<T> rezult = new List<T>();
			serializer.SerializeMember<T>( "list", ref rezult );
			return rezult;
		}
		
		
	}
}