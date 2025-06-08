using UnityEditor;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace UnityODE
{
    [CustomEditor(typeof(OdeRope))]
    public class OdeRopeEditor : Editor
    {
        private OdeRope script { get { return target as OdeRope; } }

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            if (Application.isPlaying)
                return;

            if (script.segments < 4) script.segments = 4;
            if (script.linkCount < 2) script.linkCount = 2;
            if (script.limit1 < 0) script.limit1 = 0;
            if (script.limit2 < 0) script.limit2 = 0;
            if (script.friction < 0) script.friction = 0;

            EditorGUILayout.Space();
            EditorGUILayout.BeginHorizontal();
            if (GUILayout.Button(new GUIContent("Clear")))
                Clear();
            if (GUILayout.Button(new GUIContent("Build")))
            {
                Clear();
                Build();
            }
            EditorGUILayout.EndHorizontal();
        }

        void Clear()
        {
            //delete last joint
            if (script.endJoint != null)
                DestroyImmediate(script.endJoint);

            //delite links
            foreach (var item in script.links)
                DestroyImmediate(item);
            script.links.Clear();

            //delete mesh Render
            if (script.skin != null && script.skin.sharedMesh != null)
            {
                script.skin.sharedMesh.Clear();
                script.skin.sharedMesh = null;
                //    DestroyImmediate(script.skin);
            }
        }

        void Build()
        {
            if (script.lenght <= 0 || script.diameter <= 0)
                return;

            //create physics
            float linkLength = script.lenght / script.linkCount;
            float linkLength05 = linkLength * 0.5f;
            float linkMass = script.mass / script.linkCount;
            float radius = script.diameter * 0.5f;
            OdeBody lastBody = script.startBody;
            OdeBody lastBeforeBody = null;
            for (int i = 0; i < script.linkCount; i++)
            {
                var obj = new GameObject("OdeRopeLink " + i.ToString());
                obj.transform.parent = script.transform;
                obj.transform.position = script.transform.position + script.transform.rotation * new Vector3(0, -linkLength * (i + 1) + linkLength05);
                obj.transform.rotation = script.transform.rotation;

                var collider = obj.AddComponent<CapsuleCollider>();
                collider.radius = radius;
                collider.height = linkLength;

                var body = obj.AddComponent<OdeBody>();
                body.container = script.transform;
                body.inertial.mass = linkMass;
                body.isKinematic = false;

                if (lastBeforeBody != null && lastBeforeBody != script.startBody)
                    lastBeforeBody.collision.ignored.Add(body);

                if (lastBody != null)
                {
                    var joint = obj.AddComponent<OdeJointUniversal>();
                    joint.connectedBody = lastBody;
                    joint.anchor = new Vector3(0, linkLength05, 0);
                    joint.axis1 = new Vector3(1, 0, 0);
                    joint.axis2 = new Vector3(0, 1, 0);
                    joint.limits1.min = -script.limit1;
                    joint.limits1.max = script.limit1;
                    joint.limits2.min = -script.limit2;
                    joint.limits2.max = script.limit2;
                 //   joint.limits1.force = script.friction;
                  //  joint.limits2.force = script.friction;
                }
                lastBeforeBody = lastBody;
                lastBody = body;

                script.links.Add(obj);
            }

            if (script.endBody != null)
            {
                var joint = script.endBody.gameObject.AddComponent<OdeJointHinge>();
                joint.connectedBody = lastBody;
                joint.Axis = new Vector3(1, 0, 0);
             //   joint.Mode = OdeJointMove.MoveMode.force;
                script.endJoint = joint;
            }

            //create mesh    
            if (script.skin == null)
            {
                script.skin = script.gameObject.AddComponent<SkinnedMeshRenderer>();
                script.skin.updateWhenOffscreen = true;
            }

            Transform[] boneTransforms = new Transform[script.linkCount + 1];
            Matrix4x4[] bindPoses = new Matrix4x4[script.linkCount + 1];
            for (int i = 0; i < script.linkCount; i++)
            {
                boneTransforms[i] = script.links[i].transform;
                bindPoses[i] = script.links[i].transform.worldToLocalMatrix;
                bindPoses[i] *= script.transform.localToWorldMatrix;
            }
            boneTransforms[script.linkCount] = script.links[script.linkCount - 1].transform;
            bindPoses[script.linkCount] = script.links[script.linkCount - 1].transform.worldToLocalMatrix;
            bindPoses[script.linkCount].m13 -= linkLength;
            bindPoses[script.linkCount] *= script.transform.localToWorldMatrix;

            Mesh newMesh = new Mesh();

            int nVertices = ((script.linkCount + 1) * (script.segments + 1)) + ((script.segments + 1) * 2);
            int nTrianglesRope = script.linkCount * script.segments * 2;
            int nTrianglesSections = 2 * (script.segments - 2);

            Vector3[] vertices = new Vector3[nVertices];
            Vector2[] mapping = new Vector2[nVertices];
            BoneWeight[] weights = new BoneWeight[nVertices];
            int[] trianglesRope = new int[nTrianglesRope * 3];
            int[] trianglesSections = new int[nTrianglesSections * 3];

            int nVertexIndex = 0;
            FillLinkMeshIndicesSections(0, script.linkCount + 1, ref trianglesSections);
            for (int nLink = 0; nLink < script.linkCount + 1; nLink++)
            {
                int nBoneIndex0 = nLink <= script.linkCount ? nLink : script.linkCount + 1;
                int nBoneIndex1 = nBoneIndex0;

                if (nLink < script.linkCount)
                    FillLinkMeshIndicesRope(nLink, script.linkCount + 1, ref trianglesRope);

                bool bFirst = false;
                bool bLast = false;
                int nRepeats = 1;

                if (nLink == 0) { nRepeats++; bFirst = true; }
                if (nLink == script.linkCount) { nRepeats++; bLast = true; }

                for (int nRepeat = 0; nRepeat < nRepeats; nRepeat++)
                {
                    for (int nSide = 0; nSide < script.segments + 1; nSide++)
                    {
                        float fRopeT = (float)nLink / (float)script.linkCount;
                        float fCos = Mathf.Cos(((float)nSide / (float)script.segments) * Mathf.PI * 2.0f);
                        float fSin = Mathf.Sin(((float)nSide / (float)script.segments) * Mathf.PI * 2.0f);

                        vertices[nVertexIndex] = new Vector3(fCos * radius, 0, fSin * radius);
                        vertices[nVertexIndex] = (boneTransforms[nBoneIndex0].TransformPoint(vertices[nVertexIndex]));
                        vertices[nVertexIndex] = script.transform.InverseTransformPoint(vertices[nVertexIndex]);
                        vertices[nVertexIndex] += new Vector3(0, linkLength * 0.5f);

                        if ((bFirst && nRepeat == 0) || (bLast && nRepeat == (nRepeats - 1)))
                            mapping[nVertexIndex] = new Vector2(Mathf.Clamp01((fCos + 1.0f) * 0.5f), Mathf.Clamp01((fSin + 1.0f) * 0.5f));
                        else
                            mapping[nVertexIndex] = new Vector2(fRopeT * script.lenght, (float)nSide / (float)script.segments);

                        weights[nVertexIndex].boneIndex0 = nBoneIndex0;
                        weights[nVertexIndex].boneIndex1 = nBoneIndex1;
                        weights[nVertexIndex].weight0 = 1;
                        weights[nVertexIndex].weight1 = 0;

                        nVertexIndex++;
                    }
                }
            }

            newMesh.vertices = vertices;
            newMesh.uv = mapping;
            newMesh.boneWeights = weights;
            newMesh.bindposes = bindPoses;

            newMesh.subMeshCount = 2;
            newMesh.SetTriangles(trianglesRope, 0);
            newMesh.SetTriangles(trianglesSections, 1);
            newMesh.RecalculateNormals();

            script.skin.bones = boneTransforms;
            script.skin.sharedMesh = newMesh;
            script.skin.materials = new Material[] { script.materialExternal, script.materialInternal };
        }

        void FillLinkMeshIndicesRope(int nLinearLinkIndex, int nTotalLinks, ref int[] indices, bool bBrokenLink = false)
        {
            int nTriangleIndex = nLinearLinkIndex * script.segments * 2;
            int nLinkVertexIndexStart = (nLinearLinkIndex * (script.segments + 1)) + (script.segments + 1);

            for (int nSide = 0; nSide < script.segments; nSide++)
            {
                int nVertexIndex = nLinkVertexIndexStart + nSide;

                indices[nTriangleIndex * 3 + 2] = nVertexIndex;
                indices[nTriangleIndex * 3 + 1] = nVertexIndex + script.segments + 1;
                indices[nTriangleIndex * 3 + 0] = nVertexIndex + 1;
                indices[nTriangleIndex * 3 + 5] = nVertexIndex + 1;
                indices[nTriangleIndex * 3 + 4] = nVertexIndex + script.segments + 1;
                indices[nTriangleIndex * 3 + 3] = nVertexIndex + 1 + script.segments + 1;
                nTriangleIndex += 2;
            }
        }

        void FillLinkMeshIndicesSections(int nLinearLinkIndex, int nTotalLinks, ref int[] indices, bool bBrokenLink = false)
        {
            int nTriangleIndex = 0;
            int nLinkVertexIndexStart = 0;

            for (int nBaseTri = 0; nBaseTri < script.segments - 2; nBaseTri++)
            {
                indices[nTriangleIndex * 3 + 0] = nLinkVertexIndexStart;
                indices[nTriangleIndex * 3 + 1] = nLinkVertexIndexStart + (nBaseTri + 2);
                indices[nTriangleIndex * 3 + 2] = nLinkVertexIndexStart + (nBaseTri + 1);
                nTriangleIndex++;
            }

            nLinkVertexIndexStart = ((script.linkCount + 1) * (script.segments + 1)) + (script.segments + 1);

            for (int nTopTri = 0; nTopTri < script.segments - 2; nTopTri++)
            {
                indices[nTriangleIndex * 3 + 2] = nLinkVertexIndexStart;
                indices[nTriangleIndex * 3 + 1] = nLinkVertexIndexStart + (nTopTri + 2);
                indices[nTriangleIndex * 3 + 0] = nLinkVertexIndexStart + (nTopTri + 1);
                nTriangleIndex++;
            }
        }

    }
}