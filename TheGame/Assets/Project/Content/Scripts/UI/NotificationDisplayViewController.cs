using System;
using Assets.Project.Content.Scripts.UI.Notification;
using UnityEngine;
using System.Collections;
using UnityTools.Other;

public class NotificationDisplayViewController : MonoBehaviour, IShowable  {

	#region Variables

	private NotificationDisplayViewModel _model;

	public event Action OnCloseButtonEvent;

	#endregion

	#region MonoBehaviour Action

	// Use this for initialization
	void Start ()
	{
		_model = GetComponent<NotificationDisplayViewModel>();

		if (null == _model)
		{
			throw new MissingComponentException("NotificationDisplayViewController.Start - cann't find NotificationDisplayViewModel component");
		}

		SubscribeEvents();
		Hide();
	}
	
	// Update is called once per frame
	void Update () {

	}

	#endregion

	#region Actions

	public void Init(Notification notification)
	{
		if (null == notification)
		{
			throw new NullReferenceException("NotificationDisplayViewController.Init - ntification is null");
		}
		_model.ChapterName.text = notification.ChapterName;
		_model.Description.text = notification.Description;
	}

	private void SubscribeEvents()
	{
		_model.CloseButton.onClick = OnCloseButtonClick;
	}

	private void OnCloseButtonClick(GameObject sender)
	{
		Debug.Log("NotificationDisplayViewController.OnCloseButtonClick - OK");

		if (null != OnCloseButtonEvent)
		{
			OnCloseButtonEvent();
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
