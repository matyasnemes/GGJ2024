using UnityEngine;
using UnityEngine.UI;

public class UICharacterController : MonoBehaviour
{
    public GameObject character;

    private GameController gameController;

    private void Awake()
    {
        var button = gameObject.transform.GetChild(0).GetChild(0);
        button.GetComponent<Button>().onClick.AddListener(OnClick);

        var text = button.GetChild(0).GetComponentInChildren<TMPro.TextMeshProUGUI>();
        text.text = character.name;

        var uiIcon = gameObject.transform.GetChild(1).GetChild(0).gameObject;
        character.transform.Find("Character Display").GetComponent<CharacterDisplay>().RegisterUIIcon(uiIcon);
    }

    void Start()
    {
        gameController = GameObject.Find("GameController").GetComponent<GameController>();
    }

    public void OnClick()
    {
        gameController.Accuse(character);
    }
}
