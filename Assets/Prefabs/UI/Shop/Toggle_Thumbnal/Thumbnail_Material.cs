using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class Thumbnail_Material : MonoBehaviour
{

    [Header("Thumbail Properties")]
    // public string Name = "ThumbnailName";
    public MaterialID ID;
  

    private MaterialTemplate material;
    

    [Header("Thumbnails Fields")]
    [SerializeField]
    private TextMeshProUGUI ThumbNail_MetalType;
    [SerializeField]
    private TextMeshProUGUI ThumbNail_ItemPrice;
    [SerializeField]
    private Image ThumbNail_ItemImage;

    [Header("Master Parent")]
    [SerializeField]
    private GameObject Material_Spec_View;
    // Start is called before the first frame update

    void Start()
    {
        Init();
    }

    public MaterialTemplate GetMaterial() {
        return material;
    }

    private MaterialTemplate RetriveID(string ID)
    {

        for (int i = 0; i < MachineShopData.Materials.Array.Length; i++)
        {
            if (MachineShopData.Materials.Array[i].ID == ID.ToString())
            {
                return MachineShopData.Materials.Array[i];
            }
        }

        return null;
    }
    // Update is called once per frame
    public void Init()
    {
        
        material = RetriveID(ID.ToString()); //Must not return Null. 

        //set the UI content
        if (material != null)
        {
            ThumbNail_MetalType.text = material.MetalType;
            ThumbNail_ItemPrice.text = material.Price.ToString();

            if (material.ItemImage != null)
                ThumbNail_ItemImage.sprite = material.ItemImage;
        }
    }
    public void ShowSpecs() {
            if (material != null)
                Material_Spec_View.GetComponent<Material_Spec_View>().Init(material);
      
    }
   
    public enum MaterialID
    {
        Material1, Material2, Material3, Material4, Material5, Material6, Material7, Material8, Material9, Material10
    }

}
