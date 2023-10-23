using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewSpriteSet", menuName = "G4M/SpriteSet")]
public class SpriteArray : ScriptableObject
{
    [SerializeField]
    public Sprite[] Array;
}

