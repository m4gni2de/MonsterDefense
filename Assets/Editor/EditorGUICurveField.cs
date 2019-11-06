using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class EditorGUICurveField : EditorWindow
{
    AnimationCurve curveX = AnimationCurve.Linear(0, 0, 1, 0);
    AnimationCurve curveY = AnimationCurve.Linear(0, 0, 1, 1);
    AnimationCurve curveZ = AnimationCurve.Linear(0, 0, 1, 0);

    [MenuItem("Window/Custom/Curve Field demo")]
    static void Init()
    {
        EditorWindow window = GetWindow(typeof(EditorGUICurveField));
        window.position = new Rect(0, 0, 400, 199);
        window.Show();
    }

    void OnGUI()
    {
        curveX = EditorGUI.CurveField(
            new Rect(3, 3, position.width - 6, 50),
            "Animation on X", curveX);
        curveY = EditorGUI.CurveField(
            new Rect(3, 56, position.width - 6, 50),
            "Animation on Y", curveY);
        curveZ = EditorGUI.CurveField(
            new Rect(3, 109, position.width - 6, 50),
            "Animation on Z", curveZ);

        if (GUI.Button(new Rect(3, 163, position.width - 6, 30), "Generate Curve"))
            AddCurveToSelectedGameObject();
    }

    // A GameObject with the FollowAnimationCurve script must be selected
    void AddCurveToSelectedGameObject()
    {
        //if (Selection.activeGameObject)
        //{
        //    FollowAnimationCurve comp =
        //        Selection.activeGameObject.GetComponent<FollowAnimationCurve>();
        //    comp.SetCurves(curveX, curveY, curveZ);
        //}
        //else
        //{
        //    Debug.LogError("No Game Object selected for adding an animation curve");
        //}
    }
}