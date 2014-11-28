using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace UnityTools.OnGUIEnhancers {
	public class OnGUITabView {
	
	    readonly OnGUICombobox _tabsCombobox;

        public OnGUITabView(List<IOnGUIEditableItem> tabObjects, IOnGUIEditableItem selectedTab)
        {
            ArrayList tabObjectsArray = new ArrayList();
            foreach (var onGuiEditableItem in tabObjects)
            {
                tabObjectsArray.Add(onGuiEditableItem);
            }
            _tabsCombobox = new OnGUICombobox(null, tabObjectsArray, selectedTab, null);
		}
        public void AddTab(IOnGUIEditableItem tabObject)
        {
			_tabsCombobox.AddItem(tabObject);
		}
        public void RemoveTab(IOnGUIEditableItem tabObject)
        {
			int ind = _tabsCombobox.GetInd(tabObject);
			if (ind==-1)
				return;
			_tabsCombobox.RemoveAt(ind);
		}
		public int SelectedTabInd {
			get { return _tabsCombobox.SelectedInd;	}
		}
		public void DrawHorizontal() {
			GUILayout.BeginVertical();
			_tabsCombobox.DrawHorizontal();
			DrawTabContent();
			GUILayout.EndVertical();
		}
		public void DrawVertical(params GUILayoutOption[] tabOptions) {
			GUILayout.BeginHorizontal();
			_tabsCombobox.DrawVertical(tabOptions);
			DrawTabContent();
			GUILayout.EndHorizontal();
		}
		private void DrawTabContent() {
			IOnGUIEditableItem item = _tabsCombobox.SelectedItem as IOnGUIEditableItem;
		    if (item == null)
		        return;

			item.DrawEditingGUI();
		}

	    public void Update()
	    {
            IOnGUIEditableItem item = _tabsCombobox.SelectedItem as IOnGUIEditableItem;
            if (item == null)
                return;

            item.UpdateEditing();
	    }
	}
}
