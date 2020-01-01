using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CreateAssetMenu()]
public class Scriptable_GameManagementData : ScriptableObject
{
    public float winHeight;
    public byte health;
    public float gravity;
    public float FallingVel;
}
