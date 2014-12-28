using System.Collections.Generic;
using UnityEngine;
using System.Collections;

public class UIController : MonoBehaviour {

	#region Variables

	private MainAppViewController _mainAppViewController;
	private AlarmManagerViewController _alarmManagerViewController;
	private TimerSetViewController _timerSetViewController;
	private ChapterSelectViewController _chapterSelectViewController;
	private NotificationDisplayViewController _notificationDisplayViewController;
	private AlarmSetViewController _alarmSetViewController;
	private TimerDisplayViewController _timerDisplayViewController;


	#endregion

	#region Monobehaviour actions

	// Use this for initialization
	void Start ()
	{
		_mainAppViewController = FindObjectOfType<MainAppViewController>();
		_alarmManagerViewController = FindObjectOfType<AlarmManagerViewController>();
		_timerSetViewController = FindObjectOfType<TimerSetViewController>();
		_chapterSelectViewController = FindObjectOfType<ChapterSelectViewController>();
		_notificationDisplayViewController = FindObjectOfType<NotificationDisplayViewController>();
		_alarmSetViewController = FindObjectOfType<AlarmSetViewController>();
		_timerDisplayViewController = FindObjectOfType<TimerDisplayViewController>();

		SubscribeEvents();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	private void OnDestroy()
	{
		UnSubscribeEvents();
	}

	#endregion

	#region Actions

	private void SubscribeEvents()
	{
		_mainAppViewController.OnMainAppViewAlarmChangeEvent += OnMainAppViewAlarmChangeClick;
		_mainAppViewController.OnMainAppViewTimerChangeEvent += OnMainAppViewTimerChangeClick;
		_mainAppViewController.OnMainAppViewChangeChapterEvent += OnMainAppViewChapterChangeClick;
		_mainAppViewController.OnMainAppViewApplyButtonEvent += OnMainAppViewApplyButtonClick;

		_alarmManagerViewController.OnBackButtonEvent += OnAlarmManagerBackButton;
		_alarmManagerViewController.OnAddButtonEvent += OnAlarmManagerAddButtonClicked;

		_timerSetViewController.OnCancelButtonEvent += OnTimerSetViewCancelButton;
		_timerSetViewController.OnSaveButtonEvent += OnTimerSetViewSaveButton;

		_chapterSelectViewController.OnApplyButtonEvent += OnChapterSelectViewApplyButton;
		_chapterSelectViewController.OnCancelButtonEvent += OnChaterSelectViewCancelButton;

		_notificationDisplayViewController.OnCloseButtonEvent += _mainAppViewController.OnNotificationDisplayCloseButton;
		_notificationDisplayViewController.OnCloseButtonEvent += OnNotificationDisplayViewCloseButton;

		_alarmSetViewController.OnApplyEvent += OnAlarmSetApplyButtonClick;
		_alarmSetViewController.OnCancelEvent += OnAlarmSetCancelButtonClick;

		_timerDisplayViewController.OnCancelButtonEvent += OnTimerDisplayCancelButtonClick;
		_timerDisplayViewController.OnMainTimerEndEvent += OnTimerDisplayMainTimerEnd;
	}

	private void UnSubscribeEvents()
	{
		_mainAppViewController.OnMainAppViewAlarmChangeEvent -= OnMainAppViewAlarmChangeClick;
		_mainAppViewController.OnMainAppViewTimerChangeEvent -= OnMainAppViewTimerChangeClick;
		_mainAppViewController.OnMainAppViewChangeChapterEvent -= OnMainAppViewChapterChangeClick;
		_mainAppViewController.OnMainAppViewApplyButtonEvent -= OnMainAppViewApplyButtonClick;

		_alarmManagerViewController.OnBackButtonEvent -= OnAlarmManagerBackButton;
		_alarmManagerViewController.OnAddButtonEvent -= OnAlarmManagerAddButtonClicked;

		_timerSetViewController.OnCancelButtonEvent -= OnTimerSetViewCancelButton;
		_timerSetViewController.OnSaveButtonEvent -= OnTimerSetViewSaveButton;

		_chapterSelectViewController.OnApplyButtonEvent -= OnChapterSelectViewApplyButton;
		_chapterSelectViewController.OnCancelButtonEvent -= OnChaterSelectViewCancelButton;

		_notificationDisplayViewController.OnCloseButtonEvent -= OnNotificationDisplayViewCloseButton;
		_notificationDisplayViewController.OnCloseButtonEvent -= _mainAppViewController.OnNotificationDisplayCloseButton;

		_alarmSetViewController.OnApplyEvent -= OnAlarmSetApplyButtonClick;
		_alarmSetViewController.OnCancelEvent -= OnAlarmSetCancelButtonClick;

		_timerDisplayViewController.OnCancelButtonEvent -= OnTimerDisplayCancelButtonClick;
		_timerDisplayViewController.OnMainTimerEndEvent -= OnTimerDisplayMainTimerEnd;
	}

	#region MainApp View Actions

	private void OnMainAppViewAlarmChangeClick()
	{
		Debug.Log("UIController.OnAlarmSetTimeClick - OK");

		_mainAppViewController.Hide();
		_alarmManagerViewController.Show();
	}

	private void OnMainAppViewTimerChangeClick()
	{
		Debug.Log("UIController.OnMainAppViewTimerChangeClick - OK");
		_mainAppViewController.Hide();
		_timerSetViewController.Show();
	}

	private void OnMainAppViewChapterChangeClick()
	{
		Debug.Log("UIController.OnMainAppViewChapterChangeClick - OK");

		_mainAppViewController.Hide();
		_chapterSelectViewController.Show();
	}

	private void OnMainAppViewApplyButtonClick()
	{
		Debug.Log("UIController.OnMainAppViewChapterChangeClick - OK, selected chapter count: " + _mainAppViewController.SelectedChapterCount);

		if (_mainAppViewController.SelectedChapterCount < 1)
		{
			// need to add pop up message

			return;
		}

		if (_mainAppViewController.Setting == MainAppViewModel.MainSetting.Timer)
		{
			_mainAppViewController.Hide();
			_timerDisplayViewController.InitSimpleTimer();

			Debug.Log("UIController.OnMainAppViewApplyButtonClick - OK, notification count: " +
			          _timerDisplayViewController.ParentTimer.NotificationCount);

			_mainAppViewController.SetTimer(_timerDisplayViewController.ParentTimer);
			_timerDisplayViewController.Show();
		}
		//_notificationDisplayViewController.Init(_mainAppViewController.CurrentNotification);

		//_notificationDisplayViewController.Show();
	}

	#endregion

	#region Alarm Manager View Actions

	private void OnAlarmManagerBackButton()
	{
		Debug.Log("UIController.OnAlarmSetBackButton - OK");
		_alarmManagerViewController.Hide();
		_mainAppViewController.Show();
	}

	private void OnAlarmManagerAddButtonClicked()
	{
		Debug.Log("UIController.OnAlarmManagerAddButtonClicked - OK");
		_alarmManagerViewController.Hide();
		_alarmSetViewController.Show();
	}

	#endregion

	#region Timer Set View Actions

	private void OnTimerSetViewCancelButton()
	{
		Debug.Log("UIController.OnTimerSetViewCancelButton - OK");

		_timerSetViewController.Hide();
		_mainAppViewController.Show();
	}

	private void OnTimerSetViewSaveButton(int hour, int minutes, int interval)
	{
		Debug.Log("UIController.OnTimerSetViewSaveButton - OK");
		_timerSetViewController.Hide();

		_mainAppViewController.SetTimer(hour, minutes, interval);

		_mainAppViewController.Show();
	}
		 
	#endregion

	#region Chapter Select View Actions

	private void OnChaterSelectViewCancelButton()
	{
		Debug.Log("UIController.OnChaterSelectViewCanceButton - OK");
		_chapterSelectViewController.Hide();
		_mainAppViewController.Show();
	}

	private void OnChapterSelectViewApplyButton(List<ChapterHelperController> chapters)
	{
		Debug.Log("UIController.OnChapterSelectViewApplyButton - OK");
		_chapterSelectViewController.Hide();

		_mainAppViewController.InitSelectedChapterList(chapters);

		_mainAppViewController.Show();
	}

	#endregion

	#region Notification Display View Action

	private void OnNotificationDisplayViewCloseButton()
	{
		Debug.Log("UIController.OnNotificationDisplayViewCloseButton - OK");

		_notificationDisplayViewController.Hide();

		if (_mainAppViewController.NotificationCount > 1)
		{
			_timerDisplayViewController.StartIdleTimer();
			_timerDisplayViewController.Show();
		}
		else
		{
			_mainAppViewController.Show();	
		}

		
	}

	#endregion

	#region Alarm Set View Actions

	private void OnAlarmSetCancelButtonClick()
	{
		Debug.Log("UIController.OnAlarmSetCancelButtonClick - OK");

		_alarmSetViewController.Hide();
		_alarmManagerViewController.Show();
	}

	private void OnAlarmSetApplyButtonClick()
	{
		Debug.Log("UIController.OnAlarmSetCancelButtonClick - OK");
		_alarmSetViewController.Hide();
		_alarmManagerViewController.Show();
	}

	#endregion

	#region Timer Display Actions

	private void OnTimerDisplayCancelButtonClick()
	{
		Debug.Log("UIController.OnTimerDisplayCancelButtonClick - OK");

		//TODO need to add MessageBox controller (yes/no/ok/cancel button)

		_timerDisplayViewController.Hide();
		_mainAppViewController.Show();
	}

	private void OnTimerDisplayMainTimerEnd()
	{
		Debug.Log("UIController.OnTimerDisplayMainTimerEnd - OK");

		_timerDisplayViewController.Hide();

		_notificationDisplayViewController.Init(NotificationController.Instance.GetNotifications());
		//_notificationDisplayViewController.Init(_mainAppViewController.CurrentNotification);

		_notificationDisplayViewController.Show();
	}

	#endregion

	#endregion
}
