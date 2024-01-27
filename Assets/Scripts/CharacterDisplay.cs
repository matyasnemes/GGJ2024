using UnityEngine;
using UnityEngine.UI;

public class CharacterDisplay : MonoBehaviour
{

    public Sprite headDown;
    public Sprite headUp;
    public Sprite bodyDown;
    public Sprite bodyUp;
    public Sprite hand;

    public SpriteRenderer headRenderer;
    public SpriteRenderer bodyRenderer;
    public SpriteRenderer handRenderer1;
    public SpriteRenderer handRenderer2;

    public GameObject uiIcon;

    public bool faceDown = true;

    private Image handImage1;
    private Image handImage2;
    private Image headImage;
    private Image bodyImage;

    // Start is called before the first frame update
    void Start()
    {
        bodyImage = uiIcon.transform.GetChild(0).gameObject.GetComponent<Image>();
        headImage = uiIcon.transform.GetChild(1).gameObject.GetComponent<Image>();
        handImage1 = uiIcon.transform.GetChild(2).gameObject.GetComponent<Image>();
        handImage2 = uiIcon.transform.GetChild(3).gameObject.GetComponent<Image>();

        handImage1.sprite = hand;
        handImage2.sprite = hand;

        if (faceDown)
        {
            FaceDown();
        }
        else
        {
            FaceUp();
        }

        handRenderer1.sprite = hand;
        handRenderer2.sprite = hand;
    }

    public void FaceDown()
    {
        faceDown = true;
        headRenderer.sprite = headDown;
        bodyRenderer.sprite = bodyDown;
        headRenderer.sortingOrder = 11;
        handRenderer1.sortingOrder = 11;
        handRenderer2.sortingOrder = 11;
        bodyRenderer.sortingOrder = 10;

        headImage.sprite = headDown;
        bodyImage.sprite = bodyDown;
    }

    public void FaceUp()
    {
        faceDown = false;
        headRenderer.sprite = headUp;
        bodyRenderer.sprite = bodyUp;
        headRenderer.sortingOrder = 10;
        handRenderer1.sortingOrder = 10;
        handRenderer2.sortingOrder = 10;
        bodyRenderer.sortingOrder = 11;

        headImage.sprite = headUp;
        bodyImage.sprite = bodyUp;
    }
}
