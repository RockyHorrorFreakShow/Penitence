using UnityEngine;

public class WeaponManager : MonoBehaviour
{
    public GameObject[] weaponPrefabs;
    public Camera playerCamera;

    public GameObject pistolUI;
    public GameObject crossbowUI;
    public GameObject syringerUI;

    private BaseWeapon currentWeapon;

    void Start()
    {
        EquipWeapon(0);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            EquipWeapon(0);
            pistolUI.SetActive(true);
            crossbowUI.SetActive(false);
            syringerUI.SetActive(false);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            EquipWeapon(1);
            pistolUI.SetActive(false);
            crossbowUI.SetActive(true);
            syringerUI.SetActive(false);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            EquipWeapon(2);
            pistolUI.SetActive(false);
            crossbowUI.SetActive(false);
            syringerUI.SetActive(true);
        }

        if (Input.GetButtonDown("Fire1") && currentWeapon != null)
        {
            currentWeapon.Fire();
        }
    }

    void EquipWeapon(int index)
    {
        if (index < 0 || index >= weaponPrefabs.Length) return;

        if (currentWeapon != null)
        {
            Destroy(currentWeapon.gameObject);
        }

        GameObject weaponGO = Instantiate(weaponPrefabs[index], transform.position, transform.rotation);
        currentWeapon = weaponGO.GetComponent<BaseWeapon>();
        currentWeapon.playerCamera = playerCamera;
    }
}
