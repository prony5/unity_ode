using UnityEditor;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace UnityODE
{
    [CustomEditor(typeof(OdeSoftBox))]
    public class OdeSoftBoxEditor : Editor
    {
        private OdeSoftBox script { get { return target as OdeSoftBox; } }

        private object sync = new object();
        private bool needRebuild = true;

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            if (Application.isPlaying)
                return;

            if (script.mass <= 0 || script.boxesInRow <= 0 || script.size.x <= 0 || script.size.y <= 0 || script.size.z <= 0)
                return;

            EditorGUILayout.Space();
            EditorGUILayout.BeginHorizontal();
            if (GUILayout.Button(new GUIContent("Clear")))
                Clear();
            if (GUILayout.Button(new GUIContent("Build")))
            {
                Clear();
                Create();
            }
            EditorGUILayout.EndHorizontal();

            //need rebuild
            if (needRebuild || script.lastBoxesInRow != script.boxesInRow || script.lastSize != script.size)
            {
                needRebuild = false;
                script.lastBoxesInRow = script.boxesInRow;
                script.lastSize = script.size;
                lock (sync)
                {
                    script.boxes.Clear();

                    Build(Vector3.up);
                    Build(Vector3.down);
                    Build(Vector3.forward);
                    Build(Vector3.back);
                    Build(Vector3.left);
                    Build(Vector3.right);

                    //remove dublicate
                    int i = 0;
                    while (i < script.boxes.Count - 1)
                    {
                        bool next = true;
                        for (int j = i + 1; j < script.boxes.Count; j++)
                        {
                            if (script.boxes[i].center == script.boxes[j].center)
                            {
                                script.boxes.RemoveAt(i);
                                next = false;
                                break;
                            }
                        }
                        if (next) i++;
                    }
                    script.boxesCount = script.boxes.Count;
                }
            }
        }

        private void OnSceneGUI()
        {
            lock (sync)
            {
                Handles.color = OdeEdittorDefault.color;
                foreach (var item in script.boxes)
                    Handles.DrawWireCube(script.transform.position + item.center, item.size);
            }
        }

        void Build(Vector3 plane)
        {
            var size = script.size / script.boxesInRow;
            var cross = new Vector3(plane.y, plane.z, plane.x);
            var cross2 = new Vector3(plane.z, plane.x, plane.y);
            var ofs = ((script.size * 0.5f) - (size * 0.5f)) * Mathf.Sign(plane.x + plane.y + plane.z);

            var row = 0;
            var col = 0;
            while (col < script.boxesInRow)
            {
                script.boxes.Add(new OdeSoftBox.Box(ofs - Vector3.Scale(size * row, cross), size));
                row++;
                if (row >= script.boxesInRow)
                {
                    ofs -= Vector3.Scale(size, cross2);
                    row = 0;
                    col++;
                }
            }
        }

        void Create()
        {
            if (script.boxes.Count == 0)
                return;

            var mass = script.mass / script.boxes.Count;
            for (int i = 0; i < script.boxes.Count; i++)
            {
                var obj = new GameObject("box " + i.ToString());
                obj.transform.parent = script.transform;
                obj.transform.localPosition = script.boxes[i].center;

                var collider = obj.AddComponent<BoxCollider>();
                collider.size = script.boxes[i].size;

                var body = obj.AddComponent<OdeBody>();
                body.container = script.transform;
                body.inertial.mass = mass;
                body.isKinematic = false;

                script.boxObjs.Add(obj);
            }
            /*
                        //set self collision ignore
                        var bodies = script.GetComponentsInChildren<OdeBody>();
                        for (int i = 0; i < bodies.Length - 1; i++)
                        {
                            for (int j = i + 1; j < bodies.Length; j++)
                                bodies[i].collision.ignored.Add(bodies[j]);

                            var joint = bodies[i].gameObject.AddComponent<OdeJointFixed>();
                            joint.connectedBody = bodies[i + 1];
                        }
                        */
        }

        void Clear()
        {
            foreach (var item in script.boxObjs)
                DestroyImmediate(item);
            script.boxObjs.Clear();
        }
    }
}
