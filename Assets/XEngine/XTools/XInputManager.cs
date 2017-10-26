using UnityEngine;
using System.Collections;
using X.Tools;
using UnityEngine.Events;
using System.Collections.Generic;

namespace X.Engine
{
    public class XInputManager : XMonoSingleton<XInputManager>
    {
        public string PlayerID = "Player1";
        public enum XInputForcedMode { None, Mobile, Desktop}
        public enum MovementControl { Joystick, Arrows}
        public bool AutoMobileDetection = true;

        public XInputForcedMode mForcedMode;
        public MovementControl mMovementControl = MovementControl.Joystick;

        public bool IsMobile { get; protected set; }

        public bool SmoothMovement = true;
        public Vector2 Threshold = new Vector2(0.1f, 0.1f);

        public XInput.XButton JumpButton { get; protected set; }
        public XInput.XButton PauseButton { get; protected set; }
        

        public Vector2 PrimaryMovement { get { return mPrimaryMovement;} }


        protected List<XInput.XButton> mButtonList;
        protected Vector2 mPrimaryMovement = Vector2.zero;
        protected string mAxisHorizontal;
        protected string mAxisVertical;

        protected override void OnStart()
        {
            base.OnStart();
            ControlModeDetection();
            InitButton();
            InitAxis();

        }

        protected virtual void LateUpdate()
        {
            ProcessButtonState();
        }

        protected virtual void Update()
        {
            if (!IsMobile)
            {
                mPrimaryMovement = GetAxis();
                GetInputButtons();
            }
        }

        protected virtual void ControlModeDetection()
        {
            //UI层控件切换处理
            if (AutoMobileDetection)
            {
#if UNITY_ANDROID || UNITY_IPHONE
                IsMobile = true;
#endif 
            }

            if (mForcedMode == XInputForcedMode.Mobile)
            {
                IsMobile = true;
            }

            if (mForcedMode == XInputForcedMode.Desktop)
            {
                IsMobile = false;
            }
        }

        protected virtual void InitButton()
        {
            mButtonList = new List<XInput.XButton>();
            mButtonList.Add(JumpButton = new XInput.XButton(PlayerID, "Jump", JumpButtonDown, JumpButtonPressed, JumpButtonUp));
            mButtonList.Add(PauseButton = new XInput.XButton(PlayerID, "Pause", PauseButtonDown, PauseButtonPressed, PauseButtonUp));
        }

        protected virtual void InitAxis()
        {
            mAxisHorizontal = PlayerID + "_" + "Horizontal";
            mAxisVertical = PlayerID + "_" + "Vertical";
        }


        protected virtual void GetInputButtons()
        {
            for (int i = 0, cnt = mButtonList.Count; i < cnt; i++)
            {
                if (Input.GetButton(mButtonList[i].ButtonID))
                {
                    mButtonList[i].TriggerButtonPressed();
                }
                if (Input.GetButtonDown(mButtonList[i].ButtonID))
                {
                    mButtonList[i].TriggerButtonDown();
                }
                if (Input.GetButtonUp(mButtonList[i].ButtonID))
                {
                    mButtonList[i].TriggerButtonUp();
                }
            }
        }

        public virtual void ProcessButtonState()
        {
            for (int i = 0, cnt = mButtonList.Count; i < cnt; i++)
            {
                if (mButtonList[i].State.CurrentSate == XInput.XButtonState.ButtonDown)
                {
                    mButtonList[i].State.ChangeState(XInput.XButtonState.ButtonPressed);
                }
                if (mButtonList[i].State.CurrentSate == XInput.XButtonState.ButtonUp)
                {
                    mButtonList[i].State.ChangeState(XInput.XButtonState.Off);
                }
            }
        }


        public virtual void SetMovement(Vector2 movement)
        {
            
            mPrimaryMovement.x = movement.x;
            mPrimaryMovement.y = movement.y;
        }

        public virtual void SetHorizontalMovement(float horizontalInput)
        {
            mPrimaryMovement.x = horizontalInput;
        }

        public virtual void SetVerticalMovement(float verticalInput)
        {
            mPrimaryMovement.y = verticalInput;
        }

        public virtual Vector2 GetAxis()
        {
            Vector2 movement = Vector2.zero;
            if (!IsMobile)
            {
                if (SmoothMovement)
                {
                    movement.x = Input.GetAxis(mAxisHorizontal);
                    movement.y = Input.GetAxis(mAxisVertical);
                }
                else
                {
                    movement.x = Input.GetAxisRaw(mAxisHorizontal);
                    movement.y = Input.GetAxisRaw(mAxisVertical);
                }
            }

            return movement;
        }

        public virtual void JumpButtonDown() { JumpButton.State.ChangeState(XInput.XButtonState.ButtonDown); }
        public virtual void JumpButtonPressed() { JumpButton.State.ChangeState(XInput.XButtonState.ButtonPressed); }
        public virtual void JumpButtonUp() { JumpButton.State.ChangeState(XInput.XButtonState.ButtonUp); }

        public virtual void PauseButtonDown() { PauseButton.State.ChangeState(XInput.XButtonState.ButtonDown); }
        public virtual void PauseButtonPressed() { PauseButton.State.ChangeState(XInput.XButtonState.ButtonPressed); }
        public virtual void PauseButtonUp() { PauseButton.State.ChangeState(XInput.XButtonState.ButtonUp); }

    }
}