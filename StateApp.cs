using System;
using VContainer.Unity;

namespace App.States
{
    public class StateApp : IStartable
    {
        public enum States
        {
            First,
            Second,
            Third
        }

        public States State { get; private set; }

        public static Action<States> OnStateChanged;

        public void Start()
        {
            //AppState.OnStateChanged += (state) =>
            //{
            //    if (state == State.States.First)
            //            // Do action.
            //};

            UnityEngine.Debug.Log("AppState Start");
        }

        public void SetState(States state)
        {
            if (State == state)
                return;

            UnityEngine.Debug.Log("State: " + state);
            State = state;

            OnStateChanged?.Invoke(state);
        }
    }
}

