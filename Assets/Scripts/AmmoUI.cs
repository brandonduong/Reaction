using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AmmoUI : MonoBehaviour
{
    private Text ammoText;
    private GunManager gunManager;

    private void Start()
    {
        ammoText = GetComponent<Text>();
        gunManager = FindObjectOfType<GunManager>();
    }

    // Update is called once per frame
    void Update()
    {
        if (gunManager.currentGun != null)
        {
            ammoText.text = gunManager.currentGun.CurrentAmmo.ToString() + "/" + gunManager.currentGun.MaxAmmo.ToString();
        }
        else
        {
            ammoText.text = "";
        }
    }
}
