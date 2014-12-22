using System;
using UnityEngine;
using System.Collections;
using UnityTools.Other;

public class AlarmSetViewController : MonoBehaviour, IShowable  {

	#region Variables

	private AlarmSetViewModel _model;

	public event Action OnApplyEvent;
	public event Action OnCancelEvent;

	#endregion

	#region MonoBehaviour Actions

	// Use this for initialization
	void Start ()
	{

		_model = GetComponent<AlarmSetViewModel>();

		if (null == _model)
		{
			throw new MissingComponentException("AlarmSetViewController.Start - cann't find AlarmSetViewModel component");
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
		_model.ApplyButton.onClick = OnApplyButtonClick;

		_model.CancelButton.onClick = OnCancelButtonClick;
	}

	private void OnCancelButtonClick(GameObject sender)
	{
		Debug.Log("AlarmSerViewController.OnCancelButtonClick - OK");

		if (null != OnCancelEvent)
		{
			OnCancelEvent();
		}
	}

	private void OnApplyButtonClick(GameObject sender)
	{
		Debug.Log("AlarmSerViewController.OnApplyButtonClick - OK");

		if (null != OnApplyEvent)
		{
			OnApplyEvent();
		}
	}

	private void OnHourSelect()
	{
		if (_model == null || _model.HourPicker == null)
			return;

		_model.HourSelect = Convert.ToInt32(_model.HourPicker.CurrentValue);

		Debug.Log("AlarmSerViewController.OnHourSelect - OK, hours is: " + _model.HourSelect);
	}

	private void OnMinutesSelect()
	{
		if (_model == null || _model.MinutePicker == null)
			return;

		_model.MinuteSelect = Convert.ToInt32(_model.MinutePicker.CurrentValue);

		Debug.Log("AlarmSerViewController.OnMinutesSelect - OK, minute is: " + _model.MinuteSelect);
	}

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
