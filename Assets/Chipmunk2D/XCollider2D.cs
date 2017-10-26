using UnityEngine;
using X.Engine;
using UnityEngine.EventSystems;


public enum ColliderType
{
    Circle,
    Segment,
    Box,
    Polygon
}

//[RequireComponent(typeof(XRigidbody2D))]
public class XCollider2D : XMonoBehavior,  IBeginDragHandler, IDragHandler, IEndDragHandler
{
    private cpBody mRigidbody2D;
    private cpShape mCollider2D;
    public cpShape Collider2D { get { return mCollider2D; } private set { mCollider2D = value; } }


    [HideInInspector]
    public ColliderType mShapeType = ColliderType.Box;

    [HideInInspector]
    public int mGroup = cp.NO_GROUP;
    [HideInInspector]
    public cpShapeFilterMask mCategory = cpShapeFilterMask.Default;
    [HideInInspector]
    public cpShapeFilterMask mMask = cpShapeFilterMask.Default;

    //only for segement
    [HideInInspector]
    //[Header("Segment")]
    public Vector2 mStart = Vector2.zero;
    [HideInInspector]
    public Vector2 mEnd = Vector2.zero;

    [HideInInspector]
    public bool mTrigger = false; // sensor
    [HideInInspector]
    public float mRadius = 4f;
    [HideInInspector]
    public float mElasticity = 1.0f;
    [HideInInspector]
    public float mFriction = 1.0f;


    //only for circle
    [HideInInspector]
    //[Header("Circle")]
    public Vector2 mCenter = Vector2.zero;

    [HideInInspector]
    public Vector2 mSize = Vector2.one;

    //only for polygon
    //[HideInInspector]
    //[Header("Polyon")]
    public Vector2[] mVects = new Vector2[4] { new Vector2(1, -0.5f), new Vector2( 1, 0.5f ) , new Vector2(-1, 0.5f) , new Vector2(-1, -0.5f) };

    
    protected override void OnAwake()
    {
        base.OnAwake();

        Vector2 pos = this.transform.position;//static body(0,0) should add postion , Dynamic use Rigidbody position as parent
        XRigidbody2D xRigidbody2D = this.GetComponent<XRigidbody2D>();
        if (xRigidbody2D != null)
        {
            if (xRigidbody2D.mBodyType != cpBodyType.STATIC)
            {
                xRigidbody2D.Init();
                mRigidbody2D = xRigidbody2D.Rigidbody2D;
                pos = Vector2.zero;
            }
            else
            {
                mRigidbody2D = XSpaceManager.Instance.Space.GetStaticBody();
            }
            
        }
        else
        {
            //静态的，不能移动
            mRigidbody2D = XSpaceManager.Instance.Space.GetStaticBody();
        }

        //local
        Vector2 left, bottom, right, top;
        Quaternion rotation;
        
        switch (mShapeType) {
            case ColliderType.Segment:
                float angle = Vector2.Angle(this.mEnd - this.mStart, Vector2.right);
                rotation = Quaternion.AngleAxis(angle + this.transform.eulerAngles.z, Vector3.forward);

                left = rotation * new Vector2(this.mStart.x, this.mStart.y - this.mRadius);
                bottom = rotation * new Vector2(this.mEnd.x, this.mEnd.y - this.mRadius);
                right = rotation * new Vector2(this.mEnd.x, this.mEnd.y + this.mRadius);
                top = rotation * new Vector2(this.mStart.x, this.mStart.y + this.mRadius);

                Vector2 start = (Vector2)pos + (left + top) / 2;
                Vector2 end = (Vector2)pos + (bottom + right) / 2;

                mCollider2D = new cpSegmentShape(mRigidbody2D, new cpVect(start.x, start.y) * XSpaceManager.PixelsPerUnit, new cpVect(end.x, end.y) * XSpaceManager.PixelsPerUnit, mRadius * XSpaceManager.PixelsPerUnit);

                break;
            case ColliderType.Circle:
                mCollider2D = new cpCircleShape(mRigidbody2D, mRadius * XSpaceManager.PixelsPerUnit, new cpVect(pos.x + mCenter.x , pos.y + mCenter.y) * XSpaceManager.PixelsPerUnit);
                break;
            case ColliderType.Box:
                mCollider2D = cpPolyShape.BoxShape2(mRigidbody2D, new cpBB((pos.x + mCenter.x - mSize.x * this.transform.localScale.x/2) * XSpaceManager.PixelsPerUnit, (pos.y + mCenter.y - mSize.y * this.transform.localScale.y/2) * XSpaceManager.PixelsPerUnit, (pos.x + mCenter.x + mSize.x * this.transform.localScale.x/2) * XSpaceManager.PixelsPerUnit,  (pos.y + mCenter.y + mSize.y * this.transform.localScale.y /2) * XSpaceManager.PixelsPerUnit) , mRadius * XSpaceManager.PixelsPerUnit);
                break;
            case ColliderType.Polygon:
                int count = mVects.Length;
                cpVect [] vts = new cpVect [mVects.Length];
                
                for (int i = 0; i < count; i++)
                {
                    vts[i] = cpVect.Zero;
                    vts[i].x = (pos.x + mVects[i].x) * XSpaceManager.PixelsPerUnit;
                    vts[i].y = (pos.y + mVects[i].y) * XSpaceManager.PixelsPerUnit;
                }
                mCollider2D = new cpPolyShape(mRigidbody2D, mVects.Length, vts, mRadius * XSpaceManager.PixelsPerUnit);
                break;
            //case cpShapeType.NumShapes:
            //    break;
        }

        mCollider2D.SetSensor(mTrigger);
        mCollider2D.SetElasticity(mElasticity);
        mCollider2D.SetFriction(mFriction);
        cpShapeFilter filter = new cpShapeFilter(mGroup, (int)mCategory, (int)mMask);
        mCollider2D.SetFilter(filter);
    }

    protected void OnEnable()
    {
      //  X.Tools.LogUtils.Debug("XCollider2D AddShape");
      //  X.Tools.LogUtils.Debug(X.Tools.LogTag.Test, this.gameObject.name + ":" + mRigidbody2D.GetPosition().ToString());
        if (mRigidbody2D.bodyType != cpBodyType.STATIC)
            XSpaceManager.Instance.Space.AddBody(mRigidbody2D);
        XSpaceManager.Instance.Space.AddShape(mCollider2D);
    }

    protected void OnDisable()
    {
       // X.Tools.LogUtils.Debug("XCollider2D RemoveShape");
        
         XSpaceManager.Instance.Space.RemoveShape(mCollider2D);
        if (mRigidbody2D.bodyType != cpBodyType.STATIC)
            XSpaceManager.Instance.Space.DeactivateBody(mRigidbody2D);
    }

    private void OnDrawGizmos()
    {
       // Gizmos.color = Color.yellow;
    }

    private void OnDrawGizmosSelected()
    {
        
    }


    public void OnBeginDrag(PointerEventData data)
    {
        X.Tools.LogUtils.Debug("XCollider2D BeginDrag:" + this.gameObject.name); 
    }

    public void OnDrag(PointerEventData data)
    {
        if (mRigidbody2D != null)
        {
            cpVect pos = new cpVect(data.position.x - Screen.width / 2, data.position.y - Screen.height / 2);
            mRigidbody2D.SetPosition(pos);
        }
    }

    public void OnEndDrag(PointerEventData data)
    {
        X.Tools.LogUtils.Debug("XCollider2D EndDrag:" + this.gameObject.name);
    }

}

