using UnityEngine;
using System.Collections;

public class SwitchOnOffModel : MonoBehaviour {

	#region Variables

	public UIEventListener SwitchOnOffButton;

	public UISprite Toggle;
	public UISprite Foreground;

	[HideInInspector] public bool IsSwitchOn;
	[HideInInspector] public bool IsAnimated;

	#endregion
}
