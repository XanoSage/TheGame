using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Project.Content.Scripts.Timer
{
	public class SimpleTimer {
		#region Constatnts

		private const int TotalMinutesInOneDay = 24*60;

		#endregion

		#region Variables

		private int _hour;

		public int Hour
		{
			get { return _hour; }
		}

		private int _minutes;

		public int Minutes
		{
			get { return _minutes; }
		}

		private int _interval;

		public int Interval
		{
			get { return _interval; }
		}

		private int _notificationCount;

		public int NotificationCount
		{
			get { return _notificationCount; }
		}

		#endregion

		#region Constructor

		public SimpleTimer()
		{
			_hour = 0;
			_minutes = 0;
			_interval = 0;

			_notificationCount = 0;
		}

		public SimpleTimer(int hour, int minutes, int interval)
		{
			_hour = hour;
			_minutes = minutes;
			_interval = interval;
		}

		public static SimpleTimer Create(int hour, int minutes, int interval)
		{
			return new SimpleTimer(hour, minutes, interval);
		}

		#endregion

		#region Actions

		public void CalculateNotificationsCount()
		{
			DateTime now = DateTime.Now;
			int currentTimeInMinutes = now.Hour*60 + now.Minute;
			int restOfDayInMinutes = TotalMinutesInOneDay - currentTimeInMinutes;

			int notifCountWithoutInterval = restOfDayInMinutes/(Hour*60 + Minutes);
			_notificationCount = restOfDayInMinutes/(Hour*60 + Minutes + _interval);

			if (_notificationCount == 0)
				_notificationCount = 1;

			Debug.Log(string.Format("time in minutes: {0}, rest of day: {1}, notification count without interval: {2}, notification count: {3}", 
			                            currentTimeInMinutes, restOfDayInMinutes, notifCountWithoutInterval, _notificationCount));
		}

		#endregion
	}
}
