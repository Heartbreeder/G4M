using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Doozy.Engine;
using System.Xml.Linq;

public class PlayerData : MonoBehaviour
{
    #region Basic Profile Data
    public string ProfileName;
    public int AvatarIndex;
    public bool MillingEnabled;
    public bool TurningEnabled;
    #endregion

    #region Profile Progression Data
    public float Exp;
    public int Money;
    public int MillingStage;
    public int TurningStage;
    public bool[] MillingTutorialsRead;
    public bool[] TurningTutorialsRead;
    public int FirstTimeTutorialState = -1;  //    Starting point = -1,   NoMoreTutorilas = -2

    #endregion

    #region Inventory Data
    public List<InventoryToolData> InventoryTools;
    public List<InventoryMaterialData> InventoryMaterials;
    #endregion

    #region Machine Shop Data
    public List<ActiveMachineData> MachinesList;
    //TODO: Add a MachineData class
        //Here, we will keep a list of MachineData
        //TODO: make a Machine Data Scriptable object and save the templates

    #endregion

    #region Active Mission Data

    public ActiveMissionData ActiveMission;
    public List<ActiveMissionData> NextMissionList;
    public int CorrectMissionCombo;
    public int[] TimesCompletedPerMissionMilling;
    public int[] TimesCompletedPerMissionTurning;

    #endregion

    #region Gameplay Specific Data
    // Data included here is not loaded.
    // Instead, it is initialised on Start/Deserialise and are altered while the game is executed.
    // Data included here will be deleted when the App closes without saving.

    public Sprite AvatarSprite;
    public bool CursorLocked;
    public int LockLevel;
    public int UniqueCodeGenerator;
    public int ActiveMachine;
    public List<string> ProfileNames;
    public List<string> ChatboxMessages;

    public GameObject playerTarget;
   
    #endregion

    #region Evaluation Data
    [Multiline]
    public string PlayerLog;
    public float TimeMemory;
    #endregion

    #region Save System Functions

    //Save the current Player data in the file "PlayerName"
    public void SavePlayer()
    {
        if (string.IsNullOrEmpty(ProfileName))
        {
            Debug.Log("Save System: No profile selected; Aborting the Save profile action.");
            return;
        }

        Debug.Log("Save System: Saving Player Data for Profile: " + ProfileName + ".");
        SaveSystem.SavePlayerData(this, ProfileName);
    }

    //Load a Player's Data from a file
    public void LoadPlayer(string name)
    {
        Debug.Log("Save System: Loading Player Data for Profile: " + ProfileName + ".");
        PlayerDataContainer pdc = SaveSystem.LoadPlayerData(name);
        if (pdc != null)
        {
            ClearPlayerData();
            Init(pdc);
            gameObject.GetComponent<OptionsData>().LastPlayerProfile = name;
            gameObject.GetComponent<OptionsData>().SaveOptions();
            FireEventLoadProfile();
        }
        else
        {
            Debug.LogError("Save System Error: Player Data for Profile " + ProfileName + " not found; Load aborted.");
        }
    }

    //Delete profile
    public void DeleteProfile(string name)
    {
        Debug.Log("Save System: Deleting Player Data Data for Profile: " + ProfileName + ".");
        SaveSystem.DeletePlayerData(name);
        //If we deleted the active profile then clear the active data
        if (name == ProfileName)
        {
            ClearPlayerData();
        }
    }

    //Clear profile data
    public void ClearPlayerData()
    {
        PlayerDataContainer empty = new PlayerDataContainer();
        Init(empty);
        NextMissionList.Clear();
        InventoryTools.Clear();
        InventoryMaterials.Clear();
        MachinesList.Clear();
        ProfileName = "";
    }

    //Load/ reload the Profile names List
    public void UpdateProfileNames()
    {

        ProfileNames.Clear();
        SaveSystem.LoadAllPlayerNames();
        foreach (string name in SaveSystem.ProfileNames)
        {
            ProfileNames.Add(name);
        }
    }

    #endregion

    #region Deserialiser

    public void Init(PlayerDataContainer data)
    {
        // Basic Data
        ProfileName = data.ProfileName;
        AvatarIndex = data.AvatarIndex;
        //Load sprite from index
        AvatarSprite = GetComponent<MachineShopData>().PlayerAvatarSet.Array[AvatarIndex];

        MillingEnabled = data.MillingEnabled;
        TurningEnabled = data.TurningEnabled;

        //Machine shop data
        MachinesList.Clear();
        if (data.MachineID != null)
        {
            for (int i=0; i < data.MachineID.Length; i++)
            {
                MachinesList.Add(new ActiveMachineData(data.MachineID[i], data.MachineName[i]));
            }
        }

        //Mission Data
        Exp = data.Exp;
        Money = data.Money;
        MillingStage = data.MillingStage;
        TurningStage = data.TurningStage;


        //Tutorials Read
        InitTutorialsReadArray(true, data.MillingTutorialsRead);

        InitTutorialsReadArray(false, data.TurningTutorialsRead);

        FirstTimeTutorialState = data.FirstTimeTutorialState;

        //active mission
        ActiveMission = new ActiveMissionData(data.AMisMilling, data.AMmissionIndex, data.AMisMissionStageUp, data.AMrewardGoldPerItem, data.AMrewardExpPerMission, data.AMrequestedQuantity, data.AMrequestedMaterialID, data.AMrequestedMaterialDimensions, data.AMwrittenCode, data.AMcompletedQuantity, data.AMisCodeChecked, data.AMisBeingExecuted, data.AMexecutionCarveQuantity, data.AMdelay, data.AMdelayTimePassed, data.AMshownDelay, data.AMexecutingMachine, data.AMadditionalDescription);
        //next mission list
        if (data.NMisMilling != null)
        {
            for (int i = 0; i < data.NMisMilling.Length; i++)
            {
                NextMissionList.Add(new ActiveMissionData(data.NMisMilling[i], data.NMmissionIndex[i], data.NMisMissionStageUp[i], data.NMrewardGoldPerItem[i], data.NMrewardExpPerMission[i], data.NMrequestedQuantity[i], data.NMrequestedMaterialID[i], data.NMrequestedMaterialDimensions[i], data.NMadditionalDescription[i]));
            }
        }

        //addiditonal mission data
        CorrectMissionCombo = data.CorrectMissionCombo;

        if (data.TimesCompletedPerMissionMilling != null)
        {
            TimesCompletedPerMissionMilling = new int[MachineShopData.MillingMissions.templateArray.Length];
            for (int i = 0; i < TimesCompletedPerMissionMilling.Length; i++)
                TimesCompletedPerMissionMilling[i] = data.TimesCompletedPerMissionMilling[i];
            for (int i = TimesCompletedPerMissionMilling.Length; i < MachineShopData.MillingMissions.templateArray.Length; i++)
                TimesCompletedPerMissionMilling[i] = 0;
        }

        if (data.TimesCompletedPerMissionTurning != null)
        {
            TimesCompletedPerMissionTurning = new int[MachineShopData.TurningMissions.templateArray.Length];
            for (int i = 0; i < TimesCompletedPerMissionTurning.Length; i++)
                TimesCompletedPerMissionTurning[i] = data.TimesCompletedPerMissionTurning[i];
            for (int i = TimesCompletedPerMissionTurning.Length; i < MachineShopData.TurningMissions.templateArray.Length; i++)
                TimesCompletedPerMissionTurning[i] = 0;
        }

        //Inventory Data

        UniqueCodeGenerator = 0;
        //tools
        InventoryTools = new List<InventoryToolData>();
        if (data.ToolID != null)
        {
            for (int i = 0; i < data.ToolID.Length; i++)
            {
                AddTool(data.ToolID[i], data.ToolDimensions[i], data.ToolDurability[i], data.ToolMachineID[i]);
            }
        }
        else
        {
            AddTool("", "", 0);
        }

        //mats
        InventoryMaterials = new List<InventoryMaterialData>();
        if (data.MaterialID != null)
        {
            for (int i = 0; i < data.MaterialID.Length; i++)
            {
                AddMaterial(data.MaterialID[i], data.MaterialDimensions[i], data.MaterialQuantity[i], data.MaterialMachineID[i]);
            }

        }
        else
        {
            AddMaterial("", "", 0);
        }

        //Evaluation data
        PlayerLog = data.PlayerLog;
        TimeMemory = data.TimeMemory;

    }

    #endregion

    #region Create a new Player
    public PlayerData(string name, int avatar)
    {
        SetNewPlayer(name, avatar, false, true);
    }

    public PlayerData(string name, int avatar, bool millingEnabled)
    {
        SetNewPlayer(name, avatar, millingEnabled, true);

    }

    public void SetNewPlayer(string name, int avatar)
    {
        SetNewPlayer(name, avatar, false, true);
    }

    public void SetNewPlayer(string name, int avatar, bool startMilling, bool startHaas)
    {
        if(!string.IsNullOrEmpty(ProfileName) )
            SavePlayer();
        ProfileName = name;
        AvatarIndex = avatar;
        //Avatar sprite
        AvatarSprite = MachineShopData.Avatars.Array[avatar];

        MillingEnabled = startMilling;
        TurningEnabled = !startHaas;

        MachinesList = new List<ActiveMachineData>();

        //Add first machine
        if (startMilling)
        {
            if (startHaas)
            {
                MachinesList.Add(new ActiveMachineData(1, "VF-1"));
            }
            else
            {
                MachinesList.Add(new ActiveMachineData(1, "DMU50"));
            }
        }
        else
        {
            if (startHaas)
            {
                MachinesList.Add(new ActiveMachineData(1, "DS-30Y"));
            }
            else
            {
                MachinesList.Add(new ActiveMachineData(1, "CTX300"));
            }
        }

        Exp = 0;
        MillingStage = 0;
        InitTutorialsReadArray(true, null);
        InitTutorialsReadArray(false, null);
        TurningStage = 0;
        FirstTimeTutorialState = 0;
        ActiveMission = new ActiveMissionData(true, -1);

        Money = 2000;
        InventoryTools = new List<InventoryToolData>();
        InventoryMaterials = new List<InventoryMaterialData>();
        UniqueCodeGenerator = 0;

        ActiveMission = new ActiveMissionData(false,-1);
        NextMissionList = new List<ActiveMissionData>();
        TimesCompletedPerMissionMilling = new int[MachineShopData.MillingMissions.templateArray.Length+1];
        TimesCompletedPerMissionTurning = new int[MachineShopData.TurningMissions.templateArray.Length+1];

        PlayerLog = "";
        TimeMemory = 0;
        PlayerLog += "[" + TimeMemory + "]" + "Player Data: Profile Created.\n";

        gameObject.GetComponent<MissionManager>().GetNewMissions();

        SavePlayer();

        this.GetComponent<OptionsData>().LastPlayerProfile = name;
        this.GetComponent<OptionsData>().SaveOptions();
    }
    #endregion

    #region Unity Functions

    private void Start()
    {
        InitTutorialsReadArray(true, MillingTutorialsRead);
        InitTutorialsReadArray(false, TurningTutorialsRead);
        CursorLocked = true;
        LockLevel = 0;
        ActiveMachine = 0;
        ChatboxMessages = new List<string>();

    }

    private void OnApplicationQuit()
    {
        SavePlayer();
    }

    private void Update()
    {
        TimeMemory += Time.deltaTime;
    }

    #endregion

    #region Tutorials Read List Managers

    public void InitTutorialsReadArray(bool isMilling, bool[] old)
    {
        TutorialArray tempArray = new TutorialArray();
        if (isMilling)
        {
            tempArray = MachineShopData.MillingTutorials;
        }
        else
        {
            tempArray = MachineShopData.TurningTutorials;
        }

        if (tempArray == null)
        {
            Debug.LogError("Player Data System: Tutorial data file is empty.");
            return;
        }

        bool[] readArray = new bool[tempArray.Array.Length];
        for (int i=0; i < readArray.Length; i++)
        {
            if (old != null)
            {
                if (i < old.Length)
                {
                    readArray[i] = old[i];
                }
                else
                {
                    readArray[i] = false;
                }
            }
            else
            {
                readArray[i] = false;
            }
        }

        if (isMilling)
            MillingTutorialsRead = readArray;
        else
            TurningTutorialsRead = readArray;

    }

    #endregion

    #region Inventory Tool List Managers
    public void AddTool(string ID, string dimensions)
    {
        AddTool(ID, dimensions, -1);
    }

    public void AddTool(string ID, string dimensions, int Durability)
    {
        //MachineID=0 means it is added to the player's inventory.
        AddTool(ID, dimensions, Durability, 0);
    }

    public void AddTool(string ID, string dimensions, int Durability, int MachineID)
    {
        int code = UniqueCodeGenerator;
        UniqueCodeGenerator++;
        InventoryToolData newTool = new InventoryToolData(ID, dimensions, Durability, code, MachineID);
        AddTool(newTool);
    }

    public void AddTool(InventoryToolData data)
    {
        //Remove the empty material
        if (InventoryTools.Count == 1 && string.IsNullOrEmpty(InventoryTools[0].ID))
            RemoveToolAtIndex(0);

        //Add the new one
        InventoryTools.Add(data);
        FireEventInventoryChanged();
    }

    public InventoryToolData GetTool(int UniqueID)
    {

        for (int i = 0; i < InventoryTools.Count; i++)
        {
            if (InventoryTools[i].UniqueCode == UniqueID)
                return InventoryTools[i];
        }
        return null;
    }
    
    public List<int> GetToolsInMachine(int MachineID)
    {
        List<int> ret = new List<int>();
        for (int i = 0; i < InventoryTools.Count; i++)
        {
            if (InventoryTools[i].MachineID == MachineID)
                ret.Add(InventoryTools[i].UniqueCode);
        }
        return ret;
    }

    public void TransferTool(int UniqueID, int MachineID)
    {
        for (int i = 0; i < InventoryTools.Count; i++)
        {
            if (InventoryTools[i].UniqueCode == UniqueID)
            {
                InventoryTools[i].MachineID = MachineID;
                FireEventInventoryChanged();
            }
        }
    }

    public void RemoveTool(int UniqueID)
    {
        for(int i=0; i < InventoryTools.Count; i++)
        {
            if (InventoryTools[i].UniqueCode == UniqueID)
                RemoveToolAtIndex(i);
        }
    }

    public void RemoveToolAtIndex(int index)
    {
        InventoryTools.RemoveAt(index);
        FireEventInventoryChanged();
    }

    public int ReduceToolDurability(int UniqueID, int amount)
    {
        //0= durability reduced, -1=tool not found, >0 durability reduced;tool broken;returns durability exess; 
        for (int i = 0; i < InventoryTools.Count; i++)
        {
            if (InventoryTools[i].UniqueCode == UniqueID)
            {
                if(amount > InventoryTools[i].ToolDurability)
                {
                    int exess = amount - InventoryTools[i].ToolDurability;
                    InventoryTools[i].ToolDurability = 0;
                    //RemoveToolAtIndex(i);
                    FireEventInventoryChanged();
                    return exess;
                }
                else
                {
                    InventoryTools[i].ToolDurability -= amount;
                    //if (InventoryTools[i].ToolDurability == 0)
                    //    RemoveToolAtIndex(i);
                    FireEventInventoryChanged();
                    return 0;
                }                
            }                   
        }
        return -1;
    }

    public void RepairTool(int UniqueID)
    {
        for (int i = 0; i < InventoryTools.Count; i++)
        {
            if (InventoryTools[i].UniqueCode == UniqueID)
            {
                InventoryTools[i].ToolDurability = InventoryTools[i].Template.Durability;
                Money -= InventoryTools[i].Template.RepairCost;

            }
        }
   }

    #endregion

    #region Inventory Material List Managers
    public void AddMaterial(string ID, string dimensions, int Quantity)
    {
        //MachineID=0 means it is added to the player's inventory.
        AddMaterial(ID, dimensions, Quantity, 0);
    }

    public void AddMaterial(string ID, string dimensions, int Quantity, int MachineID)
    {
        InventoryMaterialData newMat = new InventoryMaterialData(ID, dimensions, Quantity, MachineID);
        AddMaterial(newMat);
    }

    public void AddMaterial(InventoryMaterialData data)
    {
        //Remove the empty one
        if (InventoryMaterials.Count == 1 && string.IsNullOrEmpty(InventoryMaterials[0].ID))
            RemoveMaterialAtIndex(0);

        //Add to existing
        for (int i = 0; i < InventoryMaterials.Count; i++)
        {
            if (InventoryMaterials[i].MachineID == data.MachineID)//In the same machine/inventory
            {
                if (InventoryMaterials[i].ID == data.ID && InventoryMaterials[i].MaterialDimensions == data.MaterialDimensions)
                {
                    InventoryMaterials[i].MaterialQuantity += data.MaterialQuantity;
                    FireEventInventoryChanged();
                    return;
                }
                else//There is another; but machines except player inv can only have one type of material, so the one in the machine returns to the player.
                {
                    if (data.MachineID > 0)
                    {
                        AddMaterial(InventoryMaterials[i].ID, InventoryMaterials[i].MaterialDimensions, InventoryMaterials[i].MaterialQuantity, 0);
                        ReduceQuantity(InventoryMaterials[i].ID, InventoryMaterials[i].MaterialDimensions, InventoryMaterials[i].MaterialQuantity, InventoryMaterials[i].MachineID);
                    }
                }
            }
        }

        //Add new
        InventoryMaterials.Add(data);
        FireEventInventoryChanged();
    }

    public InventoryMaterialData GetMaterial(string ID, string Dimensions, int MachineID)
    {
        for (int i = 0; i < InventoryMaterials.Count; i++)
        {
            if (InventoryMaterials[i].ID == ID && InventoryMaterials[i].MaterialDimensions == Dimensions && InventoryMaterials[i].MachineID == MachineID)
                return InventoryMaterials[i];  
        }
        return null;
    }

    public InventoryMaterialData GetMaterial(int MachineID)
    {
        for (int i = 0; i < InventoryMaterials.Count; i++)
        {
            if (InventoryMaterials[i].MachineID == MachineID)
                return InventoryMaterials[i];
        }
        return null;
    }

    public void TransferMaterial(string ID, string Dimensions, int Quantity, int FromMachineID, int ToMachineID)
    {
        for (int i = 0; i < InventoryMaterials.Count; i++)
        {
            if (InventoryMaterials[i].ID == ID && InventoryMaterials[i].MaterialDimensions == Dimensions && InventoryMaterials[i].MachineID == FromMachineID)
            {
                int ret= ReduceQuantity(ID, Dimensions, Quantity, FromMachineID);
                if (ret < 0)
                {
                    Debug.Log("Transfer material error; Material not found.");
                }else if (ret == 0)
                {
                    AddMaterial(ID, Dimensions, Quantity, ToMachineID);
                    FireEventInventoryChanged();
                }
                else if (ret > 0)
                {
                    Debug.Log("Transfer material error; Tried to move too much quantyty; Adding only leftover materials");
                    int temp = Quantity - ret;
                    AddMaterial(ID, Dimensions, temp, ToMachineID);
                    FireEventInventoryChanged();
                }

            }
        }
    }

    public void RemoveMaterial(string ID, string Dimensions, int MachineID)
    {
        for (int i = 0; i < InventoryMaterials.Count; i++)
        {
            if (InventoryMaterials[i].ID == ID && InventoryMaterials[i].MaterialDimensions == Dimensions && InventoryMaterials[i].MachineID == MachineID)
                RemoveMaterialAtIndex(i);
        }
    }

    public void RemoveMaterialAtIndex(int index)
    {
        InventoryMaterials.RemoveAt(index);
        FireEventInventoryChanged();
    }

    public int ReduceQuantity(int MachineID, int Quantity)
    {
        Debug.Log("Reduce Quantity called!");
        //0= quantity reduced, -1=material not found, >0 quantity reduced;Material finished;returns quantity exess; 
        for (int i = 0; i < InventoryMaterials.Count; i++)
        {
            if (InventoryMaterials[i].MachineID == MachineID)
            {
                if (Quantity > InventoryMaterials[i].MaterialQuantity)
                {
                    int exess = Quantity - InventoryMaterials[i].MaterialQuantity;
                    RemoveMaterialAtIndex(i);
                    return exess;
                }
                else
                {
                    InventoryMaterials[i].MaterialQuantity -= Quantity;
                    if (InventoryMaterials[i].MaterialQuantity == 0)
                        RemoveMaterialAtIndex(i);
                    FireEventInventoryChanged();
                    return 0;
                }
            }
        }
        return -1;
    }

    public int ReduceQuantity(string ID, string Dimensions, int Quantity, int MachineID)
    {
        Debug.Log("Reduce Quantity called!");
        //0= quantity reduced, -1=material not found, >0 quantity reduced;Material finished;returns quantity exess; 
        for (int i = 0; i < InventoryMaterials.Count; i++)
        {
            if (InventoryMaterials[i].ID == ID && InventoryMaterials[i].MaterialDimensions == Dimensions && InventoryMaterials[i].MachineID == MachineID)
            {
                if(Quantity > InventoryMaterials[i].MaterialQuantity)
                {
                    int exess = Quantity - InventoryMaterials[i].MaterialQuantity;
                    RemoveMaterialAtIndex(i);
                    return exess;
                }
                else
                {
                    InventoryMaterials[i].MaterialQuantity -= Quantity;
                    if (InventoryMaterials[i].MaterialQuantity == 0)
                        RemoveMaterialAtIndex(i);
                    FireEventInventoryChanged();
                    return 0;
                }
            }
        }
        return -1;
    }
    #endregion

    #region Buy and Sell Functions

    //Buy Tool
    public int BuyTool(string ID, string dimensions)
    {
        for(int i=0; i < MachineShopData.Tools.Array.Length; i++)
        {
            if(MachineShopData.Tools.Array[i].ID == ID)
            {
                //We found the tool
                if (Money >= MachineShopData.Tools.Array[i].Price)
                {
                    //We have enough money
                }
                else
                {
                    //We don't have enough money; buy tools anyways
                }
                ReduceMoney(MachineShopData.Tools.Array[i].Price);
                AddTool(ID, dimensions, MachineShopData.Tools.Array[i].Durability);
                return 0;

            }
        }
        //Error; Tool not found.
        return -1;
    }

    //Sell Tool
    public void SellTool(int UniqueID)
    {
        //Get money
        for (int i = 0; i < InventoryTools.Count; i++)
        {
            if (InventoryTools[i].UniqueCode == UniqueID)
            {
                string ID = InventoryTools[i].ID;
                int durability = InventoryTools[i].ToolDurability;
                //find init Template for the initial durability and price
                for (int j = 0; j < MachineShopData.Tools.Array.Length; j++)
                {
                    if (MachineShopData.Tools.Array[j].ID == ID)
                    {
                        //We found the tool
                        int InitDura = MachineShopData.Tools.Array[j].Durability;
                        int InitPrice = MachineShopData.Tools.Array[j].Price;
                        AddMoney(InitPrice * (durability / InitDura));
                    }
                }
                //Remove tool if exists; if not found means it is an invalid one so just delete it.
                RemoveToolAtIndex(i);
            }
        }
    }

    //Buy Material
    public int BuyMaterial(string ID, string dimensions, int Quantity)
    {
        for (int i = 0; i < MachineShopData.Materials.Array.Length; i++)
        {
            if (MachineShopData.Materials.Array[i].ID == ID)
            {
                int price = MachineShopData.Materials.Array[i].Price;
                //We found the material
                if (Money >= price)
                {
                    //We have enough money
                }
                else
                {
                    //We don't have enough money; buy tools anyways
                }
                ReduceMoney(price * Quantity);
                AddMaterial(ID, dimensions, Quantity);
                return 0;

            }
        }
        //Error; material not found.
        return -1;
    }

    //Sell Material
    public void SellMaterial(string ID, string Dimensions, int Quantity, int MachineID)
    {
        int InitPrice = 0;
        //Get money
        for (int i = 0; i < InventoryMaterials.Count; i++)
        {
 
            if (InventoryMaterials[i].ID == ID && InventoryMaterials[i].MaterialDimensions == Dimensions && InventoryMaterials[i].MachineID == MachineID)
            {
                //find init Template for the initial durability and price
                for (int j = 0; j < MachineShopData.Materials.Array.Length; j++)
                {
                    if (MachineShopData.Materials.Array[i].ID == ID)
                    {
                        //We found the material
                        InitPrice = MachineShopData.Materials.Array[j].Price;
                    }
                }
            }
        }
        //Remove tool if exists; if not found means it is an invalid one so just delete it.
        int ret = ReduceQuantity(ID,Dimensions,Quantity,MachineID);
            
        if (ret == 0)//Quantity reduced; add full money
            AddMoney(InitPrice * Quantity);
        else if (ret > 0)//Less than quantity was available; gain money equal to howerer much was available
            AddMoney(InitPrice * (Quantity - ret));

        FireEventInventoryChanged();
    }

    //Buy a New Machine
    //TODO
    #endregion

    #region Currency Managers (Money and EXP)

    //Add money
    public void AddMoney(int amount)
    {
        Money += amount;
        FireEventMoneyUpdate();
    }

    //Reduce Money
    public void ReduceMoney(int amount)
    {
        Money -= amount;
        FireEventMoneyUpdate();
    }

    //Set money
    public void SetMoney(int amount)
    {
        Money = amount;
        FireEventMoneyUpdate();
    }
    
    //Add Exp 
    public void AddExp (int amount)
    {
        Exp += amount;
        FireEventXpUpdate();
    }

    //Remove Exp
    public void RemoveExp (int amount)
    {
        Exp -= amount;
        FireEventXpUpdate();
    }

    public void SetExp(int amount)
    {
        Exp = amount;
        FireEventXpUpdate();
    }

    #endregion

    #region Machine functions
    public ActiveMachineData GetMachineData(int machineID)
    {
        foreach (ActiveMachineData dat in MachinesList)
        {
            if (dat.MachineID == machineID)
            {
                return dat;
            }
        }
        return null;
    }

    #endregion

    #region Doozy Event Managers
    // Execute Doozy event- Any event
    public void ExecuteEventByName(string name)
    {
        GameEventMessage.SendEvent(name);
    }

    public void ExecuteEventByName(string name, GameObject obj) //resently added 10/12/2020
    {
        GameEventMessage.SendEvent(name, obj);
    }

    //Execute the Inventory Changed Event
    public void FireEventInventoryChanged()
    {
        ExecuteEventByName("UpdatePlayerInventory");
    }

    public void FireEventLoadProfile()
    {
        ExecuteEventByName("LoadProfile");
    }

    public void FireEventMoneyUpdate()
    {
        ExecuteEventByName("MoneyUpdate");
    }
    public void FireEventXpUpdate()
    {
        ExecuteEventByName("XpUpdate");
    }
    
    public void SetCursorLock (bool value)
    {
        if (value)
        {
            LockLevel--;
            if (LockLevel <= 0)
            {
                LockLevel = 0;
                CursorLocked = true;
            }
        }
        else
        {
            LockLevel++;
            if (LockLevel > 0)
            {
                CursorLocked = false;
            }
        }

    }

    #endregion

}

[System.Serializable]
public class PlayerDataContainer
{
    #region Basic Profile Data
    public string ProfileName;
    public int AvatarIndex;

    public bool MillingEnabled;
    public bool TurningEnabled;
    #endregion

    #region Profile Progression Data
    public float Exp;
    public int MillingStage;
    public int TurningStage;
    public int Money;
    public bool[] MillingTutorialsRead;
    public bool[] TurningTutorialsRead;
    public int FirstTimeTutorialState;

    #endregion

    #region Inventory Data
    //Tools
    public string[] ToolID;
    public int[] ToolDurability;
    public string[] ToolDimensions;
    public int[] ToolMachineID;
    //Materials
    public string[] MaterialID;
    public int[] MaterialQuantity;
    public string[] MaterialDimensions;
    public int[] MaterialMachineID;
    #endregion

    #region Machine Shop Data
    public int[] MachineID;
    public string[] MachineName;
    #endregion

    #region Active Mission Data
    //Active Mission
    public bool AMisMilling;
    public int AMmissionIndex;
    public bool AMisMissionStageUp;
    public int AMrewardGoldPerItem;
    public int AMrewardExpPerMission;
    public int AMrequestedQuantity;
    public string AMrequestedMaterialID;
    public string AMrequestedMaterialDimensions;
    public string AMwrittenCode;
    public int AMcompletedQuantity;
    public bool AMisCodeChecked;
    public bool AMisBeingExecuted;
    public int AMexecutionCarveQuantity;
    public float AMdelay;
    public float AMdelayTimePassed;
    public float AMshownDelay;
    public int AMexecutingMachine;
    public string AMadditionalDescription;
    //next mission list
    public bool[] NMisMilling;
    public int[] NMmissionIndex;
    public bool[] NMisMissionStageUp;
    public int[] NMrewardGoldPerItem;
    public int[] NMrewardExpPerMission;
    public int[] NMrequestedQuantity;
    public string[] NMrequestedMaterialID;
    public string[] NMrequestedMaterialDimensions;
    public string[] NMadditionalDescription;
    //public List<ActiveMissionData> NextMissionList;
    //additional mission data
    public int CorrectMissionCombo;
    public int[] TimesCompletedPerMissionMilling;
    public int[] TimesCompletedPerMissionTurning;

    //public int ActiveMissionIndex;
    //public bool ActiveMissionIsMilling;
    #endregion

    #region Evaluation Data
    public string PlayerLog;
    public float TimeMemory;
    #endregion

    #region Constructors
    public PlayerDataContainer() { }

    public PlayerDataContainer(PlayerData data)
    {
        Init(data);
    }
    #endregion

    #region Serialiser
    public void Init(PlayerData data)
    {
        //Basic Profile Data
        ProfileName = data.ProfileName;
        AvatarIndex = data.AvatarIndex;

        MillingEnabled = data.MillingEnabled;
        TurningEnabled = data.TurningEnabled;

        //Profile Progression data
        Exp = data.Exp;
        Money = data.Money;
        MillingStage = data.MillingStage;
        TurningStage = data.TurningStage;
        Money = data.Money;

        //MillingTutorialsRead
        if (data.MillingTutorialsRead != null)
        {
            MillingTutorialsRead = new bool[data.MillingTutorialsRead.Length];
            for (int i = 0; i < data.MillingTutorialsRead.Length; i++)
            {
                MillingTutorialsRead[i] = data.MillingTutorialsRead[i];
            }
        }
        else
        {
            MillingTutorialsRead = new bool[1];
            MillingTutorialsRead[0] = false;
        }

        //TurningTutorialsRead
        if (data.TurningTutorialsRead != null)
        {
            TurningTutorialsRead = new bool[data.TurningTutorialsRead.Length];
            for (int i = 0; i < data.TurningTutorialsRead.Length; i++)
            {
                TurningTutorialsRead[i] = data.TurningTutorialsRead[i];
            }
        }
        else
        {
            TurningTutorialsRead = new bool[1];
            TurningTutorialsRead[0] = false;
        }

        //1st time tutrials read
        FirstTimeTutorialState = data.FirstTimeTutorialState;

        //Machine Shop Data
        MachineID = new int[data.MachinesList.Count];
        MachineName = new string[data.MachinesList.Count];
        for (int i = 0; i < data.MachinesList.Count; i++)
        {
            MachineID[i] = data.MachinesList[i].MachineID;
            MachineName[i] = data.MachinesList[i].MachineName;
        }

        //Active Mission Data

        //Active mission
        AMisMilling = data.ActiveMission.IsMilling;
        AMmissionIndex = data.ActiveMission.MissionIndex;
        AMisMissionStageUp = data.ActiveMission.IsMissionStageUp;
        AMrewardGoldPerItem = data.ActiveMission.RewardGoldPerItem;
        AMrewardExpPerMission = data.ActiveMission.RewardExpPerMission;
        AMrequestedQuantity = data.ActiveMission.RequestedQuantity;
        AMrequestedMaterialID = data.ActiveMission.RequestedMaterialID;
        AMrequestedMaterialDimensions = data.ActiveMission.RequestedMaterialDimensions;
        AMwrittenCode = data.ActiveMission.WrittenCode;
        AMcompletedQuantity = data.ActiveMission.CompletedQuantity;
        AMisCodeChecked = data.ActiveMission.IsCodeChecked;
        AMisBeingExecuted = data.ActiveMission.IsBeingExecuted;
        AMexecutionCarveQuantity = data.ActiveMission.ExecutionCarveQuantity;
        AMdelay = data.ActiveMission.Delay;
        AMdelayTimePassed = data.ActiveMission.DelayTimePassed;
        AMshownDelay = data.ActiveMission.ShownDelay;
        AMexecutingMachine = data.ActiveMission.ExecutingMachine;
        AMadditionalDescription = data.ActiveMission.AdditionalDescription;

        //next mission list
        NMisMilling = new bool[data.NextMissionList.Count];
        NMmissionIndex = new int[data.NextMissionList.Count];
        NMisMissionStageUp = new bool[data.NextMissionList.Count];
        NMrewardGoldPerItem = new int[data.NextMissionList.Count];
        NMrewardExpPerMission = new int[data.NextMissionList.Count];
        NMrequestedQuantity = new int[data.NextMissionList.Count];
        NMrequestedMaterialID = new string[data.NextMissionList.Count];
        NMrequestedMaterialDimensions = new string[data.NextMissionList.Count];
        NMadditionalDescription = new string[data.NextMissionList.Count];
        for (int i=0; i < data.NextMissionList.Count; i++)
        {
            NMisMilling[i] = data.NextMissionList[i].IsMilling;
            NMmissionIndex[i] = data.NextMissionList[i].MissionIndex;
            NMisMissionStageUp[i] = data.NextMissionList[i].IsMissionStageUp;
            NMrewardGoldPerItem[i] = data.NextMissionList[i].RewardGoldPerItem;
            NMrewardExpPerMission[i] = data.NextMissionList[i].RewardExpPerMission;
            NMrequestedQuantity[i] = data.NextMissionList[i].RequestedQuantity;
            NMrequestedMaterialID[i] = data.NextMissionList[i].RequestedMaterialID;
            NMrequestedMaterialDimensions[i] = data.NextMissionList[i].RequestedMaterialDimensions;
            NMadditionalDescription[i] = data.NextMissionList[i].AdditionalDescription;
        }

        //additional mission data
        CorrectMissionCombo = data.CorrectMissionCombo;
        TimesCompletedPerMissionMilling = new int[data.TimesCompletedPerMissionMilling.Length];
        for (int i = 0; i < TimesCompletedPerMissionMilling.Length; i++)
            TimesCompletedPerMissionMilling[i] = data.TimesCompletedPerMissionMilling[i];
        TimesCompletedPerMissionTurning = new int[data.TimesCompletedPerMissionTurning.Length];
        for (int i = 0; i < TimesCompletedPerMissionTurning.Length; i++)
            TimesCompletedPerMissionTurning[i] = data.TimesCompletedPerMissionTurning[i];


        //Inventory Data

        //Tools
        if (data.InventoryTools.Count > 0)
        {
            ToolID = new string[data.InventoryTools.Count];
            ToolDimensions = new string[data.InventoryTools.Count];
            ToolDurability = new int[data.InventoryTools.Count];
            ToolMachineID = new int[data.InventoryTools.Count];
            //Save the Tools one by one
            for (int i=0;i<data.InventoryTools.Count; i++)
            {
                ToolID[i] = data.InventoryTools[i].ID;
                ToolDimensions[i] = data.InventoryTools[i].ToolDimensions; 
                ToolDurability[i] = data.InventoryTools[i].ToolDurability;
                ToolMachineID[i] = data.InventoryTools[i].MachineID;
            }
        }
        else
        {
            //No Tools in list; Add one empty field as placeholder because Null cannot be saved
            ToolID = new string[1];
            ToolID[0] = "";
            ToolDimensions = new string[1];
            ToolDimensions[0] = "";
            ToolDurability = new int[1];
            ToolDurability[0] = 0;
            ToolMachineID = new int[1];
            ToolMachineID[0] = 0;
        }

        //Materials
        if(data.InventoryMaterials.Count > 0)
        {
            MaterialID = new string[data.InventoryMaterials.Count];
            MaterialDimensions = new string[data.InventoryMaterials.Count];
            MaterialQuantity = new int[data.InventoryMaterials.Count];
            MaterialMachineID = new int[data.InventoryMaterials.Count];
            //Save the Materials one by one
            for (int i=0; i< data.InventoryMaterials.Count; i++)
            {
                MaterialID[i] = data.InventoryMaterials[i].ID;
                MaterialDimensions[i] = data.InventoryMaterials[i].MaterialDimensions;
                MaterialQuantity[i] = data.InventoryMaterials[i].MaterialQuantity;
                MaterialMachineID[i] = data.InventoryMaterials[i].MachineID;
            }
            
        }
        else
        {
            //No materials in list; Add one empty field as placeholder because Null cannot be saved
            MaterialID = new string[1];
            MaterialID[0] = "";
            MaterialDimensions = new string[1];
            MaterialDimensions[0] = "";
            MaterialQuantity = new int[1];
            MaterialQuantity[0] = 0;
            MaterialMachineID = new int[1];
            MaterialMachineID[0] = 0;
        }

        //Evaluation data
        PlayerLog = data.PlayerLog;
        TimeMemory = data.TimeMemory;

    }
    #endregion
}

[System.Serializable]
public class InventoryToolData
{
    #region Data
    public int UniqueCode;
    public string ID;
    public string ToolDimensions;
    public int ToolDurability;
    public int MachineID;// 0= inventory, >0 loaded in machine;

    public ToolTemplate Template;
    #endregion

    #region Constructor
    public InventoryToolData(string iD, string dimensions, int durability, int uniqueCode, int machineID)
    {
        ID = iD;
        ToolDimensions = dimensions;
        ToolDurability = durability;
        UniqueCode = uniqueCode;
        MachineID = machineID;

        
        if (GameMaster.Instance.GetComponent<MachineShopData>().toolSet == null)
        {
            Debug.LogError("Inventory System: Tool Database empty. Cannot load Tool Template.");
            return;
        }
        if (GameMaster.Instance.GetComponent<MachineShopData>().toolSet.Array == null)
        {
            Debug.LogError("Inventory System: Tool Database Array empty. Cannot load Tool Template.");
            return;
        }

        if (string.IsNullOrEmpty(ID))
        {
            Debug.LogWarning("Inventory System: Tool ID is null or empty. Aborting the Load Tool Template Operation.");
            return;
        }
            

        for (int i=0; i < GameMaster.Instance.GetComponent<MachineShopData>().toolSet.Array.Length; i++)
        {
            if (ID == GameMaster.Instance.GetComponent<MachineShopData>().toolSet.Array[i].ID)
            {
                Template = GameMaster.Instance.GetComponent<MachineShopData>().toolSet.Array[i];
            }
        }

        if (Template == null)
        {
            Debug.LogError("Inventory System: Tool template not found in the database.");
            return;
        }

        if (ToolDurability < 0)
        {
            ToolDurability = Template.Durability;
        }
        
    }
    #endregion
}

[System.Serializable]
public class InventoryMaterialData
{
    #region Data
    public string ID;
    public string MaterialDimensions;
    public int MaterialQuantity;
    public int MachineID;// 0= inventory, >0 loaded in machine;

    public MaterialTemplate Template;
    #endregion

    #region Constructor
    public InventoryMaterialData(string iD, string dimensions, int quantity, int machineID)
    {
        ID = iD;
        MaterialDimensions = dimensions;
        MaterialQuantity = quantity;
        MachineID = machineID;

        if (GameMaster.Instance.GetComponent<MachineShopData>().materialsSet == null)
        {
            Debug.LogError("Inventory System: Material Database empty. cannot load Material Template.");
            return;
        }
        if (GameMaster.Instance.GetComponent<MachineShopData>().materialsSet.Array == null)
        {
            Debug.LogError("Inventory System: Material Database Array empty. cannot load Material Template.");
            return;
        }
        if (string.IsNullOrEmpty(ID))
        {
            Debug.LogWarning("Inventory System: Material ID is null or empty. Aborting the Load Material Template Operation.");
            return;
        }

        for (int i=0; i< GameMaster.Instance.GetComponent<MachineShopData>().materialsSet.Array.Length; i++)
        {
            if (string.Equals(ID, GameMaster.Instance.GetComponent<MachineShopData>().materialsSet.Array[i].ID))
            {
                Template = GameMaster.Instance.GetComponent<MachineShopData>().materialsSet.Array[i];
            }
        }
        if(Template==null)
            Debug.LogError("Inventory System: Material template not found in the database.");
    }
    #endregion
}

[System.Serializable]
public class ActiveMissionData
{
    #region Data
    //Mission info
    public bool IsMilling; //If true, the Mission is a Milling one. If not, it is a Turning mission.
    public int MissionIndex;
    public bool IsMissionStageUp;

    //Dynamic mission data; calculated while generating the mission.
    public int RewardGoldPerItem;
    public int RewardExpPerMission;

    public int RequestedQuantity;
    public string RequestedMaterialID;
    public string RequestedMaterialDimensions;

    public string AdditionalDescription;

    //partial progress-code
    public string WrittenCode;
    public int CompletedQuantity;
    public bool IsCodeChecked;

    //partial progress- execution
    public bool IsBeingExecuted;
    public int ExecutionCarveQuantity;
    public float Delay;
    public float DelayTimePassed;
    public float ShownDelay;
    public int ExecutingMachine;

    //Base mission template
    public MissionTemplate Template;
    #endregion

    #region Constructor
    public ActiveMissionData (bool isMilling, int missionIndex)
    {
        Init(isMilling, missionIndex, false, 0, 0, 0, "", "", "", 0, false, false, 0, 0, 0, 0, 0, "");

    }

    public ActiveMissionData(bool isMilling, int missionIndex, bool isMissionStageUp, int rewardGoldPerItem, int rewardExpPerMission, int requestedQuantity, string requestedMaterialID, string requestedMaterialDimensions, string additionalDescription)
    {
        Init(isMilling, missionIndex, isMissionStageUp, rewardGoldPerItem, rewardExpPerMission, requestedQuantity, requestedMaterialID, requestedMaterialDimensions, "", 0, false, false,0, 0, 0, 0, 0, additionalDescription);

    }
    /*
    public ActiveMissionData(bool isMilling, int missionIndex, bool isMissionStageUp, int rewardGoldPerItem, int rewardExpPerMission, int requestedQuantity, string requestedMaterialID, string requestedMaterialDimensions, string writtenCode, int completedQuantity, bool isCodeChecked, bool isBeingExecuted, int executionCarveQuantity, float delay, float delayTimePassed, float shownDelay, int executingMachine)
    {
        Init(isMilling, missionIndex, isMissionStageUp, rewardGoldPerItem, rewardExpPerMission, requestedQuantity, requestedMaterialID, requestedMaterialDimensions, writtenCode, completedQuantity, isCodeChecked, isBeingExecuted,executionCarveQuantity, delay, delayTimePassed, shownDelay, executingMachine);

    }*/

    public ActiveMissionData(bool isMilling, int missionIndex, bool isMissionStageUp, int rewardGoldPerItem, int rewardExpPerMission, int requestedQuantity, string requestedMaterialID, string requestedMaterialDimensions, string writtenCode, int completedQuantity, bool isCodeChecked, bool isBeingExecuted, int executionCarveQuantity, float delay, float delayTimePassed, float shownDelay, int executingMachine, string additionalDescription)
    {
        Init(isMilling, missionIndex, isMissionStageUp, rewardGoldPerItem, rewardExpPerMission, requestedQuantity, requestedMaterialID, requestedMaterialDimensions, writtenCode, completedQuantity, isCodeChecked, isBeingExecuted, executionCarveQuantity, delay, delayTimePassed, shownDelay, executingMachine, additionalDescription);

    }

    public void Init(bool isMilling, int missionIndex, bool isMissionStageUp, int rewardGoldPerItem, int rewardExpPerMission, int requestedQuantity, string requestedMaterialID, string requestedMaterialDimensions, string writtenCode, int completedQuantity, bool isCodeChecked, bool isBeingExecuted, int executionCarveQuantity, float delay, float delayTimePassed, float shownDelay, int executingMachine, string additionalDescription)
    {
        IsMilling = isMilling;
        MissionIndex = missionIndex;

        IsMissionStageUp = isMissionStageUp;
        RewardGoldPerItem = rewardGoldPerItem;
        RewardExpPerMission = rewardExpPerMission;
        RequestedQuantity = requestedQuantity;
        RequestedMaterialID = requestedMaterialID;
        RequestedMaterialDimensions = requestedMaterialDimensions;
        WrittenCode = writtenCode;
        CompletedQuantity = completedQuantity;
        IsCodeChecked = isCodeChecked;
        IsBeingExecuted = isBeingExecuted;
        ExecutionCarveQuantity = executionCarveQuantity;
        Delay = delay;
        DelayTimePassed = delayTimePassed;
        ShownDelay = shownDelay;
        ExecutingMachine = executingMachine;
        AdditionalDescription = additionalDescription;

        //Set Template
        if (MissionIndex < 0)
            return;

        MissionArray data;
        if (isMilling)
            data = GameMaster.Instance.GetComponent<MachineShopData>().millingMissionSet;
        else
            data = GameMaster.Instance.GetComponent<MachineShopData>().turningMissionSet;


        if (data == null)
        {
            Debug.LogError("Mission System: Mission Database empty. cannot load Mission Template.");
            return;
        }
        if (data.templateArray == null)
        {
            Debug.LogError("Mission System: Mission Database Array empty. cannot load Mission Template.");
            return;
        }

        if (missionIndex >= data.templateArray.Length)
        {
            Debug.LogError("Mission System: Mission Index out of bounds.");
            return;
        }

        Template = data.templateArray[missionIndex];
    }
    #endregion
}

[System.Serializable]
public class ActiveMachineData
{
    #region Data
    public int MachineID;
    public string MachineName;

    public MachineTemplate Template;
    #endregion

    #region Constructor
    public ActiveMachineData(int machineID, string machineName)
    {
        MachineID = machineID;
        MachineName = machineName;

        if (GameMaster.Instance.GetComponent<MachineShopData>().MachineSet == null)
        {
            Debug.LogError("Inventory System: Machine Database empty. cannot load Machine Template.");
            return;
        }
        if (GameMaster.Instance.GetComponent<MachineShopData>().MachineSet.Array == null)
        {
            Debug.LogError("Inventory System: Machine Database Array empty. cannot load Machine Template.");
            return;
        }
        if (string.IsNullOrEmpty(MachineName))
        {
            Debug.LogWarning("Inventory System: Machine Name is null or empty. Aborting the Load Machine Template Operation.");
            return;
        }

        for (int i = 0; i < GameMaster.Instance.GetComponent<MachineShopData>().MachineSet.Array.Length; i++)
        {
            if (string.Equals(MachineName, GameMaster.Instance.GetComponent<MachineShopData>().MachineSet.Array[i].MachineName))
            {
                Template = GameMaster.Instance.GetComponent<MachineShopData>().MachineSet.Array[i];
            }
        }
        if (Template == null)
            Debug.LogError("Inventory System: Machine template not found in the database.");
    }
    #endregion
}