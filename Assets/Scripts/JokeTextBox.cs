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
    string jokeToDisplay = "";
    float jokeTellingSpeed = 2.0f;
    TextMeshProUGUI textmesh;
    Image img;
    // Start is called before the first frame update
    void Start()
    {
        img = GetComponent<Image>();
        textmesh = GetComponentInChildren<TextMeshProUGUI>();

        img.enabled = false;
        textmesh.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        if(textTimeLeft > 0.0f)
        {
            textTimeLeft -= Time.deltaTime;

            if(textTimeLeft <= 0.0f)
            {        
                img.enabled = false;
                textmesh.enabled = false;

                int index = (int)((textTimeMax - textTimeLeft)/(textTimeMax-1));

                if(index >= jokeToDisplay.Length) index = jokeToDisplay.Length;
                Debug.Log(index);
                textmesh.text = jokeToDisplay.Substring(0, index);
            }
        }
    }

    public void DisplayJoke(string joke)
    {
        textTimeLeft = textTimeMax;        
        img.enabled = true;
        textmesh.enabled = true;
        jokeToDisplay = joke;
    }
}
