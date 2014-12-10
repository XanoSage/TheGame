using UnityEngine;
using System.Collections;

public class ChapterHelperModel : MonoBehaviour {

	#region MyRegion

	public UILabel ChapterName;

	public UISprite ChapterIddleIcon;
	public UISprite ChapterSelectIcon;

	public UIEventListener ChapterSelectButton;

	[HideInInspector] public int Index;

	[HideInInspector] public bool IsSelected;

	#endregion
}
