using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MachineShopData : MonoBehaviour
{
    public MaterialArray materialsSet;
    public static MaterialArray Materials;

    public ToolArray toolSet;
    public static ToolArray Tools;

    public MissionArray millingMissionSet;
    public static MissionArray MillingMissions;

    public MissionArray turningMissionSet;
    public static MissionArray TurningMissions;

    public TutorialArray millingTutorialSet;
    public static TutorialArray MillingTutorials;

    public TutorialArray turningTutorialSet;
    public static TutorialArray TurningTutorials;

    public SpriteArray PlayerAvatarSet;
    public static SpriteArray Avatars;

    public MachineArray MachineSet;
    public static MachineArray Machines;

    private void Awake()
    {
        Materials = materialsSet;
        Tools = toolSet;
        MillingMissions = millingMissionSet;
        TurningMissions = turningMissionSet;
        MillingTutorials = millingTutorialSet;
        TurningTutorials = turningTutorialSet;
        Avatars = PlayerAvatarSet;
        Machines = MachineSet;
    }
}


