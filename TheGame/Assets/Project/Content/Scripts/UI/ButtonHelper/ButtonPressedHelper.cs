using UnityEngine;
using System.Collections;

public class ButtonPressedHelper : MonoBehaviour {

	#region Variables

	[SerializeField] private UIEventListener _button;

	[SerializeField] private UIWidget _buttonPressed;

	#endregion

	#region MonoBehaviour Actions

	// Use this for initialization
	void Start ()
	{
		_button.onPress = OnButtonPressed;
	}
	
	// Update is called once per frame
	void Update () {

	}

	#endregion

	#region Actions

	private void OnButtonPressed(GameObject sender, bool isPressed)
	{
		Debug.Log("ButtonPressedHelper.OnButtonPressed - OK");

		if (isPressed)
		{
			_buttonPressed.gameObject.SetActive(true);
		}
		else
		{
			_buttonPressed.gameObject.SetActive(false);
		}
	}

	#endregion
}
