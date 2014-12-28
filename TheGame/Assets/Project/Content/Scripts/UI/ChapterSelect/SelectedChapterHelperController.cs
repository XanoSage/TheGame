﻿using UnityEngine;
using System.Collections;

public class SelectedChapterHelperController : MonoBehaviour {

	#region Variables

	private SelectedChapterHelperModel _model;

	public int WidgetHeight
	{
		get
		{
			return GetComponent<UIWidget>() ? GetComponent<UIWidget>().height : 0;
		}
	}

	public int Index { get { return _model.Index; } }

	#endregion


	#region MonoBehaviour Actions

	private void Awake()
	{
		_model = GetComponent<SelectedChapterHelperModel>();

		if (null == _model)
		{
			throw new MissingComponentException("SelectedChapterHelperController.Awake - cann't find SelectedChapterHelperModel component");
		}
	}

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {

	}

	#endregion

	#region Actions

	public void Init(string chapterName, int index)
	{
		if (null == _model)
			return;

		_model.ChapterName.text = chapterName;
		_model.Index = index;
	}

	#endregion
}
