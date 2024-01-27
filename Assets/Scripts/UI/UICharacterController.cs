using UnityEngine;
using UnityEngine.UI;

public class UICharacterController : MonoBehaviour
{
    public GameObject character;

    private GameController gameController;

    // Start is called before the first frame update
    private void Awake()
    {
        var button = gameObject.transform.GetChild(0).GetChild(0).GetComponent<Button>();
        button.onClick.AddListener(OnClick);

        var uiIcon = gameObject.transform.GetChild(1).GetChild(0).gameObject;
        character.transform.Find("Character Display").GetComponent<CharacterDisplay>().RegisterUIIcon(uiIcon);
    }

    void Start()
    {
        gameController = GameObject.Find("GameController").GetComponent<GameController>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void OnClick()
    {
        gameController.Accuse(character);
    }
}
