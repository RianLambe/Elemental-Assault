using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WinsPanel : MonoBehaviour
{
    public GameObject roundWinCard;
    public GameObject fire;
    public GameObject ice;
    public GameObject water;
    public enum type {Fire, Ice, Water }
    public PhotonView pVWisPanel;

    private void Start() {
        pVWisPanel = GetComponent<PhotonView>();
    }

    [PunRPC]
    public void AddWin(int cardType) {

        if (cardType == 0) {
            GameObject newCard = Instantiate(roundWinCard, Vector3.zero, Quaternion.identity);
            newCard.GetComponent<WinCard>().element = WinCard.elements.Fire;
            newCard.transform.SetParent(fire.transform);
            newCard.transform.localScale = new Vector3(1,1,1);
        }
        else if (cardType == 1) {
            GameObject newCard = Instantiate(roundWinCard, Vector3.zero, Quaternion.identity);
            newCard.GetComponent<WinCard>().element = WinCard.elements.Ice;
            newCard.transform.SetParent(ice.transform);
            newCard.transform.localScale = new Vector3(1, 1, 1);
        }
        else if (cardType == 2) {
            GameObject newCard = Instantiate(roundWinCard, Vector3.zero, Quaternion.identity);
            newCard.GetComponent<WinCard>().element = WinCard.elements.Water;
            newCard.transform.SetParent(water.transform);
            newCard.transform.localScale = new Vector3(1, 1, 1);
        }
    }
}
