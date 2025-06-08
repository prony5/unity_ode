using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace UnityODE
{
    [CanEditMultipleObjects, CustomEditor(typeof(OdeMotor))]
    public class OdeMotorEditor : Editor
    {
        private OdeMotor Script { get { return target as OdeMotor; } }

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            if (Script.KV == null || Script.KV.keys == null)
                return;

            float maxTorque = Script.KT * Script.nominalCurrent;

            Keyframe[] newKeys;
            if (Script.KV.keys.Length < 2)
                newKeys = new Keyframe[2];
            else
            {
                newKeys = Script.KV.keys;
                if (newKeys[0].value != maxTorque && Script.noLoadSpeed > 0 && maxTorque > 0 && newKeys[0].value > 0 && newKeys[newKeys.Length - 1].time > 0)
                {
                    var deltaT = Script.noLoadSpeed / newKeys[newKeys.Length - 1].time;
                    var deltaV = maxTorque / newKeys[0].value;

                    for (int i = 1; i < newKeys.Length - 1; i++)
                    {
                        newKeys[i].time *= deltaT;
                        newKeys[i].value *= deltaV;
                    }
                }
            }

            newKeys[0].time = 0;
            newKeys[0].value = maxTorque;
            newKeys[newKeys.Length - 1].time = Script.noLoadSpeed;
            newKeys[newKeys.Length - 1].value = 0;

            Script.KV.keys = newKeys;
        }
    }

    [CanEditMultipleObjects, CustomEditor(typeof(OdeMotorLinear))]
    public class OdeMotorLinearEditor : Editor
    {
        private OdeMotorLinear Script { get { return target as OdeMotorLinear; } }

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            if (Script.KV == null || Script.KV.keys == null)
                return;

            float maxTorque = Script.KT * Script.nominalCurrent;

            Keyframe[] newKeys;
            if (Script.KV.keys.Length < 2)
                newKeys = new Keyframe[2];
            else
            {
                newKeys = Script.KV.keys;
                if (newKeys[0].value != maxTorque && Script.noLoadSpeed > 0 && maxTorque > 0 && newKeys[0].value > 0 && newKeys[newKeys.Length - 1].time > 0)
                {
                    var deltaT = Script.noLoadSpeed / newKeys[newKeys.Length - 1].time;
                    var deltaV = maxTorque / newKeys[0].value;

                    for (int i = 1; i < newKeys.Length - 1; i++)
                    {
                        newKeys[i].time *= deltaT;
                        newKeys[i].value *= deltaV;
                    }
                }
            }

            newKeys[0].time = 0;
            newKeys[0].value = maxTorque;
            newKeys[newKeys.Length - 1].time = Script.noLoadSpeed;
            newKeys[newKeys.Length - 1].value = 0;

            Script.KV.keys = newKeys;
        }
    }
}