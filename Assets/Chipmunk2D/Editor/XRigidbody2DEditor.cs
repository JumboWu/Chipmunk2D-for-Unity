using UnityEditor;
using UnityEngine;
using X.Engine;

[CustomEditor(typeof(XRigidbody2D))]
class XRigidbody2DEditor : Editor
{
    private void OnEnable()
    {
        
    }

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        XRigidbody2D rigidbody2D = target as XRigidbody2D;
        rigidbody2D.mPostion = EditorGUILayout.Vector2Field("位置Position", new Vector2(rigidbody2D.transform.position.x * XSpaceManager.PixelsPerUnit, rigidbody2D.transform.position.y * XSpaceManager.PixelsPerUnit));
        rigidbody2D.mAngle = EditorGUILayout.FloatField("角度Angle", cp.cpfradian(rigidbody2D.transform.localEulerAngles.z));
        rigidbody2D.mBodyType = (cpBodyType)EditorGUILayout.EnumPopup("刚体类型BodyType", rigidbody2D.mBodyType);
        rigidbody2D.mMass = EditorGUILayout.FloatField("质量Mass", rigidbody2D.mMass);
        rigidbody2D.mMonent = EditorGUILayout.FloatField("惯性Monent", rigidbody2D.mMonent);

        if (GUI.changed)
            EditorUtility.SetDirty(target);
    }


    private void OnSceneGUI()
    {

    }
}
