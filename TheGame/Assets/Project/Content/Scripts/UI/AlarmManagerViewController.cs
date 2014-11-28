using UnityEngine;
using System.Collections;
using UnityTools.Other;

public class AlarmManagerViewController : MonoBehaviour, IShowable
{

	#region Veriables

	private AlarmManagerViewModel _model;

	#endregion

	#region Monobehaviour actions

	// Use this for initialization
	private void Start()
	{
		_model = GetComponent<AlarmManagerViewModel>();

		if (null == _model)
		{
			throw new MissingComponentException("AlarmManagerController.Start - cann't find AlarmManagerViewModel component");
		}

		SubscribeEvents();

		Hide();
	}

	// Update is called once per frame
	private void Update()
	{

	}

	#endregion

	#region action

	private void SubscribeEvents()
	{
		_model.ChangeButton.onClick = OnChangeButtonClick;
		_model.ChangeButton.onPress = OnChangeButtonPressed;

		_model.AddButton.onClick = OnAddButtonClick;
		_model.AddButton.onPress = OnAddButtonPress;

		_model.BackButton.onClick = OnBackButtonClick;
	}

	private void Unsubscribe()
	{
		
	}

	private void OnBackButtonClick(GameObject sender)
	{
		Debug.Log("AlarmManagerViewController.OnBackButtonClick - OK");
	}

	private void OnAddButtonClick(GameObject sender)
	{
		Debug.Log("AlarmManagerViewController.OnAddButtonClick - OK");
	}

	private void OnAddButtonPress(GameObject sender, bool isPressed)
	{
		Debug.Log("AlarmManagerViewController.OnAddButtonPress - OK");

		if (isPressed)
		{
			_model.AddButtonPressed.gameObject.SetActive(true);
		}
		else
		{
			_model.AddButtonPressed.gameObject.SetActive(false);
		}
	}

	private void OnChangeButtonClick(GameObject sender)
	{
		Debug.Log("AlarmManagerViewController.OnChangeButtonClick - OK");
	}

	private void OnChangeButtonPressed(GameObject sender, bool isPressed)
	{
		Debug.Log("AlarmManagerViewController.OnChangeButtonPressed - OK");

		if (isPressed)
		{
			_model.ChangeButtonPressed.gameObject.SetActive(true);
		}
		else
		{
			_model.ChangeButtonPressed.gameObject.SetActive(false);
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

	#endregion
}