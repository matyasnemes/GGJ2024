using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{
    public List<Character> npcs = new List<Character>();
    public JokeFactory jokeFactory;
    public Text timer;
    public float gameTime = 120;

    System.Random rd = new System.Random();
    Character robot;

    private void Awake()
    {
        SetUpNPCs();
    }

    void Update()
    {
        if (gameTime - Time.deltaTime > 0)
        {
            gameTime -= Time.deltaTime;
            updateTimer();
        }
        else
        {
            Debug.Log("TIME IS UP YOU LOSER!");
            gameTime = 0;
        }
    }

    public List<JokeItemType> GetRobotFunnyJokes()
    {
        return robot.funnyJokes;
    }

    public void Accuse(GameObject characterObejct)
    {
        if (characterObejct.GetComponent<Character>().AreYouARobot())
        {
            Debug.Log("You won");
        }
        else
        {
            Debug.Log("You lost");
        }
    }

    void SetUpNPCs()
    {
        List<JokeItemType> jokes = jokeFactory.jokeItemTypes;

        //ChooseARobot
        robot = npcs[rd.Next(npcs.Count)];
        robot.YouAreARobot();

        //Choose a sense of humor for the robot
        string robotFindsFunny = GenerateRandomNLongBinary(jokes.Count);

        //Generate a sense of humor for everyone else
        foreach (var npc in npcs)
        {
            string humour = "";

            if (npc != robot)
            {
                bool found = false;

                //generate a random sense of humour hopefully different than the robots
                for (int i = 0; i < 100 && !found; i++)
                {
                    humour = GenerateRandomNLongBinary(jokes.Count);
                    if (humour != robotFindsFunny) found = true;
                }

                //if the RNG gods hate the player, force their hand
                if (!found) humour = robotFindsFunny == "0000" ? "0001" : "0000";
            }
            else
            {
                humour = robotFindsFunny;
            }

            if (humour == "") Debug.LogError("Somehow, the npc has not been generated a sense of humour");

            //Set the sense of humour
            for (int i = 0; i < jokes.Count; i++)
            {
                if (humour[i] == '1')
                {
                    npc.ThisIsFunny(jokes[i]);
                }
            }
        }

    }

    void updateTimer()
    {
        float minutes = Mathf.FloorToInt(gameTime / 60);
        float seconds = Mathf.FloorToInt(gameTime % 60);

        timer.text = string.Format("{0}:{1:00}", minutes, seconds);
    }

    string GenerateRandomNLongBinary(int n)
    {
        string ret = "";

        for (int i = 0; i < n; i++)
        {
            ret += rd.Next(2);
        }

        return ret;
    }
}
