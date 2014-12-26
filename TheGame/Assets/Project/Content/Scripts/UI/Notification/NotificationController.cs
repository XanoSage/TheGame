using System.Collections.Generic;
using Assets.Project.Content.Scripts.UI.Notification;
using UnityEngine;
using System.Collections;

public class NotificationController : MonoBehaviour {

	#region Variables

	public static NotificationController Instance { get; private set; }

	private Dictionary<string, NotificationContainer> _notificationsAll;

	#endregion

	#region MonoBehavour Actions

	void Awake()
	{
		Instance = this;
	}
	
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	#endregion

	#region Actions

	private void Init()
	{
		
	}

	#endregion
}
