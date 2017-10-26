using UnityEngine;

namespace X.Engine
{
    public class XCharacter : XMonoBehavior
    {
        public enum FacingDirection { Left, Right }
        [Header("初始角色面向")]
        public FacingDirection mInitFacingDiraticon = FacingDirection.Right;
        public bool IsFacingRight { get; set; }
        [Header("角色模型")]
        public GameObject mCharacterModel;
        [Header("是否需要改变面向")]
        public bool mFlipOnDirectionChange = true;
        [Header("角色模型面向改变Scale")]
        public Vector3 mFlipValue = new Vector3(-1, 1, 1);

        protected XController mController;
        protected SpriteRenderer mSpriteRenderer;
        protected Color mInitColor;

        protected XCharacterAbility[] mCharacterAbilities;
        protected float mOriginalGravity;

        protected override void OnAwake()
        {
            base.OnAwake();
            Init();
        }


        protected virtual void Init()
        {
            if (mInitFacingDiraticon == FacingDirection.Left)
            {
                IsFacingRight = false;
            }
            else
            {
                IsFacingRight = true;
            }

            mSpriteRenderer = GetComponent<SpriteRenderer>();
            mController = GetComponent<XController>();
            mCharacterAbilities = GetComponents<XCharacterAbility>();

            mOriginalGravity = mController.Parameters.Gravity;
        }


        public virtual void update(float dt)
        {
            HandleCharacterStatus();
            PreProcessAbilities();
            ProcessAbilities();
            LateProcessAbilities();
        }

        protected virtual void HandleCharacterStatus()
        {

        }

        protected virtual void PreProcessAbilities()
        {
            if (mCharacterAbilities == null)
                return;

            for (int i = 0, cnt = mCharacterAbilities.Length; i < cnt; i++)
            {
                if (mCharacterAbilities[i].enabled)
                    mCharacterAbilities[i].ProcessAbility();
            }
        }

        protected virtual void ProcessAbilities()
        {
            if (mCharacterAbilities == null)
                return;

            for (int i = 0, cnt = mCharacterAbilities.Length; i < cnt; i++)
            {
                if (mCharacterAbilities[i].enabled)
                    mCharacterAbilities[i].ProcessAbility();
            }
        }

        protected virtual void LateProcessAbilities()
        {
            if (mCharacterAbilities == null)
                return;

            for (int i = 0, cnt = mCharacterAbilities.Length; i < cnt; i++)
            {
                if (mCharacterAbilities[i].enabled)
                    mCharacterAbilities[i].LateProcessAbility();
            }
        }



        public virtual void Face(FacingDirection facingDirection)
        {
            if (facingDirection == FacingDirection.Right)
            {
                if (IsFacingRight)
                    Flip();
            }
            else
            {
                if (IsFacingRight)
                    Flip();
            }
        }

        public virtual void Flip()
        {
            if (!mFlipOnDirectionChange)
                return;

            if (mCharacterModel != null)
            {
                mCharacterModel.transform.localScale = Vector3.Scale(mCharacterModel.transform.localScale, mFlipValue);
            }
            else
            {
                if (mSpriteRenderer != null)
                {
                    mSpriteRenderer.flipX = !mSpriteRenderer.flipX;
                }
            }

            IsFacingRight = !IsFacingRight;

            if (mCharacterAbilities == null)
                return;

            for (int i = 0, cnt = mCharacterAbilities.Length; i < cnt; i++)
            {
                if (mCharacterAbilities[i].enabled)
                    mCharacterAbilities[i].Flip();
            }

        }

        public virtual void Reset()
        {
            //角色行为 遍历更新
            if (mCharacterAbilities == null)
                return;

            for (int i = 0, cnt = mCharacterAbilities.Length; i < cnt; i++)
            {
                if (mCharacterAbilities[i].enabled)
                    mCharacterAbilities[i].Reset();
            }
        }

    }
}
