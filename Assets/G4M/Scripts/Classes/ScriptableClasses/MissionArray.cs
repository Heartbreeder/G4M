using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewMissionArray", menuName = "G4M/MissionSet")]
public class MissionArray : ScriptableObject
{
    [SerializeField]
    public MissionTemplate[] templateArray;
}
