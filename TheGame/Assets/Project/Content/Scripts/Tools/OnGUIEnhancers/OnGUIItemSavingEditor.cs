using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityTools.Serialization;

namespace UnityTools.OnGUIEnhancers
{
	public interface IOnGUIEditableSaveableItem : ISerializable, IOnGUIEditableItem {
	}
	public class OnGUIItemSavingEditor<T> : OnGUIItemEditor where T : class, IOnGUIEditableSaveableItem, new() {
		private string serializingPath;
		public OnGUIItemSavingEditor(string serializingPath, IOnGUIEditableItemsContainer container) : base(container) 
		{
			this.serializingPath = serializingPath;
		}
		protected override void DrawAllItemsEditingControls ()
		{
			GUILayout.BeginHorizontal();
			base.DrawAllItemsEditingControls();
			if (GUILayout.Button("Save all")) {
				Save();
			}
			if (GUILayout.Button("Load all")) {
				Load();
			}
			GUILayout.EndHorizontal();
		}

		public void Save ()
		{
			List<T> serializableList = new List<T>();
			foreach (IOnGUIEditableItem item in Items) {
				T serializable = item as T;
				if (serializable==null)
					Debug.LogError("OnGUIEditableSaveableItem.Save");
				serializableList.Add(serializable);
			}
			JSONSerializationToFile.SaveListToFile<T>( serializingPath, serializableList );
		}

		public void Load ()
		{
			List<T> serializableList = JSONSerializationToFile.LoadListFromFile<T>( serializingPath );
			while (Items.Count>0)
				RemoveItem( Items[0] );
			foreach (T item in serializableList)
				AddItem( item );
		}
	}
}
