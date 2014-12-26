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

		public int Hour { get; set; }

		public int Minutes { get; set; }

		public int Interval { get; set; }

		public int NotificationCount { get; set; }

		public int Seconds { get; set; }

		public float TotalInSeconds { get; private set; }

		public float CurrentTotalInSeconds { get; set; }

		#endregion

		#region Constructor

		public SimpleTimer()
		{
			Hour = 0;
			Minutes = 0;
			Interval = 0;

			NotificationCount = 0;
			TotalInSeconds = 0f;
			CurrentTotalInSeconds = 0;
		}

		public SimpleTimer(int hour, int minutes, int interval)
		{
			Hour = hour;
			Minutes = minutes;
			Interval = interval;
			NotificationCount = 0;

			TotalInSeconds = Minutes*60 + Hour *60*60;
		}

		public SimpleTimer(SimpleTimer timer)
		{
			Hour = timer.Hour;
			Minutes = timer.Minutes;
			Seconds = timer.Seconds;
			Interval = timer.Interval;
			NotificationCount = 0;

			TotalInSeconds = Minutes*60 + Hour *60*60;
			CurrentTotalInSeconds = 0;
		}

		public SimpleTimer(int totalMinutes)
		{
			Hour = totalMinutes/60;
			Minutes = totalMinutes%60;
			Interval = 0;
			Seconds = 0;
			NotificationCount = 0;

			TotalInSeconds = Minutes*60 + Hour *60*60;
			CurrentTotalInSeconds = 0;
		}

		public static SimpleTimer Create(int hour, int minutes, int interval)
		{
			return new SimpleTimer(hour, minutes, interval);
		}

		public static SimpleTimer Create(SimpleTimer timer)
		{
			return new SimpleTimer(timer);
		}

		public static SimpleTimer Create(int totalMinutes)
		{
			return new SimpleTimer(totalMinutes);
		}

		#endregion

		#region Actions

		public void CalculateNotificationsCount()
		{
			DateTime now = DateTime.Now;
			int currentTimeInMinutes = now.Hour*60 + now.Minute;
			int restOfDayInMinutes = TotalMinutesInOneDay - currentTimeInMinutes;

			int notifCountWithoutInterval = restOfDayInMinutes/(Hour*60 + Minutes);
			NotificationCount = restOfDayInMinutes/(Hour*60 + Minutes + Interval);

			if (NotificationCount == 0)
				NotificationCount = 1;

			Debug.Log(string.Format("time in minutes: {0}, rest of day: {1}, notification count without interval: {2}, notification count: {3}", 
			                            currentTimeInMinutes, restOfDayInMinutes, notifCountWithoutInterval, NotificationCount));
		}

		public override string ToString () {
			string str = string.Format("{0:D2}:{1:D2}:{2:D2}", Hour, Minutes, Seconds);
			return str;
		}

		#endregion
	}

	public class TimerController {
		#region Constants

		private const float OneSeconds = 1f;
		private const int SecondInMinute = 59;

		#endregion

		#region Variables

		private SimpleTimer _timer;

		public event Action OnTimerEndAction;

		public event Action<string> OnTimerChangeAction;

		private float _timerCount;

		private DateTime _dateStartTime;

		#endregion

		#region Constructor

		public TimerController()
		{
			_timer = null;
			_dateStartTime = DateTime.UtcNow;
			Debug.Log("CurrentTime: " + _dateStartTime);
		}

		public TimerController(SimpleTimer timer)
		{
			_timer = timer;
			_dateStartTime = DateTime.UtcNow;
			Debug.Log("CurrentTime: " + _dateStartTime);
		}

		public static TimerController Create(SimpleTimer timer)
		{
			return	new TimerController(timer);
		}

		#endregion

		#region Actions
		
		private void OnTimerEnd()
		{
			_timer.CurrentTotalInSeconds = 0;

			if (null != OnTimerEndAction)
				OnTimerEndAction();
		}

		public void Update()
		{
			if (_timer == null)
				return;

			if (IsEndOfTimer())
			{
				OnTimerEnd();
				return;
			}

			_timer.CurrentTotalInSeconds += Time.deltaTime;

			if (_timerCount < OneSeconds)
			{
				_timerCount += Time.deltaTime;
			}

			if (_timerCount >= OneSeconds)
			{
				_timerCount = 0f;
				MinusOneSeconds();
			}
		}

		private void MinusOneSeconds()
		{
			_timer.Seconds -= 1;
			if (_timer.Seconds < 0)
			{
				if (_timer.Minutes > 0)
				{
					_timer.Minutes -= 1;
					_timer.Seconds = SecondInMinute;
				}
				else if (_timer.Minutes <= 0)
				{
					if (_timer.Hour > 0)
					{
						_timer.Hour -= 1;
						_timer.Minutes = SecondInMinute;
						_timer.Seconds = SecondInMinute;
					}
					else
					{
						_timer.Minutes = 0;
					}

					if (IsEndOfTimer())
					{
						OnTimerEnd();
					}
				}
			}

			if (null != OnTimerChangeAction)
			{
				OnTimerChangeAction(_timer.ToString());
			}
		}

		private bool IsEndOfTimer()
		{
			return _timer.Hour == 0 && _timer.Minutes == 0 && _timer.Seconds == 0;
		}


		#endregion
	}
}
