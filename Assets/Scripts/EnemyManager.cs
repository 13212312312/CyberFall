using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] GameObject enemyHolder;
    [SerializeField] GameObject[] enemiesPrefabs;
    [SerializeField] public int money;
    private int currentMoney;

    public GameObject GenerateEnemies()
    {
        currentMoney = money;
        var enemyHolderDup = Instantiate(enemyHolder,enemyHolder.transform.position, Quaternion.identity);
        while(verifyIfICanBuy())
        {
            var chosenEnemy = Random.Range(0,enemiesPrefabs.Length);
            var InstantiatedEnemy = Instantiate(enemiesPrefabs[chosenEnemy],new Vector3(0,0,0), Quaternion.identity);
            InstantiatedEnemy.transform.parent = enemyHolderDup.transform;
            currentMoney = currentMoney - enemiesPrefabs[chosenEnemy].GetComponent<EnemyPrice>().GetPrice();
        }
        return enemyHolderDup;
    }

    bool verifyIfICanBuy()
    {
        foreach( var enemy in enemiesPrefabs)
        {
            if(enemy.GetComponent<EnemyPrice>().GetPrice() <= currentMoney)
            {
                return true;
            }
        }
        return false;
    }
}
