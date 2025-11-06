using TMPro;
using UnityEngine;

//Attribute which allows right click->Create
[CreateAssetMenu(fileName = "New Item", menuName = "Items/New Item")]
public class Item : ScriptableObject //Extending SO allows us to have an object which exists in the project, not in the scene
{

    public Sprite icon;
    public string description = "";
    public bool isConsumable = false;

    public int width = 1;
    public int height = 1;

    /*    public void Use()
        {
            Debug.Log("Used item: " + name + " - " + description);
        }*/
    public void Use(TextMeshProUGUI nameText, TextMeshProUGUI descriptionText)
    {
        if (nameText != null)
            nameText.text = name;

        if (descriptionText != null)
            descriptionText.text = description;
    }

}
