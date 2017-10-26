using UnityEngine;

namespace X.Engine
{
    public class XInput : XMonoBehavior
    {
        public enum XButtonState { Off, ButtonDown, ButtonPressed, ButtonUp}

        public static XButtonState ProcessAxisAsButton(string axisName, float threshold, XButtonState currentState)
        {
            float axisValue = Input.GetAxis(axisName);
            XButtonState state;

            if (axisValue < threshold)
            {
                if (currentState == XButtonState.ButtonPressed)
                {
                    state = XButtonState.ButtonUp;
                }
                else
                {
                    state = XButtonState.Off;
                }
            }
            else
            {
                if (currentState == XButtonState.Off)
                {
                    state = XButtonState.ButtonDown;
                }
                else
                {
                    state = XButtonState.ButtonPressed;
                }
            }

            return state;
        }

        public class XButton
        {
            public XStateMachine<XInput.XButtonState> State { get; protected set; }
            public string ButtonID;

            public delegate void ButtonDownDelegate();
            public delegate void ButtonPressedDelegate();
            public delegate void ButtonUpDelegate();

            public ButtonDownDelegate ButtonDown;
            public ButtonPressedDelegate ButtonPressed;
            public ButtonUpDelegate ButtonUp;



            public XButton(string playerID, string buttonID, ButtonDownDelegate buttonDown, ButtonPressedDelegate buttonPressed, ButtonUpDelegate buttonUp)
            {
                ButtonID = playerID + "_" + buttonID;
                ButtonDown = buttonDown;
                ButtonPressed = buttonPressed;
                ButtonUp = buttonUp;

                State = new XStateMachine<XButtonState>();
                State.ChangeState(XInput.XButtonState.Off);
            }


            public virtual void TriggerButtonDown()
            {
                ButtonDown();
            }

            public virtual void TriggerButtonPressed()
            {
                ButtonPressed();
            }

            public virtual void TriggerButtonUp()
            {
                ButtonUp();
            }
        }
    }
}
