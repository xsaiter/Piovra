using System;
using System.Collections.Generic;

namespace Piovra;

public sealed class EventAggregator {
    static readonly Lazy<EventAggregator> _sole = new(() => new EventAggregator());
    static EventAggregator Sole => _sole.Value;
    EventAggregator() { }

    public static TEvent GetEvent<TEvent>() where TEvent : IEvent, new() =>
        Sole.GetOrCreateEvent<TEvent>();

    readonly Dictionary<Type, IEvent> _events = [];

    TEvent GetOrCreateEvent<TEvent>() where TEvent : IEvent, new() {
        var key = Key<TEvent>();
        if (_events.TryGetValue(key, out var value)) {
            return (TEvent)value;
        }
        var newEvent = new TEvent();
        _events.Add(key, newEvent);
        return newEvent;
    }

    static Type Key<TEvent>() => typeof(TEvent);

    public interface IEvent { }

    public class Event<TPayload> : IEvent where TPayload : notnull {
        readonly Dictionary<Type, List<Act>> _map = [];

        public bool Publish(TPayload payload) {
            var key = Key();
            if (_map.TryGetValue(key, out var acts)) {
                acts.ForEach(_ => _.Action(payload));
                return true;
            }
            return false;
        }

        public void Subscribe(Action<TPayload> action) {
            var key = Key();
            if (_map.TryGetValue(key, out var acts)) {
                acts.Add(Act.From(action));
            } else {
                _map.Add(key, Act.From(action).AsList());
            }
        }

        public bool Unsubscribe(Action<TPayload> action) {
            var key = Key();
            if (_map.TryGetValue(key, out var acts)) {
                acts.RemoveAll(_ => _.To<TPayload>() == action);
                return true;
            }
            return false;
        }

        Type Key() => GetType();

        record Act(object OrigAction, Action<object> Action) {
            public Action<T> To<T>() => (Action<T>)OrigAction;
            public static Act From<T>(Action<T> action) => new(action, _ => action((T)_));
        }
    }
}
