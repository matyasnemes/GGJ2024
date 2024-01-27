using UnityEngine;
using UnityEngine.UI;

public class RobotJokes : MonoBehaviour
{
    void Start()
    {
        var gameController = GameObject.Find("GameController").GetComponent<GameController>();
        var jokes = gameController.GetRobotFunnyJokes();

        for (int i = 0; i < jokes.Count; i++)
        {
            var image = transform.GetChild(i + 1).gameObject.GetComponent<Image>();
            image.sprite = jokes[i].sprite();
            image.enabled = true;
        }
    }
}
