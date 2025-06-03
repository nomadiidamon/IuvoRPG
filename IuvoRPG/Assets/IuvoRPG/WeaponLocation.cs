using System.Runtime.InteropServices.WindowsRuntime;
using UnityEditor.Rendering;
using UnityEngine;

public class WeaponLocation : MonoBehaviour
{
    [SerializeField] private Transform weaponLocation;
    [SerializeField] private GameObject weapon;

    [SerializeField] public bool isWeaponVisible = false;

    void Start()
    {
        if (weaponLocation == null)
        {
            Debug.Log("Weapon position cannot be null");
        }
        if (weapon == null)
        {
            Debug.Log("Weapon cannot be null");
        }
    }

    void Update()
    {
        if (isWeaponVisible)
        {
            weapon.SetActive(true);
        }
        else { weapon.SetActive(false); }
    }

    #region Getters & Setters
    public void SetWeaponLocation(Transform newLocation) => weaponLocation = newLocation;
    public void SetWeaponVisibility(bool active) => isWeaponVisible = active;
    public void SetWeapon(GameObject newWeapon) => weapon = newWeapon;
    public GameObject GetWeapon() => weapon;
    public Transform GetWeaponLocation() => weaponLocation;





    #endregion
}
