using System.Runtime.CompilerServices;
using UnityEngine;

public class WeaponManager : MonoBehaviour
{
    [Header("Weapon Prefabs")]
    public GameObject[] weaponPrefabs; 
    public Camera playerCamera;

    [Header("Weapon UI Canvases")]
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
        // Weapon switch input
        if (Input.GetKeyDown(KeyCode.Alpha1)) EquipWeapon(0);
        else if (Input.GetKeyDown(KeyCode.Alpha2)) EquipWeapon(1);
        else if (Input.GetKeyDown(KeyCode.Alpha3)) EquipWeapon(2);

        // Fire and animate
        if (Input.GetButtonDown("Fire1") && currentWeapon != null)
        {
            currentWeapon.Fire();
        }
    }

    void EquipWeapon(int index)
    {
        if (index < 0 || index >= weaponPrefabs.Length) return;

        if (currentWeapon != null)
            Destroy(currentWeapon.gameObject);

        // Instantiate new
        var weaponGO = Instantiate(weaponPrefabs[index], transform.position, transform.rotation);
        currentWeapon = weaponGO.GetComponent<BaseWeapon>();
        currentWeapon.playerCamera = playerCamera;
        currentWeapon.gameObject.SetActive(true);

        pistolUI.SetActive(false);
        crossbowUI.SetActive(false);
        syringerUI.SetActive(false);

        switch (index)
        {
            case 0:
                pistolUI.SetActive(true);
                break;
            case 1:
                crossbowUI.SetActive(true);
                break;
            case 2:
                syringerUI.SetActive(true);
                break;
            default:
                break;
        }
    }
}

