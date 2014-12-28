using System;
using System.Collections.Generic;
using Assets.Project.Content.Scripts.Timer;
using Assets.Project.Content.Scripts.UI.Notification;
using UnityEngine;
using System.Collections;
using UnityTools.Other;
using Random = UnityEngine.Random;

public class MainAppViewController : MonoBehaviour, IShowable {

	#region Constants

	private const string SelectedChapterPrefab = "Prefabs/UI/SelectedChapterPrefab";
	private const int SelectedChapterContainerBaseHeight = 60;
	private const float BaseStartPosY = -50;

	#endregion

	#region Variables

	private MainAppViewModel _model;

	public Notification CurrentNotification { get { return _model.CurrentNotification; } }

	public int SelectedChapterCount {get { return _model.SelectedChapters.Count; }}

	public MainAppViewModel.MainSetting Setting {get { return _model.Setting; }}

	public int NotificationCount {get { return _model.TheTimer.NotificationCount; }}



	public event Action OnMainAppViewAlarmChangeEvent;
	public event Action OnMainAppViewTimerChangeEvent;

	public event Action OnMainAppViewChangeChapterEvent;

	public event Action OnMainAppViewApplyButtonEvent;

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

		_model.SelectedChapters = new List<SelectedChapterHelperController>();

		_model.SelectedChapterContainer.height = SelectedChapterContainerBaseHeight;

		_model.CurrentNotification = null;

		SubscribeEvents();

		SetDefaultTimer();

		ShowAlarmTimeLabel();
		HideTimerTimeLabel();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	private void OnDestroy()
	{
		UnsubscribeEvents();
	}
	#endregion

	#region Actions
	
	private void SubscribeEvents()
	{
		_model.ChangeShowTimeButton.onClick = OnChangeShowTimeClick;
		_model.ChangeShowTimeButton.onPress = OnChangeShowTimePressed;

		_model.ChangeChapterButton.onClick = OnChangeChapterButtonClick;

		_model.AlarmToggle.onChange = new List<EventDelegate>();
		_model.AlarmToggle.onChange.Add(new EventDelegate(OnAlarmToggleChange));

		_model.TimerToggle.onChange = new List<EventDelegate>();
		_model.TimerToggle.onChange.Add(new EventDelegate(OnTimerToggleChange));

		_model.ApplyButton.onClick = OnApplyButtonClick;
	}

	private void UnsubscribeEvents()
	{
		_model.AlarmToggle.onChange.Clear();

		_model.TimerToggle.onChange.Clear();
	}

	public void SetTimer(int hour, int minutes, int interval)
	{
		_model.TheTimer = SimpleTimer.Create(hour, minutes, interval);
		InitTimerDisplayLabel();
		InitNotificationCountLabel();
	}

	private void SetDefaultTimer()
	{
		SetTimer(10, 20, 15);
	}

	public void SetTimer(SimpleTimer timer)
	{
		Debug.Log("MainAappViewController.SetTimer(SimpleTimer) - OK, notification count: " + timer.NotificationCount);
		_model.TheTimer = timer;
		UpdateNotificationCountLabel();
	}

	private void InitTimerDisplayLabel()
	{
		if (_model.TheTimer == null)
			return;

		string str = string.Format("{0:D2}:{1:D2}", _model.TheTimer.Hour, _model.TheTimer.Minutes);

		_model.TimerTimeShowLabel.text = str;
	}

	public void OnNotificationDisplayCloseButton()
	{
		//Debug.Log("MainAppViewController.OnNotificationDisplayCloseButton - OK");

		if (_model.TheTimer.NotificationCount > 1)
		{
			_model.TheTimer.CalculateNotificationsCount();	
		}
		else
		{
			_model.TheTimer.NotificationCount --;	
		}
		
		if (_model.TheTimer.NotificationCount < 0)
		{
			_model.TheTimer.NotificationCount = 0;
		}

		Debug.Log("MainAppViewController.OnNotificationDisplayCloseButton - OK, notification count: " + _model.TheTimer.NotificationCount);
		UpdateNotificationCountLabel();
	}

	private void InitNotificationCountLabel()
	{
		if (_model.TheTimer == null)
		{
			return;
		}

		_model.TheTimer.CalculateNotificationsCount();

		_model.MessageShowCountLabel.text = _model.TheTimer.NotificationCount.ToString();
	}

	public void UpdateNotificationCountLabel()
	{
		if (_model.TheTimer == null)
		{
			return;
		}

		_model.MessageShowCountLabel.text = _model.TheTimer.NotificationCount.ToString();
	}



	#region Button Handler Actions

	private void OnChangeShowTimeClick(GameObject sender)
	{
		Debug.Log("MainAppViewController.OnChangeShowTimeClick - OK");

		if (_model.Setting == MainAppViewModel.MainSetting.Alarm)
		{
			if (null != OnMainAppViewAlarmChangeEvent)
			{
				OnMainAppViewAlarmChangeEvent();
			}
		}
		else
		{
			if (null != OnMainAppViewTimerChangeEvent)
			{
				OnMainAppViewTimerChangeEvent();
			}
		}
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

	private void OnAlarmToggleChange()
	{
		Debug.Log("MainAppViewController.OnAlarmToggleChange - OK");

		if (_model.AlarmToggle.value)
		{
			HideTimerTimeLabel();
			ShowAlarmTimeLabel();
			_model.Setting = MainAppViewModel.MainSetting.Alarm;
		}

	}

	private void OnTimerToggleChange()
	{
		Debug.Log("MainAppViewController.OnAlarmToggleChange - OK");

		if (_model.TimerToggle.value)
		{
			HideAlarmTimeLabel();
			ShowTimerTimeLabel();

			_model.Setting = MainAppViewModel.MainSetting.Timer;
		}
	}

	private void ShowAlarmTimeLabel()
	{
		_model.AlarmTimeShowLabel.gameObject.SetActive(true);
	}

	private void HideAlarmTimeLabel()
	{
		_model.AlarmTimeShowLabel.gameObject.SetActive(false);
	}

	private void ShowTimerTimeLabel()
	{
		_model.TimerTimeShowLabel.gameObject.SetActive(true);
	}

	private void HideTimerTimeLabel()
	{
		_model.TimerTimeShowLabel.gameObject.SetActive(false);
	}

	private void OnChangeChapterButtonClick(GameObject sender)
	{
		Debug.Log("MainAppViewController.OnChangeChapterButtonClick - OK");

		if (null != OnMainAppViewChangeChapterEvent)
		{
			OnMainAppViewChangeChapterEvent();
		}
	}
	#endregion

	#region Selected Chapter Actions

	public void InitSelectedChapterList(List<ChapterHelperController> chapters)
	{
		if (_model.SelectedChapters.Count > 0)
			RemoveSelectedChapterList();

		int counter = 0;

		foreach (ChapterHelperController chapter in chapters)
		{
			if (!chapter.IsSelected)
				continue;

			GameObject selectedChapterToMake = (GameObject) Resources.Load(SelectedChapterPrefab);

			SelectedChapterHelperController selectedChapter =
				NGUITools.AddChild(_model.SelectedChapterContainer.gameObject, selectedChapterToMake)
				         .GetComponent<SelectedChapterHelperController>();

			_model.SelectedChapterContainer.height += selectedChapter.WidgetHeight;

			selectedChapter.Init(chapter.Name, chapter.Index);
			
			Vector3 currPos = selectedChapter.transform.localPosition;

			int shiftY = selectedChapter.WidgetHeight;

			selectedChapter.transform.localPosition = new Vector3(currPos.x, BaseStartPosY + currPos.y - shiftY*counter, currPos.z);

			_model.SelectedChapters.Add(selectedChapter);

			counter++;
		}

		_model.ScrollView.ResetPosition();
		_model.ScrollView.UpdateScrollbars();
	}

	private void RemoveSelectedChapterList()
	{
		for (int i = 0; i < _model.SelectedChapters.Count; i++)
		{
			Destroy(_model.SelectedChapters[i].gameObject);
		}

		_model.SelectedChapters.Clear();
		_model.SelectedChapterContainer.height = SelectedChapterContainerBaseHeight;
	}

	#endregion

	#region Apply Event Actions

	private void OnApplyButtonClick(GameObject sender)
	{
		Debug.Log("MainAppViewController.OnApplyButtonClick - OK");

		TryGetAnyText();

		NotificationController.Instance.InitSelectedChapter(_model.SelectedChapters);

		if (null != OnMainAppViewApplyButtonEvent)
		{
			OnMainAppViewApplyButtonEvent();
		}
	}

	private void TryGetAnyText()
	{
		int indexChId = Random.Range(0, 14);
		int indexNotification = Random.Range(0, 50);

		ChapterSelectViewController chapterSelect = FindObjectOfType<ChapterSelectViewController>();

		if (null == chapterSelect)
		{
			throw new MissingComponentException(
				"MainAppViewController.TryGetEnyText - cannot find ChapterSelectViewController component");
		}

		string chapterId = chapterSelect.GetChapterId(indexChId);
		string description = Language.Get(string.Format("{0}_{1}", chapterId, indexNotification)); //Privratnik_vechnosti_10
		//string description = Localization.Get("Privratnik_vechnosti_11");

		Debug.Log(string.Format("TryToGetAnyText: indexChId: {0}, chaperId: {1}, indexNotification: {2} \ndescription: {3}",
								indexChId, chapterId, indexNotification, description));

		//Debug.Log(Language.Get(description));

		_model.CurrentNotification = Notification.Create(chapterSelect.GetChapterName(indexChId), chapterId, description);

		//Localization.ShowEntire();
	}

	#endregion

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
