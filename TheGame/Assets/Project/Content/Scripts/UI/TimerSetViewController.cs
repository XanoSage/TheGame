using System;
using UnityEngine;
using System.Collections;
using UnityTools.Other;

public class TimerSetViewController : MonoBehaviour, IShowable  {

	#region Variables

	private TimerSetViewModel _model;

	public event Action OnCancelButtonEvent;

	public event Action OnSaveButtonEvent;

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
		//Hide();
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

		if (null != OnSaveButtonEvent)
		{
			OnSaveButtonEvent();
		}
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

	void OnIntervalSelect()
	{
		if (_model == null || _model.TextPicker == null)
			return;

		Debug.Log("TimerSetViewController.OnIntervalSelect - OK, text is: " + Convert.ToInt32( _model.TextPicker.CurrentLabelText.Split(' ')[0]));

		//string str = "12 sdf";
		//str = string.Split(' ');
	}

	#endregion
}
