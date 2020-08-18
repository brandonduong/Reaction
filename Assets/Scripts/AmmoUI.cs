using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AmmoUI : MonoBehaviour
{
    private Text ammoText;

    private WeaponHandling weapon;

    private void Start()
    {
        ammoText = GetComponent<Text>();
        weapon = FindObjectOfType<WeaponHandling>();
    }

    // Update is called once per frame
    void Update()
    {
        ammoText.text = weapon.currentGun.CurrentAmmo.ToString() + "/" + weapon.currentGun.MaxAmmo.ToString();
    }
}
