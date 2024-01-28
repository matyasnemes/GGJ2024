using System.Xml.Linq;
using UnityEngine;
using UnityEngine.UI;
using static UnityEngine.GraphicsBuffer;

public class CharacterDisplay : MonoBehaviour
{

    public Sprite headDown;
    public Sprite headUp;
    public Sprite bodyDown;
    public Sprite bodyUp;
    public Sprite hand;

    public Sprite hand1;
    public Sprite hand2;
    public Sprite hand1Back;
    public Sprite hand2Back;

    public SpriteRenderer headRenderer;
    public SpriteRenderer bodyRenderer;
    public SpriteRenderer handRenderer1;
    public SpriteRenderer handRenderer2;

    public Sprite robotBody;
    public Sprite robotHead;
    public Sprite robotHand;

    public Sprite meatBody;
    public Sprite meatHead;
    public Sprite meatHand;


    public bool faceDown = true;
    public bool insidesRevealed = false;

    private GameObject uiIcon;

    private Image handImage1;
    private Image handImage2;
    private Image headImage;
    private Image bodyImage;

    // Start is called before the first frame update
    void Start()
    {
        headOrigin = headRenderer.gameObject.transform.localPosition;
        hand1Origin = handRenderer1.gameObject.transform.localPosition;
        hand2Origin = handRenderer2.gameObject.transform.localPosition;


        var nameTag = transform.Find("Name");
        if (nameTag)
        {
            nameTag.GetComponent<TMPro.TextMeshPro>().text = transform.parent.name;
        }


        if (uiIcon)
        {
            bodyImage = uiIcon.transform.GetChild(0).gameObject.GetComponent<Image>();
            headImage = uiIcon.transform.GetChild(1).gameObject.GetComponent<Image>();
            handImage1 = uiIcon.transform.GetChild(2).gameObject.GetComponent<Image>();
            handImage2 = uiIcon.transform.GetChild(3).gameObject.GetComponent<Image>();
            handImage1.sprite = hand;
            handImage2.sprite = hand;
        }

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

        StartMove();
    }

    public void FaceDown()
    {
        if (!insidesRevealed)
        {
            faceDown = true;
            headRenderer.sprite = headDown;
            bodyRenderer.sprite = bodyDown;
            headRenderer.sortingOrder = 11;
            handRenderer1.sortingOrder = 11;
            handRenderer2.sortingOrder = 11;
            bodyRenderer.sortingOrder = 10;

            if (hand1 != null)
            {
                handRenderer1.sprite = hand1;
            }
            if (hand2 != null)
            {
                handRenderer2.sprite = hand2;
            }

            if (uiIcon)
            {
                headImage.sprite = headDown;
                bodyImage.sprite = bodyDown;
            }
        }
    }

    public void FaceUp()
    {
        if (!insidesRevealed)
        {
            faceDown = false;
            headRenderer.sprite = headUp;
            bodyRenderer.sprite = bodyUp;
            headRenderer.sortingOrder = 10;
            handRenderer1.sortingOrder = 10;
            handRenderer2.sortingOrder = 10;
            bodyRenderer.sortingOrder = 11;

            if (hand1Back != null)
            {
                handRenderer1.sprite = hand1Back;
            }
            if (hand2Back != null)
            {
                handRenderer2.sprite = hand2Back;
            }

            if (uiIcon)
            {
                headImage.sprite = headUp;
                bodyImage.sprite = bodyUp;
            }
        }
    }

    public void TurnIntoRobot()
    {
        FaceDown();
        headRenderer.sprite = robotHead;
        handRenderer1.sprite = robotHand;
        handRenderer2.sprite = robotHand;
        bodyRenderer.sprite = robotBody;
        insidesRevealed = true;
    }

    public void TurnIntoMeat()
    {
        FaceDown();
        headRenderer.sprite = meatHead;
        handRenderer1.sprite = meatHand;
        handRenderer2.sprite = meatHand;
        bodyRenderer.sprite = meatBody;
        insidesRevealed = true;
    }

    public void RegisterUIIcon(GameObject icon)
    {
        uiIcon = icon;
    }

    private Vector3 headOrigin;
    private Vector3 hand1Origin;
    private Vector3 hand2Origin;

    private Vector3 headTarget;
    private Vector3 hand1Target;
    private Vector3 hand2Target;

    private float headSpeed = 1.0f;
    private float hand1Speed = 1.0f;
    private float hand2Speed = 1.0f;

    private bool headForwards = true;
    private bool hand1Forwards = true;
    private bool hand2Forwards = true;

    public string state = "Nothing";

    public void Update()
    {
        float headStep = headSpeed * Time.deltaTime;
        float hand1Step = hand1Speed * Time.deltaTime;
        float hand2Step = hand2Speed * Time.deltaTime;
        headRenderer.gameObject.transform.localPosition
            = Vector3.MoveTowards(headRenderer.gameObject.transform.localPosition, headTarget, headStep);
        handRenderer1.gameObject.transform.localPosition
            = Vector3.MoveTowards(handRenderer1.gameObject.transform.localPosition, hand1Target, hand1Step);
        handRenderer2.gameObject.transform.localPosition
            = Vector3.MoveTowards(handRenderer2.gameObject.transform.localPosition, hand2Target, hand2Step);

        if (Vector3.Distance(headRenderer.gameObject.transform.localPosition, headTarget) < 0.001f)
        {
            HeadLoop();
        }
        if (Vector3.Distance(handRenderer1.gameObject.transform.localPosition, hand1Target) < 0.001f)
        {
            Hand1Loop();
        }
        if (Vector3.Distance(handRenderer2.gameObject.transform.localPosition, hand2Target) < 0.001f)
        {
            Hand2Loop();
        }

        if(Input.GetKeyUp(KeyCode.U)) {
            StartMove();
        }
        if (Input.GetKeyUp(KeyCode.I))
        {
            StartIdle();
        }
        if (Input.GetKeyUp(KeyCode.O))
        {
            StartPiss();
        }
        if (Input.GetKeyUp(KeyCode.J))
        {
            StartShit();
        }
        if (Input.GetKeyUp(KeyCode.K))
        {
            StartEat();
        }
        if (Input.GetKeyUp(KeyCode.L))
        {
            StartVend();
        }
    }

    private Vector3 moveHeadOffset = new Vector3(0.0f, 0.05f, 0.0f);
    private Vector3 moveHand1Offset = new Vector3(0.0f, 0.25f, 0.0f);
    private Vector3 moveHand2Offset = new Vector3(0.0f, -0.25f, 0.0f);
    private float moveHeadSpeed = 0.2f;
    private float moveHand1Speed = 1.0f;
    private float moveHand2Speed = 1.0f;

    private Vector3 idleHeadOffset = new Vector3(0.0f, 0.05f, 0.0f);
    private Vector3 idleHand1Offset = new Vector3(0.1f, 0.0f, 0.0f);
    private Vector3 idleHand2Offset = new Vector3(-0.1f, 0.0f, 0.0f);
    private float idleHeadSpeed = 0.2f;
    private float idleHand1Speed = 0.5f;
    private float idleHand2Speed = 0.5f;

    private Vector3 pissHeadOffset = new Vector3(0.0f, -0.05f, 0.0f);
    private Vector3 pissHand1Offset = new Vector3(-1.0f, 0.0f, 0.0f);
    private Vector3 pissHand2Offset = new Vector3(1.0f, 0.0f, 0.0f);
    private float pissHeadSpeed = 0.5f;
    private float pissHand1Speed = 2.0f;
    private float pissHand2Speed = 2.0f;

    private Vector3 shitHeadOffset = new Vector3(0.0f, -0.05f, 0.0f);
    private Vector3 shitHand1Offset = new Vector3(0.0f, 1.0f, 0.0f);
    private Vector3 shitHand2Offset = new Vector3(0.0f, 1.0f, 0.0f);
    private float shitHeadSpeed = 0.5f;
    private float shitHand1Speed = 1.0f;
    private float shitHand2Speed = 1.0f;

    private Vector3 eatHeadOffset = new Vector3(0.0f, -0.05f, 0.0f);
    private Vector3 eatHand1Offset = new Vector3(0.0f, 0.8f, 0.0f);
    private Vector3 eatHand2Offset = new Vector3(0.0f, 0.4f, 0.0f);
    private Vector3 eatHand1Offset2 = new Vector3(0.0f, 0.4f, 0.0f);
    private Vector3 eatHand2Offset2 = new Vector3(0.0f, 0.8f, 0.0f);
    private float eatHeadSpeed = 0.5f;
    private float eatHand1Speed = 2.0f;
    private float eatHand2Speed = 2.0f;

    private Vector3 vendHeadOffset = new Vector3(0.0f, 0.0f, 0.0f);
    private Vector3 vendHand1Offset = new Vector3(0.2f, 1.0f, 0.0f);
    private Vector3 vendHand2Offset = new Vector3(-0.2f, 1.0f, 0.0f);
    private Vector3 vendHand1Offset2 = new Vector3(-0.2f, 1.0f, 0.0f);
    private Vector3 vendHand2Offset2 = new Vector3(0.2f, 1.0f, 0.0f);
    private float vendHeadSpeed = 0.5f;
    private float vendHand1Speed = 2.0f;
    private float vendHand2Speed = 2.0f;

    private void StartMove()
    {
        string newState = "Move";
        if (!state.Equals(newState))
        {
            ResetPositions();
            headForwards = true;
            hand1Forwards = true;
            hand2Forwards = true;
            headSpeed = moveHeadSpeed;
            hand1Speed = moveHand1Speed;
            hand2Speed = moveHand2Speed;
            state = newState;
            headTarget = headOrigin + moveHeadOffset;
            hand1Target = hand1Origin + moveHand1Offset;
            hand2Target = hand2Origin + moveHand2Offset;
        }
    }

    private void StartEat()
    {
        string newState = "Eat";
        if (!state.Equals(newState))
        {
            ResetPositions();
            headForwards = true;
            hand1Forwards = true;
            hand2Forwards = true;
            headSpeed = eatHeadSpeed;
            hand1Speed = eatHand1Speed;
            hand2Speed = eatHand2Speed;
            state = newState;
            headTarget = headOrigin + eatHeadOffset;
            hand1Target = hand1Origin + eatHand1Offset;
            hand2Target = hand2Origin + eatHand2Offset;
        }
    }
    private void StartVend()
    {
        string newState = "Vend";
        if (!state.Equals(newState))
        {
            ResetPositions();
            headForwards = true;
            hand1Forwards = true;
            hand2Forwards = true;
            headSpeed = vendHeadSpeed;
            hand1Speed = vendHand1Speed;
            hand2Speed = vendHand2Speed;
            state = newState;
            headTarget = headOrigin + vendHeadOffset;
            hand1Target = hand1Origin + vendHand1Offset;
            hand2Target = hand2Origin + vendHand2Offset;
        }
    }

    private void StartIdle()
    {
        string newState = "Idle";
        if (!state.Equals(newState))
        {
            ResetPositions();
            headForwards = true;
            hand1Forwards = true;
            hand2Forwards = true;
            headSpeed = idleHeadSpeed;
            hand1Speed = idleHand1Speed;
            hand2Speed = idleHand2Speed;
            state = newState;
            headTarget = headOrigin + idleHeadOffset;
            hand1Target = hand1Origin + idleHand1Offset;
            hand2Target = hand2Origin + idleHand2Offset;
        }
    }

    private void StartPiss()
    {
        string newState = "Piss";
        if (!state.Equals(newState))
        {
            ResetPositions();
            headForwards = true;
            hand1Forwards = true;
            hand2Forwards = true;
            headSpeed = pissHeadSpeed;
            hand1Speed = pissHand1Speed;
            hand2Speed = pissHand2Speed;
            state = newState;
            headTarget = headOrigin + pissHeadOffset;
            hand1Target = hand1Origin + pissHand1Offset;
            hand2Target = hand2Origin + pissHand2Offset;
        }
    }

    private void StartShit()
    {
        string newState = "Shit";
        if (!state.Equals(newState))
        {
            ResetPositions();
            headForwards = true;
            hand1Forwards = true;
            hand2Forwards = true;
            headSpeed = shitHeadSpeed;
            hand1Speed = shitHand1Speed;
            hand2Speed = shitHand2Speed;
            state = newState;
            headTarget = headOrigin + shitHeadOffset;
            hand1Target = hand1Origin + shitHand1Offset;
            hand2Target = hand2Origin + shitHand2Offset;
        }
    }

    private void HeadLoop()
    {
        switch (state)
        {
            case "Move":
                if (headForwards)
                {
                    headForwards = false;
                    headTarget = headOrigin - moveHeadOffset;
                }
                else
                {
                    headForwards = true;
                    headTarget = headOrigin + moveHeadOffset;
                }
                break;
            case "Idle":
                if (headForwards)
                {
                    headForwards = false;
                    headTarget = headOrigin - idleHeadOffset;
                }
                else
                {
                    headForwards = true;
                    headTarget = headOrigin + idleHeadOffset;
                }
                break;
        }
    }

    private void Hand1Loop()
    {
        switch (state)
        {
            case "Move":
                if (hand1Forwards)
                {
                    hand1Forwards = false;
                    hand1Target = hand1Origin - moveHand1Offset;
                }
                else
                {
                    hand1Forwards = true;
                    hand1Target = hand1Origin + moveHand1Offset;
                }
                break;
            case "Idle":
                if (hand1Forwards)
                {
                    hand1Forwards = false;
                    hand1Target = hand1Origin - idleHand1Offset;
                }
                else
                {
                    hand1Forwards = true;
                    hand1Target = hand1Origin + idleHand1Offset;
                }
                break;
            case "Eat":
                if (hand1Forwards)
                {
                    hand1Forwards = false;
                    hand1Target = hand1Origin + eatHand1Offset2;
                }
                else
                {
                    hand1Forwards = true;
                    hand1Target = hand1Origin + eatHand1Offset;
                }
                break;
            case "Vend":
                if (hand1Forwards)
                {
                    hand1Forwards = false;
                    hand1Target = hand1Origin + vendHand1Offset2;
                }
                else
                {
                    hand1Forwards = true;
                    hand1Target = hand1Origin + vendHand1Offset;
                }
                break;
        }
    }

    private void Hand2Loop()
    {
        switch (state)
        {
            case "Move":
                if (hand2Forwards)
                {
                    hand2Forwards = false;
                    hand2Target = hand2Origin - moveHand2Offset;
                }
                else
                {
                    hand2Forwards = true;
                    hand2Target = hand2Origin + moveHand2Offset;
                }
                break;
            case "Idle":
                if (hand2Forwards)
                {
                    hand2Forwards = false;
                    hand2Target = hand2Origin - idleHand2Offset;
                }
                else
                {
                    hand2Forwards = true;
                    hand2Target = hand2Origin + idleHand2Offset;
                }
                break;
            case "Eat":
                if (hand2Forwards)
                {
                    hand2Forwards = false;
                    hand2Target = hand2Origin + eatHand2Offset2;
                }
                else
                {
                    hand2Forwards = true;
                    hand2Target = hand2Origin + eatHand2Offset;
                }
                break;
            case "Vend":
                if (hand2Forwards)
                {
                    hand2Forwards = false;
                    hand2Target = hand2Origin + vendHand2Offset2;
                }
                else
                {
                    hand2Forwards = true;
                    hand2Target = hand2Origin + vendHand2Offset;
                }
                break;
        }
    }

    private void ResetPositions()
    {
        headRenderer.gameObject.transform.localPosition = headOrigin;
        handRenderer1.gameObject.transform.localPosition = hand1Origin;
        handRenderer2.gameObject.transform.localPosition = hand2Origin;
    }
}
