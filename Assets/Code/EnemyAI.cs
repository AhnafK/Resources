using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAI : MonoBehaviour
{
    Transform player;
    public float speed = 2f;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    void Update()
    {
        if (player.position.x > transform.position.x ||
        player.position.x < transform.position.x)
        {
            transform.localScale *= new Vector2(-1, 1);
        }
        Vector3 direction = (player.position - transform.position).normalized;
        transform.Translate(direction  * speed * Time.deltaTime);
    }
}
