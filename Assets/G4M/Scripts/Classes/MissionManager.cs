using System;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;

public class MissionManager : MonoBehaviour
{
    #region Static Values
    public static int ExpPerStage = 100;
    public static int BaseExpPerMission = 100;
    public static float BaseGoldRewardMultiplier = 1.8f;
    public static float DelayTimePerObject = 1f;

    public static int DefaultQuantity = 20;
    public static string DefaultMaterialMilling = "Material1";
    public static string DefaultMaterialDimensionsMilling = "150x100x30";
    public static string DefaultMaterialTurning = "Material5";
    public static string DefaultMaterialDimensionsTurning = "Φ30";
    #endregion

    #region Unity Functions
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    #endregion

    #region Mission Generation
    public void GetNewMissions()
    {
        PlayerData pd = gameObject.GetComponent<PlayerData>();

        pd.ActiveMission.MissionIndex = -1;
        ClearNextMissionList();

        pd.PlayerLog += "[" + GameMaster.Instance.GetComponent<PlayerData>().TimeMemory + "]" + "New missions Generated" + ".\n";


        //Milling missions
        if (pd.MillingEnabled)
        {
            bool isMilling = true;

            //Tutorial mission for stage+1
            if (pd.Exp >= pd.MillingStage * ExpPerStage)
            {
                for (int i = 0; i < MachineShopData.MillingMissions.templateArray.Length; i++)
                {
                    MissionTemplate mt = MachineShopData.MillingMissions.templateArray[i];
                    if (mt.Stage == pd.MillingStage + 1 && mt.isThisMissionTutorialForTheStage)
                    {
                        //Debug.Log("adding tutorial");
                        pd.NextMissionList.Add(GenerateMission(isMilling, i, true));
                        break;
                    }

                }
            }

            //Least completed mission of current stage
            int minComp = 10000;
            int minIndex = -1;
            for (int i = 0; i < MachineShopData.MillingMissions.templateArray.Length; i++)
            {
                MissionTemplate mt = MachineShopData.MillingMissions.templateArray[i];
                if (mt.Stage == pd.MillingStage && pd.TimesCompletedPerMissionMilling[i]<minComp)
                {
                    minComp = pd.TimesCompletedPerMissionMilling[i];
                    minIndex = i;
                }

            }
            if (minIndex != -1)
            {
                //Debug.Log("adding current");
                pd.NextMissionList.Add(GenerateMission(isMilling, minIndex, false));
            }

            //Least completed mission of lower stages
            minComp = 10000;
            minIndex = -1;
            for (int i = 0; i < MachineShopData.MillingMissions.templateArray.Length; i++)
            {
                MissionTemplate mt = MachineShopData.MillingMissions.templateArray[i];
                if (mt.Stage < pd.MillingStage && pd.TimesCompletedPerMissionMilling[i] < minComp)
                {
                    minComp = pd.TimesCompletedPerMissionMilling[i];
                    minIndex = i;
                }

            }
            if (minIndex != -1)
            {
                //Debug.Log("adding current");
                pd.NextMissionList.Add(GenerateMission(isMilling, minIndex, false));
            }

            //Random missions
            //TODO
        }

        //Turning Missions
        if (pd.TurningEnabled)
        {
            bool isMilling = false;

            //Tutorial mission for stage+1
            if (pd.Exp >= pd.TurningStage * ExpPerStage)
            {
                for (int i = 0; i < MachineShopData.TurningMissions.templateArray.Length; i++)
                {
                    MissionTemplate mt = MachineShopData.TurningMissions.templateArray[i];
                    if (mt.Stage == pd.TurningStage + 1 && mt.isThisMissionTutorialForTheStage)
                    {
                        Debug.Log("adding tutorial Tur");
                        pd.NextMissionList.Add(GenerateMission(isMilling, i, true));
                        break;
                    }
                }
            }

            //Least completed mission of current stage
            int minComp = 10000;
            int minIndex = -1;
            for (int i = 0; i < MachineShopData.TurningMissions.templateArray.Length; i++)
            {
                MissionTemplate mt = MachineShopData.TurningMissions.templateArray[i];
                if (mt.Stage == pd.TurningStage && pd.TimesCompletedPerMissionTurning[i] < minComp)
                {
                    minComp = pd.TimesCompletedPerMissionTurning[i];
                    minIndex = i;
                }

            }
            if (minIndex != -1)
            {
                //Debug.Log("adding current Tur");
                pd.NextMissionList.Add(GenerateMission(isMilling, minIndex, false));
            }

            //Least completed mission of previous stages
            minComp = 10000;
            minIndex = -1;
            for (int i = 0; i < MachineShopData.TurningMissions.templateArray.Length; i++)
            {
                MissionTemplate mt = MachineShopData.TurningMissions.templateArray[i];
                if (mt.Stage < pd.TurningStage && pd.TimesCompletedPerMissionTurning[i] < minComp)
                {
                    minComp = pd.TimesCompletedPerMissionTurning[i];
                    minIndex = i;
                }

            }
            if (minIndex != -1)
            {
                //Debug.Log("adding current Tur");
                pd.NextMissionList.Add(GenerateMission(isMilling, minIndex, false));
            }

            //Random missions
            //TODO
        }


    }

    public ActiveMissionData GenerateMission(bool isMilling, int index, bool isStageUp)
    {
        ActiveMissionData ret = new ActiveMissionData(isMilling, index);
        PlayerData pd = gameObject.GetComponent<PlayerData>();
        if (ret.Template == null)
            return ret;

        if (isMilling)
        {
            //if first time, set default milling values
            if (pd.TimesCompletedPerMissionMilling[index] == 0)
            {
                //If it is the first time we get a mission, use the default parameters (cheapest material, fixed quantity...)
                ret.RequestedQuantity = DefaultQuantity;
                ret.RequestedMaterialID = DefaultMaterialMilling;
                ret.RequestedMaterialDimensions = DefaultMaterialDimensionsMilling;
                //ret.RequestedMaterialID
            }
            //otherwise randomise each value
            else
            {
                ret.RequestedQuantity = (int) UnityEngine.Random.Range(DefaultQuantity * 0.5f, DefaultQuantity * 2f);
                ret.RequestedMaterialID = DefaultMaterialMilling;
                ret.RequestedMaterialDimensions = DefaultMaterialDimensionsMilling;
            }
        }
        else
        {
            //if first time, set default milling values
            if (pd.TimesCompletedPerMissionTurning[index] == 0)
            {
                ret.RequestedQuantity = DefaultQuantity;
                ret.RequestedMaterialID = DefaultMaterialTurning;
                ret.RequestedMaterialDimensions = DefaultMaterialDimensionsTurning;
            }
            //otherwise randomise each value
            else
            {
                ret.RequestedQuantity = (int)UnityEngine.Random.Range(DefaultQuantity * 0.5f, DefaultQuantity * 2f);
                ret.RequestedMaterialID = DefaultMaterialTurning;
                ret.RequestedMaterialDimensions = DefaultMaterialDimensionsTurning;
            }
        }

        ret.IsMissionStageUp = isStageUp;
        //gold gain
        float missiongold;
        MaterialTemplate temp = new MaterialTemplate();
        foreach (MaterialTemplate mt in MachineShopData.Materials.Array)
        {
            if (mt.ID == ret.RequestedMaterialID)
            {
                temp = mt;
            }
        }
        missiongold = temp.Price * BaseGoldRewardMultiplier;
        ret.RewardGoldPerItem = (int) missiongold;

        //exp gain
        float missionexp;
        if (ret.IsMissionStageUp)
        {
            missionexp = BaseExpPerMission;
        }
        else
        {
            float combomultiplier;
            if (pd.CorrectMissionCombo < 2)
                combomultiplier = 1;
            else
                combomultiplier = 1 + (0.1f * (pd.CorrectMissionCombo - 2));
            missionexp = BaseExpPerMission * combomultiplier;
        }
        if (isMilling && ret.Template.Stage < pd.MillingStage)
            missionexp = missionexp * 0.2f;
        if (!isMilling && ret.Template.Stage < pd.TurningStage)
            missionexp = missionexp * 0.2f;

        ret.RewardExpPerMission = (int) missionexp;

        ret.AdditionalDescription = "Να κατασκευαστούν <color=orange>" + ret.RequestedQuantity + " τεμάχια </color>.";

        return ret;

        //Else randomize with weights.
    }

    public void SetActiveMission(int index)
    {
        PlayerData pd = gameObject.GetComponent<PlayerData>();
        pd.ActiveMission = pd.NextMissionList[index];
        ClearNextMissionList();

        pd.PlayerLog += "[" + GameMaster.Instance.GetComponent<PlayerData>().TimeMemory + "]" + "Mission Accepted: " + pd.ActiveMission.Template.Name + ". Is Milling: " + (pd.ActiveMission.IsMilling?"Yes":"No") + ".\n"; 


        /*
        if (pd.ActiveMission.IsMissionStageUp)
        {
            if (pd.ActiveMission.IsMilling) pd.MillingStage++;
            else pd.TurningStage++;
        }*/
        //alternative check
        if (pd.ActiveMission.IsMilling && pd.ActiveMission.IsMissionStageUp && pd.ActiveMission.Template.Stage > pd.MillingStage)
        {
            pd.MillingStage++;
        }
        if (!pd.ActiveMission.IsMilling && pd.ActiveMission.IsMissionStageUp && pd.ActiveMission.Template.Stage > pd.TurningStage)
        {
            pd.TurningStage++;
        }

        pd.ExecuteEventByName("MissionAccepted");
    }

    public void ClearNextMissionList()
    {
        gameObject.GetComponent<PlayerData>().NextMissionList.Clear();
    }

    #endregion

    #region Ex- Check Code Solution
    /* previous execute code
    public void CheckSolution(string input)
    {

        //input = input.Trim();
        //input = Regex.Replace(input, @"s", "");
        string solution = GameMaster.Instance.GetComponent<PlayerData>().ActiveMission.Template.MiddleCode;
        //solution.Trim();

        //solution = Regex.Replace(solution, @"s", "");

        //Debug.Log("Mission Manager: input:"+input);
        //Debug.Log("Mission Manager: solution:" + solution);
        string[] inp = input.Split('\n');
        string[] sol = solution.Split('\n');

        string popupconcat = "";

        bool correct = true;
        int inputOffset = 0;
        int i;
        for (i=0; i < sol.Length; i++)
        {

            if (inp.Length-1 <i + inputOffset)
            {
                correct = false;
                //Debug.Log("Mission Manager: Code requires additional steps.");
                popupconcat += "Ο κώδικας χρειαζέται παραπάνω εντολές." + "\n";
                //GameMaster.Instance.GetComponent<PopUpMessagesSender>().Test_FULL(message);
                break;
            }

            string compIn = Regex.Replace(inp[i + inputOffset], @"s+", String.Empty);
            string compSo = Regex.Replace(sol[i], @"s+", String.Empty);
            compIn = Regex.Replace(compIn, " ", String.Empty);
            compSo = Regex.Replace(compSo, " ", String.Empty);
            compIn = compIn.Trim();
            compSo = compSo.Trim();

            if (String.IsNullOrWhiteSpace(compIn))
            {
                inputOffset++;
                i--;
            }
            else
            {
                //pop up
               // GameMaster.Instance.GetComponent<PopUpMessagesSender>().Test_FULL("|" + compIn + "||" + compSo + "|");
                //Debug.LogWarning("|" + compIn + "||" + compSo + "|");
                //Debug.Log("in length" + compIn.Length + "so length" + compSo.Length);
              

                if (!string.Equals(compIn, compSo, StringComparison.OrdinalIgnoreCase))//TODO Replace the comaprision here
                {
                    correct = false;
                    //pop up
                    //GameMaster.Instance.GetComponent<PopUpMessagesSender>().Test_FULL("Mission Manager: Error in line " + (i + 1) + ".");
                    Debug.Log("Mission Manager: Error in line " + (i + 1) + ".");
                    popupconcat += "Σφάλμα στην γραμμή " + (i + 1) + "." + "\n";
                }
                else
                {
                    //pop up
                  //  GameMaster.Instance.GetComponent<PopUpMessagesSender>().Test_FULL("Mission Manager: Line " + (i + 1) + " is correct.");

                    Debug.Log("Mission Manager: Line " + (i + 1) + " is correct.");
                }
            }
        }

        bool addedFewerSteps = false;
        if (inp.Length > i + inputOffset)
        {
            for (int j=i; j+inputOffset<inp.Length; j++)
            {
                string compIn = Regex.Replace(inp[j + inputOffset], @"s+", String.Empty);
                compIn = Regex.Replace(compIn, " ", String.Empty);
                compIn = compIn.Trim();
                if (String.IsNullOrWhiteSpace(compIn))
                {
                    inputOffset++;
                    j--;
                }
                else
                {
                    correct = false;
                    //pop up
                    //GameMaster.Instance.GetComponent<PopUpMessagesSender>().Test_FULL("Mission Manager: Error in line " + (j + 1) + ". Code requires fewer steps.");
                    //Debug.Log("Mission Manager: Error in line " + (j + 1) + ". Code requires fewer steps.");
                    if (!addedFewerSteps)
                        popupconcat += "Ο κώδικας χρειάζεται λιγότερες εντολές." + "\n";
                    addedFewerSteps = true;
                }
            }
        }

        if (correct)
        {
            //pop up
           // GameMaster.Instance.GetComponent<PopUpMessagesSender>().Test_FULL("Mission Manager: Code is correct");
            Debug.Log("Mission Manager: Code is correct");
            popupconcat += "Ο κώδικας είναι σωστός." + "\n";
            GameMaster.Instance.GetComponent<PlayerData>().ActiveMission.IsCodeChecked = true;
            GameMaster.Instance.GetComponent<PlayerData>().ExecuteEventByName("CodeChecked");

        }
        GameMaster.Instance.GetComponent<PopUpMessagesSender>().Test_FULL(popupconcat);
    }

    */
    #endregion

    #region Check Code Solution

    public void CheckSolution(string input)
    {
        string lastXValue, lastYValue, lastZValue;
        string lastCommand;
        lastXValue = "  ";
        lastYValue = "  ";
        lastZValue = "  ";

        //input = input.Trim();
        //input = Regex.Replace(input, @"s", "");
        string topCode = GameMaster.Instance.GetComponent<PlayerData>().ActiveMission.Template.TopCode;
        string solution = GameMaster.Instance.GetComponent<PlayerData>().ActiveMission.Template.MiddleCode;

        //solution.Trim();

        //solution = Regex.Replace(solution, @"s", "");

        //Debug.Log("Mission Manager: input:"+input);
        //Debug.Log("Mission Manager: solution:" + solution);
        string[] inp = input.Split('\n');
        string[] sol = solution.Split('\n');
        string[] topSplit = topCode.Split('\n');

        string popupconcat = "";

        bool correct = true;
        int inputOffset = 0;
        int i, j, k;

        //Initialize X Y Z Values
        string[] commandSplit= new string[1];
        foreach (string line in topSplit)
        {
            commandSplit = line.Split(' ');
            foreach (string param in commandSplit)
            {
                if (param[0] == 'X')
                {
                    lastXValue = param.Substring(1);
                }
                else if (param[0] == 'Y')
                {
                    lastYValue = param.Substring(1);
                }
                else if (param[0] == 'Z')
                {
                    lastZValue = param.Substring(1);
                }
            }
        }

        //Save last Command
        if (commandSplit.Length > 0)
        {
            lastCommand = commandSplit[0];
        }
        else 
        {
            lastCommand = String.Empty;
        }

        List<string> inpParamList = new List<string>();
        List<string> solParamList = new List<string>();
        int checkStart;

        // Comparison Start
        for (i = 0; i < sol.Length; i++)
        {
            inpParamList.Clear();
            solParamList.Clear();
            checkStart = 0;

            if (inp.Length - 1 < i + inputOffset)
            {
                correct = false;
                //Debug.Log("Mission Manager: Code requires additional steps.");
                popupconcat += "Ο κώδικας χρειαζέται παραπάνω εντολές." + "\n";
                //GameMaster.Instance.GetComponent<PopUpMessagesSender>().Test_FULL(message);
                break;
            }

            commandSplit = inp[i + inputOffset].Split(' ');
            foreach (string param in commandSplit)
            {
                if (param.Length > 0)
                    inpParamList.Add(param);
            }

            commandSplit = sol[i].Split(' ');
            foreach (string param in commandSplit)
            {
                if (param.Length > 0)
                    solParamList.Add(param);
            }

            /*
            string compIn = Regex.Replace(inp[i + inputOffset], @"s+", String.Empty);
            string compSo = Regex.Replace(sol[i], @"s+", String.Empty); 

            //Remove white spaces inside string
            compIn = Regex.Replace(compIn, " ", String.Empty);
            compSo = Regex.Replace(compSo, " ", String.Empty);
            //Remove white spaces before and after string
            compIn = compIn.Trim();
            compSo = compSo.Trim();
            */

            if (String.IsNullOrWhiteSpace(inp[i + inputOffset]))
            {
                inputOffset++;
                i--;
            }
            else
            {
                //pop up
                // GameMaster.Instance.GetComponent<PopUpMessagesSender>().Test_FULL("|" + compIn + "||" + compSo + "|");
                //Debug.LogWarning("|" + compIn + "||" + compSo + "|");
                //Debug.Log("in length" + compIn.Length + "so length" + compSo.Length);

                // incorrect Code conditions
                if (!string.Equals(inpParamList[0], solParamList[0], StringComparison.OrdinalIgnoreCase))
                {
                    if (!string.Equals(solParamList[0], lastCommand, StringComparison.OrdinalIgnoreCase))
                    {
                        correct = false;
                        Debug.Log("Mission Manager: Error in line " + (i + 1) + ".");
                        popupconcat += "Σφάλμα στην γραμμή " + (i + 1) + ". Δεν έχει οριστεί κατάλληλη εντολή G." + "\n";
                    }
                    checkStart = 1;
                }
                else
                {
                    lastCommand = inpParamList[0];  // update last command typed by the user
                }

                //Check input and solution code by parameters. Also check if some of X Y Z values may be ignored.
                j = checkStart;
                k = 0;
                while (j < solParamList.Count && k < inpParamList.Count)
                {
                    if (string.Equals(inpParamList[k][0].ToString(), solParamList[j][0].ToString(), StringComparison.OrdinalIgnoreCase))
                    {
                        if (string.Equals(solParamList[j], inpParamList[k], StringComparison.OrdinalIgnoreCase))
                        {
                            //correct
                            if (inpParamList[k][0] == 'x' || inpParamList[k][0] == 'X')
                            {
                                lastXValue = inpParamList[k].Substring(1);
                            }
                            else if (inpParamList[k][0] == 'y' || inpParamList[k][0] == 'Y')
                            {
                                lastYValue = inpParamList[k].Substring(1);
                            }
                            else if (inpParamList[k][0] == 'Z' || inpParamList[k][0] == 'Z')
                            {
                                lastZValue = inpParamList[k].Substring(1);
                            }
                            j++;
                            k++;
                        }
                        else
                        {
                            //wrong
                            correct = false;
                            popupconcat += "Σφάλμα στην γραμμή " + (i + 1) + ". Λάθος Συντεταγμένες." + "\n";
                            k++;
                            j++;

                        }
                    }
                    else
                    {
                        if ((solParamList[j][0] == 'X' && string.Equals(lastXValue, solParamList[j].Substring(1))) ||
                       (solParamList[j][0] == 'Y' && string.Equals(lastYValue, solParamList[j].Substring(1))) ||
                       (solParamList[j][0] == 'Z' && string.Equals(lastZValue, solParamList[j].Substring(1))))
                        {
                            //skip sol
                            j++;
                        }
                        else if (((inpParamList[k][0] == 'x' || inpParamList[k][0] == 'X') && string.Equals(lastXValue, inpParamList[k].Substring(1))) ||
                          ((inpParamList[k][0] == 'y' || inpParamList[k][0] == 'Y') && string.Equals(lastYValue, inpParamList[k].Substring(1))) ||
                          ((inpParamList[k][0] == 'z' || inpParamList[k][0] == 'Z') && string.Equals(lastZValue, inpParamList[k].Substring(1))))
                        {
                            //skip inp
                            k++;
                        }
                        else
                        {
                            //skoupidiii
                            correct = false;
                            popupconcat += "Σφάλμα στην γραμμή " + (i + 1) + "." + "\n";
                            k++;
                            j++;
                        }

                    }
                    //TODO XYZ memory cannon in order

                 

                }

                if (k < inpParamList.Count)
                {
                    correct = false;
                    popupconcat += "Σφάλμα στην γραμμή " + (i + 1) + ". Η εντολή περιέχει περιττούς χαρακτήρες." + "\n";
                }

                if (j < solParamList.Count)
                {
                    correct = false;
                    popupconcat += "Σφάλμα στην γραμμή " + (i + 1) + ". Η εντολή χρειάζεται περισσότερες παραμέτρους." + "\n";
                }

                if (correct)
                {
                    Debug.Log("Mission Manager: Line " + (i + 1) + " is correct.");
                }


                /*
                if (!string.Equals(compIn, compSo, StringComparison.OrdinalIgnoreCase))//TODO Replace the comaprision here
                {
                    correct = false;
                    //pop up
                    //GameMaster.Instance.GetComponent<PopUpMessagesSender>().Test_FULL("Mission Manager: Error in line " + (i + 1) + ".");
                    Debug.Log("Mission Manager: Error in line " + (i + 1) + ".");
                    popupconcat += "Σφάλμα στην γραμμή " + (i + 1) + "." + "\n";
                }
                else
                {
                    //pop up
                    //  GameMaster.Instance.GetComponent<PopUpMessagesSender>().Test_FULL("Mission Manager: Line " + (i + 1) + " is correct.");

                    Debug.Log("Mission Manager: Line " + (i + 1) + " is correct.");
                }
                */
            }
        }

        bool addedFewerSteps = false;
        if (inp.Length > i + inputOffset)
        {
            for (j = i; j + inputOffset < inp.Length; j++)
            {
                string compIn = Regex.Replace(inp[j + inputOffset], @"s+", String.Empty);
                compIn = Regex.Replace(compIn, " ", String.Empty);
                compIn = compIn.Trim();
                if (String.IsNullOrWhiteSpace(compIn))
                {
                    inputOffset++;
                    j--;
                }
                else
                {
                    correct = false;
                    //pop up
                    //GameMaster.Instance.GetComponent<PopUpMessagesSender>().Test_FULL("Mission Manager: Error in line " + (j + 1) + ". Code requires fewer steps.");
                    //Debug.Log("Mission Manager: Error in line " + (j + 1) + ". Code requires fewer steps.");
                    if (!addedFewerSteps)
                        popupconcat += "Ο κώδικας χρειάζεται λιγότερες εντολές." + "\n";
                    addedFewerSteps = true;
                }
            }
        }

        if (correct)
        {
            //pop up
            // GameMaster.Instance.GetComponent<PopUpMessagesSender>().Test_FULL("Mission Manager: Code is correct");
            Debug.Log("Mission Manager: Code is correct");
            popupconcat += "Ο κώδικας είναι σωστός." + "\n";
            GameMaster.Instance.GetComponent<PlayerData>().ActiveMission.IsCodeChecked = true;
            GameMaster.Instance.GetComponent<PlayerData>().ExecuteEventByName("CodeChecked");

        }

        //GameMaster.Instance.GetComponent<PlayerData>().PlayerLog
        if (correct)
            GameMaster.Instance.GetComponent<PlayerData>().PlayerLog += "[" + GameMaster.Instance.GetComponent<PlayerData>().TimeMemory + "]" + "Mission Check: Code correct. \n";
        else
            GameMaster.Instance.GetComponent<PlayerData>().PlayerLog += "[" + GameMaster.Instance.GetComponent<PlayerData>().TimeMemory + "]" + "Mission Check: Code error. \n";

        GameMaster.Instance.GetComponent<PopUpMessagesSender>().Test_FULL(popupconcat);
    }

#endregion

    #region Execute Final Code

    public string CheckMaterialAvailability()
    {
        PlayerData pd = GameMaster.Instance.GetComponent<PlayerData>();

        int inventoryIndex =-1;
        bool complete = false;
        string ret = "";

        int remaningRequestQuantity = pd.ActiveMission.RequestedQuantity - pd.ActiveMission.CompletedQuantity;
        foreach (InventoryMaterialData mat in pd.InventoryMaterials)
        {
            if (string.Equals(pd.ActiveMission.RequestedMaterialID, mat.ID) && string.Equals(pd.ActiveMission.RequestedMaterialDimensions, mat.MaterialDimensions))
            {
                inventoryIndex = mat.MachineID;
                if (mat.MaterialQuantity < remaningRequestQuantity && mat.MachineID > 0)
                    ret += "Υπάρχουν <color=orange>" + mat.MaterialQuantity + "/" + remaningRequestQuantity + "</color> τεμάχια στην μηχανή. ";
                else if (mat.MachineID == 0)
                    ret += "Υπάρχουν <color=orange>" + mat.MaterialQuantity + "/" + remaningRequestQuantity + "</color> τεμάχια στην αποθήκη. ";
                else
                {
                    //ret += "Υπάρχουν <color=orange>" + mat.MaterialQuantity + "/" + remaningRequestQuantity + "<\\color> τεμάχια στην μηχανή. ";
                    complete = true;
                }
                
            }
        }

        if (complete)
            return null;
        else
            return ret + "Αγόρασε υλικά στο Κατάστημα υλικών.";

    }

    public string CheckToolAvailability()
    {
        PlayerData pd = GameMaster.Instance.GetComponent<PlayerData>();

        int inventoryIndex = -1;
        bool complete = false;
        string ret = "";
        int remaningRequestQuantity = pd.ActiveMission.RequestedQuantity - pd.ActiveMission.CompletedQuantity;
        foreach (RequestedTool rt in pd.ActiveMission.Template.RequestedToolList)
        {
            string toolString = "";
            foreach (InventoryToolData too in pd.InventoryTools)
            { 
                if (string.Equals(rt.RequestedToolID, too.ID) && string.Equals(rt.RequestedToolDimensions, too.ToolDimensions))
                {
                    if (too.MachineID >= inventoryIndex)
                    {
                        inventoryIndex = too.MachineID;
                        if (inventoryIndex > 0)
                        {
                            if (too.ToolDurability > remaningRequestQuantity)
                            {
                                complete = true;
                                toolString = "Το εργαλείο " + too.Template.ToolName + " είναι φορτωμένο στην μηχανή.\n";
                            }
                            else
                            {
                                if (!complete) toolString = "Το εργαλείο " + too.Template.ToolName + " είναι φορτωμένο αλλά χρειάζεται επισκευή.\n";
                            }
                        }
                        else
                        {
                            toolString = "Το εργαλείο " + too.Template.ToolName + " υπάρχει στην αποθήκη.\n";
                        }
                    }

                }
            }
            ret += toolString;
        }

        if (complete)
            return null;
        else
            if (String.IsNullOrEmpty(ret))
                return "Αγόρασε εργαλεία στο Κατάστημα εργαλείων.";
            else
                return ret;

    }

    public void ExecuteCode(int MachineID)
    {
        PlayerData pd = GameMaster.Instance.GetComponent<PlayerData>();

        Debug.Log("Mission Manager: Attempting to execute code.");
        //Check if the Gcode is written
        if (!pd.ActiveMission.IsCodeChecked)
        {
            //pop up
            GameMaster.Instance.GetComponent<PopUpMessagesSender>().Test_FULL("Mission Manager: You have not checked the correctness of the code.");
            Debug.Log("Mission Manager: You have not checked the correctness of the code.");
            return;
        }

        //Check if Machine type can execute the written code type (milling/turning & HAAS/Siemens)
        if (pd.ActiveMission.IsMilling != pd.GetMachineData(pd.ActiveMachine).Template.IsMilling)
        {
            //pop up
            GameMaster.Instance.GetComponent<PopUpMessagesSender>().Test_FULL("Mission Manager: The active machine is not of the requested type (milling/turning).");
            Debug.Log("Mission Manager: The active machine is not of the requested type (milling/turning).");
            return;
        }

        int availableQuantity = 0;
        int availableDurability = 999999999;

        //Check if requested material is in machine AND how much durability is available
        bool IsCorrect = false;
        foreach(InventoryMaterialData mat in pd.InventoryMaterials)
        {
            if(mat.MachineID == MachineID && string.Equals(pd.ActiveMission.RequestedMaterialID,mat.ID) && string.Equals(pd.ActiveMission.RequestedMaterialDimensions,mat.MaterialDimensions))
            {
                IsCorrect = true;
                availableQuantity += mat.MaterialQuantity;
            }
        }
        if (!IsCorrect)
        {
            string message = "Το ζητούμενο υλικό κατεργασίας δεν βρίσκετε στην μηχανή";
        //pop up
        GameMaster.Instance.GetComponent<PopUpMessagesSender>().Test_SimplePopUp(message);
        GameMaster.Instance.GetComponent<PlayerData>().ExecuteEventByName("MaterialLoaded", new GameObject(CheckMaterialAvailability()));
        GameMaster.Instance.GetComponent<PopUpMessagesSender>().Test_ChatBox(message);
            
        Debug.Log(message);

            return;
        }

        //Check if requested tools are in machine AND how much quantity is available
        IsCorrect = false;
        int correctToolQuantity = 0;
        foreach (RequestedTool rt in pd.ActiveMission.Template.RequestedToolList)
        {
            int toolAvailableDurability = 0;
            foreach (InventoryToolData too in pd.InventoryTools)
            {
                if (too.MachineID == MachineID && string.Equals(rt.RequestedToolID, too.ID) && string.Equals(rt.RequestedToolDimensions, too.ToolDimensions))
                {
                    correctToolQuantity++;
                    toolAvailableDurability += too.ToolDurability;
                }
            }
            if (toolAvailableDurability < availableDurability) availableDurability = toolAvailableDurability;
        }
        if (correctToolQuantity == pd.ActiveMission.Template.RequestedToolList.Length) IsCorrect = true;

        if (!IsCorrect)
        {
        //pop up
            string message = "Κάποιο ζητούμενο εργαλείο δεν βρίσκετε στην μηχανή ";
            GameMaster.Instance.GetComponent<PopUpMessagesSender>().Test_ChatBox(message);
            GameMaster.Instance.GetComponent<PlayerData>().ExecuteEventByName("ToolLoaded", new GameObject(CheckToolAvailability()));
            GameMaster.Instance.GetComponent<PopUpMessagesSender>().Test_SimplePopUp(message);//this pop 

        Debug.Log(message);
            return;
        }

        //Do the pre-execution
        int PossibleCuts = Mathf.Min(availableDurability, availableQuantity, (pd.ActiveMission.RequestedQuantity - pd.ActiveMission.CompletedQuantity));
        float delay = PossibleCuts * DelayTimePerObject;
        float shownDelay = pd.ActiveMission.RequestedQuantity * DelayTimePerObject;

        //Save into the Active Mission Data
        pd.ActiveMission.IsBeingExecuted = true;
        pd.ActiveMission.Delay = delay;
        pd.ActiveMission.ShownDelay = shownDelay;
        pd.ActiveMission.ExecutingMachine = MachineID;
        pd.ActiveMission.ExecutionCarveQuantity = PossibleCuts;


    //Fire the event
    pd.ExecuteEventByName("ExecutionStarting");

        //Invoke the post-execution
        Debug.Log("Mission manager: The setup is complete. The mission will complete in " + delay + " seconds.");
        Invoke("PostExecuteCode", delay);

    }

    public void PostExecuteCode()
    {
        PlayerData pd = GameMaster.Instance.GetComponent<PlayerData>();

        //Find all useful tools in the machine
        Debug.Log(pd.ActiveMission.Template.RequestedToolList.Length);
        IntList[] toUseTools = new IntList[pd.ActiveMission.Template.RequestedToolList.Length];
        for(int j = 0; j < toUseTools.Length; j++)
        {
            toUseTools[j] = new IntList();
            toUseTools[j].data = new List<int>();
        }
        int rtQuantity = 0;
        foreach (RequestedTool rt in pd.ActiveMission.Template.RequestedToolList)
        {
            foreach (InventoryToolData too in pd.InventoryTools)
            {
                if (too.MachineID == pd.ActiveMission.ExecutingMachine && string.Equals(rt.RequestedToolID, too.ID) && string.Equals(rt.RequestedToolDimensions, too.ToolDimensions))
                {
                    toUseTools[rtQuantity].data.Add(too.UniqueCode);
                }
            }
            rtQuantity++;
        }
        //Carve objects
        int terminalCondition = 0;
        int leftoverCuts = pd.ActiveMission.RequestedQuantity - pd.ActiveMission.CompletedQuantity;

        int possibleCuts = 999999999;
        //Check the tool with the minimum leftover quantity
        int[] availableQuantityPerTool = new int[pd.ActiveMission.Template.RequestedToolList.Length];
        for (int i = 0; i < availableQuantityPerTool.Length; i++)
        {
            foreach(int j in toUseTools[i].data)
            {
                availableQuantityPerTool[i] += pd.GetTool(j).ToolDurability;
            }
            if (availableQuantityPerTool[i] < possibleCuts) { possibleCuts = availableQuantityPerTool[i]; terminalCondition = 1; }
        }

        //Check if the materials are fewer than the tools
        if (pd.GetMaterial(pd.ActiveMission.ExecutingMachine).MaterialQuantity < possibleCuts) { possibleCuts = pd.GetMaterial(pd.ActiveMission.ExecutingMachine).MaterialQuantity; terminalCondition = 2; }

        //now check if the minimum of all is enough to finish the mission
        if (possibleCuts > leftoverCuts) possibleCuts = leftoverCuts; terminalCondition = 0;

        //Reduce the tool durabilities
        for(int i=0; i < toUseTools.Length; i++)
        {
            int leftoverCutsPerTool = possibleCuts;
            for(int j=0; j> toUseTools[i].data.Count; j++)
            {
                int toolDura = pd.GetTool(toUseTools[i].data[j]).ToolDurability;
                if (toolDura > leftoverCutsPerTool)
                {
                    pd.ReduceToolDurability(toUseTools[i].data[j], leftoverCutsPerTool);
                    leftoverCutsPerTool = 0;
                    break;
                }
                else
                {
                    pd.ReduceToolDurability(toUseTools[i].data[j], toolDura);
                    leftoverCutsPerTool -= toolDura;
                } 
            }
        }

        //reduce material quantity
        pd.ReduceQuantity(pd.ActiveMission.ExecutingMachine, possibleCuts);
        pd.AddMoney(pd.ActiveMission.RewardGoldPerItem * possibleCuts);
        pd.ActiveMission.CompletedQuantity += possibleCuts;

        /*
        foreach (int toolIndex in toUseTools)
        {
            
            int currentCuts = Mathf.Min(pd.GetTool(toolIndex).ToolDurability, pd.GetMaterial(pd.ActiveMission.ExecutingMachine).MaterialQuantity, leftoverCuts);

            if (currentCuts == pd.GetTool(toolIndex).ToolDurability) terminalCondition = 1; //not enough durability
            else if (currentCuts == pd.GetMaterial(pd.ActiveMission.ExecutingMachine).MaterialQuantity) terminalCondition = 2; //not enough materials
            else terminalCondition = 0; //the mission is complete

            pd.ReduceQuantity(pd.ActiveMission.ExecutingMachine, currentCuts);
            pd.ReduceToolDurability(toolIndex, currentCuts);
            pd.AddMoney(pd.ActiveMission.RewardGoldPerItem * currentCuts);
            pd.ActiveMission.CompletedQuantity += currentCuts;

        }
        */
        //Re-calibrate the Active mission data
        pd.ActiveMission.IsBeingExecuted = false;

        pd.ExecuteEventByName("ExecutionFinishing");

        //If total completion
        if (pd.ActiveMission.CompletedQuantity == pd.ActiveMission.RequestedQuantity)
        {
        //Debug.Log("Mission Manager: The mission is complete.");
        GameMaster.Instance.GetComponent<PlayerData>().PlayerLog += "[" + GameMaster.Instance.GetComponent<PlayerData>().TimeMemory + "]" + "Mission Complete: The Mission" + pd.ActiveMission.MissionIndex + " is complete. \n";
        pd.AddExp(pd.ActiveMission.RewardExpPerMission);
            
            //Add +1 to times completed per mission
            if (pd.ActiveMission.IsMilling) pd.TimesCompletedPerMissionMilling[pd.ActiveMission.MissionIndex]++;
            else pd.TimesCompletedPerMissionTurning[pd.ActiveMission.MissionIndex]++;

            pd.ActiveMission.MissionIndex = -1;
            //popyp for full completion
            GetNewMissions();
            pd.ExecuteEventByName("NewMissionsGenerated");
        }
        //If partial completion
        else
        {
            Debug.Log("Mission Manager: The mission is partially complete; further attention is required.");
            //popup for partial completion
        }
    }

    #endregion

}


public class IntList
{
    public List<int> data;
}