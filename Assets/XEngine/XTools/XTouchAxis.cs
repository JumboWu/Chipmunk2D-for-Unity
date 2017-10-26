using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using System;

namespace X.Engine
{
    [Serializable]
    public class AxisEvent : UnityEvent<float> { }

    [RequireComponent(typeof(Rect))]
    [RequireComponent(typeof(CanvasGroup))]

    public class XTouchAxis : XMonoBehavior, IPointerDownHandler, IPointerUpHandler, IPointerExitHandler, IPointerEnterHandler
    {
        public enum XButtonSate { Off, ButtonDown, ButtonPressed, ButtonUp}

        public UnityEvent AxisPressedFirstTime;
        public UnityEvent AxisReleased;
        public AxisEvent AxisPressed;

        public float PressedOpacity = 0.5f;
        public float AxisValue;

        public bool MouseMode = false;

        public XButtonSate CurrentState { get; protected set; }

        protected CanvasGroup mCanvasGroup;
        protected float mInitOpacity;

        protected override void OnAwake()
        {
            base.OnAwake();

            mCanvasGroup = GetComponent<CanvasGroup>();
            if (mCanvasGroup != null)
            {
                mInitOpacity = mCanvasGroup.alpha;
            }

            ResetButton();
        }

        protected virtual void Update()
        {
            if (AxisPressed != null)
            {
                if (CurrentState == XButtonSate.ButtonPressed)
                {
                    AxisPressed.Invoke(AxisValue);
                }
            }
        }

        protected virtual void LateUpdate()
        {
            if (CurrentState == XButtonSate.ButtonUp)
            {
                CurrentState = XButtonSate.Off;
            }
            if (CurrentState == XButtonSate.ButtonDown)
            {
                CurrentState = XButtonSate.ButtonPressed;
            }
        }

        protected virtual void ResetButton()
        {
            CurrentState = XButtonSate.Off;
            mCanvasGroup.alpha = mInitOpacity;
        }

        protected virtual void OnEnable()
        {
            ResetButton();
        }

        public void OnPointerEnter(PointerEventData data)
        {
            if (!MouseMode)
            {
                OnPointerDown(data);
            }
        }

        public void OnPointerExit(PointerEventData data)
        {
            if(!MouseMode)
            {
                OnPointerUp(data);
            }
        }

        public virtual void OnPointerDown(PointerEventData data)
        {
            if (CurrentState != XButtonSate.Off)
                return;

            CurrentState = XButtonSate.ButtonDown;
            if(mCanvasGroup != null)
            {
                mCanvasGroup.alpha = PressedOpacity;
            }
            if (AxisPressedFirstTime != null)
            {
                AxisPressedFirstTime.Invoke();
            }
        }

        public virtual void OnPointerUp(PointerEventData data)
        {
            if (CurrentState != XButtonSate.ButtonPressed && CurrentState != XButtonSate.ButtonDown)
                return;

            CurrentState = XButtonSate.ButtonUp;
            if(mCanvasGroup != null)
            {
                mCanvasGroup.alpha = mInitOpacity;
            }

            if (AxisReleased != null)
            {
                AxisReleased.Invoke();
            }
            AxisPressed.Invoke(0);
        }
    }
}
