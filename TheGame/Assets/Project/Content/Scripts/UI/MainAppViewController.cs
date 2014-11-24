using System;
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

	}
	
	// Update is called once per frame
	void Update () {
	
	}
	#endregion

	#region Actions
	
	private void SubscribeEvents()
	{
		_model.ChangeShowTimeButton.onClick = OnChangeShowTimeClick;
		_model.ChangeShowTimeButton.onPress = OnChangeShowTimePressed;
	}

	private void UnsubscribeEvents()
	{

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

	#endregion

	#endregion
}
