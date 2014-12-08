using UnityEngine;
using System.Collections;

public class TimerSetViewModel : MonoBehaviour {

	#region Variables

	public UIWidget ElementContainer;

	public UIEventListener CancelButton;
	public UIEventListener SaveButton;

	public IPTextPicker TextPicker;
	public IPNumberPicker HourPicker;
	public IPNumberPicker MinutePicker;

	[HideInInspector] public int HourSelect;
	[HideInInspector] public int MinuteSelect;

	[HideInInspector] public int IntervalSelectionMinutes;

	#endregion
}
