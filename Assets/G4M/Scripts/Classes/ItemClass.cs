using UnityEngine;
[System.Serializable]
public class ItemClass {

public string Category { get; set; }
public string[] Dimensions { get; set; }
public string Decription { get; set; }
public int Price { get; set; }
public Sprite ItemImage { get; set; }



    public ItemClass(string category, string[] dimensions, string decription, int price, Sprite itemImage)
    {
       
        Category = category;
        Dimensions = dimensions;
        Decription = decription;
        Price = price;
        ItemImage = itemImage;
    }
}

[System.Serializable]
public class MaterialClass : ItemClass
{
    
    public string ID;
    public string MetalType { get; set; }
    public MaterialClass(string iD,string metalType, string category, string[] dimensions, string decription, int price, Sprite itemImage) : base(category, dimensions, decription, price, itemImage)
    {
        ID = iD;
        MetalType = metalType;
    }

   

}

[System.Serializable]
public class ToolClass : ItemClass
{
    
    public string ID { get; set; }
    public string ToolName { get; set; }

    public ToolClass(string id, string toolName, string category, string[] dimensions, string decription, int price, Sprite itemImage) : base(category, dimensions, decription, price, itemImage)
    {
        ID = id;
        ToolName = toolName;
    }



}