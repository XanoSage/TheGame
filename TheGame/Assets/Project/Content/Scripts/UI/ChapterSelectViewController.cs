using System;
using UnityEngine;
using System.Collections;
using UnityTools.Other;

public class ChapterSelectViewController : MonoBehaviour, IShowable {

	#region Variables

	private ChapterSelectViewModel _model;

	public event Action OnCancelButtonEvent;
	public event Action OnApplyButtonEvent;

	#endregion

	#region Monobehaviour Actions

	// Use this for initialization
	void Start ()
	{
		_model = GetComponent<ChapterSelectViewModel>();

		if (null == _model)
		{
			throw new MissingComponentException("ChapterSelectViewController.Start - cann't find ChapterSelectViewModel component");
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
		_model.ApplyButton.onClick = OnApplyButtonClicked;

		_model.CancelButton.onClick = OnCancelButtonClick;
	}

	private void OnCancelButtonClick(GameObject sender)
	{
		Debug.Log("ChapterSelectViewController.OnCancelButtonClick - OK");

		if (null != OnCancelButtonEvent)
		{
			OnCancelButtonEvent();
		}
	}

	private void OnApplyButtonClicked(GameObject sender)
	{
		Debug.Log("ChapterSelectViewController.OnApplyButtonClicked - OK");

		if (null != OnApplyButtonEvent)
		{
			OnApplyButtonEvent();
		}
	}

	#endregion

	#region IShowable Implenentation

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
