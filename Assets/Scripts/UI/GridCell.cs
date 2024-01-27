using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class GridCell : MonoBehaviour, IPointerDownHandler
{
    public Sprite laughSprite;
    public Sprite mehSprite;
    public Sprite questionmarkSprite;

    private Image image;

    enum State { Laugh, Meh, QuestionMark, Empty };
    private State state = State.Empty;

    void Start()
    {
        image = GetComponent<Image>();
        image.enabled = false;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Left)
        {
            SetState(State.Laugh);
        }
        else if (eventData.button == PointerEventData.InputButton.Right)
        {
            SetState(State.Meh);
        }
        else if (eventData.button == PointerEventData.InputButton.Middle)
        {
            SetState(State.QuestionMark);
        }
    }

    private void SetState(State newState)
    {

        if (state == newState)
        {
            image.enabled = false;
            state = State.Empty;
        }
        else
        {
            switch (newState)
            {
                case State.Laugh: image.sprite = laughSprite; break;
                case State.Meh: image.sprite = mehSprite; break;
                case State.QuestionMark: image.sprite = questionmarkSprite; break;
            }
            image.enabled = true;
            state = newState;
        }
    }
}
