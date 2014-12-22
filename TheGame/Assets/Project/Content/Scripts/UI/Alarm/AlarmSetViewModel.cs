using UnityEngine;
using System.Collections;

public class AlarmSetViewModel : MonoBehaviour {

	#region Variables

	public UIWidget ElementContainer;

	public SwitchOnOffController SoundSwitchOnOff;

	public IPNumberPicker HourPicker;
	public IPNumberPicker MinutePicker;

	public UIEventListener CancelButton;
	public UIEventListener ApplyButton;

	[HideInInspector] public int HourSelect;
	[HideInInspector] public int MinuteSelect;

	#endregion
}
