using UnityEngine;
using System.Collections;

namespace UnityTools.OnGUIEnhancers {
	public class OnGUICombobox {
	
		private ArrayList items;
		private object selectedItem;
		private string name;
		public object SelectedItem {
			get { return selectedItem; }
			set { 
				selectedItem = value;
				if (OnItemSelected!=null)
					OnItemSelected(selectedItem);
			}
		}
		public int SelectedInd {
			get { return items.IndexOf( selectedItem ); }
			set { SelectedItem = items[value]; }
		}
		public int GetInd(object item) {
			return items.IndexOf(item);
		}
		System.Action<object> OnItemSelected;
		private string cancel = "cancel";
		private bool alwaysOpen;
		public OnGUICombobox(string name, ArrayList items, object selectedItem, System.Action<object> OnItemSelected) {
			if (string.IsNullOrEmpty(name))
				alwaysOpen = true;
			this.name = name;
			this.items = (ArrayList)items.Clone();
			this.selectedItem = selectedItem;
			this.OnItemSelected = OnItemSelected;
			if (!alwaysOpen)
				this.items.Insert( 0, cancel );
		}
		public void AddItem(object item) {
			items.Add(item);
		}
		public void RemoveAt(int ind) {
			items.RemoveAt(ind);
		}
		private Vector2 scrollPos;
		private bool opened;
		public void DrawHorizontal(params GUILayoutOption[] options) {
			Draw( true, options );
		}
		public void DrawVertical(params GUILayoutOption[] options) {
			Draw( false, options );
		}
		private void Draw(bool horizontal, params GUILayoutOption[] options) {
			if (opened || alwaysOpen) {
				if (horizontal)
					DrawHorizontalOpened(options);
				else
					DrawVerticalOpened(options);
			}
			else
			{
				string caption;
				if (selectedItem == null || selectedItem == cancel)
					caption = name;
				else
					caption = selectedItem.ToString();
				if (GUILayout.Button( caption ))
					opened = true;
			}
		}
		private void DrawHorizontalOpened(params GUILayoutOption[] options) {
			GUILayout.BeginHorizontal(options);
			scrollPos = GUILayout.BeginScrollView(scrollPos);
			DrawContents();
			GUILayout.EndScrollView();
			GUILayout.EndHorizontal();
		}
		private void DrawVerticalOpened(params GUILayoutOption[] options) {
			GUILayout.BeginVertical(options);
			scrollPos = GUILayout.BeginScrollView(scrollPos);
			DrawContents();
			GUILayout.EndScrollView();
			GUILayout.EndVertical();
		}
		private void DrawContents() {
			foreach (object item in items) {
				GUIStyle style = GUI.skin.button;
				if (selectedItem==item) {
					style.normal.textColor = Color.green;
					style.hover.textColor = Color.green;
				}
				else {
					style.normal.textColor = Color.white;
					style.hover.textColor = Color.white;
				}
				if (GUILayout.Button(item.ToString(), style)) {
					if (item != cancel)
						selectedItem = item;
					if (item != cancel && OnItemSelected!=null)
						OnItemSelected(item);
					if (!alwaysOpen)
						opened = false;
				}
				style.normal.textColor = Color.white;
				style.hover.textColor = Color.white;
			}
		}
	}
}