using App.States;
using System;
using UnityEngine;
using VContainer;

// Used with StateApp to set the states via an on screen GUI.

namespace App.State.Prototyping
{
    public class StateSetPrototyping : MonoBehaviour
    {
        [Inject]
        StateApp stateApp;

        private void Start()
        {
            StateApp.OnStateChanged += (currentState) =>
            {
                state = currentState;
            };
        }

        StateApp.States state;

        void OnGUI()
        {
            // Determine the initial position for the first button
            int buttonX = 300;
            int buttonY = 10;
            int buttonWidth = 150;
            int buttonHeight = 20;
            int verticalSpacing = 10; // Space between buttons

            // Loop through all possible states
            foreach (StateApp.States enumValue in Enum.GetValues(typeof(StateApp.States)))
            {
                // Create a button for this state
                if (GUI.Button(new Rect(buttonX, buttonY, buttonWidth, buttonHeight), enumValue.ToString()))
                {
                    stateApp.SetState(enumValue);
                }

                // Move down for the next button
                buttonY += buttonHeight + verticalSpacing;
            }

            // Create a "Next state" button
            if (GUI.Button(new Rect(buttonX, buttonY, buttonWidth, buttonHeight), "Next state"))
            {
                stateApp.NextState();
            }

            // Move down to display the current state
            buttonY += buttonHeight + verticalSpacing;

            // Display the current state
            GUI.Label(new Rect(buttonX, buttonY, buttonWidth, buttonHeight), $"Current state: {state}");
        }
    }
}

