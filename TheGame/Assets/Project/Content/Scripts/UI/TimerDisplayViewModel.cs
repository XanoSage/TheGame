using UnityEngine;
using System.Collections;

public class TimerDisplayViewModel : MonoBehaviour {

	#region Variables

	public UIWidget ElementContainer;

	public UIEventListener CancelButton;
	public UIEventListener PauseButton;

	public UILabel ContinueLabelIdle;
	public UILabel ContinueLabelPressed;

	public UILabel PauseLabelIdle;
	public UILabel PauseLabelPressed;

	[HideInInspector] public bool IsTimerOnPause;

	#endregion
}
