using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Path : MonoBehaviour
{
    Vector3 lastPosition;
    bool first = true;
    [SerializeField] public float duration;
    void Start()
    {
        lastPosition = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        Debug.DrawLine (transform.position, lastPosition, Color.yellow, duration, false);
        lastPosition = transform.position;
    }
}
