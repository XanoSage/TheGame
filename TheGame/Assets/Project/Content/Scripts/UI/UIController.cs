using UnityEngine;
using System.Collections;

public class UIController : MonoBehaviour {

	#region Variables

	private MainAppViewController _mainAppViewController;
	private AlarmManagerViewController _alarmManagerViewController;
	private TimerSetViewController _timerSetViewController;

	#endregion

	#region Monobehaviour actions

	// Use this for initialization
	void Start ()
	{
		_mainAppViewController = FindObjectOfType<MainAppViewController>();
		_alarmManagerViewController = FindObjectOfType<AlarmManagerViewController>();
		_timerSetViewController = FindObjectOfType<TimerSetViewController>();

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

		_alarmManagerViewController.OnBackButtonEvent += OnAlarmManagerBackButton;

		_timerSetViewController.OnCancelButtonEvent += OnTimerSetViewCancelButton;
		_timerSetViewController.OnSaveButtonEvent += OnTimerSetViewSaveButton;
	}

	private void UnSubscribeEvents()
	{
		_mainAppViewController.OnMainAppViewAlarmChangeEvent -= OnMainAppViewAlarmChangeClick;
		_mainAppViewController.OnMainAppViewTimerChangeEvent -= OnMainAppViewTimerChangeClick;

		_alarmManagerViewController.OnBackButtonEvent -= OnAlarmManagerBackButton;

		_timerSetViewController.OnCancelButtonEvent -= OnTimerSetViewCancelButton;
		_timerSetViewController.OnSaveButtonEvent -= OnTimerSetViewSaveButton;
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
	}

	#endregion

	#region Alarm Manager View Actions

	private void OnAlarmManagerBackButton()
	{
		Debug.Log("UIController.OnAlarmSetBackButton - OK");
		_alarmManagerViewController.Hide();
		_mainAppViewController.Show();
	}

	#endregion

	#region Timer Set View Actions

	private void OnTimerSetViewCancelButton()
	{
		Debug.Log("UIController.OnTimerSetViewCancelButton - OK");

		_timerSetViewController.Hide();
		_mainAppViewController.Show();
	}

	private void OnTimerSetViewSaveButton()
	{
		Debug.Log("UIController.OnTimerSetViewCancelButton - OK");
		_timerSetViewController.Hide();
		_mainAppViewController.Show();
	}
		 
	#endregion

	
	#endregion
}
