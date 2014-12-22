using UnityEngine;
using System.Collections;
using UnityTools.Other;

public class TimerDisplayViewController : MonoBehaviour, IShowable {

	#region Variables

	private TimerDisplayViewModel _model;

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
		_model.PauseButton.onClick = OnPauseButtonClick;
	}

	private void OnCancelButtonClick(GameObject sender)
	{
		Debug.Log("TimerDisplayViewController.OnCancelButtonClick - OK");
	}

	private void OnPauseButtonClick(GameObject sender)
	{
		Debug.Log("TimerDisplayViewController.OnPauseButtonClick - OK");

		_model.IsTimerOnPause = !_model.IsTimerOnPause;

		OnPausePressedAction(_model.IsTimerOnPause);
	}

	private void OnPausePressedAction(bool isPaused)
	{
		if (isPaused)
		{
			_model.PauseLabelIdle.gameObject.SetActive(false);
			_model.PauseLabelPressed.gameObject.SetActive(false);

			_model.ContinueLabelIdle.gameObject.SetActive(true);
			_model.ContinueLabelPressed.gameObject.SetActive(true);
		}
		else
		{
			_model.PauseLabelIdle.gameObject.SetActive(true);
			_model.PauseLabelPressed.gameObject.SetActive(true);

			_model.ContinueLabelIdle.gameObject.SetActive(false);
			_model.ContinueLabelPressed.gameObject.SetActive(false);
		}
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
