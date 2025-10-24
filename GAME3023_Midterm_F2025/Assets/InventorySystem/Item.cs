using TMPro;
using UnityEngine;

//Attribute which allows right click->Create
[CreateAssetMenu(fileName = "New Item", menuName = "Items/New Item")]
public class Item : ScriptableObject //Extending SO allows us to have an object which exists in the project, not in the scene
{
    public Sprite icon;
    public string description = "";
    public bool isConsumable = false;

    [Header("Stat Effects")]
    public int hp = 0;
    public int mp = 0;
    public int str = 0;
    public int dex = 0;
    public int intel = 0;
    public int def = 0;
    public int stamina = 0;

    [Header("UI References")]
    public TextMeshProUGUI hpText;
    public TextMeshProUGUI mpText;
    public TextMeshProUGUI strText;
    public TextMeshProUGUI dexText;
    public TextMeshProUGUI intText;
    public TextMeshProUGUI defText;
    public TextMeshProUGUI staminaText;

    [Header("Current Stats")]
    public int currentHP = 100;
    public int currentMP = 50;
    public int currentSTR = 10;
    public int currentDEX = 10;
    public int currentINT = 10;
    public int currentDEF = 5;
    public int currentSTA = 20;

    public void Use()
    {



        Debug.Log("Used item: " + name + " - " + description);
    }
}
