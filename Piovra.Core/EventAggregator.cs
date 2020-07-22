using System;
using System.Collections.Generic;

namespace Piovra {
    public sealed class EventAggregator {
        static readonly Lazy<EventAggregator> _sole = new Lazy<EventAggregator>(() => new EventAggregator());
        static EventAggregator Sole => _sole.Value;
        EventAggregator() { }

        public static TEvent GetEvent<TEvent>() where TEvent : IEvent, new() => Sole.GetOrCreateEvent<TEvent>();

        readonly Dictionary<Type, IEvent> _events = new Dictionary<Type, IEvent>();

        TEvent GetOrCreateEvent<TEvent>() where TEvent : IEvent, new() {
            var key = Key<TEvent>();
            if (_events.ContainsKey(key)) {
                return (TEvent)_events[key];
            }
            var newEvent = new TEvent();
            _events.Add(key, newEvent);
            return newEvent;
        }

        Type Key<TEvent>() => typeof(TEvent);

        public interface IEvent { }

        public class Event<TPayload> : IEvent {
            readonly Dictionary<Type, List<Act>> _map = new Dictionary<Type, List<Act>>();

            public bool Publish(TPayload payload) {
                var key = Key();
                if (_map.ContainsKey(key)) {
                    var acts = _map[key];
                    acts.ForEach(_ => _.Action(payload));
                    return true;
                }
                return false;
            }

            public void Subscribe(Action<TPayload> action) {
                var key = Key();
                if (_map.ContainsKey(key)) {
                    var acts = _map[key];
                    acts.Add(Act.From(action));
                } else {
                    _map.Add(key, Act.From(action).AsList());
                }
            }

            public bool Unsubscribe(Action<TPayload> action) {
                var key = Key();
                if (_map.ContainsKey(key)) {
                    var acts = _map[key];
                    acts.RemoveAll(_ => _.To<TPayload>() == action);
                    return true;
                }
                return false;
            }

            Type Key() => GetType();

            public class Act {
                object OrigAction { get; }
                public Action<object> Action { get; }
                Act(object origAction, Action<object> action) => (OrigAction, Action) = (origAction, action);
                public Action<T> To<T>() => (Action<T>)OrigAction;
                public static Act From<T>(Action<T> action) => new Act(action, _ => action((T)_));
            }
        }
    }
}