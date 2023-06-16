using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pulsing : MonoBehaviour
{
    [SerializeField] public float maxSize;
    [SerializeField] public float minSize;
    [SerializeField] public float speed;
    private int direction = 1;
    private float currentMultiplier;
    private Vector3 size;
    // Start is called before the first frame update
    void Start()
    {
        size = transform.localScale;
        currentMultiplier = minSize;
    }

    // Update is called once per frame
    void Update()
    {
        currentMultiplier = currentMultiplier + direction * speed * Time.deltaTime;
        if(currentMultiplier > maxSize || currentMultiplier < minSize)
        {
            direction = - direction;
        }
        Vector3 scale = size;
        scale.x *= currentMultiplier;
        scale.y *= currentMultiplier;
        transform.localScale = scale;
    }
}
