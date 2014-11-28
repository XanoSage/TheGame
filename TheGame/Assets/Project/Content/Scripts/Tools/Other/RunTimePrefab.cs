using UnityEngine;
using System.Collections;

public class RunTimePrefab : MonoBehaviour {

	[SerializeField] private bool _draggingFromHierarchyCreation;
	[SerializeField] private bool _destroy;

	protected virtual void OnDraggedFromHierarchy() {
	}

	protected virtual void OnDestroyClicked() {
		Destroy( gameObject );
	}

	protected virtual void Awake() {
		if (_draggingFromHierarchyCreation)
			OnDraggedFromHierarchy();
	}

	protected virtual void Update() {
		if (_destroy)
			OnDestroyClicked();
	}
}
