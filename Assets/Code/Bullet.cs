using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    // Start is called before the first frame update
    public int power = 1;
    Vector2 size;
    void Start()
    {
        Destroy(gameObject, 5);
        size = transform.localScale/power;
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Enemy" || collision.gameObject.tag == "Wall")
        {
            if(collision.gameObject.tag == "Enemy"){
                Destroy(collision.gameObject);
            }
            if(power > 1){
                power--;
                transform.localScale = size*power;
            }
            else{
                Destroy(this.gameObject);
            }
        }
    }
}
