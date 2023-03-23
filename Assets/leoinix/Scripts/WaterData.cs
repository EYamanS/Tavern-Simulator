using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName ="Water Data",menuName = "Meyhane Simulator/ Water Data")]
public class WaterData : ScriptableObject
{
    public Fish[] CatchableFishes;
    public int minRodLevel;

    public float minWaitTime;
    public float maxWaitTime;
}
