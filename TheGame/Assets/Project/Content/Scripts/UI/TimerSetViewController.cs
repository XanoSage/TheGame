using System;
using UnityEngine;
using System.Collections;
using UnityTools.Other;

public class TimerSetViewController : MonoBehaviour, IShowable  {

	#region Variables

	private TimerSetViewModel _model;

	public event Action OnCancelButtonEvent;

	public event Action<int, int, int> OnSaveButtonEvent;

	#endregion

	#region Monobehaviour Actions


	// Use this for initialization
	void Start ()
	{

		_model = GetComponent<TimerSetViewModel>();

		if (null == _model)
		{
			throw new MissingComponentException("TimerSetViewController.Start - cann't find TimerSetViewModel component");
		}

		SubscribeEvents();
		Hide();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	#endregion

	#region Actions

	private void SubscribeEvents()
	{
		_model.CancelButton.onClick = OnCancelButtonClick;
		_model.SaveButton.onClick = OnSaveButtonClick;
	}

	private void OnCancelButtonClick(GameObject sender)
	{
		Debug.Log("TimerSetViewController.OnCancelButtonClick - OK");

		if (null != OnCancelButtonEvent)
		{
			OnCancelButtonEvent();
		}
	}

	private void OnSaveButtonClick(GameObject sender)
	{
		Debug.Log("TimerSetViewController.OnSaveButtonClick - OK");

		OnHourSelect();
		OnMinutesSelect();
		OnIntervalSelect();

		if (null != OnSaveButtonEvent)
		{
			OnSaveButtonEvent(_model.HourSelect, _model.MinuteSelect, _model.IntervalSelectionMinutes);
		}
	}

	void OnIntervalSelect()
	{
		if (_model == null || _model.TextPicker == null)
			return;

		_model.IntervalSelectionMinutes = GetInterval(Convert.ToInt32(_model.TextPicker.CurrentLabelText.Split(' ')[0]));

		Debug.Log("TimerSetViewController.OnIntervalSelect - OK, interval in minutes is: " + _model.IntervalSelectionMinutes);
		
	}

	private int GetInterval(int interval)
	{
		int minutes = 0;

		if (interval == 5 || interval == 10 || interval == 15 || interval == 30)
			minutes = interval;
		else
		{
			minutes = interval*60;
		}

		return minutes;
	}

	private void OnHourSelect()
	{
		if (_model == null || _model.HourPicker == null)
			return;

		_model.HourSelect = Convert.ToInt32(_model.HourPicker.CurrentValue);

		Debug.Log("TimerSetViewController.OnHourSelect - OK, hours is: " + _model.HourSelect);
	}

	private void OnMinutesSelect()
	{
		if (_model == null || _model.MinutePicker == null)
			return;

		_model.MinuteSelect = Convert.ToInt32(_model.MinutePicker.CurrentValue);

		Debug.Log("TimerSetViewController.OnMinutesSelect - OK, minutes is: " + _model.MinuteSelect);
	}

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
