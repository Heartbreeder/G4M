using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ToolTemplate
{
    public string ID;
    public string ToolName;
    public string Category;
    public string[] Dimensions;
    public string Decription;
    public int Price;
    public Sprite ItemImage;
    public int Durability;
    public int RepairCost;

    public ToolTemplate() { }

    public ToolTemplate(string iD, string toolName, string category, string[] dimensions, string decription, int price, Sprite itemImage, int repairCost)
    {
        ID = iD;
        ToolName = toolName;
        Category = category;
        Dimensions = dimensions;
        Decription = decription;
        Price = price;
        ItemImage = itemImage;
        RepairCost = repairCost;
    }
}
