using UnityEngine;
using System.Collections;

public class MainAppViewModel : MonoBehaviour {

	#region Variables

	public enum MainSetting
	{
		Alarm,
		Timer
	}

	public UIWidget ElementContainer;

	public UIToggle AlarmToggle;
	public UIToggle TimerToggle;

	public UILabel AlarmTimeShowLabel;
	public UILabel TimerTimeShowLabel;
	public UILabel MessageShowCountLabel;

	public UIEventListener ChangeShowTimeButton;
	public UIWidget ShowTimeIddleContainer;
	public UIWidget ShowTimePressedContainer;

	[HideInInspector] public MainSetting Setting;

	#endregion
}
