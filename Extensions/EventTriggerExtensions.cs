
namespace PofyTools
{
	using System;
	using System.Collections;
	using UnityEngine;
	using UnityEngine.Events;
	using UnityEngine.EventSystems;

	public static class EventTriggerExtensions
	{
		public static void AddEventTriggerListener (this EventTrigger trigger,
		                                            EventTriggerType eventType,
		                                            System.Action<BaseEventData> callback)
		{
			EventTrigger.Entry entry = new EventTrigger.Entry ();
			entry.eventID = eventType;
			entry.callback = new EventTrigger.TriggerEvent ();
			entry.callback.AddListener (new UnityAction<BaseEventData> (callback));
			trigger.triggers.Add (entry);
		}
	}
}