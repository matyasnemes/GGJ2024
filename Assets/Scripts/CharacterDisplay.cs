using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

    public bool faceDown = true;

    // Start is called before the first frame update
    void Start()
    {
        if(faceDown) {
            FaceDown();
        }
        else {
            FaceUp();
        }
        handRenderer1.sprite = hand;
        handRenderer2.sprite = hand;
    }

    public void FaceDown() {
        faceDown = true;
        headRenderer.sprite = headDown;
        bodyRenderer.sprite = bodyDown;
        headRenderer.sortingOrder = 11;
        handRenderer1.sortingOrder = 11;
        handRenderer2.sortingOrder = 11;
        bodyRenderer.sortingOrder = 10;
    }

    public void FaceUp() {
        faceDown = false;
        headRenderer.sprite = headUp;
        bodyRenderer.sprite = bodyUp;
        headRenderer.sortingOrder = 10;
        handRenderer1.sortingOrder = 10;
        handRenderer2.sortingOrder = 10;
        bodyRenderer.sortingOrder = 11;
    }
}
