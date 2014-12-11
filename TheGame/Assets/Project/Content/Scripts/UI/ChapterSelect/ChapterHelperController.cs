using UnityEngine;
using System.Collections;

public class ChapterHelperController : MonoBehaviour {

	#region Variables

	private ChapterHelperModel _model;

	public bool IsSelected
	{
		get { return _model.IsSelected; }
	}

	public int Index { get { return _model.Index; }}

	public string Name { get { return _model.ChapterName.text; }}
	
	#endregion

	#region Monobehaviour Actions

	// Use this for initialization
	private void Start()
	{



	}

	void Awake()
	{
		_model = GetComponent<ChapterHelperModel>();

		if (null == _model)
		{
			throw new MissingComponentException("ChapterHelperController.Start - cann't find ChapterHelperModel component");
		}
	}
	
	// Update is called once per frame
	void Update () {

	}
	#endregion

	#region Actions

	public void Init(int index, string chapterName)
	{
		//if (null == _model)
		//	return;

		_model.Index = index;

		_model.ChapterName.text = chapterName;

		UnSelectChapter();
	}

	public void SelectChapter()
	{
		if (null == _model)
		{
			return;
		}

		_model.ChapterIddleIcon.gameObject.SetActive(false);
		_model.ChapterSelectIcon.gameObject.SetActive(true);

		_model.IsSelected = true;
	}

	public void UnSelectChapter()
	{
		if (null == _model)
			return;

		_model.ChapterIddleIcon.gameObject.SetActive(true);
		_model.ChapterSelectIcon.gameObject.SetActive(false);

		_model.IsSelected = false;
	}

	#endregion
}
