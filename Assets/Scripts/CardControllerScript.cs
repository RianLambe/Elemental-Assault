using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Dishooks;

public class CardControllerScript : MonoBehaviour
{
    public Card cardType;
    public TMP_Text valueText;
    public Image elementImage;
    public Image backgroundImage;
    public bool canInteract;
    public List<TMP_ColorGradient> textColors;
    public PhotonView pV;
    public List<Card> allCards;

    public DishookItem TempWebHook;

    // Start is called before the first frame update
    private void Start() {
        pV = GetComponent<PhotonView>();
    }

    [PunRPC]
    public void UpdateCard(int cardIndex, int playerHand, int handSlot) {
        cardType = allCards[cardIndex];

        //Place in hand
        gameObject.transform.SetParent(GameObject.Find("Player " + playerHand + " panel").transform.GetChild(handSlot));
        this.GetComponent<RectTransform>().anchoredPosition = new Vector3(0, 0, 0);
        this.transform.localScale = new Vector3(1, 1, 1);

        if(playerHand == 1 && PhotonNetwork.IsMasterClient) {
            this.GetComponent<Animator>().SetTrigger("flipCard");
            canInteract = true;
        }
        else if (playerHand == 2 && PhotonNetwork.IsMasterClient == false) {
            this.GetComponent<Animator>().SetTrigger("flipCard");
            canInteract = true;
        }


        //Initializes values
        valueText.text = ""+cardType.value;
        elementImage.sprite = cardType.elementImage;
        backgroundImage.sprite = cardType.background;
        
        if(cardType.value == 10) {
            valueText.colorGradientPreset = textColors[1];
        }
        else {
            valueText.colorGradientPreset = textColors[0];
        }
    }

    public void Click() {
        //Takes local click event and tells all clients
        if (canInteract && GameManager.canUseCards) { 
            GameManager.canUseCards = false;
            pV.RPC("ClickMulti", RpcTarget.All, PhotonNetwork.IsMasterClient);
        }

        //Dishook.Send("Player played a : " + cardType.name, TempWebHook);
    }

    [PunRPC]
    public void ClickMulti(bool isMaster) {
        if(isMaster) {
            GameManager.player1Card = cardType;
        }
        else {
            GameManager.Player2Card = cardType;
        }

        GameManager.playedCards.Add(this.gameObject);

        bool p1 = gameObject.transform.parent.transform.parent.gameObject == GameObject.Find("Player 1 panel");

        gameObject.transform.SetParent(FindObjectOfType<Canvas>().transform);

        if (p1 == true) {
            LeanTween.moveLocal(transform.gameObject, new Vector2(-165, 0), .3f);
        }
        else {
            LeanTween.moveLocal(transform.gameObject, new Vector2(165, 0), .3f);
        }

        FindObjectOfType<Canvas>().GetComponent<GameControllerScript>().StartCoroutine("CheckWin");
        LeanTween.moveLocal(backgroundImage.gameObject, new Vector2(0, 0), .1f);
        canInteract = false;
    }

    public void Hover() {
        if(canInteract)
        LeanTween.moveLocal(backgroundImage.gameObject, new Vector2(0, 70), .1f);

    }

    public void Unhover() {
        LeanTween.moveLocal(backgroundImage.gameObject, new Vector2(0, 0), .1f);
    }
}
