using Assets.Project.Content.Scripts.Timer;
using UnityEngine;
using System.Collections;

public class TimerDisplayViewModel : MonoBehaviour {

	#region Variables

	public enum TimerState
	{
		Stop,
		ShowNotifications,
		MainTimerWork,
		MainTimerOnPause,
		IdleTimerWork, 
		IdleTimerOnPause
	}

	public UIWidget ElementContainer;

	public UIEventListener CancelButton;
	public UIEventListener PauseButton;

	public UILabel ContinueLabelIdle;
	public UILabel ContinueLabelPressed;

	public UILabel PauseLabelIdle;
	public UILabel PauseLabelPressed;

	public UILabel MainTimerLabel;
	public UILabel IdleTimerLabel;

	[HideInInspector] public bool IsTimerOnPause;
	[HideInInspector] public TimerState TimerViewState;

	[HideInInspector] public SimpleTimer ParentTimer;
	[HideInInspector] public SimpleTimer MainTimer;
	[HideInInspector] public SimpleTimer IdleTimer;

	[HideInInspector] public TimerController MainTimerController;
	[HideInInspector] public TimerController IdleTimerController;

	#endregion
}
