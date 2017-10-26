using X.Engine;
using UnityEngine;

class XPhysicalDebugDraw : cpDebugDraw
{
    public static cpColor CONSTRAINT_COLOR = cpColor.Grey; //new cpColor(0, 1, 0, 0.5f);
    public static cpColor TRANSPARENT_COLOR = new cpColor(0, 0, 0, 0.0f);

    cpVect[] springPoints = new cpVect[]{
    new cpVect(0.00f, 0.0f),
    new cpVect(0.20f, 0.0f),
    new cpVect(0.25f, 3.0f),
    new cpVect(0.30f, -6.0f),
    new cpVect(0.35f, 6.0f),
    new cpVect(0.40f, -6.0f),
    new cpVect(0.45f, 6.0f),
    new cpVect(0.50f, -6.0f),
    new cpVect(0.55f, 6.0f),
    new cpVect(0.60f, -6.0f),
    new cpVect(0.65f, 6.0f),
    new cpVect(0.70f, -3.0f),
    new cpVect(0.75f, 6.0f),
    new cpVect(0.80f, 0.0f),
    new cpVect(1.00f, 0.0f)
        };

    private cpSpace mSpace;
    public XPhysicalDebugDraw(cpSpace space)
    {
        mSpace = space;
    }


    public override void DrawPolygon(cpVect[] vertices, int vertexCount, cpColor color)
    {
        for (int i = 0; i < vertexCount; i++)
        {
            if (i != vertexCount - 1)
                DrawSegment(vertices[i], vertices[i + 1], color);
            else
                DrawSegment(vertices[i], vertices[0], color);

        }
    }

    /// Draw a solid closed polygon provided in CCW order.
    public override void DrawSolidPolygon(cpVect[] vertices, int vertexCount, cpColor color)
    {
        
    }

    /// Draw a circle.
    public override void DrawCircle(cpVect center, float radius, cpColor color)
    {
        float rh = radius / 2;

        Vector2 p1 = new Vector2(center.x, center.y - radius);
        Vector2 p1_tan_a = new Vector2(center.x - rh, center.y - radius);
        Vector2 p1_tan_b = new Vector2(center.x + rh, center.y - radius);

        Vector2 p2 = new Vector2(center.x + radius, center.y);
        Vector2 p2_tan_a = new Vector2(center.x + radius, center.y - rh);
        Vector2 p2_tan_b = new Vector2(center.x + radius, center.y + rh);

        Vector2 p3 = new Vector2(center.x, center.y + radius);
        Vector2 p3_tan_a = new Vector2(center.x - rh, center.y + radius);
        Vector2 p3_tan_b = new Vector2(center.x + rh, center.y + radius);

        Vector2 p4 = new Vector2(center.x - radius, center.y);
        Vector2 p4_tan_a = new Vector2(center.x - radius, center.y - rh);
        Vector2 p4_tan_b = new Vector2(center.x - radius, center.y + rh);

        DrawBezierLine(p1, p1_tan_b, p2, p2_tan_a, color, 1, 5);
        DrawBezierLine(p2, p2_tan_b, p3, p3_tan_b, color, 1, 5);
        DrawBezierLine(p3, p3_tan_a, p4, p4_tan_b, color, 1, 5);
        DrawBezierLine(p4, p4_tan_a, p1, p1_tan_a, color, 1, 5);
    }

    public  void DrawBezierLine(Vector2 start, Vector2 startTangent, Vector2 end, Vector2 endTangent, cpColor color, float width, int segments)
    {
        Vector2 lastV = CubeBezier(start, startTangent, end, endTangent, 0);
        for (int i = 1; i < segments + 1; ++i)
        {
            Vector2 v = CubeBezier(start, startTangent, end, endTangent, i / (float)segments);
            DrawSegment(new cpVect(lastV.x , lastV.y), new cpVect(v.x, v.y), width, color);
            lastV = v;
        }
    }


    private  Vector2 CubeBezier(Vector2 s, Vector2 st, Vector2 e, Vector2 et, float t)
    {
        float rt = 1 - t;
        return rt * rt * rt * s + 3 * rt * rt * t * st + 3 * rt * t * t * et + t * t * t * e;
    }

    /// Draw a solid circle.
    public override void DrawSolidCircle(cpVect center, float radius, cpVect axis, cpColor color)
    {

    }

    /// Draw a line segment.
    public override void DrawSegment(cpVect p1, cpVect p2, cpColor color)
    {
        DrawSegment(p1, p2, 1, color);
    }

    /// Draw a line segment with a strokeWidth.
    public override void DrawSegment(cpVect pp1, cpVect pp2, float lineWidth, cpColor color)
    {
        Vector3 p1 = Camera.main.ScreenToWorldPoint(new Vector3(pp1.x, pp1.y, 0f));
        Vector3 p2 = Camera.main.ScreenToWorldPoint(new Vector3(pp2.x, pp2.y, 0f));

        float dx = p2.x - p1.x;
        float dy = p2.y - p1.y;
        float len = Mathf.Sqrt(dx * dx + dy * dy);

        if (len < 0.001f)
            return;

        float wdx = lineWidth * dy / len;
        float wdy = lineWidth * dx / len;

        Matrix4x4 matrix = Matrix4x4.identity;

        matrix.m00 = dx;
        matrix.m01 = -wdx;
        matrix.m03 = (float)(p1.x + 0.5 * wdx);
        matrix.m10 = dy;
        matrix.m11 = wdy;
        matrix.m13 = p1.y - 0.5f * wdy;

        GL.PushMatrix();
        GL.LoadOrtho();
        GL.Color(Color.green);
        GL.MultMatrix(matrix);

        GL.PopMatrix();
    }

    public override void DrawString(int x, int y, string format, params object[] objects)
    {

    }

    public override void DrawSpring(cpVect a, cpVect b, cpColor cpColor)
    {

    }

    public override void DrawPoint(cpVect p, float size, cpColor color)
    {

    }

    public override void DrawBB(cpBB bb, cpColor color)
    {

    }

    public void DrawDot(cpVect pos , float radius, cpColor color)
    {

    }

    public void DrawShape(cpShape shape)
    {
        cpBody body = shape.body;
        cpColor color = cp.GetShapeColor(shape); ;// ColorForBody(body);

        switch (shape.shapeType)
        {
            case cpShapeType.Circle:
                {
                    cpCircleShape circle = (cpCircleShape)shape;

                    if ((Flags & cpDrawFlags.BB) == cpDrawFlags.BB || (Flags & cpDrawFlags.All) == cpDrawFlags.All)
                        Draw(circle.bb);

                    if ((Flags & cpDrawFlags.Shapes) == cpDrawFlags.Shapes|| (Flags & cpDrawFlags.All) == cpDrawFlags.All)
                        Draw(circle, color);
                }
                break;
            case cpShapeType.Segment:
                {
                    cpSegmentShape seg = (cpSegmentShape)shape;
                    if ((Flags & cpDrawFlags.BB) == cpDrawFlags.BB || (Flags & cpDrawFlags.All) == cpDrawFlags.All)
                        Draw(seg.bb);

                    if ((Flags & cpDrawFlags.Shapes) == cpDrawFlags.Shapes || (Flags & cpDrawFlags.All) == cpDrawFlags.All)
                        Draw(seg, color);
                }
                break;
            case cpShapeType.Polygon:
                {
                    cpPolyShape poly = (cpPolyShape)shape;
                    if ((Flags & cpDrawFlags.BB) == cpDrawFlags.BB || (Flags & cpDrawFlags.All) == cpDrawFlags.All)
                        Draw(poly.bb);

                    if ((Flags & cpDrawFlags.Shapes) == cpDrawFlags.Shapes || (Flags & cpDrawFlags.All) == cpDrawFlags.All)
                    {
                        Draw(poly, color);
                    }
                }
                break;
            default:
                cp.AssertHard(false, "Bad assertion in DrawShape()");
                break;
        }
    }

    public void DrawConstraint(cpConstraint constraint)
    {
        System.Type klass = constraint.GetType();
        if (klass == typeof(cpPinJoint))
            Draw((cpPinJoint)constraint);
        else if (klass == typeof(cpSlideJoint))
            Draw((cpSlideJoint)constraint);
        else if (klass == typeof(cpGrooveJoint))
            Draw((cpGrooveJoint)constraint);
        else if (klass == typeof(cpDampedSpring))
            Draw((cpDampedSpring)constraint);
        else if (klass == typeof(cpDampedRotarySpring))
            Draw((cpDampedRotarySpring)constraint);
        else if (klass == typeof(cpSimpleMotor))
            Draw((cpSimpleMotor)constraint);
    }

    public void Draw(cpPolyShape poly, cpColor color)
    {
        cpColor fill = new cpColor(color);
        fill.a = cp.cpflerp(color.a, 1.0f, 0.5f);
        DrawPolygon(poly.GetVertices(), poly.Count, color);
    }

    public void Draw(cpBB bb)
    {
        Draw(bb, cpColor.CyanBlue);
    }

    public void Draw(cpBB bb, cpColor color)
    {
        DrawPolygon(new cpVect[] {

                        new cpVect(bb.r, bb.b),
                    new cpVect(bb.r, bb.t),
                    new cpVect(bb.l, bb.t),
                    new cpVect(bb.l, bb.b)

                }, 4, color);
    }

    public void Draw(cpContact contact)
    {
        DrawDot(contact.r1, 0.5f, cpColor.Red);
        DrawDot(contact.r2, 0.5f, cpColor.Red);
    }

    public void Draw(cpCircleShape circle, cpColor color)
    {
        cpVect center = circle.tc;
        float radius = circle.r;
        cpVect To = cpVect.cpvadd(cpVect.cpvmult(circle.body.GetRotation(), circle.r), (circle.tc));
        DrawCircle(center, cp.cpfmax(radius, 1.0f), color);
        DrawSegment(center, To, 0.5f, cpColor.Grey);
    }

    private void Draw(cpSegmentShape seg, cpColor color)
    {
        DrawFatSegment(seg.ta, seg.tb, seg.r, color);
    }

    private void DrawFatSegment(cpVect ta, cpVect tb, float r, cpColor color)
    {
        cpColor fill = new cpColor(color);
        fill.a = cp.cpflerp(color.a, 1.0f, 0.5f);

        DrawSegment(ta, tb, Mathf.Max(1, r), fill);
    }

    public void Draw(cpVect point)
    {
        Draw(point, 0.5f);
    }

    public void Draw(cpVect point, cpColor color)
    {
        Draw(point, 0.5f, color);
    }

    public void Draw(cpVect point, float radius)
    {
        DrawDot(point, radius, cpColor.Red);
    }

    public void Draw(cpVect point, float radius, cpColor color)
    {
        DrawDot(point, radius, color);
    }

    private void Draw(cpDampedRotarySpring constraint)
    {
        //Not used
    }

    private void Draw(cpDampedSpring constraint)
    {
        var a = constraint.a.LocalToWorld(constraint.GetAnchorA());
        var b = constraint.b.LocalToWorld(constraint.GetAnchorB());

        DrawSpring(a, b, CONSTRAINT_COLOR);
    }

    public void Draw(cpSimpleMotor cpSimpleMotor)
    {
        //Not used
    }

    private void Draw(cpGrooveJoint constraint)
    {
        var a = constraint.a.LocalToWorld(constraint.grv_a);
        var b = constraint.a.LocalToWorld(constraint.grv_b);
        var c = constraint.b.LocalToWorld(constraint.anchorB);

        DrawSegment(a, b, 1, CONSTRAINT_COLOR);
        DrawCircle(c, 5f, CONSTRAINT_COLOR);
    }

    private void Draw(cpPivotJoint constraint)
    {
        cpVect a = cpTransform.Point(constraint.a.transform, constraint.GetAnchorA());
        cpVect b = cpTransform.Point(constraint.b.transform, constraint.GetAnchorB());

        //DrawSegment(a, b, 1, cpColor.Grey);
        DrawDot(a, 3, CONSTRAINT_COLOR);
        DrawDot(b, 3, CONSTRAINT_COLOR);
    }

    public void Draw(cpSlideJoint constraint)
    {
        cpVect a = cpTransform.Point(constraint.a.transform, constraint.GetAnchorA());
        cpVect b = cpTransform.Point(constraint.b.transform, constraint.GetAnchorB());

        DrawSegment(a, b, 1, cpColor.Grey);
        DrawDot(a, 5, CONSTRAINT_COLOR);
        DrawDot(b, 5, CONSTRAINT_COLOR);
    }

    public void Draw(cpPinJoint constraint)
    {
        cpVect a = cpTransform.Point(constraint.a.transform, constraint.GetAnchorA());
        cpVect b = cpTransform.Point(constraint.b.transform, constraint.GetAnchorB());

        DrawSegment(a, b, 1, cpColor.Grey);
        DrawDot(a, 5, CONSTRAINT_COLOR);
        DrawDot(b, 5, CONSTRAINT_COLOR);
    }


    public void DebugDraw()
    {
        if (mSpace == null)
            return;

        if ((Flags & cpDrawFlags.All) == cpDrawFlags.All || (Flags & cpDrawFlags.BB) == cpDrawFlags.BB || (Flags & cpDrawFlags.Shapes) == cpDrawFlags.Shapes)
        {
            mSpace.EachShape(DrawShape);
        }

        if ((Flags & cpDrawFlags.Joints) == cpDrawFlags.Joints || (Flags & cpDrawFlags.All) == cpDrawFlags.All)
        {
            mSpace.EachConstraint(DrawConstraint);
        }

        if ((Flags & cpDrawFlags.All) == cpDrawFlags.All || (Flags & cpDrawFlags.ContactPoints) == cpDrawFlags.ContactPoints)
        {
            for (var i = 0; i < mSpace.arbiters.Count; i++)
            {
                for (int j = 0; j < mSpace.arbiters[i].contacts.Count; j++)
                {
                    Draw(mSpace.arbiters[i].contacts[i]);
                }
            }
        }
    }



}

