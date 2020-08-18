using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GunNameUI : MonoBehaviour
{
    private Text gunNameText;
    private GunManager gunManager;

    private void Start()
    {
        gunNameText = GetComponent<Text>();
        gunManager = FindObjectOfType<GunManager>();
    }

    // Update is called once per frame
    void Update()
    {
        // Get name of current gun
        gunNameText.text = gunManager.currentGun.GetType().Name;
    }
}
