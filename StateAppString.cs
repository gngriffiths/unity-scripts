using System;

namespace App.States
{
    public class StateAppString
    {
        // Scripts that act on the state can use the static properties. Scripts that change the state need to inject StateApp.
        public static string CurrentState { get; private set; }
        public static Action<string> OnStateChanged;

        public void SetState(string currentState)
        {
            if (CurrentState == currentState)
                return;

            CurrentState = currentState;

            UnityEngine.Debug.Log($"AppState: {CurrentState}");
            OnStateChanged?.Invoke(CurrentState);
        }
    }
}

