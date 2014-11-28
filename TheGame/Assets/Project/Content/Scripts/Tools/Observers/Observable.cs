using System.Collections.Generic;

namespace UnityTools.Observers {
	public abstract class Observable : IObservable {
		private List<IObserver> _observers = new List<IObserver>();
		public void AddObserver(IObserver observer) {	
			if (_observers.Contains( observer ))
				return;
			_observers.Add( observer );
			// observer.UpdateData( this );
		}
		public void RemoveObserver(IObserver observer) {			
			_observers.Remove( observer );
		}
		public virtual void NotifyObservers() {
			if (BatchChangeInProcess)
				return;
			for (int i=_observers.Count-1;i>=0;i--)
				_observers[i].UpdateData( this );
		}
		
		protected bool BatchChangeInProcess { get { return _batchChangeCallsCount > 0; } }
		private int _batchChangeCallsCount;
		/// <summary>
		/// Call this method before you start to edit multiple items in baseData at once.
		/// </summary>
		public void StartBatchChange() {
			_batchChangeCallsCount++;
		}
		/// <summary>
		/// Call this method after you start to edit multiple items in baseData at once.
		/// </summary>
		public void EndBatchChange() {
			_batchChangeCallsCount--;
			NotifyObservers();
		}
	}

	public interface IObservable {
		void AddObserver(IObserver observer);
		void RemoveObserver(IObserver observer);
	}
}