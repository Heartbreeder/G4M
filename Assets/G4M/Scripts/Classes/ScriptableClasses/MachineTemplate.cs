using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class MachineTemplate
{
    public string MachineName;
    public int MaxTools;
    public bool IsMilling;
    public bool IsHaas;
    public GameObject MachinePrefab;
    public Sprite MachineSprite;

    public MachineTemplate() { }

    public MachineTemplate(string machineName, int maxTools, bool isMilling, bool isHaas, GameObject machinePrefab, Sprite machineSprite)
    {
        MachineName = machineName;
        MaxTools = maxTools;
        IsMilling = isMilling;
        IsHaas = isHaas;
        MachinePrefab = machinePrefab;
        MachineSprite = machineSprite;
    }
}
