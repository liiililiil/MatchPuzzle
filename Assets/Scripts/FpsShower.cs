using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

public class FpsShower : MonoBehaviour
{
    StringBuilder stringBuilder = new StringBuilder();
    public Text text;
    float time;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        stringBuilder.Clear();
        stringBuilder.Append(1f / Time.deltaTime);
        stringBuilder.Append(", Fps (");
        stringBuilder.Append(Time.deltaTime * 1000);
        stringBuilder.Append(")ms");
        text.text = stringBuilder.ToString();
    }
}
