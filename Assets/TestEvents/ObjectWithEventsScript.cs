using UnityEngine;
using System.Collections;
using System;

public interface ISubscriber {
	void HandleEvent();
}

public class ObjectWithEventsScript : MonoBehaviour/*, ISubscriber*/ {

//	public Action<ObjectWithEventsScript> testEvent;

	public bool engineEventsMode = false;

	int coco = 100;

	readonly MainEventDelegate testDelegate = delegate(EventsTestSceneScript testScene) {
		// coco -= 1;
	};

	public void TriggerEvent() {
//		if (testEvent != null) {
//			testEvent (this);
//		}
	}
		

	public void HandleEvent (EventsTestSceneScript source)
	{
		source.eventsTriggered += 1;
	}

	public static void HandleEventStatic (EventsTestSceneScript source)
	{
		
	}

	public void SubscribeToEvent(EventsTestSceneScript testScript) {
		if (engineEventsMode) {
			testScript.mainEvent += HandleEvent;
		} else {
			//testScript.mainEventSubscribers.Add(this);
			//testScript.mainEventDelegates.Add(HandleEvent);
			testScript.mainEventList.Add(HandleEventStatic);
		}
	}

	public void UnsubscribeToEvent(EventsTestSceneScript testScript) {
		if (engineEventsMode) {
			testScript.mainEvent -= HandleEvent;
		} else {
			//testScript.mainEventSubscribers.Remove(HandleEvent);
			//testScript.mainEventDelegates.Remove(HandleEvent);
			testScript.mainEventList.Remove(HandleEventStatic);
		}
	}

}
