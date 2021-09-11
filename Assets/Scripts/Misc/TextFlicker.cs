using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UI;

public class TextFlicker : MonoBehaviour
{
    [SerializeField] private float flickerSpeed = 2.0f;

    private Text text;
    private float timer;

    // Start is called before the first frame update
    void Start()
    {
        text = GetComponent<Text>();
        timer = 0.0f;
    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime * flickerSpeed;
    
        Color c = text.color;
        c.a = (Mathf.Sin(timer) + 1.0f) * 0.5f;
        text.color = c;
    }
}
