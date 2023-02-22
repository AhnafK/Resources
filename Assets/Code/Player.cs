using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    Rigidbody2D _rigidbody;
    public int bullets = 1;
    public float distForAmmo = 2;
    public float distForHunger = 1;
    public int bulletSpeed = 10;
    public int ammo = 0;
    public float health = 100;
    float maxHealth;
    public float hunger = 100;
    float maxHunger;
    public float initialSpeed = 5;
    float speed;
    public GameObject bulletPrefab;
    public Text ammoText;
    public Text healthText;
    public Text hungerText;
    public Text loadedText;
    public Image hungerBar;
    public Image healthBar;
    

    Vector2 lastPos;
    float distance;

    void Start()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
        speed = initialSpeed;
        bullets = 1;
        maxHealth = health;
        maxHunger = hunger;
        lastPos = transform.position;
    }

    void FixedUpdate()
    {
        // reduce hunger and gain ammo based on distance moved
        distance += Vector2.Distance(transform.position, lastPos);
        hunger -= Vector2.Distance(transform.position, lastPos) / distForHunger;
        if (hunger <= 0)
        {
            health += hunger/10;
            hunger = 0;
            speed = initialSpeed / 5;
        }
        else
        {
            speed = initialSpeed;
        }
        if (hunger > maxHunger)
        {
            health += (hunger - maxHunger) / 3;
            hunger = maxHunger;
        }
        if (health > maxHealth)
        {
            health = maxHealth;
        }
        else if (health <= 0)
        {
            Destroy(gameObject);
        }
        if (distance >= distForAmmo)
        {
            ammo += (int)(distance / distForAmmo);
            distance = distance % distForAmmo;
        }
        lastPos = transform.position;
        hungerText.text = ""+(int)hunger;
        healthText.text = ""+(int)health;
        ammoText.text = ""+ammo;
        healthBar.fillAmount = health/maxHealth;
        hungerBar.fillAmount = hunger/maxHunger;
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
            loadedText.text = ""+bullets;
        }
    }

    // move
    public void OnMove(InputAction.CallbackContext context)
    {
        Vector2 move = context.ReadValue<Vector2>();
        _rigidbody.velocity = move * speed;
    }

    // shoot towards mouse
    public void OnShoot(InputAction.CallbackContext context)
    {
        if (context.started && ammo >= bullets)
        {            
            
            Vector2 currPos = transform.position;
            Vector2 mousePos = Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue());
            GameObject firedBullet = Instantiate(bulletPrefab, transform.position, Quaternion.Euler(0, 0, Mathf.Atan2(mousePos.y - currPos.y, mousePos.x - currPos.x) * Mathf.Rad2Deg + 90));
            firedBullet.transform.localScale *= bullets;
            firedBullet.GetComponent<Rigidbody2D>().velocity = (mousePos - currPos).normalized * bulletSpeed;
            firedBullet.GetComponent<Bullet>().power = bullets;
            ammo -= bullets;
        }
    }

    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Food")
        {
            // get hunger from food visual scripting component
            hunger += collision.gameObject.GetComponent<food>().foodValue;
            Destroy(collision.gameObject);
        }
    }

    public void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Food")
        {
            // get hunger from food visual scripting component
            hunger += collision.gameObject.GetComponent<food>().foodValue;
            Destroy(collision.gameObject);
        }
        else if (collision.gameObject.tag == "Enemy")
        {
            health -= 10;

            // push player away from enemy
            Vector2 currPos = transform.position;
            Vector2 enemyPos = collision.gameObject.transform.position;
            _rigidbody.AddForce((currPos - enemyPos).normalized * 800);

        }
        
    }

}
