using System;
using System.Collections.Generic;
using UnityEngine;

namespace STU {

    public interface IEventArgs {

    }

    public static class EventDispatcher {

        private class Dispatcher<T> {

            private class Listener {

                private readonly List<Action> listeners;

                public Listener(int capacity = 8) {
                    listeners = new List<Action>(capacity);
                }

                public void AddListener(Action callback) {
                    listeners.Add(callback);
                }
                public void RemoveListener(Action callback) {
                    listeners.Remove(callback);
                }
                public void Dispatch() {
                    foreach(Action listener in listeners) {
                        listener?.Invoke();
                    }
                }
            }

            private readonly Dictionary<T, Listener> listeners;

            public Dispatcher(int capacity = 64) {
                listeners = new Dictionary<T, Listener>(capacity);
            }

            public void AddListener(T key, Action callback) {
                Listener listener;
                if(listeners.TryGetValue(key, out listener)) {
                    listener.AddListener(callback);
                } else {
                    listener = new Listener();
                    listener.AddListener(callback);
                    listeners.Add(key, listener);
                }
            }

            public void RemoveListener(T key, Action callback) {
                Listener listener;
                if(listeners.TryGetValue(key, out listener)) {
                    listener.RemoveListener(callback);
                }
            }

            public void Dispatch(T key) {
                Listener listener;
                if(listeners.TryGetValue(key, out listener)) {
                    listener.Dispatch();
                }
            }
        }

        private class GenericDispatcher {

            private class Listener {

                private readonly Dictionary<int, Action<IEventArgs>> listeners;

                public Listener(int capacity = 8) {
                    this.listeners = new Dictionary<int, Action<IEventArgs>>(capacity);
                }

                public void AddListener<T>(Action<T> callback) where T : IEventArgs {
                    int key = callback.GetHashCode();
                    listeners[key] = ToConvert(callback);
                }

                public void RemoveListener<T>(Action<T> callback) where T : IEventArgs {
                    int key = callback.GetHashCode();
                    listeners.Remove(key);
                }

                public void Dispatch<T>(T eventArgs) where T : IEventArgs {
                    foreach(Action<IEventArgs> listener in listeners.Values) {
                        listener?.Invoke(eventArgs);
                    }
                }

                private Action<IEventArgs> ToConvert<T>(Action<T> callback) {
                    return (eventArgs) => callback?.Invoke((T)eventArgs);
                }
            }

            private readonly Dictionary<Type, Listener> listeners;

            public GenericDispatcher(int capacity = 64) {
                listeners = new Dictionary<Type, Listener>(capacity);
            }

            public void AddListener<T>(Action<T> callback) where T : IEventArgs {
                Listener listener;
                Type key = typeof(T);
                if(listeners.TryGetValue(key, out listener)) {
                    listener.AddListener(callback);
                } else {
                    listener = new Listener();
                    listener.AddListener(callback);
                    listeners.Add(key, listener);
                }
            }

            public void RemoveListener<T>(Action<T> callback) where T : IEventArgs {
                Listener listener;
                if(listeners.TryGetValue(typeof(T), out listener)) {
                    listener.RemoveListener(callback);
                }
            }

            public void Dispatch<T>(T eventArgs) where T : IEventArgs {
                Listener listener;
                if(listeners.TryGetValue(typeof(T), out listener)) {
                    listener.Dispatch(eventArgs);
                }
            }
        }

        private static readonly Dispatcher<int> integerDispatcher = new Dispatcher<int>();
        private static readonly Dispatcher<string> stringDispatcher = new Dispatcher<string>();
        private static readonly GenericDispatcher genericDispatcher = new GenericDispatcher();

        #region Integer Dispatcher

        /// <summary>
        /// Add an event listener.
        /// </summary>
        /// <param name="key"> Unique key to the event. </param>
        /// <param name="callback"> Event callback. </param>
        public static void AddListener(int key, Action callback) {
            integerDispatcher.AddListener(key, callback);
        }

        /// <summary>
        /// Remove an event listener.
        /// </summary>
        /// <param name="key"> Unique key to filter events. </param>
        /// <param name="callback"> Event callback. </param>
        public static void RemoveListener(int key, Action callback) {
            integerDispatcher.RemoveListener(key, callback);
        }

        /// <summary>
        /// Broadcast an event.
        /// </summary>
        /// <param name="key"> Unique key to filter events. </param>
        public static void Dispatch(int key) {
            integerDispatcher.Dispatch(key);
        }

        #endregion

        #region String Dispatcher

        /// <summary>
        /// Add an event listener.
        /// </summary>
        /// <param name="key"> Unique key to the event. </param>
        /// <param name="callback"> Event callback. </param>
        public static void AddListener(string key, Action callback) {
            stringDispatcher.AddListener(key, callback);
        }

        /// <summary>
        /// Remove an event listener.
        /// </summary>
        /// <param name="key"> Unique key to filter events. </param>
        /// <param name="callback"> Event callback. </param>
        public static void RemoveListener(string key, Action callback) {
            stringDispatcher.RemoveListener(key, callback);
        }

        /// <summary>
        /// Broadcast an event.
        /// </summary>
        /// <param name="key"> Unique key to filter events. </param>
        public static void Dispatch(string key) {
            stringDispatcher.Dispatch(key);
        }

        #endregion

        #region Generic Dispatcher

        /// <summary>
        /// Add an event listener.
        /// </summary>
        /// <typeparam name="T"> The type of event. </typeparam>
        /// <param name="callback"> Event callback. </param>
        public static void AddListener<T>(Action<T> callback) where T : IEventArgs {
            genericDispatcher.AddListener(callback);
        }

        /// <summary>
        /// Remove an event listener.
        /// </summary>
        /// <typeparam name="T"> The type of event. </typeparam>
        /// <param name="callback"> Event callback. </param>
        public static void RemoveListener<T>(Action<T> callback) where T : IEventArgs {
            genericDispatcher.RemoveListener(callback);
        }

        /// <summary>
        /// Broadcast an event.
        /// </summary>
        /// <typeparam name="T"> The type of event. </typeparam>
        /// <param name="eventArgs"> Event argument. </param>
        public static void Dispatch<T>(T eventArgs) where T : IEventArgs {
            genericDispatcher.Dispatch(eventArgs);
        }

        #endregion

    }
}
