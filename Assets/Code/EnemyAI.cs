using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAI : MonoBehaviour
{
    Transform player;
    public float speed = 2f;
    Rigidbody2D _rigidbody;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        _rigidbody = GetComponent<Rigidbody2D>();
    }

    void FixedUpdate()
    {
        if (player && player.position.x > transform.position.x ||
        player.position.x < transform.position.x)
        {
            transform.localScale *= new Vector2(-1, 1);
        }
        Vector3 direction = (player.position - transform.position).normalized;
        _rigidbody.velocity = direction * speed;
        //transform.Translate(direction  * speed * Time.deltaTime);
    }
}
