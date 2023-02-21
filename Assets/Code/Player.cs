using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    Rigidbody2D _rigidbody;
    public int bullets;
    public float health = 100;
    public float hunger = 100;
    public GameObject bulletPrefab;

    // Start is called before the first frame update
    void Start()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
        bullets = 1;
    }

    // Update is called once per frame
    void Update()
    {
       
    }

    // Called when player scrolls the wheel on the mouse
    public void OnScroll(InputAction.CallbackContext context)
    {
        if(context.started){
            if (context.ReadValue<float>() > 0)
            {
                Debug.Log("Scrolling up");
                if (bullets < 5)
                {
                    bullets++;
                }
            }
            else if (context.ReadValue<float>() < 0)
            {
                Debug.Log("Scrolling down");
                if (bullets > 1)
                {
                    bullets--;
                }
            }
        }
    }

    // move
    public void OnMove(InputAction.CallbackContext context)
    {
        Vector2 move = context.ReadValue<Vector2>();
        _rigidbody.velocity = move * 5;
    }

    // shoot towards mouse
    public void OnShoot(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            for (int i = 0; i < bullets; i++)
            {
                GameObject firedBullet = Instantiate(bulletPrefab, transform.position, Quaternion.identity);
                firedBullet.transform.rotation = Quaternion.Euler(0, 0, Mathf.Atan2(Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue()).y - transform.position.y, Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue()).x - transform.position.x) * Mathf.Rad2Deg);
                firedBullet.transform.localScale *= bullets;
                firedBullet.GetComponent<Rigidbody2D>().velocity = (Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue()) - transform.position).normalized * 10;
            }
        }
    }

}
