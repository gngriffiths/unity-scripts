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

        public States CurrentState { get; private set; }

        public static Action<States> OnStateChanged;

        public void Start()
        {
            //AppState.OnStateChanged += (CurrentState) =>
            //{
            //    if (CurrentState == CurrentState.States.First)
            //            // Do action.
            //};

            UnityEngine.Debug.Log($"AppState: {CurrentState}");
        }

        public void SetState(States CurrentState)
        {
            if (CurrentState == CurrentState)
                return;

            UnityEngine.Debug.Log("CurrentState: " + CurrentState);
            CurrentState = CurrentState;

            OnStateChanged?.Invoke(CurrentState);
        }
    }
}

