using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewTutorialSet", menuName = "G4M/TutorialSet")]
public class TutorialArray : ScriptableObject
{
    [SerializeField]
    public TutorialTemplate[] Array;
}
