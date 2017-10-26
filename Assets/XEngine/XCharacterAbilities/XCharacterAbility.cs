using UnityEngine;

namespace X.Engine
{
    [RequireComponent(typeof(XCharacter))]
    public class XCharacterAbility : XMonoBehavior
    {
        public bool mAbilityPermitted = true;

        protected XCharacter mCharacter;
        protected XController mController;

        protected SpriteRenderer mSpriteRenderer;

    

        protected override void OnStart()
        {
            base.OnStart();
            Init();
            
        }

        protected virtual void Init()
        {
            mCharacter = GetComponent<XCharacter>();
            mController = GetComponent<XController>();
            mSpriteRenderer = GetComponent<SpriteRenderer>();
       
        }

        public virtual void Flip()
        {

        }

        public virtual void Reset()
        {

        }

        public virtual void SetPermit(bool permitted)
        {
            mAbilityPermitted = permitted;
        }

        public virtual void PreProcessAbility()
        {
           
        }

        public virtual void ProcessAbility()
        {
           
        }

        public virtual void LateProcessAbility()
        {
           
        }
    }
}
