using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Player : MonoBehaviour
{
    Rigidbody2D _rigidbody;
    public int bullets = 1;
    public float distForAmmo = 2;
    public float distForHunger = 1;
    public int bulletSpeed = 10;
    public int ammo = 0;
    public int maxAmmo = 25;
    public float health = 100;
    float maxHealth;
    public float hunger = 100;
    public int starvationDivider = 5;
    float maxHunger;
    public float initialSpeed = 5;
    float speed;
    public GameObject bulletPrefab;
    public Text ammoText;
    public Text healthText;
    public Text hungerText;
    public Text loadedText;
    public Text scoreText;
    public Image hungerBar;
    public Image healthBar;
    bool canShoot;
    public List<Text> invHungers;
    public List<Image> invFood;

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
        canShoot = true;
    }

    void FixedUpdate()
    {
        // reduce hunger and gain ammo based on distance moved
        distance += Vector2.Distance(transform.position, lastPos);
        hunger -= Vector2.Distance(transform.position, lastPos) / distForHunger;

        // Starvation
        if (hunger <= 0)
        {
            health += hunger/starvationDivider;
            hunger = 0;
            speed = initialSpeed / 5;
        }
        else
        {
            speed = initialSpeed;
        }

        // Overeating
        if (hunger > maxHunger)
        {
            health += (hunger - maxHunger) / 3;
            hunger = maxHunger;
        }

        // Enforcing max health
        if (health > maxHealth)
        {
            health = maxHealth;
        }

        // Current death implementation
        else if (health <= 0)
        {
            SceneManager.LoadScene("gameOver");
            //Destroy(gameObject);
            
        }

        // Gain ammo
        if (distance >= distForAmmo)
        {
            ammo += (int)(distance / distForAmmo);
            distance = distance % distForAmmo;
            if (ammo > maxAmmo)
            {
                ammo = maxAmmo;
            }
        }


        // Update UI
        scoreText.text = int.Parse(scoreText.text) + Time.deltaTime*200 + "";
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
                if (bullets > 0)
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
        if (canShoot && context.started && ammo >= bullets && bullets > 0)
        {            
            
            Vector2 currPos = transform.position;
            Vector2 mousePos = Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue());
            GameObject firedBullet = Instantiate(bulletPrefab, transform.position, Quaternion.Euler(0, 0, Mathf.Atan2(mousePos.y - currPos.y, mousePos.x - currPos.x) * Mathf.Rad2Deg + 90));
            firedBullet.transform.localScale *= bullets;
            firedBullet.GetComponent<Rigidbody2D>().velocity = (mousePos - currPos).normalized * bulletSpeed;
            firedBullet.GetComponent<Bullet>().power = bullets;
            firedBullet.GetComponent<Bullet>().scoreText = scoreText;
            ammo -= bullets;
            canShoot = false;
            StartCoroutine(ShootDelay());
        }
    }

    IEnumerator ShootDelay()
    {
        yield return new WaitForSeconds(0.2f);
        canShoot = true;
    }

    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Food")
        {
            // get hunger from food visual scripting component
            for(int i = 0; i < invFood.Count; i++){
                if(invFood[i].enabled == false){
                    invFood[i].sprite = collision.gameObject.GetComponent<SpriteRenderer>().sprite;
                    invFood[i].enabled = true;                    
                    invHungers[i].text = "" + collision.gameObject.GetComponent<food>().foodValue;                    
                    Destroy(collision.gameObject);
                    break;
                }
            }
        }
    }

    public void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Food")
        {
            // get hunger from food visual scripting component
            for(int i = 0; i < invFood.Count; i++){
                if(invFood[i].enabled == false){
                    invFood[i].sprite = collision.gameObject.GetComponent<SpriteRenderer>().sprite;
                    invFood[i].enabled = true;                    
                    invHungers[i].text = "" + collision.gameObject.GetComponent<food>().foodValue;                    
                    Destroy(collision.gameObject);
                    break;
                }
            }
            
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

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (foodCount.instance.num >= 1)
            {
                hunger += 20;
                foodCount.instance.useFood();
            }

          
        }

        if (health <= 0)
        {
            SceneManager.LoadScene("GameOver");
        }

    }

    public void onReset(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);            
        }
    }

    public void onSelect3()
    {
        if (invFood[3].enabled == true)
        {
            hunger += int.Parse(invHungers[2].text);
            invFood[3].enabled = false;
            invHungers[3].text = "" + 0;
        }
    }

    public void onSelect2()
    {
        if (invFood[2].enabled == true)
        {
            hunger += int.Parse(invHungers[2].text);
            invFood[2].enabled = false;
            invHungers[2].text = "" + 0;
        }
    }

    public void onSelect1()
    {
        if (invFood[1].enabled == true)
        {
            hunger += int.Parse(invHungers[1].text);
            invFood[1].enabled = false;
            invHungers[1].text = "" + 0;
        }
    }

    public void onSelect0()
    {
        if (invFood[0].enabled == true)
        {
            hunger += int.Parse(invHungers[0].text);
            invFood[0].enabled = false;
            invHungers[0].text = "" + 0;
        }
    }
}
