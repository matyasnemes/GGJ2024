using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class JokeTextBox : MonoBehaviour
{
    public string text;
    public float textTimeMax = 3.0f;

    float textTimeLeft = 0.0f;
    // Start is called before the first frame update
    void Start()
    {
        GetComponent<Image>().enabled = false;
        GetComponentInChildren<TextMeshProUGUI>().enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        if(textTimeLeft > 0.0f)
        {
            textTimeLeft -= Time.deltaTime;

            if(textTimeLeft <= 0.0f)
            {
                GetComponent<Image>().enabled = false;
                GetComponentInChildren<TextMeshProUGUI>().enabled = false;
            }
        }
    }

    public void DisplayJoke(string joke)
    {
        textTimeLeft = textTimeMax;
        GetComponent<Image>().enabled = true;
        GetComponentInChildren<TextMeshProUGUI>().enabled = true;
    }
}
