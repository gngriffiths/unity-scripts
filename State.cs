using System;
//using Zenject;

namespace App.States
{
    public class AppState //: IInitializable
    {
        public enum States
        {
            First,
            Second,
            Third
        }

        public States State { get; private set; }

        public static Action<States> OnStateChangedStatic;

        //public void Initialize()
        //{
        //    State.OnStateChangedStatic += (state) =>
        //    {
        //        if (state == State.States.First)
        //            // Do action.
        //    };
        //}

        public void SetState(States state)
        {
            if (State == state)
                return;

            UnityEngine.Debug.Log("State: " + state);
            State = state;

            OnStateChangedStatic?.Invoke(state);
        }
    }
}

