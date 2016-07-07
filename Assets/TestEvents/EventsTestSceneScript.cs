using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class EventListBase<T>
{
	protected readonly List<T> handlers = new List<T> ();

	public void Add(T t)
	{
		handlers.Add (t);
	}

	public void Remove(T t)
	{
		handlers.Remove (t);
	}

	public void Clear()
	{
		handlers.Clear ();
	}

	public static EventListBase<T> operator +(EventListBase<T> c1, T t)
	{
		c1.Add (t);
		return c1;
	}

	public static EventListBase<T> operator -(EventListBase<T> c1, T t)
	{
		c1.Remove (t);
		return c1;
	}
}

public class EventList<T> : EventListBase<T>
{
	protected readonly Action<T> handlerInvoker;

	public EventList(Action<T> handlerInvoker) {
		this.handlerInvoker = handlerInvoker;
	}
		
	public void Invoke()
	{
		for (int i = 0; i < handlers.Count; i++) {
			var handler = handlers [i];
			handlerInvoker (handler);
		}
	}
}

public class GenericEventList : EventListBase<Action>
{
	public void OnEvent()
	{
		for (int i = 0; i < handlers.Count; i++) {
			var handler = handlers [i];
			handler.Invoke ();
		}
	}
}

public class HealthEventList : EventListBase<MainEventDelegateWithDamage>
{
	public void OnEvent(EventsTestSceneScript testSceneScript, float damage)
	{
		for (int i = 0; i < handlers.Count; i++) {
			var handler = handlers [i];
			handler.Invoke (testSceneScript, damage);
		}
	}
}

public delegate void MainEventDelegateWithDamage(EventsTestSceneScript testScene, float damage);
public delegate void MainEventDelegate(EventsTestSceneScript testScene);

//public delegate void OtherEventDelegate(EventsTestSceneScript testScene);

public class EventsTestSceneScript : MonoBehaviour {

	public GameObject prefab;
	public int packCount = 10;
	List<ObjectWithEventsScript> objects = new List<ObjectWithEventsScript>();

	public UnityEngine.UI.Text objectsCountText;
	public UnityEngine.UI.Text infoText;

	// public Action mainEvent;
	//public List<ISubscriber> mainEventSubscribers = new List<ISubscriber> ();
//	public List<Action<EventsTestSceneScript>> mainEventDelegates = new List<Action<EventsTestSceneScript>> ();

	public event Action<EventsTestSceneScript> mainEvent;

	bool engineEventsMode = false;

	public int eventsTriggered = 0;

	void EventDelegateInvoker (MainEventDelegate m)
	{
		m.Invoke (this);
	}

//	void EventDelegateOtherInvoker (OtherEventDelegate m)
//	{
//		m.Invoke (this);
//	}

	public EventList<MainEventDelegate> mainEventList;

	public HealthEventList EventDamage = new HealthEventList();
	public HealthEventList EventDeath = new HealthEventList();

	//EventList<OtherEventDelegate> otherEventList;


	void Awake() {
		mainEventList = new EventList<MainEventDelegate> (EventDelegateInvoker);
		//otherEventList = new EventList<OtherEventDelegate> (EventDelegateOtherInvoker);
	}

	// Update is called once per frame
	void Update () {
//		if (Input.GetKeyDown (KeyCode.Alpha1)) {			
//			CreateObjectPack ();
//		} else if (Input.GetKeyDown (KeyCode.R)) {			
//			RemoveEvents ();
//			//flagRemove = true;
//		} else if (Input.GetKeyDown (KeyCode.S)) {			
//			SubscribeEvents ();
//		} else if (Input.GetKeyDown (KeyCode.U)) {			
//			UnsubscribeEvents ();
//			//flagRemove = false;
//		} else if (Input.GetKeyDown (KeyCode.T)) {
//			TriggerEvents ();
//		}

//		SubscribeEvents ();
//		if (flagRemove) {			
//			RemoveEvents ();
//		} else {
//			UnsubscribeEvents ();
//		}
		SubscribeEvents ();
		UnsubscribeEvents ();
	}


	public void CreateObjectPack() {
		for (int i = 0; i < packCount; i++) {
			var obj = GameObject.Instantiate (prefab);
			obj.transform.position = new Vector3 (UnityEngine.Random.Range (-40, 40), UnityEngine.Random.Range (-40, 40), 0);
			var objectWithEventsScript = obj.GetComponent<ObjectWithEventsScript> ();
			objectWithEventsScript.engineEventsMode = this.engineEventsMode;
			objects.Add (objectWithEventsScript);
		}
		objectsCountText.text = "Objects: " + objects.Count.ToString ();
		infoText.text = "Created " + packCount  + " objects";
	}

	void HandleEvent (ObjectWithEventsScript obj)
	{
	}

	public void SubscribeEvents() {
		for (int i = 0, objectsCount = objects.Count; i < objectsCount; i++) {
			var obj = objects [i];
			//obj.testEvent += HandleEvent;
			obj.SubscribeToEvent(this);
		}
		infoText.text = "Subscribe events";
	}

	public void UnsubscribeEvents() {
				for (int i = 0, objectsCount = objects.Count; i < objectsCount; i++) {
			var obj = objects [i];
			//obj.testEvent -= HandleEvent;
			obj.UnsubscribeToEvent(this);
		}
		infoText.text = "Unsubscribe events";
	}

	public void RemoveEvents() {
//		for (int i = 0, objectsCount = objects.Count; i < objectsCount; i++) {
//			var obj = objects [i];
//			obj.testEvent = null;
//		}
		if (engineEventsMode) {
			this.mainEvent = null;
		} else {
			//this.mainEventSubscribers.Clear();
//			this.mainEventDelegates.Clear();
			this.mainEventList.Clear();
		}
		infoText.text = "Cleared events";
	}

	public void TriggerEvents() {
		eventsTriggered = 0;
		if (engineEventsMode) {
			for (int i = 0, objectsCount = objects.Count; i < objectsCount; i++) {
				var obj = objects [i];
				obj.TriggerEvent ();
			}
		} else {
//			for (int i = 0, mainEventSubscribersCount = mainEventSubscribers.Count; i < mainEventSubscribersCount; i++) {
//				var obj = mainEventSubscribers [i];
//				obj.HandleEvent ();
////			}
//			for (int i = 0, mainEventSubscribersCount = mainEventDelegates.Count; i < mainEventSubscribersCount; i++) {
//				var obj = mainEventDelegates [i];
//				obj.Invoke (this);
//			}
			this.mainEventList.Invoke();
			EventDamage.OnEvent (this, 100.0f);
		}
		infoText.text = "Triggered events = " + eventsTriggered.ToString();
	}

	public void ToggleEventMode() {
		engineEventsMode = !engineEventsMode;
		for (int i = 0, objectsCount = objects.Count; i < objectsCount; i++) {
			var obj = objects [i];
			obj.engineEventsMode = engineEventsMode;
		}
		infoText.text = "engineEventsMode = " + (engineEventsMode ? "ON" : "OFF");
	}
}
