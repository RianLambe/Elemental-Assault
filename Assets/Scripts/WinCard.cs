using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WinCard : MonoBehaviour
{
    public Image elementImage;
    public Image backgroundImage;
    public List<Sprite> allElements;
    public List<Sprite> allBackgrounds;
    public enum elements {Fire, Ice, Water }
    public elements element;

    private void Start() {
        switch (element) {
            case elements.Fire:
                elementImage.sprite = allElements[0];
                backgroundImage.sprite = allBackgrounds[0];
                break;
            case elements.Ice:
                elementImage.sprite = allElements[1];
                backgroundImage.sprite = allBackgrounds[1];
                break;
            case elements.Water:
                elementImage.sprite = allElements[2];
                backgroundImage.sprite = allBackgrounds[2];
                break;
        }
        gameObject.transform.localScale = new Vector3(1,1,1);
    }
}
