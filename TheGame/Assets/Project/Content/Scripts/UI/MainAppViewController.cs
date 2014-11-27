using System;
using System.Collections.Generic;
using UnityEngine;
using System.Collections;

public class MainAppViewController : MonoBehaviour {

	#region Variables

	private MainAppViewModel _model;

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

	#region Button Handler Actions

	private void OnChangeShowTimeClick(GameObject sender)
	{
		Debug.Log("MainAppViewController.OnChangeShowTimeClick - OK");
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
	#endregion

	#endregion
}
