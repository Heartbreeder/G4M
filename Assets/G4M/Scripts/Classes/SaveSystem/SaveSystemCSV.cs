using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Globalization;

/*
public class SaveSystemCSV : MonoBehaviour
{
    public List<PlayerListNode> Players;

    private void Awake()
    {
        Players = new List<PlayerListNode>();
        SaveManagerCSV.LoadPlayers();

    }

    private void OnApplicationQuit()
    {
        SaveManagerCSV.SavePlayers(Players);
    }
}*/

public class PlayerListNode
{
    public string ID;
    public string Name;
    public string YOB;
    public bool ADHD;
    public int CurrentLevel;

    public PlayerListNode (string iD, string name, string yOB, bool aDHD, int currentLevel)
    {
        ID = iD;
        Name = name;
        YOB = yOB;
        ADHD = aDHD;
        CurrentLevel = currentLevel;
    }
}



public static class SaveManagerCSV
{
    public static void SavePlayers(List<PlayerListNode> playerList)
    {
        string fileName = "Players.csv";

        if (!Directory.Exists(Application.persistentDataPath + "/Saves/"))
        {
            Directory.CreateDirectory(Application.persistentDataPath + "/Saves/");
        }
        
        string path = Application.persistentDataPath + "/Saves/" + fileName;

        if (File.Exists(path))
        {
            File.Delete(path);
        }

        StreamWriter stream = new StreamWriter(path);

        foreach (PlayerListNode line in playerList)
        {
            //serialise
            string entry = line.ID + "," + line.Name + "," + line.YOB + "," + line.ADHD + "," + line.CurrentLevel;
            stream.WriteLine(entry);
        }

        stream.Close();
    }

    public static List<PlayerListNode> LoadPlayers()
    {
        string fileName = "Players.csv";

        List<PlayerListNode> ret = new List<PlayerListNode>();

        if (!Directory.Exists(Application.persistentDataPath + "/Saves/"))
        {
            Directory.CreateDirectory(Application.persistentDataPath + "/Saves/");
        }

        string path = Application.persistentDataPath + "/Saves/" + fileName;

        if (File.Exists(path))
        {
            //read file
            StreamReader stream = new StreamReader(path);


            while (!stream.EndOfStream)
            {
                string line = stream.ReadLine();
                //deserialsie
                string[] words = line.Split(',');
                PlayerListNode node = new PlayerListNode(words[0], words[1], words[2], bool.Parse(words[3]), int.Parse(words[4]));
                ret.Add(node);
            }

            stream.Close();

            return ret;

        }
        else
        {
            Debug.Log("Options file not found.");
            return null;
        }

    }
}