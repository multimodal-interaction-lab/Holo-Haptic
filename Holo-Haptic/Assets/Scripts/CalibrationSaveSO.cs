using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Scriptable Object/Calibration Save")]
public class CalibrationSaveSO : ScriptableObject
{
    // Local position and rotation
    public Vector3 LocalPosition;
    public Quaternion LocalRotation;
}
