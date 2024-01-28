using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{
    public List<Character> npcs = new List<Character>();
    public JokeFactory jokeFactory;
    public Text timer;
    public float gameTime = 120;
    public string winSceneName = "";
    public string loseSceneName = "";

    public Player player;
    public GameObject npcPlayerPrefab;
    public JokeTextBox jokeTextBox;

    public Camera playerCamera;
    public Camera mapCamera;

    private Character accusedNPC;
    private Character npcPlayer;
    public List<string> accusitionStrings = new List<string> { "Gentlemen, with my superb�cognitive skills I've come to the conclusion that our robot is no one else then...", "..." };

    System.Random rd = new System.Random();
    Character robot;

    private int waitingNumber = 0;

    private void Awake()
    {
        SetUpNPCs();
    }

    private void Start()
    {
        playerCamera.enabled = true;
        mapCamera.enabled = false;
        waitingNumber = npcs.Count + 1;
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
            gameTime = 0;
            Lose();
        }
        UpdadteCamera();
    }

    public List<JokeItemType> GetRobotFunnyJokes()
    {
        return robot.funnyJokes;
    }

    public void Accuse(GameObject characterObejct)
    {
        playerCamera.enabled = false;
        mapCamera.enabled = true;
        mapCamera.transform.position = playerCamera.transform.position;
        mapCamera.orthographicSize = playerCamera.orthographicSize;
        mapCamera.gameObject.GetComponent<MapCameraController>().ZoomOut();

        var npcPlayerObject = Instantiate(npcPlayerPrefab);
        npcPlayerObject.transform.position = player.transform.position;
        npcPlayer = npcPlayerObject.GetComponent<Character>();
        npcPlayer.finalWorkstation = GameObject.Find("Detective Final WS").GetComponent<WorkStation>();
        npcPlayer.SetCurrentRoom(player.CurrentRoom);

        foreach (var npc in npcs)
        {
            npc.GoToFinalWorkstation();
        }
        npcPlayer.GoToFinalWorkstation();

        accusedNPC = characterObejct.GetComponent<Character>();
        Destroy(player.gameObject);

        //if (characterObejct.GetComponent<Character>().AreYouARobot())
        //{
        //    Win();
        //}
        //else
        //{
        //    Lose();
        //}
    }

    public void RegisterFinalArrival()
    {
        waitingNumber--;
        if (waitingNumber == 0)
        {
            StartCoroutine(StartAccusing());
        }
    }

    System.Collections.IEnumerator StartAccusing()
    {
        jokeTextBox.DisplayJoke(accusitionStrings[0]);
        yield return new WaitForSeconds(3);
        //npcPlayer.GoToNPC(accusedNPC);
        jokeTextBox.DisplayJoke(accusitionStrings[1] + accusedNPC.name);
        yield return new WaitForSeconds(2);

        var characterDisplay = accusedNPC.gameObject.GetComponentInChildren<CharacterDisplay>();
        if (accusedNPC.AreYouARobot())
        {
            characterDisplay.TurnIntoRobot();
        }
        else
        {
            characterDisplay.TurnIntoMeat();
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

    private void UpdadteCamera()
    {
        if (Input.GetKeyDown(KeyCode.M))
        {
            playerCamera.enabled = !playerCamera.enabled;
            mapCamera.enabled = !mapCamera.enabled;
        }
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

    void Lose()
    {
        if (loseSceneName != "") SceneManager.LoadScene(loseSceneName);
    }

    void Win()
    {
        if (winSceneName != "") SceneManager.LoadScene(winSceneName);
    }
}
