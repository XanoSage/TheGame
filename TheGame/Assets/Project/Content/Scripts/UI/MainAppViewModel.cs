﻿using System.Collections.Generic;
using Assets.Project.Content.Scripts.Timer;
using Assets.Project.Content.Scripts.UI.Notification;
using UnityEngine;
using System.Collections;

public class MainAppViewModel : MonoBehaviour {

	#region Variables

	public enum MainSetting
	{
		Alarm,
		Timer
	}

	public UIWidget ElementContainer;

	public UIToggle AlarmToggle;
	public UIToggle TimerToggle;

	public UILabel AlarmTimeShowLabel;
	public UILabel TimerTimeShowLabel;
	public UILabel MessageShowCountLabel;

	public UIEventListener ChangeShowTimeButton;
	public UIWidget ShowTimeIddleContainer;
	public UIWidget ShowTimePressedContainer;

	[HideInInspector] public MainSetting Setting;

	public UIEventListener ChangeChapterButton;

	public UIWidget SelectedChapterContainer;

	public UIScrollView ScrollView;

	public SimpleTimer TheTimer;

	public UIEventListener ApplyButton;

	[HideInInspector] public List<SelectedChapterHelperController> SelectedChapters;

	public Notification CurrentNotification;

	#endregion
}
