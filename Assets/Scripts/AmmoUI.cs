using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AmmoUI : MonoBehaviour
{
    private Text ammoText;

    [SerializeField] private GameObject player;

    private void Start()
    {
        ammoText = GetComponent<Text>();
    }

    // Update is called once per frame
    void Update()
    {
        ammoText.text = player.GetComponent<Weapon>().currentAmmo.ToString() + "/" + player.GetComponent<Weapon>().maxAmmo.ToString();
    }
}
