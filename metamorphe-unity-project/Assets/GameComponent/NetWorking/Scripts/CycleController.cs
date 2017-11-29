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
            talk,
            vote,
            seeRole,
            seeDeath,
            seeDeaths,
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
        TimerController timerController;
        PlayersController playersController;
        GameController gameController;

        public List<int> playersIdDeath;

        [Command]
        public void CmdInitEventsList()
        {
            timerController = GetComponent<TimerController>();
            playersController = GetComponent<PlayersController>();
            gameController = GetComponent<GameController>();

            dayCycle = DayCycle.day;
            timeOf = TimeOf.wait;

            playersIdDeath = new List<int>();

            events = new List<EventItem>();

            events.Add(new EventItemTimeOf(TimeOf.wait, false));
            events.Add(new EventItemDayCycle(DayCycle.night, true));
            events.Add(new EventItemTimeOf(TimeOf.seeRole, false, 10));
            events.Add(new EventItemTimeOf(TimeOf.metamorphe, true, 60));
            events.Add(new EventItemDayCycle(DayCycle.day, true));
            events.Add(new EventItemTimeOf(TimeOf.seeDeaths, true));
            events.Add(new EventItemTimeOf(TimeOf.talk, true, 5));
            events.Add(new EventItemTimeOf(TimeOf.vote, true, 5));
        }

        private void rotateEvent()
        {
            bool previousIsDeath = false;
            if (events[0].type == EventType.changeTimeOf && ((EventItemTimeOf)(events[0])).timeOf == TimeOf.seeDeath)
            {
                previousIsDeath = true;
                events.RemoveAt(0);
            }
            else
            {
                events[0].isValid = events[0].isRepeat;
                EventItem tmp = events[0];
                events.RemoveAt(0);
                events.Add(tmp);
            }
            if (!events[0].isValid)
            {
                rotateEvent();
                return;
            }
            if (events[0].type == EventType.changeTimeOf && ((EventItemTimeOf)(events[0])).timeOf == TimeOf.seeDeaths)
            {
                Debug.Log("see Deaths event");
                events.RemoveAt(0);
                foreach (int i in playersIdDeath)
                {
                    Debug.Log("see Deaths event");
                    events.Insert(0, new EventItemTimeOf(TimeOf.seeDeath, true, 3));
                }
            }
            if (previousIsDeath && events[0].type == EventType.changeTimeOf && !(((EventItemTimeOf)(events[0])).timeOf == TimeOf.seeDeath))
            {
                gameController.CmdCheckWin();
            }
        }

        [Command]
        public void CmdNextEvent()
        {
            rotateEvent();
            if (events[0].type == EventType.changeDayCycle)
            {
                EventItemDayCycle eventDayCycle = (EventItemDayCycle)events[0];
                CmdChangeDayCycle(eventDayCycle.dayCycle);
                CmdNextEvent();
            }
            else if (events[0].type == EventType.changeTimeOf)
            {
                EventItemTimeOf _eventTimeOf = (EventItemTimeOf)events[0];
                CmdChangeEventTime(_eventTimeOf.timeOf, _eventTimeOf.asTimer, _eventTimeOf.timer);
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
        public void CmdChangeEventTime(TimeOf _timeOf, bool asTimer, int timer)
        {
            timeOf = _timeOf;
            GameObject[] gos;
            gos = GameObject.FindGameObjectsWithTag("Player");
            string nameDeath = "";
            if (_timeOf == TimeOf.seeDeath)
            {
                nameDeath = playersController.getPlayerNameOf(playersIdDeath[0]);
                playersController.CmdGiveDeathTo(playersIdDeath[0]);
                playersIdDeath.RemoveAt(0);
            }
            foreach (GameObject go in gos)
            {
                go.GetComponent<Player.CyclePlayerController>().RpcOnEventTimeChange(_timeOf);
                if (_timeOf == TimeOf.seeDeath)
                {
                    go.GetComponent<Player.CyclePlayerController>().RpcOnReceivePlayerDeath(nameDeath);
                }
            }
            if (asTimer)
            {
                timerController.CmdSetTimer(timer);
            }
        }

        //*//   Vote System   //*//

        [Command]
        public void CmdPlayersVotesFor(int playerId)
        {
            playersIdDeath.Add(playerId);
            CmdNextEvent();
        }
        
        //*//   Event Class   //*//

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

            private bool pAsTimer;
            public bool asTimer { get { return pAsTimer; } }

            private int pTimer;
            public int timer { get { return pTimer; } }

            public EventItemTimeOf(TimeOf _timeOf, bool _isRepeat, int _timer = 0)
            {
                type = EventType.changeTimeOf;
                isRepeat = _isRepeat;
                isValid = true;

                pTimeOf = _timeOf;
                pTimer = _timer;
                if (_timer <= 0)
                {
                    pAsTimer = false;
                }
                else
                {
                    pAsTimer = true;
                }
            }
        }

    }
}