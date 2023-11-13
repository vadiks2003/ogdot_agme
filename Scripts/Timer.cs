using Godot;
using System;
using System.Collections.Generic;

/* 
used for single timed events
to make it work you must have 
1) new Timer(); it will initialize event list
2) after new Timer() is set you can put timer.Check() to update function
3) Timer.AddEvent(callback function, time since start)
4) with optimized method it is recommended to call the Timer.SortTimes() 
5) Timer.StartCountDown() begins the start time point
so,
Timer timer = new Timer();
timer.StartCountDown();
timer.AddEvent(()=>{stuff}, 5)
timer.SortTimes();

additionally if you want to switch to lazy mode feel free to Timer.SetMode(Mode.Lazy)

TODO: find something better than List to use. linked list?
*/
public class Timer
{
    struct TimeEvent{
        public Callback s_callback;
        public double whentime;
    }

    struct TimeEventLooped{
        public Callback s_callback;
        public double whentime;
        public double interval;
    }

    public enum Mode{
        Optimized,
        Lazy
    }
    double startTime;
    Mode mode;

    public delegate void Callback();
    
    List<TimeEvent> _events;
    List<TimeEventLooped> _loopedEvents;
     // insert to the update function
    public Timer(){
        _events = new List<TimeEvent>();
        _loopedEvents = new List<TimeEventLooped>();
    }
    public void StartCountDown(){
        startTime = Time.GetUnixTimeFromSystem();
    }
    public void AddEvent(Callback callback, double duraton){
        TimeEvent timeevent;
        timeevent.s_callback = callback;
        timeevent.whentime = duraton;
        _events.Add(timeevent);
    }

    public void AddLoopedEvent(Callback callback, double duration){
        TimeEventLooped timeevent;
        timeevent.s_callback = callback;

        timeevent.whentime = duration;
        timeevent.interval = duration;
        _loopedEvents.Add(timeevent);
    }
    public void RemoveAllLoopedElements(){
        _loopedEvents = new List<TimeEventLooped>();
    }

    // call manually right after adding all the AddEvents()
    // maybe i should change List to a fucking std::vector or something wtf
    public void SortTimes()
    {
        TimeEvent[] eventsArr = _events.ToArray(); 

        // since its stupid idea to remove [0] in array and smarter to remove last one, i chose to sort them from bigger to smaller  and check accordingly
        Array.Sort(eventsArr, (x, y) => y.whentime.CompareTo(x.whentime));
        for(int i = 0; i < _events.Count; i++)
        {
            _events[i] = eventsArr[i];
        }
    }
    // checks only last event and removes it from _events if its callback function was called.
    public void Check(){
        // need to have _events SortTimes() for it to work properly
        if(_events.Count > 0 && Time.GetUnixTimeFromSystem() - startTime >= _events[_events.Count-1].whentime)
        {
            _events[_events.Count-1].s_callback();
            _events.Remove(_events[_events.Count-1]);
        }

        if(_loopedEvents.Count > 0){
            for(int i = 0; i < _loopedEvents.Count; i++){
                if(Time.GetUnixTimeFromSystem() - startTime >= _loopedEvents[i].whentime){
                    _loopedEvents[i].s_callback();

                    double nextTime = _loopedEvents[i].whentime + _loopedEvents[i].interval;
                    TimeEventLooped tel;
                    tel.s_callback = _loopedEvents[i].s_callback;
                    tel.interval = _loopedEvents[i].interval;
                    tel.whentime = nextTime;
                    _loopedEvents[i] = tel;
                }
            }
        }
    }
}
