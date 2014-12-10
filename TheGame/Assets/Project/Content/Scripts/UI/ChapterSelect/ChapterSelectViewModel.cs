using System.Collections.Generic;
using UnityEngine;
using System.Collections;

public class ChapterSelectViewModel : MonoBehaviour {

	#region Variables

	public UIWidget ElementContainer;

	public UIEventListener CancelButton;
	public UIEventListener ApplyButton;

	public GameObject ChapterPrefab;
	public UIScrollView ScrollView;

	[HideInInspector] public List<ChapterHelperController> ListOfChapter;
	[HideInInspector] public List<ChapterHelperController> ListOfSelectedChapter;
	[HideInInspector] public List<bool> ListOfCurrentChange;

	#endregion
}
