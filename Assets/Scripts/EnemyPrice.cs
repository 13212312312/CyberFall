using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyPrice : MonoBehaviour
{
    [SerializeField] public int price;
    
    public int GetPrice()
    {
        return price;
    }
}
