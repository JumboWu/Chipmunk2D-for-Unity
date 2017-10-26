using UnityEngine;
using X.Engine;
using X.Tools;


 class XPlayerDemo : XMonoBehavior
 {
    public GameObject mLevel;
    public XRigidbody2D mRigidbody2D;
    public XCollider2D mCollider2D;

    cpBody mPlayerBody;
    cpShape mPlayerShape;

    Vector2 key = Vector2.zero;
    
    double timestep = 1.0 / 180.0;
    float remainingBoost = 0f;

    static float PLAYER_VELOCITY = 500.0f * 2;

    static float PLAYER_GROUND_ACCEL_TIME = 0.1f;
    float PLAYER_GROUND_ACCEL = (PLAYER_VELOCITY / PLAYER_GROUND_ACCEL_TIME);

    static float PLAYER_AIR_ACCEL_TIME = 0.25f;
    float PLAYER_AIR_ACCEL = (PLAYER_VELOCITY / PLAYER_AIR_ACCEL_TIME);

    float JUMP_HEIGHT = 50.0f * 2;
    float JUMP_BOOST_HEIGHT = 55.0f * 2;
    float FALL_VELOCITY = 900.0f * 2;
    float GRAVITY = 1000.0f * 9.8f;

    public XController mController;

    XPhysicalDebugDraw mDebugDraw;
    protected override void OnAwake()
    {
        base.OnAwake();
        mLevel.SetActive(true);
        //XSpaceManager.Instance.Space.SetGravity(new cpVect(0, -GRAVITY));
        mDebugDraw = new XPhysicalDebugDraw(XSpaceManager.Instance.Space);
        mDebugDraw.SetFlags(cpDrawFlags.All);



    }

    protected override void OnStart()
    {
        base.OnStart();

        //mPlayerBody = mRigidbody2D.Rigidbody2D;
        //mPlayerShape = mCollider2D.Collider2D;
        //mPlayerBody.SetVelocityUpdateFunc (PlayerUpdateVelocity);
        //mController = GetComponent<XController>();
    }

    protected void Update()
    {
        /*
        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            key.y += 1.0f;
        }else if (Input.GetKeyUp(KeyCode.UpArrow))
        {
            key.y += -1.0f;
        }

        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            key.y += -1.0f;
        }else if (Input.GetKeyUp(KeyCode.DownArrow))
        {
            key.y += 1.0f;
        }

        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            key.x += 1.0f;
        }
        else if (Input.GetKeyUp(KeyCode.RightArrow))
        {
            key.x += -1.0f;
        }

        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            key.x += -1.0f;
        }
        else if (Input.GetKeyUp(KeyCode.LeftArrow))
        {
            key.x += 1.0f;
        }
        */

        //update(XSpaceManager.Instance.Space, (float)timestep);
        XSpaceManager.Instance.Space.Step(Time.fixedDeltaTime);
        mController.SetForce(XInputManager.Instance.PrimaryMovement);
        mController.update(Time.fixedDeltaTime);
    }

    bool grounded = false;
    bool lastJumpState = false;
    private void update(cpSpace space, float dt)
    {
        bool jumpState = (key.y > 0.0f);
        if (jumpState && !lastJumpState && grounded)
        {
            float jump_v = cp.cpfsqrt((float)2.0*JUMP_HEIGHT*GRAVITY);
            mPlayerBody.v = cpVect.cpvadd(mPlayerBody.v, new cpVect(0.0f, jump_v));

            remainingBoost = JUMP_BOOST_HEIGHT / jump_v;
        }

        space.Step(dt);

        remainingBoost -= dt;
        lastJumpState = jumpState;
    }

    float ground = 0.0f;
    public void PlayerUpdateVelocity(cpVect gravity, float damping, float dt)
    {
        bool jumpState = key.y > 0.0f;

        cpVect groundNormal = cpVect.Zero;

        mPlayerBody.EachArbiter((arbiter, o) => BodyArbiterIteratorFunc(arbiter, ref groundNormal), null);

        grounded = (groundNormal.y > ground);
        if (groundNormal.y < ground)
            remainingBoost = ground;

        bool boost = (jumpState && remainingBoost > ground);
        cpVect g = (boost ? cpVect.Zero : gravity);

        mPlayerBody.UpdateVelocity(g, damping, dt);


        float target_vx = PLAYER_VELOCITY * key.x;

        cpVect surface_v = new cpVect(-target_vx, 0.0f);

        mPlayerShape.surfaceV = surface_v;
        mPlayerShape.u = (grounded ? PLAYER_GROUND_ACCEL / GRAVITY : ground);

        if (!grounded)
        {
            mPlayerBody.v.x = cp.cpflerpconst(mPlayerBody.v.x, target_vx, PLAYER_AIR_ACCEL * dt);
        }

        mPlayerBody.v.y = cp.cpfclamp(mPlayerBody.v.y, -FALL_VELOCITY, cp.Infinity);

    }

    public void BodyArbiterIteratorFunc(cpArbiter arbiter, ref cpVect data)
    {
        cpVect n = cpVect.cpvneg(arbiter.GetNormal());

        if (n.y > ((cpVect)data).y)
        {
            data = n;
        }
    }

    
   
}

