using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace UnityTools.Pooling {
	public class Pool : MonoBehaviour {
			
		public static Pool instance;
		private static List<PoolItem> items;	
		private static Transform pooledItemsParent;
		
		#region Initialization
		[System.Serializable]
		public class ItemCountPair {
		    public PoolItem itemPrefab;
		    public int count;
		}
		public List<ItemCountPair> startItemsDescription;
		
		void Awake () {
			instance = this;
			InitFirst();
		}
	
		public void InitFirst() {
			pooledItemsParent = transform;
			items = new List<PoolItem>();
					
			foreach (ItemCountPair pair in startItemsDescription) {
				for (int i=0;i<pair.count;i++) {
					if (pair.itemPrefab != null)
						InstantiateItem(pair.itemPrefab);
				}
			}
		}

		public static void AddMassive(PoolItem item, int count)
		{
			for (int i = 0; i < count; i++)
			{
				InstantiateItem(item);
			}
		}

		#endregion
		
		private static int GetObjectIndex(PoolItem itemPrefab) {
			for (int i = 0; i < items.Count; i++) {
				if (items[i].EqualsTo(itemPrefab)) {
					return i;
				}
			}
					
			InstantiateItem(itemPrefab);
	//		Debug.LogWarning(string.Format("Not enough {0} in pool, instantiate used", itemPrefab.ToString()));
			return items.Count-1;
		}
		
		private static void InstantiateItem(PoolItem itemPrefab) {
			PoolItem newItem = Instantiate(itemPrefab) as PoolItem;
			Pool.Push(newItem);
		}
		
		public static PoolItem Pop(PoolItem itemPrefab) {
			if (itemPrefab == null) {
				Debug.LogError("Pool.Pop got a null prefab as a parameter");
				return null;
			}
			int index = GetObjectIndex(itemPrefab);
			
			if (index == -1) {
				Debug.LogError(string.Format("POP. No such object in pool: {0}", itemPrefab));
				return null;
			}
			
			PoolItem item = items[index];
			items.RemoveAt(index);
			item.transform.parent = null;
			item.Activate();	
			return item;
		}
		
		public static void Push(PoolItem item) {
			item.Deactivate();
			item.transform.parent = pooledItemsParent;
			items.Add(item);
		}
	}
}