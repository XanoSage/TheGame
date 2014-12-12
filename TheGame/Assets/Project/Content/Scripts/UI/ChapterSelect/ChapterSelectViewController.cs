using System;
using System.Collections.Generic;
using UnityEngine;
using System.Collections;
using UnityTools.Other;

public class ChapterSelectViewController : MonoBehaviour, IShowable {

	#region Constants

	private const int ChapterCount = 10;
	private const string ChapterPrefabPath = "Prefabs/UI/ChapterPrefab";

	private readonly string[] _chaptersId =
		{
			"Slajdy", "Mayatniki", "Volna_udachi", "Ravnovesie", "Inducirovannyj_perehod",
			"Techenie_variantov", "Namerenie", "Dusha_i_razum", "Celi_i_dveri", "Frejling", "Koordinacija", "Tancy_s_tenjami",
			"Zerkalnyj_mir", "Privratnik_vechnosti"
		};

	private readonly string[] _chaptersName =
		{
			"Слайды", "Маятники", "Волна Удачи", "Равновесие", "Индуцированный Переход", "Течение Вариантов", "Намерение",
			"Душа и Разум", "Цели и Двери", "Фрэйлинг", "Координация", "Танцы с Тенями", "Зеркальный Мир", "Привратник Вечности"
		};

	#endregion

	#region Variables

	private ChapterSelectViewModel _model;

	public event Action OnCancelButtonEvent;
	public event Action<List<ChapterHelperController>> OnApplyButtonEvent;

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

		_model.ListOfChapter = new List<ChapterHelperController>();
		_model.ListOfCurrentChange = new List<bool>();

		SubscribeEvents();
		InitChapterList();
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

		UnDoChapterSelect();
		ResetListOfCurrentChange();

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
			OnApplyButtonEvent(_model.ListOfChapter);
		}

		ResetListOfCurrentChange();
	}

	private void InitChapterList()
	{
		float shiftY = 0;
		for (int i = 0; i < _chaptersId.Length; i++)
		{
			GameObject chapterPrefabToMake = (GameObject) Resources.Load(ChapterPrefabPath);

			ChapterHelperController chapterHelperController =
				NGUITools.AddChild(_model.ScrollView.gameObject, chapterPrefabToMake).GetComponent<ChapterHelperController>();

			if (null == chapterHelperController)
			{
				continue;
			}

			UIEventListener.Get(chapterHelperController.gameObject).onClick = go => OnChapterSelect(chapterHelperController.Index);

			chapterHelperController.Init(i, _chaptersName[i]);

			shiftY = (chapterHelperController.GetComponent<UIWidget>()).height;

			Vector3 pos = chapterHelperController.transform.localPosition;

			chapterHelperController.transform.localPosition = new Vector3(pos.x, pos.y - shiftY*i, pos.z);

			_model.ListOfChapter.Add(chapterHelperController);
			_model.ListOfCurrentChange.Add(false);
		}
		_model.ScrollView.ResetPosition();
		_model.ScrollView.UpdatePosition();
	}

	private void OnChapterSelect(int index)
	{
		ChapterHelperController chapter = _model.ListOfChapter[index];

		if (null == chapter)
			return;

		if (chapter.IsSelected)
		{
			chapter.UnSelectChapter();
		}
		else 
			chapter.SelectChapter();
		
		bool currentState = _model.ListOfCurrentChange[index];

		_model.ListOfCurrentChange[index] = ! currentState;

		Debug.Log("ChapterSelectViewController.OnChapterSelect - OK, chapter is: " + chapter.IsSelected);
	}

	private void ResetListOfCurrentChange()
	{
		if (null == _model.ListOfCurrentChange)
		{
			return;
		}

		for (int i = 0; i < _model.ListOfCurrentChange.Count; i++)
		{
			_model.ListOfCurrentChange[i] = false;
		}

	}

	private void UnDoChapterSelect()
	{
		for (int i = 0; i < _model.ListOfCurrentChange.Count; i++)
		{
			if (_model.ListOfCurrentChange[i] == false)
				continue;

			if (_model.ListOfChapter[i].IsSelected)
			{
				_model.ListOfChapter[i].UnSelectChapter();
			}
			else
			{
				_model.ListOfChapter[i].SelectChapter();
			}
		}
	}

	public string GetChapterId(int index)
	{
		string str = string.Empty;

		if (index < 0 || index >= _chaptersId.Length)
			return str;
		else
		{
			str = _chaptersId[index];
		}

		return str;
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
