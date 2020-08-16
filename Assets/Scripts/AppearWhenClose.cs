using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AppearWhenClose : MonoBehaviour
{
    private Animator animator;
    public GameObject player;
    private RectTransform rect;
    public float range;

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        rect = GetComponent<RectTransform>();
    }

    // Update is called once per frame
    void Update()
    {
        if (player)
        {
            if (player.GetComponent<Rigidbody2D>().position.x <= rect.position.x + range
            && player.GetComponent<Rigidbody2D>().position.x >= rect.position.x - range)
            {
                animator.SetBool("visible", true);
            }
            else
            {
                animator.SetBool("visible", false);
            }
        }
    }
}
