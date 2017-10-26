using UnityEditor;
using UnityEngine;
using X.Engine;

[CustomEditor(typeof(XCollider2D))]
class XCollider2DEditor : Editor
{
    SerializedProperty mVects;
    /*
    private void OnEnable()
    {
        
    }
    */

    public override void OnInspectorGUI()
    {
        XCollider2D collider2D = target as XCollider2D;
        //GUILayout.Label("功能：碰撞器形状设置");
        collider2D.mTrigger = EditorGUILayout.Toggle("碰撞触发Trigger", collider2D.mTrigger);
        collider2D.mShapeType = (ColliderType)EditorGUILayout.EnumPopup("碰撞器类型ShapeType", collider2D.mShapeType);
        collider2D.mRadius = EditorGUILayout.FloatField("半径Radius", collider2D.mRadius);
        collider2D.mElasticity = EditorGUILayout.FloatField("弹力Elasticity", collider2D.mElasticity);
        collider2D.mFriction = EditorGUILayout.FloatField("摩檫力Friction", collider2D.mFriction);

        collider2D.mGroup = EditorGUILayout.IntField("群组Group", collider2D.mGroup);

        collider2D.mCategory = (cpShapeFilterMask)EditorGUILayout.EnumMaskField("分类Category", collider2D.mCategory);
        collider2D.mMask = (cpShapeFilterMask)EditorGUILayout.EnumMaskField("层级Mask", collider2D.mMask);
        /*
        int select1 = (int)collider2D.mCategory;
        int select2 = (int)collider2D.mMask;
        int num = System.Enum.GetNames(typeof(cpShapeFilterMask)).Length;
        for (int i = 0; i < num; i++)
        {
            cpShapeFilterMask filter = (cpShapeFilterMask)i;

            if (IsShapeFilterMask(collider2D.mCategory, filter))
            {
                Debug.Log("selectEventType :" + filter);
            }
        }
        
    */
        switch (collider2D.mShapeType)
        {
            case ColliderType.Segment:
                collider2D.mStart = EditorGUILayout.Vector2Field("起始位置Start", collider2D.mStart);
                collider2D.mEnd = EditorGUILayout.Vector2Field("结束位置End", collider2D.mEnd);
                break;
            case ColliderType.Circle:
                collider2D.mCenter = EditorGUILayout.Vector2Field("中心Center", collider2D.mCenter);
                break;
            case ColliderType.Box:
                collider2D.mCenter = EditorGUILayout.Vector2Field("中心Center", collider2D.mCenter);
                collider2D.mSize = EditorGUILayout.Vector2Field("大小Size", collider2D.mSize);
                break;
            case ColliderType.Polygon:
                //EditorGUIUtility.LookLikeInspector();
                mVects = serializedObject.FindProperty("mVects");
                EditorGUI.BeginChangeCheck();
                EditorGUILayout.PropertyField(mVects, new GUIContent("顶点Vects"), true);
                if (EditorGUI.EndChangeCheck())
                    serializedObject.ApplyModifiedProperties();
                //EditorGUIUtility.LookLikeControls();
                break;
            default:
                EditorGUILayout.HelpBox("This Component only support segment,circle,polygon now!!! Ask Jumbo", MessageType.Error);
                break;
        }

        if (GUI.changed)
            EditorUtility.SetDirty(target);
        
    }

    private void OnSceneGUI()
    {
        XCollider2D collider2D = target as XCollider2D;
        Vector3 pos = collider2D.transform.position;
        Handles.color = Color.yellow;
        Vector2 size, left, bottom, right, top;
        Quaternion rotation;


        if (Event.current.type == EventType.Repaint)
        {
            switch (collider2D.mShapeType)
            {
                case ColliderType.Segment:
                    float angle = Vector2.Angle(collider2D.mEnd - collider2D.mStart, Vector2.right);
                    rotation = Quaternion.AngleAxis(angle + collider2D.transform.localEulerAngles.z, Vector3.forward);

                    if (collider2D.mRadius > 0)
                    {
                        left = rotation * new Vector2(collider2D.mStart.x, collider2D.mStart.y - collider2D.mRadius);
                        bottom = rotation * new Vector2(collider2D.mEnd.x, collider2D.mEnd.y - collider2D.mRadius);
                        right = rotation * new Vector2(collider2D.mEnd.x, collider2D.mEnd.y + collider2D.mRadius);
                        top = rotation * new Vector2(collider2D.mStart.x, collider2D.mStart.y + collider2D.mRadius);

                        Handles.DrawLine((Vector2)pos + left, (Vector2)pos + bottom);
                        Handles.DrawLine((Vector2)pos + bottom, (Vector2)pos + right);
                        Handles.DrawLine((Vector2)pos + right, (Vector2)pos + top);
                        Handles.DrawLine((Vector2)pos + top, (Vector2)pos + left);
                    }
                    else
                    {
                        Handles.DrawLine(pos + (rotation * collider2D.mStart),  pos + rotation * collider2D.mEnd);
                    }
                   
                    //Handles.RectangleHandleCap(0,((Vector2)pos + collider2D.mStart + (Vector2)pos + collider2D.mEnd)/2,  Quaternion.Euler(0f,0f,angle), collider2D.mRadius, EventType.Repaint);
                    break;
                case ColliderType.Circle:
                    Handles.CircleHandleCap(0, (Vector2)pos + collider2D.mCenter, collider2D.transform.localRotation, collider2D.mRadius, EventType.Repaint);
                    break;
                case ColliderType.Box:
                    rotation = Quaternion.AngleAxis(collider2D.transform.localEulerAngles.z, Vector3.forward);
                    size = new Vector2((collider2D.mSize.x) * collider2D.transform.localScale.x + collider2D.mRadius, (collider2D.mSize.y) * collider2D.transform.localScale.y + collider2D.mRadius); //(collider2D.mSize + new Vector2(collider2D.mRadius, collider2D.mRadius));
                    left = rotation * new Vector2(-size.x/2, -size.y/2 );
                    bottom = rotation * new Vector2(size.x / 2, -size.y/2);
                    right = rotation * new Vector2(size.x / 2, size.y / 2);
                    top = rotation * new Vector2(-size.x / 2, size.y / 2);
                    Handles.DrawLine((Vector2)pos + collider2D.mCenter + left, (Vector2)pos + collider2D.mCenter + bottom);
                    Handles.DrawLine((Vector2)pos + collider2D.mCenter + bottom, (Vector2)pos + collider2D.mCenter + right);
                    Handles.DrawLine((Vector2)pos + collider2D.mCenter + right, (Vector2)pos + collider2D.mCenter + top);
                    Handles.DrawLine((Vector2)pos + collider2D.mCenter + top, (Vector2)pos + collider2D.mCenter + left);
                    //Handles.DrawWireCube(rotation * ((Vector2)pos + collider2D.mCenter), collider2D.mSize + new Vector2(collider2D.mRadius, collider2D.mRadius));
                    break;
                case ColliderType.Polygon:
                    for (int i = 0;  i < collider2D.mVects.Length; i++)
                    {
                        if (i != collider2D.mVects.Length - 1)
                           Handles.DrawLine((Vector2)pos + collider2D.mVects[i], (Vector2)pos + collider2D.mVects[i + 1]);
                        else
                            Handles.DrawLine((Vector2)pos + collider2D.mVects[i], (Vector2)pos + collider2D.mVects[0]);

                        Handles.Label((Vector2)pos + collider2D.mVects[i], i.ToString());
                    }

                    break;
            }

            
        }
 

        

        //Handles.PositionHandle(Vector3.zero, Quaternion.identity);

        //Handles.Slider((target as XCollider2D).transform.position, Vector3.zero);
       
    }


    //判断是否选择了该枚举值
    protected bool IsShapeFilterMask(cpShapeFilterMask oldfilter, cpShapeFilterMask filter)
    {
        // 将枚举值转换为int 类型, 1 左移 
        int index = 1 << (int)filter;
        // 获取所有选中的枚举值
        int eventTypeResult = (int)oldfilter;
        // 按位 与
        if ((eventTypeResult & index) == index)
        {
            return true;
        }

        return false;
    }



}

