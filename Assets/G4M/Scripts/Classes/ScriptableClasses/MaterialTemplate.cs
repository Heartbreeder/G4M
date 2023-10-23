using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class MaterialTemplate
{
    public string ID;
    public string MetalType;
    public string Category;
    public string[] Dimensions;
    public string Decription;
    public int Price;
    public Sprite ItemImage;


    public MaterialTemplate() { }
    public MaterialTemplate(string iD, string metalType, string category, string[] dimensions, string decription, int price, Sprite itemImage)
    {
        ID = iD;
        MetalType = metalType;
        Category = category;
        Dimensions = dimensions;
        Decription = decription;
        Price = price;
        ItemImage = itemImage;
    }
}
