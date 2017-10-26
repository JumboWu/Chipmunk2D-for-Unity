using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace X.Engine
{
    [RequireComponent(typeof(XCollider2D))]
    [RequireComponent(typeof(XRigidbody2D))]
    public  class XController : XMonoBehavior
    {
        [Header("角色控制器默认参数")]
        public XControllerParameters DefaultParameters;
        public XControllerParameters Parameters { get { return mOverrideParameters ?? DefaultParameters; } }

        public Vector2 Speed { get { return mSpeed; } }

        protected XControllerParameters mOverrideParameters;
        protected Vector2 mSpeed;
        protected Vector2 mNewPosition;

        protected cpBody mPlayerBody;
        protected cpShape mPlayerShape;

        protected bool Grounded = false;
        protected bool LastJumpState = false;
        protected bool JumpState = false;
        //protected double TimeStep = 1.0 / 180.0;
        protected float RemainingBoost = 0f;

        protected XCharacter mCharacter;
        protected bool mFinished;

        protected override void OnStart()
        {
            base.OnStart();
            XSpaceManager.Instance.Space.SetGravity(new cpVect(0.0f, -Parameters.Gravity));
            mPlayerBody = GetComponent<XRigidbody2D>().Rigidbody2D;
            mPlayerShape = GetComponent<XCollider2D>().Collider2D;
            mPlayerBody.SetVelocityUpdateFunc(PlayerUpdateVelocity);

            mCharacter = GetComponent<XCharacter>();
        }

        protected void PlayerUpdateVelocity(cpVect gravity, float damping, float dt)
        {
            JumpState = mSpeed.y > 0.0f;

            cpVect groundNormal = cpVect.Zero;

            mPlayerBody.EachArbiter((arbiter, o) => BodyArbiterIteratorFunc(arbiter, ref groundNormal), null);

            Grounded = (groundNormal.y > 0.0f);
            if (groundNormal.y < 0.0f)
                RemainingBoost = 0.0f;

            bool boost = (JumpState && RemainingBoost > 0.0f);
            cpVect g = (boost ? cpVect.Zero : gravity);

            mPlayerBody.UpdateVelocity(g, damping, dt);

            float target_vx = Parameters.Velocity * mSpeed.x;

            if (target_vx > 0.1f)
            {
                if (!mCharacter.IsFacingRight)
                    mCharacter.Flip();
            }
            else if(target_vx < -0.1f)
            {
                if (mCharacter.IsFacingRight)
                    mCharacter.Flip();
            }

            cpVect surface_v = new cpVect(-target_vx, 0.0f);

            mPlayerShape.surfaceV = surface_v;
            mPlayerShape.u = (Grounded ? Parameters.AccelerationOnGround / Parameters.Gravity : 0.0f);

            if (!Grounded)
            {
                mPlayerBody.v.x = cp.cpflerpconst(mPlayerBody.v.x, target_vx, Parameters.AccelerationInAir * dt);
            }

            mPlayerBody.v.y = cp.cpfclamp(mPlayerBody.v.y, -Parameters.Fall_Velocity, cp.Infinity);


        }

        protected void BodyArbiterIteratorFunc(cpArbiter arbiter, ref cpVect data)
        {
            cpVect n = cpVect.cpvneg(arbiter.GetNormal());

            if (n.y > ((cpVect)data).y)
            {
                data = n;
            }
        }

        //同步更新调用 物理更新
        public void update(float dt)
        {
            JumpState = mSpeed.y > 0.0f;

            if (JumpState && !LastJumpState && Grounded)
            {
                float jump_v = cp.cpfsqrt((float)2.0 * Parameters.Jump_Height * Parameters.Gravity);
                mPlayerBody.v = cpVect.cpvadd(mPlayerBody.v, new cpVect(0.0f, jump_v));
                RemainingBoost = Parameters.Jump_Boost_Height / jump_v;
            }

            //XSpaceManager.Instance.Space.Step(dt);

            RemainingBoost -= dt;
            LastJumpState = JumpState;

            if (mCharacter != null)
            {
                mCharacter.update(dt);
            }

        }

        protected virtual void EveryFrame()
        {

        }

        public virtual void AddForce(Vector2 force)
        {
            mSpeed += force;
        }

        public virtual void AddHorizontalForce(float x)
        {
            mSpeed.x += x;
        }

        public virtual void AddVerticalForce(float y)
        {
            mSpeed.y += y;
        }

        public virtual void SetForce(Vector2 force)
        {
            mSpeed = force;
        }

        public virtual void SetHorizontalForce(float x)
        {
            mSpeed.x = x;
        }

        public virtual void SetVerticalForce(float y)
        {
            mSpeed.y = y;
        }

        public virtual void GameFrameTurn(float dt)
        {
            update(dt);
        }

        public virtual bool Finished
        {
            get
            {
                return mFinished;
            }
            set
            {
                mFinished = value;
            }
        }
    }
}
