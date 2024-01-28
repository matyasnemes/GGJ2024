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
    private List<string> accusitionStrings = new List<string> { "Gentlemen, with my superb cognitive skills I've come to the conclusion that our robot is no one else than...", "..." };
    private List<string> introStrings = new List<string> { "Listen up everybody!", "As you all know, the new AI rules strictly forbid employing robots in creative jobs.", "This fine gentleman is here from the Bureau of Illegal Creativity to find if any robot is hiding among us.", "I have absolute trust in all of you, and I'm sure everyone is a human here.", "So go on your day as usual, and let the investigator do his job!", "Dismissed!" };
    private List<string> timeoutStrings = new List<string> { "Gentlemen, it seems the Bureau was wrong and there is not a single robot here.", "I came to this conclusion by running out of my allocated time.", "I hope I can keep my job." };

    public List<AudioClip> introClips;
    public List<AudioClip> timeoutClips;
    public AudioClip loseMusic;
    public AudioClip winMusic;
    public AudioClip accuseStartClip;
    Character robot;

    private int waitingNumber = 0;
    private bool gameStarted = false;

    private GameObject panel;
    private GameObject inv;
    
    private Coroutine gameStartCoroutine;
    private bool accusationStarted = false;

    private void Awake()
    {
        SetUpNPCs();
    }

    private void Start()
    {
        Random.InitState((int)Time.time);
        playerCamera.enabled = true;
        mapCamera.enabled = false;
        waitingNumber = npcs.Count + 1;
        panel = GameObject.Find("Panel");
        inv = GameObject.Find("inv_panel");
        gameStartCoroutine = StartCoroutine(PlayIntroScene());
    }

    void Update()
    {
        if (gameStarted)
        {
            NormalUpdate();
        }
        else if (Input.GetKeyDown(KeyCode.Space))
        {
            StopCoroutine(gameStartCoroutine);
            jokeTextBox.TurnOffTextBox();
            gameStarted = true;
            foreach (var npc in npcs)
            {
                npc.enabled = true;
            }
            player.enabled = true;
            player.gameObject.GetComponent<InventoryController>().enabled = true;
            panel.SetActive(true);
            inv.SetActive(true);
            playerCamera.gameObject.GetComponent<PlayerCameraController>().enabled = true;
        }
    }

    void PlayIntroClip(int index)
    {
        AudioSource audioSource = GameObject.Find("Player").GetComponent<AudioSource>();
        if (audioSource && index < introClips.Count && introClips[index])
        {
            audioSource.PlayOneShot(introClips[index]);
        }
    }

    void PlayTimeoutClip(int index)
    {
        AudioSource audioSource = GameObject.Find("NPCPlayer(Clone)").GetComponent<AudioSource>();
        if (audioSource && index < timeoutClips.Count && timeoutClips[index])
        {
            audioSource.PlayOneShot(timeoutClips[index]);
        }
    }

    System.Collections.IEnumerator PlayIntroScene()
    {
        panel.SetActive(false);
        inv.SetActive(false);
        for (int i = 0; i < introStrings.Count; i++)
        {
            jokeTextBox.DisplayIndefinitite(introStrings[i]);
            PlayIntroClip(i);
            yield return new WaitForSeconds(introClips[i].length + 0.5f);
        }

        jokeTextBox.TurnOffTextBox();
        gameStarted = true;
        foreach (var npc in npcs)
        {
            npc.enabled = true;
        }
        player.enabled = true;
        player.gameObject.GetComponent<InventoryController>().enabled = true;
        panel.SetActive(true);
        inv.SetActive(true);
        playerCamera.gameObject.GetComponent<PlayerCameraController>().enabled = true;
    }

    void NormalUpdate()
    {
        if(accusationStarted)
        {
            return;
        }

        if (gameTime - Time.deltaTime > 0)
        {
            gameTime -= Time.deltaTime;
            updateTimer();
        }
        else
        {
            gameTime = 0;
            Accuse(null);
        }
        UpdateCamera();

        if (Input.GetKeyUp(KeyCode.Escape))
        {
            SceneManager.LoadScene("MainMenu");
        }
    }

    public List<JokeItemType> GetRobotFunnyJokes()
    {
        return robot.funnyJokes;
    }

    public void Accuse(GameObject characterObejct)
    {
        accusationStarted = true;
        GameObject.Find("Panel").SetActive(false);
        GameObject.Find("inv_panel").SetActive(false);

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
            npc.speed *= 2;
        }
        npcPlayer.GoToFinalWorkstation();
        if(characterObejct != null)
        {
            accusedNPC = characterObejct.GetComponent<Character>();
        }
        else
        {
            accusedNPC = null;
        }
        Destroy(player.gameObject);
    }

    public void RegisterFinalArrival()
    {
        waitingNumber--;
        if (waitingNumber == 0)
        {
            StartCoroutine(StartAccusing());
        }
    }

    /**
     * Plays the end game music.
     * @param[in] won: True if won, false if lost.
     */
    public void PlayEndingSound(bool won)
    {
        AudioSource audioSource = GameObject.Find("NPCPlayer(Clone)").GetComponent<AudioSource>();
        if (audioSource)
        {
            if (!won && loseMusic)
            {
                audioSource.PlayOneShot(loseMusic);
            }
            else if (won && winMusic)
            {
                audioSource.PlayOneShot(winMusic);
            }
        }
    }

    public void PlayAccuseStart()
    {
        AudioSource audioSource = GameObject.Find("NPCPlayer(Clone)").GetComponent<AudioSource>();
        if (audioSource && accuseStartClip)
        {
            audioSource.PlayOneShot(accuseStartClip);
        }
    }

    public void PlayAccusedName(Character character)
    {
        AudioSource audioSource = GameObject.Find("NPCPlayer(Clone)").GetComponent<AudioSource>();
        if (audioSource && character.nameClip)
        {
            audioSource.PlayOneShot(character.nameClip);
        }
    }

    System.Collections.IEnumerator StartAccusing()
    {
        
        bool won;
        if(accusedNPC != null)
        {
            jokeTextBox.DisplayJoke(accusitionStrings[0]);
            PlayAccuseStart();
            yield return new WaitForSeconds(accuseStartClip.length + 1.0f);
            npcPlayer.GoToNPC(accusedNPC);
            yield return new WaitForSeconds(2);
            jokeTextBox.DisplayJoke(accusitionStrings[1] + accusedNPC.name);
            PlayAccusedName(accusedNPC);
            yield return new WaitForSeconds(2);

            var characterDisplay = accusedNPC.gameObject.GetComponentInChildren<CharacterDisplay>();
            if (accusedNPC.AreYouARobot())
            {
                characterDisplay.TurnIntoRobot();
                won = true;
            }
            else
            {
                characterDisplay.TurnIntoMeat();
                won = false;
            }
        }
        else
        {
            won = false;
            for (int i = 0; i < timeoutStrings.Count; i++)
            {
                jokeTextBox.DisplayIndefinitite(timeoutStrings[i]);
                PlayTimeoutClip(i);
                yield return new WaitForSeconds(timeoutClips[i].length + 0.5f);
            }
        }        
        if(!won)
        {
            foreach (var ch in npcs)
            {
                ch.GetComponentInChildren<CharacterDisplay>().FaceDown();
            }
        }

        PlayEndingSound(won);
        yield return new WaitForSeconds(2);
        jokeTextBox.ChangeToEndingStyle();
        string finalMsg = won ? "Congratulations officer, you have successfully uncovered the robot!" : "The real joke has been you all along!" ;
        jokeTextBox.DisplayIndefinitite(finalMsg);
        yield return new WaitForSeconds(8);



        SceneManager.LoadScene("MainMenu");
    }

    void SetUpNPCs()
    {
        List<JokeItemType> jokes = jokeFactory.jokeItemTypes;

        //ChooseARobot
        robot = npcs[Random.Range(0, npcs.Count)];
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

    private void UpdateCamera()
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
            ret += Random.Range(0, 2);
        }

        return ret;
    }
}
