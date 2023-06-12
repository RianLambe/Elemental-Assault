using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Shield : MonoBehaviour
{
    public GameObject face;
    public List<Sprite> shieldImages;

    public void SetShieldColor(int index) {
        switch (index) {
            case 0:
                face.GetComponent<Image>().sprite = shieldImages[0];
                break;
            case 1:
                face.GetComponent<Image>().sprite = shieldImages[1];
                break;
            case 2:
                face.GetComponent<Image>().sprite = shieldImages[2];
                break;

        }
    }
}
