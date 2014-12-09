using System;
using System.Collections.Generic;
using Assets.Project.Content.Scripts.Timer;
using UnityEngine;
using System.Collections;
using UnityTools.Other;

public class MainAppViewController : MonoBehaviour, IShowable  {

	#region Variables

	private MainAppViewModel _model;

	public event Action OnMainAppViewAlarmChangeEvent;
	public event Action OnMainAppViewTimerChangeEvent;

	public event Action OnMainAppViewChangeChapterEvent;

	#endregion

	#region MonoBehaviour action
	// Use this for initialization
	void Start ()
	{

		_model = GetComponent<MainAppViewModel>();

		if (null == _model)
		{
			throw new MissingComponentException("MainAppViewController.Start - cann't find MainAppViewModel for _model");
		}

		SubscribeEvents();

		ShowAlarmTimeLabel();
		HideTimerTimeLabel();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	private void OnDestroy()
	{
		UnsubscribeEvents();
	}
	#endregion

	#region Actions
	
	private void SubscribeEvents()
	{
		_model.ChangeShowTimeButton.onClick = OnChangeShowTimeClick;
		_model.ChangeShowTimeButton.onPress = OnChangeShowTimePressed;

		_model.ChangeChapterButton.onClick = OnChangeChapterButtonClick;

		_model.AlarmToggle.onChange = new List<EventDelegate>();
		_model.AlarmToggle.onChange.Add(new EventDelegate(OnAlarmToggleChange));

		_model.TimerToggle.onChange = new List<EventDelegate>();
		_model.TimerToggle.onChange.Add(new EventDelegate(OnTimerToggleChange));
	}

	private void UnsubscribeEvents()
	{
		_model.AlarmToggle.onChange.Clear();

		_model.TimerToggle.onChange.Clear();
	}

	public void SetTimer(int hour, int minutes, int interval)
	{
		_model.TheTimer = SimpleTimer.Create(hour, minutes, interval);
		InitTimerDisplayLabel();
		InitNotificationCountLabel();
	}

	private void InitTimerDisplayLabel()
	{
		if (_model.TheTimer == null)
			return;

		string str = string.Format("{0:D2}:{1:D2}", _model.TheTimer.Hour, _model.TheTimer.Minutes);

		_model.TimerTimeShowLabel.text = str;
	}

	private void InitNotificationCountLabel()
	{
		if (_model.TheTimer == null)
		{
			return;
		}

		_model.TheTimer.CalculateNotificationsCount();

		_model.MessageShowCountLabel.text = _model.TheTimer.NotificationCount.ToString();
	}

	#region Button Handler Actions

	private void OnChangeShowTimeClick(GameObject sender)
	{
		Debug.Log("MainAppViewController.OnChangeShowTimeClick - OK");

		if (_model.Setting == MainAppViewModel.MainSetting.Alarm)
		{
			if (null != OnMainAppViewAlarmChangeEvent)
			{
				OnMainAppViewAlarmChangeEvent();
			}
		}
		else
		{
			if (null != OnMainAppViewTimerChangeEvent)
			{
				OnMainAppViewTimerChangeEvent();
			}
		}
	}

	private void OnChangeShowTimePressed(GameObject sender, bool isPressed)
	{
		Debug.Log("MainAppViewController.OnChangeShowTimePressed - OK");

		if (isPressed)
		{
			_model.ShowTimePressedContainer.gameObject.SetActive(true);
		}
		else
		{
			_model.ShowTimePressedContainer.gameObject.SetActive(false);
		}
	}

	private void OnAlarmToggleChange()
	{
		Debug.Log("MainAppViewController.OnAlarmToggleChange - OK");

		if (_model.AlarmToggle.value)
		{
			HideTimerTimeLabel();
			ShowAlarmTimeLabel();
			_model.Setting = MainAppViewModel.MainSetting.Alarm;
		}

	}

	private void OnTimerToggleChange()
	{
		Debug.Log("MainAppViewController.OnAlarmToggleChange - OK");

		if (_model.TimerToggle.value)
		{
			HideAlarmTimeLabel();
			ShowTimerTimeLabel();

			_model.Setting = MainAppViewModel.MainSetting.Timer;
		}
	}

	private void ShowAlarmTimeLabel()
	{
		_model.AlarmTimeShowLabel.gameObject.SetActive(true);
	}

	private void HideAlarmTimeLabel()
	{
		_model.AlarmTimeShowLabel.gameObject.SetActive(false);
	}

	private void ShowTimerTimeLabel()
	{
		_model.TimerTimeShowLabel.gameObject.SetActive(true);
	}

	private void HideTimerTimeLabel()
	{
		_model.TimerTimeShowLabel.gameObject.SetActive(false);
	}

	private void OnChangeChapterButtonClick(GameObject sender)
	{
		Debug.Log("MainAppViewController.OnChangeChapterButtonClick - OK");

		if (null != OnMainAppViewChangeChapterEvent)
		{
			OnMainAppViewChangeChapterEvent();
		}
	}
	#endregion

	#endregion

	#region IShowable implementation

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
