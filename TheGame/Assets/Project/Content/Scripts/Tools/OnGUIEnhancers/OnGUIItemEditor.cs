using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace UnityTools.OnGUIEnhancers {
	public interface IOnGUIEditableItem {
		IOnGUIEditableItem Clone ();
		void DrawEditingGUI();
		void UpdateEditing();
	}
	public interface IOnGUIEditableItemsContainer {
		List<IOnGUIEditableItem> Items { get; }
		IOnGUIEditableItem AddItem();
		void AddItem(IOnGUIEditableItem item);
		IOnGUIEditableItem CloneItem(IOnGUIEditableItem item);
		void RemoveItem(IOnGUIEditableItem item);
		void UpdateEditing();
	}
	public class OnGUIItemEditor {	
		private IOnGUIEditableItemsContainer container;
		private OnGUITabView tabs;
		public OnGUIItemEditor(IOnGUIEditableItemsContainer container) {
			this.container = container;
			tabs = new OnGUITabView( container.Items, null );
		}

		public int SelectedInd { get { return tabs.SelectedTabInd; } }

		protected List<IOnGUIEditableItem> Items { get { return container.Items; } }
		protected void AddItem( IOnGUIEditableItem item ) {
			container.AddItem( item );
			tabs.AddTab( item );
		}

		protected void RemoveItem( IOnGUIEditableItem item ) {
			container.RemoveItem( item );
			tabs.RemoveTab( item );
		}

		protected virtual void DrawAllItemsEditingControls ()
		{
			if (GUILayout.Button("Add new item")) {
				IOnGUIEditableItem item = container.AddItem();
				tabs.AddTab( item );
			}
			if (SelectedInd!=-1) {
				if (GUILayout.Button("Remove selected")) {
					IOnGUIEditableItem item = Items[SelectedInd];
					tabs.RemoveTab( item );
					container.RemoveItem( item );
				}
			}
		}

		private void DrawItemEditing( IOnGUIEditableItem item ) {
			GUILayout.BeginVertical();
			item.DrawEditingGUI();
			GUILayout.BeginHorizontal();
			if (GUILayout.Button("Clone")) {
				IOnGUIEditableItem clone = container.CloneItem(item);
				tabs.AddTab( clone );
			}
			if (GUILayout.Button("Remove")) {
				container.RemoveItem( item );
				tabs.RemoveTab( item );
			}
			GUILayout.EndHorizontal();
			GUILayout.EndVertical();
		}
		public void DrawEditingGUI() {
			GUILayout.BeginVertical();
			tabs.DrawVertical(GUILayout.MaxWidth(300));
			DrawAllItemsEditingControls();
			GUILayout.EndVertical();
		}
		
		public virtual void UpdateEditing() {
			int ind = tabs.SelectedTabInd;
			if(ind==-1)
				return;
			IOnGUIEditableItem item = Items[tabs.SelectedTabInd];
			item.UpdateEditing();
		}
	}
}
