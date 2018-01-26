using System;

namespace CSharpSnippets.CSharpLanguage.WeakEvents
{
    /// <summary>
    /// http://blog.thekieners.com/2010/02/11/simple-weak-event-listener-for-silverlight/
    /// Originally under MS-PL license.
    /// </summary>
    public class WeakEventListener<TListener, TSource, TEventArgs> where TListener : class
    {
        /// <summary>
        /// WeakReference to the rootInstance listening for the event.
        /// </summary>
        private readonly WeakReference _listener;

        /// <summary>
        /// To hold a reference to source object. With this instance the WeakEventListener 
        /// can guarantee that the handler gets unregistered when listener is released.
        /// </summary>
        private readonly WeakReference _source;

        /// <summary>
        /// Delegate to the method to call when the event fires.
        /// </summary>
        private Action<TListener, object, TEventArgs> _onEventAction;

        /// <summary>
        /// Delegate to the method to call when detaching from the event.
        /// </summary>
        private Action<WeakEventListener<TListener, TSource, TEventArgs>, TSource> _onDetachAction;

        public WeakEventListener(TListener listener, TSource source)
        {
            if (null == listener) throw new ArgumentNullException(nameof(listener));
            if (source == null) throw new ArgumentNullException(nameof(source));

            _listener = new WeakReference(listener);
            _source = new WeakReference(source);
        }

        /// <summary>
        /// Gets or sets the method to call when the event fires.
        /// </summary>
        public Action<TListener, object, TEventArgs> OnEventAction
        {
            get => _onEventAction;
            set
            {
                if (value != null && !value.Method.IsStatic)
                    throw new ArgumentException("OnEventAction method must be static " +
                              "otherwise the event WeakEventListner class does not prevent memory leaks.");

                _onEventAction = value;
            }
        }

        /// <summary>
        /// Gets or sets the method to call when detaching from the event.
        /// </summary>
        public Action<WeakEventListener<TListener, TSource, TEventArgs>, TSource> OnDetachAction
        {
            get => _onDetachAction;
            set
            {
                if (value != null && !value.Method.IsStatic)
                    throw new ArgumentException("OnDetachAction method must be static otherwise the event" +
                                                "WeakEventListner cannot guarantee to unregister the handler.");

                _onDetachAction = value;
            }
        }

        /// <summary>
        /// Handler for the subscribed event calls OnEventAction to handle it.
        /// </summary>
        public void OnEvent(object source, TEventArgs eventArgs)
        {
            var target = (TListener)_listener.Target;
            if (null != target)
                OnEventAction?.Invoke(target, source, eventArgs);
            else
                Detach();
        }

        public void Detach()
        {
            var source = (TSource)_source.Target;
            if (null == OnDetachAction || source == null) return;
            OnDetachAction(this, source);
            OnDetachAction = null;
        }

        ~WeakEventListener()
        {
            Console.WriteLine("WeakEventListener finalised");
        }
    }
}