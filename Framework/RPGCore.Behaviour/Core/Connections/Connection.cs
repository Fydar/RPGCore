using System;

namespace RPGCore.Behaviour
{
    public class Connection
    {
        public event Action OnAfterChanged;
        public event Action OnRequested;

        protected void InvokeAfterChanged()
        {
            if (OnAfterChanged != null)
                OnAfterChanged ();
        }
    }

    public class Connection<T> : Connection, IInput<T>, IOutput<T>, ILazyOutput<T>
    {
        private T internalValue;

        public T Value
        {
            get
            {
                return internalValue;
            }
            set
            {
                internalValue = value;
                InvokeAfterChanged ();
            }
        }
    }
}