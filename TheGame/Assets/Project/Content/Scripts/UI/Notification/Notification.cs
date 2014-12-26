using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Assets.Project.Content.Scripts.UI.Notification
{
	public class Notification
	{
		public string ChapterName;
		public string ChapterId;

		public string Description;

		public bool IsShowed;

		public Notification()
		{
			ChapterName = string.Empty;
			ChapterId = string.Empty;
			Description = string.Empty;
			IsShowed = false;
		}

		public Notification(string chapterName, string chapterId, string description)
		{
			ChapterId = chapterName;
			ChapterName = chapterName;
			Description = description;
			IsShowed = false;
		}

		public static Notification Create(string chapterName, string chapterId, string description)
		{
			return new Notification(chapterName, chapterId, description);
		}
	}

	public class NotificationContainer {
		#region Variables

		private string _chapterName;
		private string _chapterId;
		private List<Notification> _notifications;

		public int TotalNotificationCount { get { return _notifications.Count; }}

		public int ShowedCount { get { return _notifications.FindAll(notif => notif.IsShowed = true).Count; } }

		#endregion

		#region Constructor

		public NotificationContainer()
		{
			_chapterId = string.Empty;
			_chapterName = string.Empty;
			_notifications = new List<Notification>();
		}

		public NotificationContainer(string chapterName, string chapterId)
		{
			_chapterName = chapterName;
			_chapterId = chapterId;
			_notifications = new List<Notification>();
		}

		public static NotificationContainer Create()
		{
			return new NotificationContainer();
		}

		public static NotificationContainer Create(string chapterName, string chapterId)
		{
			return new NotificationContainer(chapterName, chapterId);
		}
		#endregion

		#region Actions

		public void AddNotification(Notification notification)
		{
			if (null == notification)
			{
				throw new NullReferenceException("NotificationContainer.AddNotification - trying to add empty notification");
			}

			if (_chapterId != notification.ChapterId)
			{
				Debug.Log("NotificationContainer.AddNotification - trying to add notification from other chapter");
				return;
			}

			_notifications.Add(notification);
		}

		public void Reset()
		{
			foreach (Notification notification in _notifications)
			{
				notification.IsShowed = false;
			}
		}

		private int _counterAttemp = 10;
		
		public Notification GetRandomNotification()
		{
			int i = 0;
			int counter = 0;
			while (true)
			{
				if (counter > _counterAttemp)
				{
					return _notifications.FirstOrDefault(notif => !notif.IsShowed);
				}

				i = Random.Range(0, _notifications.Count - 1);

				if (!_notifications[i].IsShowed)
					break;

				counter++;
			}

			return _notifications[i];
		}
		#endregion
	}
}
