using UnityEngine;
using System.Collections;

public class SwitchOnOffController : MonoBehaviour
{
	#region Constants

	private const float SwitchOffTogglePos = -20f;
	private const float SwitchOnTogglePos = 20f;

	private const float Threshold = 40f;

	#endregion

	#region Variables

	private SwitchOnOffModel _model;

	public bool IsSwitchOn
	{
		get { return _model.IsSwitchOn; }
	}

	#endregion

	#region MonoBehaviour Actions

	// Use this for initialization
	private void Start()
	{

		_model = GetComponent<SwitchOnOffModel>();

		if (null == _model)
		{
			throw new MissingComponentException("SwitchOnOffController.Start - cann't find SwitchOnOffModel component");
		}

		SubscribeEvents();
		InitState(false, true);
	}

	// Update is called once per frame
	private void Update()
	{

	}

	#endregion

	#region Actions

	private void SubscribeEvents()
	{
		_model.SwitchOnOffButton.onClick = OnSwitchOnOffClicked;

		_model.SwitchOnOffButton.onDrag = OnSwitchOnOffDrag;
	}

	private void OnSwitchOnOffDrag(GameObject go, Vector2 delta)
	{
		if (- UICamera.currentTouch.totalDelta.x > Threshold && Mathf.Abs(UICamera.currentTouch.totalDelta.y) < Threshold)
		{
			OnSwipeLeft();
		}

		else if (UICamera.currentTouch.totalDelta.x > Threshold && Mathf.Abs(UICamera.currentTouch.totalDelta.y) < Threshold)
		{
			OnSwipeRight();
		}
	}

	private void OnSwipeLeft()
	{

		if (!_model.IsSwitchOn)
			return;

		InitState(false);
	}

	private void OnSwipeRight()
	{
		if (_model.IsSwitchOn)
			return;

		InitState(true);
	}

	private void OnSwitchOnOffClicked(GameObject sender)
	{
		Debug.Log("SwitchOnOffController.OnSwitchOnOffClicked");

		if (_model.IsAnimated)
			return;

		InitState(!_model.IsSwitchOn);


	}

	private void InitState(bool isSwitchOn, bool force = false)
	{
		_model.IsSwitchOn = isSwitchOn;

		if (force)
		{
			_model.IsAnimated = false;

			Vector3 pos = _model.Toggle.transform.localPosition;

			if (isSwitchOn)
			{
				_model.Toggle.transform.localPosition = new Vector3(SwitchOnTogglePos, pos.y, pos.z);
				_model.Foreground.alpha = 1f;
			}
			else
			{
				_model.Toggle.transform.localPosition = new Vector3(SwitchOffTogglePos, pos.y, pos.z);
				_model.Foreground.alpha = 0f;
			}
			return;
		}

		_model.IsAnimated = true;

		TweenPosition tweenPos = _model.Toggle.gameObject.GetComponent<TweenPosition>();

		if (isSwitchOn)
		{
			tweenPos.PlayForward();
		}
		else
		{
			tweenPos.PlayReverse();
		}

		tweenPos.SetOnFinished(() =>
			{
				_model.IsAnimated = false;
			});

		TweenAlpha tweenAlpha = _model.Foreground.gameObject.GetComponent<TweenAlpha>();

		if (isSwitchOn)
		{
			tweenAlpha.PlayForward();
		}
		else
		{
			tweenAlpha.PlayReverse();
		}

	}

	#endregion
}
