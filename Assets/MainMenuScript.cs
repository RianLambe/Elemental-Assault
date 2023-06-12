using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Realtime;
using Photon.Pun;

public class MainMenuScript : MonoBehaviour
{
    GameObject mainMenuRef;
    public GameObject startingPage;
    public GameObject titlePanel;
    public GameObject gamePanel;
    PhotonView pv;


    private void Start() {
        mainMenuRef = this.gameObject;
        ChangePage(startingPage);
        pv = GetComponent<PhotonView>();
    }

    //Changes the page the player is viewing
    public void ChangePage(GameObject newPage) {
        foreach (Transform foundPanel in mainMenuRef.transform) {
            LeanTween.alphaCanvas(foundPanel.GetComponent<CanvasGroup>(), 0f, .5f);
            foundPanel.GetComponent<CanvasGroup>().blocksRaycasts = false;
        }

        LeanTween.alphaCanvas(newPage.GetComponent<CanvasGroup>(), 1f, .5f);
        newPage.GetComponent<CanvasGroup>().blocksRaycasts = true;
    }

    public void QuitGame () {
        Application.Quit();
    }


    public void Restart() {
        gameObject.GetComponent<MainMenuScript>().pv.RPC("FinishPageChange", RpcTarget.All);
    }
    [PunRPC]
    public void FinishPageChange() {
        foreach (Transform foundPanel in mainMenuRef.transform) {
            LeanTween.alphaCanvas(foundPanel.GetComponent<CanvasGroup>(), 0f, .5f);
            foundPanel.GetComponent<CanvasGroup>().blocksRaycasts = false;
        }

        LeanTween.alphaCanvas(gamePanel.GetComponent<CanvasGroup>(), 1f, .5f);
        gamePanel.GetComponent<CanvasGroup>().blocksRaycasts = true;
    }
}

