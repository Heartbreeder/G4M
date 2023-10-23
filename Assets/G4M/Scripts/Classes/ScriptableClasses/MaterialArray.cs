using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewMaterialSet", menuName = "G4M/MaterialSet")]
public class MaterialArray : ScriptableObject
{
    [SerializeField]
    public MaterialTemplate[] Array;
}
