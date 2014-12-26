using System;
using Assets.Project.Content.Scripts.Timer;
using UnityEngine;
using System.Collections;
using UnityTools.Other;

public class TimerDisplayViewController : MonoBehaviour, IShowable {

	#region Variables

	private TimerDisplayViewModel _model;

	public SimpleTimer ParentTimer {get { return _model.ParentTimer; }}

	public event Action OnCancelButtonEvent;
	public event Action OnMainTimerEndEvent;

	#endregion

	#region MonoBehaviour Actions
	// Use this for initialization
	void Start ()
	{

		_model = GetComponent<TimerDisplayViewModel>();

		if (null == _model)
		{
			throw new MissingComponentException("TimerDisplayViewController.Start - cann't find TimerDisplayViewModel component");
		}

		SubscribeEvents();

		_model.IsTimerOnPause = false;

		OnPausePressedAction(_model.IsTimerOnPause);

		_model.TimerViewState = TimerDisplayViewModel.TimerState.Stop;

		Hide();

		//InitSimpleTimer();

	}
	
	// Update is called once per frame
	void Update () {

		UpdateTimerViewController();
	}

	void OnDestroy()
	{
		UnSubscribeEvents();
	}

	#endregion

	#region Actions

	private void SubscribeEvents()
	{
		_model.CancelButton.onClick = OnCancelButtonClick;
		_model.PauseButton.onClick = OnPauseButtonClick;
	}

	private void SubscribeTimerActions()
	{
		_model.MainTimerController.OnTimerChangeAction += UpdateMainTimer;
		_model.MainTimerController.OnTimerEndAction += OnMainTimerEnd;

		_model.IdleTimerController.OnTimerChangeAction += UpdateIdleTimer;
		_model.IdleTimerController.OnTimerEndAction += OnIdleTimerEnd;
	}

	private void UnSubscribeEvents()
	{
		_model.MainTimerController.OnTimerChangeAction -= UpdateMainTimer;
		_model.MainTimerController.OnTimerEndAction -= OnMainTimerEnd;

		_model.IdleTimerController.OnTimerChangeAction -= UpdateIdleTimer;
		_model.IdleTimerController.OnTimerEndAction -= OnIdleTimerEnd;
	}

	private void Init(SimpleTimer timer)
	{
		_model.ParentTimer = timer;

		_model.MainTimer = SimpleTimer.Create(timer);
		_model.IdleTimer = SimpleTimer.Create(timer.Interval);

		_model.MainTimerController = TimerController.Create(_model.MainTimer);
		_model.IdleTimerController = TimerController.Create(_model.IdleTimer);

		SubscribeTimerActions();

		UpdateIdleTimer(_model.IdleTimer.ToString());
		UpdateMainTimer(_model.MainTimer.ToString());

		_model.TimerViewState = TimerDisplayViewModel.TimerState.MainTimerWork;
	}

	private void OnCancelButtonClick(GameObject sender)
	{
		Debug.Log("TimerDisplayViewController.OnCancelButtonClick - OK");
		_model.TimerViewState = TimerDisplayViewModel.TimerState.Stop;

		if (null != OnCancelButtonEvent)
		{
			OnCancelButtonEvent();
		}
	}

	private void OnPauseButtonClick(GameObject sender)
	{
		Debug.Log("TimerDisplayViewController.OnPauseButtonClick - OK");

		_model.IsTimerOnPause = !_model.IsTimerOnPause;

		OnPausePressedAction(_model.IsTimerOnPause);

		if (_model.IsTimerOnPause)
		{
			if (_model.TimerViewState == TimerDisplayViewModel.TimerState.IdleTimerWork)
			{
				_model.TimerViewState = TimerDisplayViewModel.TimerState.IdleTimerOnPause;
			}

			if (_model.TimerViewState == TimerDisplayViewModel.TimerState.MainTimerWork)
			{
				_model.TimerViewState = TimerDisplayViewModel.TimerState.MainTimerOnPause;
			}
		}
		else
		{
			if (_model.TimerViewState == TimerDisplayViewModel.TimerState.IdleTimerOnPause)
			{
				_model.TimerViewState = TimerDisplayViewModel.TimerState.IdleTimerWork;
			}

			if (_model.TimerViewState == TimerDisplayViewModel.TimerState.MainTimerOnPause)
			{
				_model.TimerViewState = TimerDisplayViewModel.TimerState.MainTimerWork;
			}
		}
	}

	private void OnPausePressedAction(bool isPaused)
	{
		if (isPaused)
		{
			_model.PauseLabelIdle.gameObject.SetActive(false);
			_model.PauseLabelPressed.gameObject.SetActive(false);

			_model.ContinueLabelIdle.gameObject.SetActive(true);
			_model.ContinueLabelPressed.gameObject.SetActive(true);

			if (_model.TimerViewState == TimerDisplayViewModel.TimerState.MainTimerWork)
			{
				_model.TimerViewState = TimerDisplayViewModel.TimerState.MainTimerOnPause;
			}

			if (_model.TimerViewState == TimerDisplayViewModel.TimerState.IdleTimerWork)
			{
				_model.TimerViewState = TimerDisplayViewModel.TimerState.IdleTimerOnPause;
			}
		}
		else
		{
			_model.PauseLabelIdle.gameObject.SetActive(true);
			_model.PauseLabelPressed.gameObject.SetActive(true);

			_model.ContinueLabelIdle.gameObject.SetActive(false);
			_model.ContinueLabelPressed.gameObject.SetActive(false);

			if (_model.TimerViewState == TimerDisplayViewModel.TimerState.MainTimerOnPause)
			{
				_model.TimerViewState = TimerDisplayViewModel.TimerState.MainTimerWork;
			}

			if (_model.TimerViewState == TimerDisplayViewModel.TimerState.IdleTimerOnPause)
			{
				_model.TimerViewState = TimerDisplayViewModel.TimerState.IdleTimerWork;
			}
		}
	}

	#region Timer Controller Action

	public void InitSimpleTimer()
	{
		SimpleTimer simple = SimpleTimer.Create(00, 01, 01);
		simple.CalculateNotificationsCount();
		//simple.NotificationCount = 5;

		Debug.Log("TimerDisplayViewController.InitSimpleTimer - notification coint is :" + simple.NotificationCount);


		Init(simple);
	}

	private void UpdateMainTimer(string currentTimerTime)
	{
		_model.MainTimerLabel.text = currentTimerTime;
	}

	private void UpdateIdleTimer(string idleTimerTime)
	{
		_model.IdleTimerLabel.text = idleTimerTime;
	}

	public void ResetTimer()
	{
		UnSubscribeEvents();

		_model.MainTimer = SimpleTimer.Create(_model.ParentTimer);
		_model.IdleTimer = SimpleTimer.Create(_model.ParentTimer.Interval);

		_model.MainTimerController = TimerController.Create(_model.MainTimer);
		_model.IdleTimerController = TimerController.Create(_model.IdleTimer);

		SubscribeTimerActions();

		UpdateIdleTimer(_model.IdleTimer.ToString());
		UpdateMainTimer(_model.MainTimer.ToString());

		_model.TimerViewState = TimerDisplayViewModel.TimerState.MainTimerWork;
	}

	private void UpdateTimerViewController()
	{
		if (_model.IdleTimer == null || _model.MainTimer == null || _model.IdleTimerController == null ||
		    _model.MainTimerController == null)
			return;

		switch (_model.TimerViewState)
		{
			case TimerDisplayViewModel.TimerState.Stop:
				break;

			case TimerDisplayViewModel.TimerState.IdleTimerOnPause:
				break;

			case TimerDisplayViewModel.TimerState.IdleTimerWork:

				_model.IdleTimerController.Update();
				UpdateSpriteProgress(_model.IdleTimer, false);

				break;

			case TimerDisplayViewModel.TimerState.MainTimerOnPause:
				break;

			case TimerDisplayViewModel.TimerState.MainTimerWork:

				_model.MainTimerController.Update();
				UpdateSpriteProgress(_model.MainTimer, true);

				break;

			case TimerDisplayViewModel.TimerState.ShowNotifications:



				break;
		}
	}

	public void StartMainTimer()
	{
		_model.TimerViewState = TimerDisplayViewModel.TimerState.MainTimerWork;
	}

	public void StartIdleTimer()
	{
		_model.TimerViewState = TimerDisplayViewModel.TimerState.IdleTimerWork;
	}

	private void OnMainTimerEnd()
	{
		Debug.Log("TimerDisplayViewController.OnMainTimerEnd - OK");
		_model.TimerViewState = TimerDisplayViewModel.TimerState.ShowNotifications;

		if (null != OnMainTimerEndEvent)
		{
			OnMainTimerEndEvent();
		}
	}

	private void OnIdleTimerEnd()
	{
		Debug.Log("TimerDisplayViewController.OnIdleTimerEnd - OK");

		_model.TimerViewState = TimerDisplayViewModel.TimerState.MainTimerWork;
		ResetTimer();
	}

	private void UpdateSpriteProgress(SimpleTimer timer, bool isBackDirection)
	{
		if (_model.ProgressSprite == null)
		{
			return;
		}

		if (isBackDirection)
		{
			_model.ProgressSprite.fillAmount = 1 - timer.CurrentTotalInSeconds/timer.TotalInSeconds;
		}
		else
		{
			_model.ProgressSprite.fillAmount = timer.CurrentTotalInSeconds/timer.TotalInSeconds;
		}
	}
	#endregion

	#endregion

	#region IShowable Implementation

	public void Show()
	{
		Visible = true;

		_model.ElementContainer.gameObject.SetActive(true);
	}

	public void Hide()
	{
		Visible = false;

		_model.ElementContainer.gameObject.SetActive(false);
	}

	public bool Visible { get; private set; }

	#endregion
}
