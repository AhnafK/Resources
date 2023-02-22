using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class foodCount : MonoBehaviour
{
    public static foodCount instance;

    public Text count;
    public int num = 0;

    private void Awake()
    {
        instance = this;
    }

    void Start()
    {
        count.text = num.ToString();

    }

    public void eatFood()
    {
        num++;
        count.text = num.ToString();
    }

    public void useFood()
    {
        num--;
        count.text = num.ToString();
    }


}
    // Update is called once per frame
    
