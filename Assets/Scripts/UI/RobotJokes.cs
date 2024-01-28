using UnityEngine;
using UnityEngine.UI;

public class RobotJokes : MonoBehaviour
{
    public Sprite laugh;
    public Sprite meh;

    void Start()
    {
        var gameController = GameObject.Find("GameController").GetComponent<GameController>();
        var jokes = gameController.GetRobotFunnyJokes();

        var pun = transform.GetChild(1).gameObject.GetComponent<Image>();
        var mom = transform.GetChild(2).gameObject.GetComponent<Image>();
        var dark = transform.GetChild(3).gameObject.GetComponent<Image>();

        pun.sprite = meh;
        mom.sprite = meh;
        dark.sprite = meh;

        foreach (var joke in jokes)
        {
            if (joke.name() == "Dark Joke")
            {
                dark.sprite = laugh;
            }
            else if (joke.name() == "Mom Joke")
            {
                mom.sprite = laugh;
            }
            else if (joke.name() == "Pun")
            {
                pun.sprite = laugh;
            }
        }
    }
}
