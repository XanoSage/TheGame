using UnityEngine;
using System.Collections;

public class DraggingScalingCamera : MonoBehaviour {

	private Transform cameraTransform;
	// Use this for initialization
	void Start () {
		cameraTransform = transform;
	}


	// Update is called once per frame
	void Update () {
		UpdateDragging();
		UpdateScaling();
	}

	#region Dragging
	private bool mouseDown;
	private Vector3 mousePos;
	private void UpdateDragging() {
		if (Input.GetMouseButton(0) != mouseDown) {
			mouseDown = !mouseDown;
		} else if (mouseDown) {
			Vector3 deltaPos = Camera.main.ScreenToWorldPoint( Input.mousePosition ) - Camera.main.ScreenToWorldPoint( mousePos );
			cameraTransform.position -= deltaPos;
		}
		if (mouseDown)
			mousePos = Input.mousePosition;
	}
	#endregion

	#region Scale by scroll
	private const float scaleSensitivity = 1.01f;
	private void UpdateScaling() {
		float wheelValue = Input.GetAxisRaw( "Mouse ScrollWheel" );
		if (Mathf.Abs( wheelValue ) < 0.01f)
			return;
		Camera.main.orthographicSize *= (1+wheelValue*scaleSensitivity);
	}
	#endregion
}
