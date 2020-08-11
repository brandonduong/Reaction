using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AmmoUI : MonoBehaviour
{
    private Text ammoText;

    private Weapon weapon;

    private void Start()
    {
        ammoText = GetComponent<Text>();
        weapon = FindObjectOfType<Weapon>();
    }

    // Update is called once per frame
    void Update()
    {
        ammoText.text = weapon.currentAmmo.ToString() + "/" + weapon.maxAmmo.ToString();
    }
}
