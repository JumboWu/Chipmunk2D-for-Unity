using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Events;
using System;

namespace X.Engine
{
    [Serializable]
    public class JoystickEvent : UnityEvent<Vector2> { }

    [RequireComponent(typeof(Rect))]
    [RequireComponent(typeof(CanvasGroup))]
    public class XTouchJoystick : XMonoBehavior, IDragHandler, IEndDragHandler
    {
        public Camera TargetCamera;
        public float PressedOpacity = 0.5f;

        public bool HorizontalAxisEnabled = true;
        public bool VerticalAxisEnabled = true;

        public float MaxRange = 1.5f;

        public JoystickEvent JoystickValue;

        public RenderMode ParentCanvasRenderMode { get; protected set; }


        protected Vector2 mCenterPosition;
        protected Vector2 mJoystickValue;
        protected RectTransform mCanvasRectTransform;

        protected Vector2 mNewTargetPosition;
        protected Vector3 mNewJoystickPosition;
        protected float mInitZPosition;

        protected CanvasGroup mCanvasGroup;
        protected float mInitOpacity;


        protected override void OnStart()
        {
            base.OnStart();
            Init();

        }

        public virtual void Init()
        {
            mCanvasRectTransform = GetComponentInParent<Canvas>().transform as RectTransform;
            mCanvasGroup = GetComponent<CanvasGroup>();

            SetCenterPosition();
            if (TargetCamera == null)
            {
                throw new Exception("XTouchJoystick: target camera is none");
            }

            ParentCanvasRenderMode = GetComponentInParent<Canvas>().renderMode;
            mInitZPosition = transform.position.z;
            mInitOpacity = mCanvasGroup.alpha;
        }

        protected virtual void Update()
        {
            if (JoystickValue != null)
            {
                if (HorizontalAxisEnabled || VerticalAxisEnabled)
                {
                    JoystickValue.Invoke(mJoystickValue);
                }
            }
        }

        public virtual void SetCenterPosition()
        {
            mCenterPosition = GetComponent<RectTransform>().transform.position;
        }

        public virtual void SetCenterPosition(Vector3 newPosition)
        {
            mCenterPosition = newPosition;
        }

        public virtual void OnDrag(PointerEventData data)
        {
            mCanvasGroup.alpha = PressedOpacity;

            if (ParentCanvasRenderMode == RenderMode.ScreenSpaceCamera)
            {
                mNewTargetPosition = TargetCamera.ScreenToWorldPoint(data.position);
            }
            else
            {
                mNewTargetPosition = data.position;
            }

            mNewTargetPosition = Vector2.ClampMagnitude(mNewTargetPosition - mCenterPosition, MaxRange);

            if (!HorizontalAxisEnabled)
            {
                mNewTargetPosition.x = 0;
            }

            if (!VerticalAxisEnabled)
            {
                mNewTargetPosition.y = 0;
            }

            mJoystickValue.x = EvaluateInputValue(mNewTargetPosition.x);
            mJoystickValue.y = EvaluateInputValue(mNewTargetPosition.y);
            
            mNewJoystickPosition = mCenterPosition + mNewTargetPosition;
            mNewJoystickPosition.z = mInitZPosition;

            transform.position = mNewJoystickPosition;

        }

        public virtual void OnEndDrag(PointerEventData data)
        {
            mNewJoystickPosition = mCenterPosition;
            mNewJoystickPosition.z = mInitZPosition;
            transform.position = mNewJoystickPosition;
            mJoystickValue.x = 0f;
            mJoystickValue.y = 0f;

            mCanvasGroup.alpha = mInitOpacity;
        }

        protected virtual float EvaluateInputValue(float pos)
        {
            return Mathf.InverseLerp(0, MaxRange, Mathf.Abs(pos)) * Math.Sign(pos);
        }

        protected virtual void OnEnable()
        {
            Init();
            mCanvasGroup.alpha = mInitOpacity;
        }

    }
}
