using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookAtCursor : MonoBehaviour
{
    // Start is called before the first frame update

    bool IsFacingRight = true;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 mouseScreenPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 direction = (mouseScreenPosition - (Vector2) transform.position).normalized;
        transform.right = direction;
        if(IsFacingRight && direction.x < 0 || !IsFacingRight && direction.x > 0)
        {
            Turn();
        }
    }

    void Turn()
    {
		Vector3 scale = transform.localScale; 
		scale.y *= -1;
		transform.localScale = scale;
 
		IsFacingRight = !IsFacingRight;
    }
}
