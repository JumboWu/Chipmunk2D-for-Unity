using UnityEngine;
using X.Engine;


public class XRigidbody2D : XMonoBehavior
{
    //bingding
    private cpBody mRigidbody2D;
    public cpBody Rigidbody2D { get { return mRigidbody2D; } private set { mRigidbody2D = value; } }

    [HideInInspector]
    public Vector2 mPostion = Vector2.zero;
    [HideInInspector]
    public float mAngle = 0f;
    [HideInInspector]
    public cpBodyType mBodyType = cpBodyType.DYNAMIC;
    [HideInInspector]
    public float mMass = 1.0f;
    [HideInInspector]
    public float mMonent = cp.Infinity;


    //public cpBodyType BodyType { get { return mRigidbody2D.bodyType; } set { mBodyType = value; mRigidbody2D.SetBodyType(mBodyType); } }

    //public float Mass { get { return mRigidbody2D.GetMass(); } set { mMass = value; mRigidbody2D.SetMass(mMass); } }
    public void Init()
    {
        mRigidbody2D = new cpBody(mMass, mMonent);
        mRigidbody2D.SetBodyType(mBodyType);
        mRigidbody2D.SetPositionUpdateFunc(UpdatePosition);//Set Update Poition callback

        cpVect pos = new cpVect(this.transform.position.x * XSpaceManager.PixelsPerUnit, this.transform.position.y * XSpaceManager.PixelsPerUnit);
        mRigidbody2D.SetPosition(pos);
        mRigidbody2D.SetAngle(cp.cpfradian(this.transform.eulerAngles.z));
        // mRigidbody2D.SetTransform(pos, cp.cpfradian(this.transform.eulerAngles.z)); //设置这个不对，position不更新

        //X.Tools.LogUtils.Debug(X.Tools.LogTag.Test, this.gameObject.name + ":" + mRigidbody2D.GetPosition().ToString());
        //this.transform.SetPositionAndRotation(Vector3.zero, Quaternion.Euler(0, 0, 0));
        //X.Tools.LogUtils.Debug(string.Format("rotation:{0}, eulerAngles:{1}, {2} ,{3}", this.transform.rotation, this.transform.eulerAngles, this.transform.localRotation, this.transform.localEulerAngles));
    }

    protected override void OnAwake()
    {
        base.OnAwake();
    }

    /*
    
    protected void OnEnable()
    {
        X.Tools.LogUtils.Debug("XRigidbody2D AddBody");
        App.Instance.xSpaceMgr.Space.AddBody(mRigidbody2D);
    }

    protected void OnDisable()
    {
        X.Tools.LogUtils.Debug("XRigidbody2D DeactivateBody");
        App.Instance.xSpaceMgr.Space.DeactivateBody(mRigidbody2D);
    }
    */

    cpVect cpPos;
    //UpdatePosition then to Rendering game
    public void UpdatePosition(float dt)
    {
        mRigidbody2D.UpdatePosition(dt);

        // X.Tools.LogUtils.Debug(X.Tools.LogTag.Test, this.gameObject.name + ":" + mRigidbody2D.GetPosition().ToString());
        cpPos = mRigidbody2D.GetPosition();
        mPostion.Set(cpPos.x, cpPos.y);
        mAngle = cp.cpfangle(mRigidbody2D.GetAngle());
        //this.transform.SetPositionAndRotation(new Vector3(cpPos.x * 0.01f, cpPos.y * 0.01f, 0f), Quaternion.Euler(0f,0f, mAngle));//z-Euler
        this.transform.position = mPostion * XSpaceManager.UnitsPerPixel;
        this.transform.rotation = Quaternion.Euler(0f, 0f, mAngle); 
    }

    
   


}

