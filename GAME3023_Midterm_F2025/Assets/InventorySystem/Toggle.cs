using UnityEngine;

public class Toggle : MonoBehaviour
{
    public GameObject Inventory;
    public GameObject Weapon;
    public GameObject Armor;



    public void OpenWeapon()
    {
        Inventory.SetActive(false);
        Weapon.SetActive(true);
        Armor.SetActive(false);

    }
    public void OpenArmor()
    {
        Inventory.SetActive(false);
        Weapon.SetActive(false);
        Armor.SetActive(true);

    }
    public void OpenInventory()
    {
        Inventory.SetActive(true);
        Weapon.SetActive(false);
        Armor.SetActive(false);

    }
}
