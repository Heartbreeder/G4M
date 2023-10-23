using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewMachineSet", menuName = "G4M/MachineSet")]
public class MachineArray : ScriptableObject
{
    [SerializeField]
    public MachineTemplate[] Array;
}
