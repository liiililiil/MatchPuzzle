using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FpsShower : MonoBehaviour
{
    public Text text;
    float time;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        time += Time.deltaTime;
        if(time >0.1f){
            text.text = $"{1f/Time.deltaTime}, Fps ({Time.deltaTime * 1000})ms";
            time = 0;
        }
        
    }
}
