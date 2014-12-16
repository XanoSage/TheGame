using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

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
}
