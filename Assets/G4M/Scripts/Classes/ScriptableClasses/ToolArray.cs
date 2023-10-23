using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewToolSet", menuName = "G4M/ToolSet")]
public class ToolArray : ScriptableObject
{
    [SerializeField]
    public ToolTemplate[] Array;
}
