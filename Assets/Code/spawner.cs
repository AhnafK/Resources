using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class spawner : MonoBehaviour
{

    public List<food> foodList;
    public List<EnemyAI> enemyList;
    public int foodCount = 5;
    public int enemyCount = 3;


    // Update is called once per frame
    void FixedUpdate()
    {
        // if no food in scene, spawn food randomly
        if (GameObject.FindGameObjectsWithTag("Food").Length == 0)
        {
            for(int i = 0; i < foodCount; i++){
                int rand = Random.Range(0, foodList.Count);
                food spawned = Instantiate(foodList[rand], new Vector3(Random.Range(-20, 20), Random.Range(-11, 11), 0), Quaternion.identity);
                //turn it into a trigger after 0.2 second delay
                StartCoroutine(TurnTrigger(spawned));

            }
        }
        // if no enemies in scene, spawn enemies randomly
        if (GameObject.FindGameObjectsWithTag("Enemy").Length == 0)
        {
            for(int i = 0; i < enemyCount; i++){
                int rand = Random.Range(0, enemyList.Count);
                Instantiate(enemyList[rand], new Vector3(Random.Range(-20, 20), Random.Range(-11, 11), 0), Quaternion.identity);
            }
            enemyCount++;
        }
    }

    IEnumerator TurnTrigger(food spawned){
        yield return new WaitForSeconds(0.1f);
        spawned.GetComponent<Collider2D>().isTrigger = true;
    }
}
