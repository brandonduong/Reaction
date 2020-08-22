using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingPlatform : MonoBehaviour
{
    public Transform[] points;
    public float speed;
    public bool waitForPlayer = false;
    public bool startWhenPlayerAhead = false;
    public float startWhenPlayerAheadBy = 0f;
    public bool stopAtEnd = false;
    public GameObject player;

    private Vector3 nextPoint;
    private bool forwards = true;

    // Start is called before the first frame update
    void Start()
    {
        nextPoint = points[0].position;
    }

    // Update is called once per frame
    void Update()
    {
        // Start when player is ahead
        if (waitForPlayer && startWhenPlayerAhead && player != null && (player.transform.position.x - transform.position.x) > startWhenPlayerAheadBy)
        {
            waitForPlayer = false;
        }

        for (int i = 0; i < points.Length; i++)
        {
            // Debug.Log(transform.position);
            if (transform.position == points[i].position)
            {
                // If reach end of path, go backwards
                if (!stopAtEnd && (i == points.Length - 1))
                {
                    // Debug.Log("switch direction");
                    forwards = false;
                }
                else if (i == 0)
                {
                    forwards = true;
                }

                if (forwards)
                {
                    // If at the end and must stop at end, stop
                    if (i == points.Length - 1 && stopAtEnd)
                    {
                        nextPoint = points[i].position;
                    }
                    else
                    {
                        nextPoint = points[i + 1].position;
                    }
                }
                else
                {
                    nextPoint = points[i - 1].position;
                }
            }
        }

        if (!waitForPlayer)
        {
            transform.position = Vector3.MoveTowards(transform.position, nextPoint, speed * Time.deltaTime);
        }
    }

    private void OnDrawGizmos()
    {
        for (int i = 0; i < points.Length - 1; i++)
        {
            Gizmos.DrawLine(points[i].position, points[i + 1].position);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // Start if player on platform
        if (collision.gameObject.tag == "Player")
        {
            waitForPlayer = false;
        }
    }
}
