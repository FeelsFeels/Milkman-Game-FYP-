using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class EventTimer : MonoBehaviour
{

    [System.Serializable]
    public class EventInformation
    {
        public string EventDescription;
        //Constructor when making new events during runtime
        public EventInformation(float calltime, UnityEvent eventToCall, bool repeating, float frequency)
        {
            timeToCall = calltime;
            unityEvent = eventToCall;
            isRepeating = repeating;
            repeatRate = frequency;
        }
        public float timeToCall;
        public UnityEvent unityEvent;
        public bool isRepeating;
        public float repeatRate;
    }
    public List<EventInformation> eventList = new List<EventInformation>();
    List<EventInformation> eventsToRemove = new List<EventInformation>(); //Required as collections cannot be modified during an enumeration #cool_vocabulary
    List<EventInformation> eventsToRepeat = new List<EventInformation>(); //Additional list to modify, then return to eventsList

    TimerManager timerManager;

    private void Awake()
    {
        timerManager = FindObjectOfType<TimerManager>();
    }

    private void Update()
    {
        //null check
        if (eventList.Count == 0 || eventList == null)
            return;
        else
            CheckForEvents();
    }

    void CheckForEvents()
    {
        //Check if should invoke events
        float currentTime = Mathf.Floor(timerManager.timeElapsedSinceStart);
        foreach (EventInformation eventInfo in eventList)
        {
            if (currentTime != eventInfo.timeToCall)
                continue;

            //Time to invoke event
            eventInfo.unityEvent.Invoke();
            if (eventInfo.isRepeating)
            {
                //Create new event, add it to list
                EventInformation newEvent = new EventInformation(eventInfo.timeToCall + eventInfo.repeatRate, eventInfo.unityEvent, eventInfo.isRepeating, eventInfo.repeatRate);
                eventsToRepeat.Add(newEvent);
            }
            eventsToRemove.Add(eventInfo);
        }
                
        //After checking is finished, time to edit the list if needed
        //Add all repeating events back into the main list
        foreach(EventInformation repeatingEvent in eventsToRepeat)
        {
            eventList.Add(repeatingEvent);
        }

        //Clear all elements in temporary list
        eventsToRepeat.Clear();

        //Remove finished events in main list
        foreach (EventInformation finishedEvent in eventsToRemove)
        {
            eventList.Remove(finishedEvent);
        }

        //Clear all elements in temporary list
        eventsToRemove.Clear();
    }

    //All this shit doesnt work, i am very sorry for messing up.
    //public void AddNewEvent(float timeBeforeCall, UnityEvent eventToAdd, bool shouldRepeat, float repeatTime)
    //{
    //    EventInformation newEvent = new EventInformation(timerManager.timeElapsedSinceStart + timeBeforeCall, eventToAdd, shouldRepeat, repeatTime);
    //    eventList.Add(newEvent);
    //}

    //public void AddNewEvent(EventInformation newEvent)
    //{
    //    eventList.Add(newEvent);
    //}

    //public void Print()
    //{
    //    print("EVENT RUNNED");
    //}
}

