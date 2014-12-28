using System.Collections.Generic;
using Assets.Project.Content.Scripts.UI.Notification;
using UnityEngine;
using System.Collections;

public class NotificationController : MonoBehaviour
{

	#region Variables

	public static NotificationController Instance { get; private set; }

	private Dictionary<string, NotificationContainer> _notificationsAll;

	private ChapterSelectViewController _chapterSelectViewController;

	private Dictionary<string, NotificationContainer> _selectedNotifications;

	#endregion

	#region MonoBehavour Actions

	private void Awake()
	{
		Instance = this;
	}

	// Use this for initialization
	private void Start()
	{
		_notificationsAll = new Dictionary<string, NotificationContainer>();
		_selectedNotifications = new Dictionary<string, NotificationContainer>();

		_chapterSelectViewController = FindObjectOfType<ChapterSelectViewController>();

		if (null == _chapterSelectViewController)
		{
			throw new MissingComponentException(
				"NotificationController.Start - cann't find ChapterSelectViewController component");
		}

		Init();
	}

	// Update is called once per frame
	private void Update()
	{

	}

	#endregion

	#region Actions

	private void Init()
	{
		List<string> chapterIds = _chapterSelectViewController.GetChapterIds();
		List<string> chapterNames = _chapterSelectViewController.GetChapterNames();

		for (int i = 0; i < chapterIds.Count; i++)
		{
			NotificationContainer container = NotificationContainer.Create(chapterNames[i], chapterIds[i]);

			int j = 0;
			while (true)
			{
				string notif = Language.Get(string.Format("{0}_{1}", chapterIds[i], j));

				if (notif.Contains("#!#"))
					break;

				Notification notification = Notification.Create(chapterNames[i], string.Format("{0}_{1}", chapterIds[i], j), notif);

				//Debug.Log(string.Format("NotificationController.Init - notification: {0} \n {1}", notif, notification.Description));

				container.AddNotification(notification);

				j++;
			}

			Debug.Log(string.Format("NotificationController.Init - chapter name: {0}, notification count: {1}", chapterIds[i], j));

			_notificationsAll.Add(chapterIds[i], container);
		}

		Debug.Log(string.Format("NotificationController.Init - done"));
	}

	public void InitSelectedChapter(List<SelectedChapterHelperController> selectedChapters)
	{
		_selectedNotifications.Clear();

		foreach (SelectedChapterHelperController selectedChapter in selectedChapters)
		{
			string chapterId = _chapterSelectViewController.GetChapterId(selectedChapter.Index);
			NotificationContainer container = _notificationsAll[chapterId];

			_selectedNotifications.Add(chapterId, container);
		}

		Debug.Log(string.Format("NotificationController.InitSelectedChapter - was added {0} selected chapters",
		                        _selectedNotifications.Count));
	}

	public Notification GetNotifications()
	{
		if (_selectedNotifications.Count < 1)
			return null;

		int i = Random.Range(0, _selectedNotifications.Count - 1);

		int counter = 0;

		NotificationContainer container = null;

		foreach (KeyValuePair<string, NotificationContainer> selectedNotification in _selectedNotifications)
		{
			if (counter == i)
			{
				container = selectedNotification.Value;
				break;
			}
			counter++;
		}

		return GetNotifications(container);
	}

	private Notification GetNotifications(NotificationContainer container)
	{
		if (null == container)
			return null;

		Notification notification = container.GetRandomNotification();

		if (null == notification)
		{
			Debug.Log("NotificationController.GetNotification - in the container "+ container.Name +"{0} was displaying all notifications. Reset it");
			container.Reset();
			notification = container.GetRandomNotification();
		}

		Debug.Log("NotificationController.GetNotification" + notification);

		return notification;
	}

	#endregion
}
