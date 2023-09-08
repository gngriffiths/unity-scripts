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

        // Scripts that act on the state can use the static properties. Scripts that change the state need to inject StateApp.
        public static States CurrentState { get; private set; }
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

        public void SetState(States currentState)
        {
            if (CurrentState == currentState)
                return;

            CurrentState = currentState;

            OnStateChanged?.Invoke(CurrentState);
        }

        public void NextState()
        {
            // Get the number of states in the States enum.
            int stateCount = Enum.GetNames(typeof(States)).Length;

            // Convert the current networkedState to an integer, add 1, and apply modulo operation to cycle back to the start if at the end.
            int nextState = ((int)CurrentState + 1) % stateCount;

            SetState((States)nextState);
        }
    }
}

