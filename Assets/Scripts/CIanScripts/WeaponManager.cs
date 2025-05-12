using UnityEngine;

public class WeaponManager : MonoBehaviour
{
    [Header("Weapon Prefabs")]
    public GameObject[] weaponPrefabs;

    [Header("References")]
    public Camera playerCamera;

    [Header("Weapon UI Canvases")]
    public GameObject pistolUI;
    public GameObject crossbowUI;
    public GameObject syringerUI;

    [Header("UI Tutorial Popup")]
    public GameObject WeaponswapTutorial;

    private BaseWeapon currentWeapon;
    private SpriteAnimatorUI currentWeaponAnimator;

    void Start()
    {
        EquipWeapon(0);
        WeaponswapTutorial.SetActive(true);
        // wire the pistol animator and reset to frame 0
        currentWeaponAnimator = pistolUI.GetComponent<SpriteAnimatorUI>();
        currentWeaponAnimator.ResetToFirstFrame();
    }

    void Update()
    {
        // remember if the player is holding Fire1 at the moment of swap
        bool holdingFire = Input.GetButton("Fire1");

        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            EquipWeapon(0);
            pistolUI.SetActive(true);
            crossbowUI.SetActive(false);
            syringerUI.SetActive(false);
            currentWeaponAnimator = pistolUI.GetComponent<SpriteAnimatorUI>();
            WeaponswapTutorial.SetActive(false);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            EquipWeapon(1);
            pistolUI.SetActive(false);
            crossbowUI.SetActive(true);
            syringerUI.SetActive(false);
            currentWeaponAnimator = crossbowUI.GetComponent<SpriteAnimatorUI>();
            WeaponswapTutorial.SetActive(false);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            EquipWeapon(2);
            pistolUI.SetActive(false);
            crossbowUI.SetActive(false);
            syringerUI.SetActive(true);
            currentWeaponAnimator = syringerUI.GetComponent<SpriteAnimatorUI>();
            WeaponswapTutorial.SetActive(false);
        }

        // every time we swap, reset the new UI to its first frame
        if (Input.GetKeyDown(KeyCode.Alpha1) ||
            Input.GetKeyDown(KeyCode.Alpha2) ||
            Input.GetKeyDown(KeyCode.Alpha3))
        {
            currentWeaponAnimator.ResetToFirstFrame();

            // if we were already holding Fire1, replay the shoot animation
            if (holdingFire)
                currentWeaponAnimator.PlayShootAnimation();
        }

        // on *new* click, fire + animate
        if (Input.GetButtonDown("Fire1") && currentWeapon != null)
        {
            currentWeapon.Fire();
            currentWeaponAnimator.PlayShootAnimation();
        }
    }

    void EquipWeapon(int index)
    {
        if (index < 0 || index >= weaponPrefabs.Length) return;

        if (currentWeapon != null)
            Destroy(currentWeapon.gameObject);

        var weaponGO = Instantiate(
            weaponPrefabs[index],
            transform.position,
            transform.rotation
        );
        currentWeapon = weaponGO.GetComponent<BaseWeapon>();
        currentWeapon.playerCamera = playerCamera;
    }
}
