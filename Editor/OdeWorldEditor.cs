using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace UnityODE
{
    public static class OdeEdittorDefault
    {
        public static Color color = new Color(1f, 0f, 0.75f, 1.0f);
        public static Color ColorAlpha(float alpha) { var c = color; c.a = alpha; return c; }
    }

    [InitializeOnLoad]
    public class OdeWorldOrder
    {
        const int mainOrder = -500;

        static void ChangeOrder(MonoScript script, int newOrder)
        {
            var currentOrder = MonoImporter.GetExecutionOrder(script);
            if (currentOrder != newOrder)
                MonoImporter.SetExecutionOrder(script, newOrder);
        }

        static Type GetParentType(Type tp)
        {
            var bt = tp.BaseType;
            if (bt == null || bt.Namespace != "UnityODE")
                return tp;
            else
                return GetParentType(bt);
        }

        static OdeWorldOrder()
        {
            foreach (MonoScript monoScript in MonoImporter.GetAllRuntimeMonoScripts())
            {
                var t = monoScript.GetClass();
                if (t == null)
                    continue;

                if (GetParentType(t) == typeof(OdeWorld))
                {
                    ChangeOrder(monoScript, mainOrder);
                    continue;
                }

                if (GetParentType(t) == typeof(OdeBody))
                {
                    ChangeOrder(monoScript, mainOrder + 1);
                    continue;
                }

                if (GetParentType(t) == typeof(OdeJoint))
                {
                    ChangeOrder(monoScript, mainOrder + 2);
                    continue;
                }

                if (GetParentType(t) == typeof(OdeJointDependents))
                {
                    ChangeOrder(monoScript, mainOrder + 3);
                    continue;
                }

                if (GetParentType(t) == typeof(OdeSensor))
                {
                    ChangeOrder(monoScript, mainOrder + 4);
                    continue;
                }

                if (t.Namespace == "UnityODE")
                {
                    ChangeOrder(monoScript, mainOrder + 50);
                }
            }
        }
    }

    [CustomEditor(typeof(OdeWorld))]
    public class OdeWorldEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            if (Application.isPlaying)
            {
                if (OdeWorld.Settings.manualStep)
                {
                    EditorGUILayout.Separator();
                    if (GUILayout.Button("Next step"))
                        OdeWorld.Step();
                }

                EditorGUILayout.Separator();
                EditorGUILayout.LabelField("Statistic", EditorStyles.boldLabel);

                var stat = OdeWorld.Statistics;
                EditorGUILayout.LabelField(string.Format("Time: {0}", stat.time));
                EditorGUILayout.LabelField(string.Format("SPS: {0}", stat.sps));
                EditorGUILayout.LabelField(string.Format("Contacts: {0}", stat.contacts));
                EditorGUILayout.LabelField(string.Format("Geoms: {0}", stat.geoms));
                EditorGUILayout.LabelField(string.Format("Bodies: {0}", stat.bodies));
                EditorGUILayout.LabelField(string.Format("Joints: {0}", stat.joints));

                Repaint();
            }
        }

    }
}