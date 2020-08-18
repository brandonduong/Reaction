using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingPlatform : MonoBehaviour
{
    public Transform[] points;
    public float speed;

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
        for (int i = 0; i < points.Length; i++)
        {
            Debug.Log(transform.position);
            if (transform.position == points[i].position)
            {
                // If reach end of path, go backwards
                if ((i == points.Length - 1) || (i == 0))
                {
                    Debug.Log("switch direction");
                    forwards = !forwards;
                }

                if (forwards)
                {
                    nextPoint = points[i + 1].position;
                }
                else
                {
                    nextPoint = points[i - 1].position;
                }
            }
        }
        transform.position = Vector3.MoveTowards(transform.position, nextPoint, speed * Time.deltaTime);
    }

    private void OnDrawGizmos()
    {
        for (int i = 0; i < points.Length - 1; i++)
        {
            Gizmos.DrawLine(points[i].position, points[i + 1].position);
        }
    }
}
