using System;

namespace X.Engine
{
    public class XStateMachine<T> where T : struct, IComparable, IConvertible, IFormattable
    {
        public T CurrentSate { get; protected set; }
        public T PreviousState { get; protected set; }

        public virtual void ChangeState(T newState)
        {
            if (CurrentSate.Equals(newState))
                return;

            PreviousState = CurrentSate;
            CurrentSate = newState;
        }

        public virtual void RestorePreviousState()
        {
            CurrentSate = PreviousState;
        }
  
    }
}
