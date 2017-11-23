using System.Collections;
using System.Collections.Generic;
using UnityEngine.Networking;
using UnityEngine;

namespace GameController
{
    public class CycleController : NetworkBehaviour
    {
        //*//   Enum   //*//

        public enum EventType
        {
            changeDayCycle,
            changeTimeOf
        }

        public enum DayCycle
        {
            day,
            night
        }
        public enum TimeOf
        {
            vote,
            metamorphe,
            wait
        }

        //*//   SyncVar   //*//

        [SyncVar]
        public DayCycle dayCycle;
        [SyncVar]
        public TimeOf timeOf;

        //*//   List of event   //*//

        List<EventItem> events;

        [Command]
        public void CmdInitEventsList()
        {
            dayCycle = DayCycle.day;
            timeOf = TimeOf.wait;

            Debug.Log("Network: Init event list");
            events = new List<EventItem>();

            events.Add(new EventItemTimeOf(TimeOf.wait, false));
            events.Add(new EventItemDayCycle(DayCycle.night, true));
            events.Add(new EventItemDayCycle(DayCycle.day, true));
            events.Add(new EventItemTimeOf(TimeOf.metamorphe, false));
        }

        private void rotateEvent()
        {
            EventItem tmp = events[0];
            events.RemoveAt(0);
            events.Add(tmp);
            if (!events[0].isValid)
            {
                rotateEvent();
                return;
            }
            Debug.Log("Game: Rotate event");
        }

        [Command]
        public void CmdNextEvent()
        {
            rotateEvent();
            if (events[0].type == EventType.changeDayCycle)
            {
                EventItemDayCycle eventDayCycle = (EventItemDayCycle)events[0];
                CmdChangeDayCycle(eventDayCycle.dayCycle);
                eventDayCycle.isValid = eventDayCycle.isRepeat;
                CmdNextEvent();
            }
            else if (events[0].type == EventType.changeTimeOf)
            {
                EventItemTimeOf eventTimeOf = (EventItemTimeOf)events[0];
                CmdChangeEventTime(eventTimeOf.timeOf);
                eventTimeOf.isValid = eventTimeOf.isRepeat;
            }
        }

        [Command]
        public void CmdChangeDayCycle(DayCycle _dayCycle)
        {
            dayCycle = _dayCycle;   
            GameObject[] gos;
            gos = GameObject.FindGameObjectsWithTag("Player");
            foreach (GameObject go in gos)
            {
                go.GetComponent<Player.CyclePlayerController>().RpcOnDayCycleChange(_dayCycle);
            }
        }

        [Command]
        public void CmdChangeEventTime(TimeOf _timeOf)
        {
            timeOf = _timeOf;
            GameObject[] gos;
            gos = GameObject.FindGameObjectsWithTag("Player");
            foreach (GameObject go in gos)
            {
                go.GetComponent<Player.CyclePlayerController>().RpcOnEventTimeChange(_timeOf);
            }
        }

        public class EventItem
        {
            public EventType type;
            public bool isRepeat;
            public bool isValid;
        }

        public class EventItemDayCycle : EventItem
        {
            private DayCycle pDayCycle;
            public DayCycle dayCycle { get { return pDayCycle; } }

            public EventItemDayCycle(DayCycle _dayCycle, bool _isRepeat)
            {
                type = EventType.changeDayCycle;
                isRepeat = _isRepeat;
                isValid = true;

                pDayCycle = _dayCycle;
            }
        }

        public class EventItemTimeOf : EventItem
        {
            private TimeOf pTimeOf;
            public TimeOf timeOf { get { return pTimeOf; } }

            public EventItemTimeOf(TimeOf _timeOf, bool _isRepeat)
            {
                type = EventType.changeTimeOf;
                isRepeat = _isRepeat;
                isValid = true;

                pTimeOf = _timeOf;
            }
        }

    }
}