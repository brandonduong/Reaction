using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AmmoUI : MonoBehaviour
{
    private Text ammoText;

    private GunManager weapon;

    private void Start()
    {
        ammoText = GetComponent<Text>();
        weapon = FindObjectOfType<GunManager>();
    }

    // Update is called once per frame
    void Update()
    {
        ammoText.text = weapon.currentGun.CurrentAmmo.ToString() + "/" + weapon.currentGun.MaxAmmo.ToString();
    }
}
