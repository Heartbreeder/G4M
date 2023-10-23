#if false
#region Check Code Solution

using System;
using System.Collections.Generic;

public void CheckSolution(string input)
{

    double lastXValue, lastYValue, lastZValue;
    string lastCommand;
    lastXValue = double.NegativeInfinity;
    lastYValue = double.NegativeInfinity;
    lastZValue = double.NegativeInfinity;

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
    string[] commandSplit;
    foreach (string line in topSplit)
    {
        commandSplit = line.Split(' ');
        foreach (string param in commandSplit)
        {
            if (param.ElementAt<char>(0) == 'X')
            {
                lastXValue = Double.Parse(param.Substring(1));
            }
            else if (param.ElementAt<char>(0) == 'Y')
            {
                lastYValue = Double.Parse(param.Substring(1));
            }
            else if (param.ElementAt<char>(0) == 'Z')
            {
                lastZValue = Double.Parse(param.Substring(1));
            }
        }
    }

    //Save last Command
    if (commandSplit.Length > 0)
    {
        lastCommand = commandSplit[commandSplit.Length - 1].Split(' ');
        if (lastCommand.Length > 0)
            lastCommand = lastCommand[0];
        else
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
            while (j < solParamList.Length)
            {
                if (!string.Equals(solParamList[j], inpParamList[k], StringComparison.OrdinalIgnoreCase))
                {

                    if ((solParamList[j].ElementAt<char>(0) == 'X' && lastXValue != Double.Parse(solParamList[j].Substring(1))) ||
                        (solParamList[j].ElementAt<char>(0) == 'Y' && lastYValue != Double.Parse(solParamList[j].Substring(1))) ||
                        (solParamList[j].ElementAt<char>(0) == 'Z' && lastZValue != Double.Parse(solParamList[j].Substring(1))))
                    {
                        correct = false;
                        popupconcat += "Σφάλμα στην γραμμή " + (i + 1) + ". Λάθος Συντεταγμένες." + "\n";
                    }

                    k--;
                }
                else
                {

                    if (inpParamList[k].ElementAt<char>(0) == 'X')
                    {
                        lastXValue = Double.Parse(inpParamList[k].Substring(1));
                    }
                    else if (inpParamList[k].ElementAt<char>(0) == 'Y')
                    {
                        lastYValue = Double.Parse(inpParamList[k].Substring(1));
                    }
                    else if (inpParamList[k].ElementAt<char>(0) == 'Z')
                    {
                        lastZValue = Double.Parse(inpParamList[k].Substring(1));
                    }
                }

                j++;
                k++;
            }

            if (k < inpParamList.Length)
            {
                correct = false;
                popupconcat += "Σφάλμα στην γραμμή " + (i + 1) + ". Η εντολή περιέχει περιττούς χαρακτήρες." + "\n";
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
        for (int j = i; j + inputOffset < inp.Length; j++)
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

#endregion
#endif