using UnityEngine;
using UnityEngine.EventSystems;

namespace X.Engine
{
    public class XTouchBigJoystickcs : XTouchJoystick, IPointerDownHandler, IPointerUpHandler
    {
        public Transform TargetJoystickKnob;

        protected Vector2 mInitJoystickKnobPosition;

        public virtual void OnPointerDown(PointerEventData data)
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

            if (TargetJoystickKnob != null)
                TargetJoystickKnob.position = mNewJoystickPosition;
        }

        public virtual void OnPointerUp(PointerEventData data)
        {

            if (TargetJoystickKnob != null)
            {
                mJoystickValue = Vector2.zero;
                TargetJoystickKnob.position = mInitJoystickKnobPosition;
            }
        }

        public override void Init()
        {
            base.Init();

            mInitJoystickKnobPosition = TargetJoystickKnob.position;
        }

        public override void OnDrag(PointerEventData data)
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

            if (TargetJoystickKnob != null)
                TargetJoystickKnob.position = mNewJoystickPosition;
            else
                transform.position = mNewJoystickPosition;
        }

        public override void OnEndDrag(PointerEventData eventData)
        {
            mNewJoystickPosition = mCenterPosition;
            mNewJoystickPosition.z = mInitZPosition;
            if (TargetJoystickKnob != null)
                TargetJoystickKnob.position = mNewJoystickPosition;
            else
                transform.position = mNewJoystickPosition;

            mJoystickValue.x = 0f;
            mJoystickValue.y = 0f;

            mCanvasGroup.alpha = mInitOpacity;


        }
    }
}
