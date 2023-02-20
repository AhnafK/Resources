using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    Rigidbody2D _rigidbody;

    // Start is called before the first frame update
    void Start()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
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
            }
            else if (context.ReadValue<float>() < 0)
            {
                Debug.Log("Scrolling down");
        }
        }
    }

    // move
    public void OnMove(InputAction.CallbackContext context)
    {
        Vector2 move = context.ReadValue<Vector2>();
        _rigidbody.velocity = move * 5;
    }

}
