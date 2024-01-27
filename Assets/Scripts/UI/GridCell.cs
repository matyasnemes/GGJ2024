using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class GridCell : MonoBehaviour, IPointerDownHandler
{
    public Sprite checkmarkSprite;
    public Sprite xmarkSprite;
    public Sprite questionmarkSprite;

    private Image image;

    enum State { CheckMarked, XMarked, QuestionMarked, Empty };
    private State state = State.Empty;

    void Start()
    {
        image = GetComponent<Image>();
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Left)
        {
            SetState(State.CheckMarked);
        }
        else if (eventData.button == PointerEventData.InputButton.Right)
        {
            SetState(State.XMarked);
        }
        else if (eventData.button == PointerEventData.InputButton.Middle)
        {
            SetState(State.QuestionMarked);
        }
    }

    private void SetState(State newState)
    {

        if (state == newState)
        {
            SetTransparency(0);
            state = State.Empty;
        }
        else
        {
            switch (newState)
            {
                case State.CheckMarked: image.sprite = checkmarkSprite; break;
                case State.XMarked: image.sprite = xmarkSprite; break;
                case State.QuestionMarked: image.sprite = questionmarkSprite; break;
            }
            SetTransparency(1);
            state = newState;
        }
    }
    private void SetTransparency(float transparency)
    {
        Color c = image.color;
        c.a = transparency;
        image.color = c;
    }
}
