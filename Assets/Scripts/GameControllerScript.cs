using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor;
using UnityEngine;
using Photon.Realtime;
using Dishooks;


public class GameControllerScript : MonoBehaviourPunCallbacks
{
    public List<Card> AllCards;
    public GameObject cardPrefab;

    [Space(10f)]
    [Header("UI References")]
    public GameObject mainMenuRef;

    public GameObject player1Panel;
    public GameObject player2Panel;
    public GameObject Player1WinsPanel;
    public GameObject Player2WinsPanel;

    public TMP_Text winText;
    public GameObject winnerPanel;
    public TMP_Text roundText;
    public TMP_Text winningPlayerText;
    public GameObject gameOverPanel;

    public PhotonView pVGameController;

    public List<GameObject> shields;
    //public GameObject startButton;
    [Header("Game over panel References")]
    public TMP_Text timePlayed;
    public TMP_Text roundsPlayed;



    [Space(10f)]
    [Header("Player scores")]
    public TMP_Text P1Fire;
    public TMP_Text P1Earth;
    public TMP_Text P1Ice;
    public TMP_Text P1Water;
    public TMP_Text P1Wind;

    public TMP_Text P2Fire;
    public TMP_Text P2Earth;
    public TMP_Text P2Ice;
    public TMP_Text P2Water;
    public TMP_Text P2Wind;

    [Space(10f)]
    [Header("Dishooks")]
    public DishookItem discordHook;

    int timeInSeconds;
    public TMP_Text timerText;

    bool awaitingRoomCreation = false;

    public void StartGame() {
        pVGameController = GetComponent<PhotonView>();
    }

    [PunRPC]
    public void PlayGame() {

        StartCoroutine("CreateCards");
        if(timerStarted == false) gameObject.GetComponent<GameControllerScript>().photonView.RPC("StartTimer", RpcTarget.All);
    }

    public void AIEnabled(bool enabled) {
        awaitingRoomCreation = true;
        GameManager.isLocal = enabled;
    }

    public override void OnJoinedRoom() {
        if (awaitingRoomCreation)
            PlayGame();

    }

    [PunRPC]
    public void SetCanPlay() {
        GameManager.canUseCards = true;
    }

    [PunRPC]
    public void SetWinText(bool active, string text) {
        winText.text = text;
        LeanTween.alphaCanvas(winText.GetComponent<CanvasGroup>(), active ? 1 : 0, .5f);
    }


    //Creates all the cards in each players hands
    public IEnumerator CreateCards() {
        for (int j = 1; j < 3; j++) {
            for (int i = 0; i < 5; i++) {
                if (GameObject.Find("Player " + j + " panel").transform.GetChild(i).transform.childCount == 0) {
                    GameObject newCard = PhotonNetwork.Instantiate(cardPrefab.name, new Vector3(0, 0, 0), Quaternion.identity);

                    if (GameManager.easyMode) {
                        newCard.GetPhotonView().RPC("UpdateCard", RpcTarget.All, Random.Range(0, 30), j, i);
                    }
                    else {
                        newCard.GetPhotonView().RPC("UpdateCard", RpcTarget.All, Random.Range(0, 50), j, i);

                        //newCard.GetComponent<CardControllerScript>().cardType = AllCards[Random.Range(0, 50)];
                    }
                    yield return new WaitForSeconds(.2f);
                }
            }
        }

        gameObject.GetComponent<GameControllerScript>().photonView.RPC("SetCanPlay", RpcTarget.All);

        if (GameManager.isLocal)
        StartCoroutine("AIPickRandom");

    }

    public void SetDifficulty(bool difficulty)
    {
        GameManager.easyMode = difficulty;
    }

    [PunRPC]
    public void ClearBoard(bool fullClear) {
        gameObject.GetComponent<GameControllerScript>().photonView.RPC("SetWinText", RpcTarget.All, false, "");

        if (fullClear) { 

            //Clear both players hands 
            foreach (Transform oldCard in player1Panel.transform) {
                if(oldCard.transform.childCount > 0) {
                    Destroy(oldCard.transform.GetChild(0).gameObject);
                    
                }
            }
            foreach (Transform oldCard in player2Panel.transform) {
                if (oldCard.transform.childCount > 0) { 
                    Destroy(oldCard.transform.GetChild(0).gameObject);
                }
            }

            foreach (GameObject playedCard in GameManager.playedCards) {
                if(playedCard != null)
                Destroy(playedCard.gameObject);
            }

            GameManager.player1Card = null;
            GameManager.Player2Card = null;
            GameManager.playedCards.Clear();


            GameManager.Player1FireWins = 0;
            GameManager.Player1EarthWins = 0;
            GameManager.Player1IceWins = 0;
            GameManager.Player1WaterWins = 0;
            GameManager.Player1WindWins = 0;

            GameManager.Player2FireWins = 0;
            GameManager.Player2EarthWins = 0;
            GameManager.Player2IceWins = 0;
            GameManager.Player2WaterWins = 0;
            GameManager.Player2WindWins = 0;

            //Clear the scoreboards
            foreach (Transform winPanel in Player1WinsPanel.transform) {
                foreach (Transform winSlot in winPanel.transform) {
                    if(winSlot != null)
                        Destroy(winSlot.gameObject);
                }
            }
            foreach (Transform winPanel in Player2WinsPanel.transform) {
                foreach (Transform winSlot in winPanel.transform) {
                    if (winSlot != null)
                        Destroy(winSlot.gameObject);
                }
            }

            //StartCoroutine("InitGame");
        }
        else {
            foreach (GameObject playedCard in GameManager.playedCards) {
                if (playedCard != null)
                    Destroy(playedCard.gameObject);
            }
            GameManager.player1Card = null;
            GameManager.Player2Card = null;
            GameManager.playedCards.Clear();

        }
    }

    //Sets up each round of the game
    public IEnumerator InitGame() {
        gameObject.GetComponent<GameControllerScript>().photonView.RPC("SetWinText", RpcTarget.All, false, "");

        gameObject.GetComponent<GameControllerScript>().photonView.RPC("ClearBoard", RpcTarget.All, true);
        yield return new WaitForSeconds(1);
        gameObject.GetComponent<GameControllerScript>().photonView.RPC("PlayGame", RpcTarget.MasterClient);

        yield return new WaitForSeconds(2);
    }

    //Generates the cards in the selected players hand
    public IEnumerator GenerateCards(GameObject generationParent) {
        //Generates random cards for player 1
        for (int i = 0; i < 5; i++) {
            if(generationParent.transform.GetChild(i).transform.childCount == 0) {
                //Local
                //GameObject newCard = Instantiate(cardPrefab, new Vector3(0, 0, 0), Quaternion.identity);
                //newCard.transform.SetParent(generationParent.transform.GetChild(i).transform);
                //newCard.GetComponent<RectTransform>().anchoredPosition = new Vector3(0, 0, 0);
                //newCard.transform.localScale = new Vector3(1, 1, 1);
                Debug.Log("Created card");

                //Networking 
                GameObject newCard = PhotonNetwork.Instantiate(cardPrefab.name, new Vector3(0, 0, 0), Quaternion.identity);
                newCard.transform.SetParent(generationParent.transform.GetChild(i).transform);
                newCard.GetComponent<RectTransform>().anchoredPosition = new Vector3(0, 0, 0);
                newCard.transform.localScale = new Vector3(1, 1, 1);
                

                if (GameManager.easyMode) {
                    //newCard.GetComponent<CardControllerScript>().cardType = AllCards[Random.Range(0, 30)];
                    newCard.GetPhotonView().RPC("UpdateCard", RpcTarget.All, Random.Range(0, 30));
                }
                else {
                    newCard.GetComponent<CardControllerScript>().cardType = AllCards[Random.Range(0, 50)];
                }
                //newCard.GetComponent<CardControllerScript>().cardType = AllCards[Random.Range(0, AllCards.Count)];

                yield return new WaitForSeconds(.2f);

                    newCard.GetComponent<CardControllerScript>().canInteract = true;
                Debug.Log(newCard.GetComponent<CardControllerScript>().canInteract);
                if (generationParent == player1Panel) {
                    newCard.GetComponent<Animator>().SetTrigger("flipCard");
                    
                }
                else {
                    //newCard.GetComponent<CardControllerScript>().canInteract = false;
                }
            }
        }
    }

    public void StartAIPick() {
        StartCoroutine("AIPickRandom");
    }

    //Picks a card at random for the AI to play
    public IEnumerator AIPickRandom() {
        yield return new WaitForSeconds(Random.Range(2, 4));

        int randomPick = Random.Range(0, player2Panel.transform.childCount);
        GameObject pickedCard = player2Panel.transform.GetChild(randomPick).gameObject.transform.GetChild(0).gameObject;
        GameManager.Player2Card = pickedCard.GetComponent<CardControllerScript>().cardType;
        pickedCard.gameObject.transform.SetParent(this.gameObject.transform);
        pickedCard.GetComponent<CardControllerScript>().canInteract = false;
        //pickedCard.gameObject.transform.GetComponent<RectTransform>().anchoredPosition = new Vector2(165, 0);
        LeanTween.moveLocal(pickedCard.transform.gameObject, new Vector2(165, 0), .3f);
        GameManager.playedCards.Add(pickedCard.gameObject);
        StartCoroutine("CheckWin");
    }

    [PunRPC]
    public void FlipCards() {
        foreach (GameObject playedCard in GameManager.playedCards) {
            playedCard.GetComponent<Animator>().SetTrigger("flipCard");
        }
    }


    public IEnumerator CheckWin() {
        if (GameManager.player1Card != null && GameManager.Player2Card != null && PhotonNetwork.IsMasterClient) {
            bool wonRound = false;

            yield return new WaitForSeconds(2);

            gameObject.GetComponent<GameControllerScript>().photonView.RPC("FlipCards", RpcTarget.All);

            yield return new WaitForSeconds(.5f);

            
            //winText.gameObject.SetActive(true);

            #region Card logic
            //Fire v Ice and Earth
            if (GameManager.player1Card.elements == Card.elementTypes.Fire && (GameManager.Player2Card.elements == Card.elementTypes.Ice || GameManager.Player2Card.elements == Card.elementTypes.Earth)) {
                winText.text = "Player 1 wins the round";
                //Player1WinsPanel.GetComponent<WinsPanel>().AddWin(WinsPanel.type.Fire);
                Player1WinsPanel.GetComponent<WinsPanel>().pVWisPanel.RPC("AddWin", RpcTarget.All, 0);
                GameManager.Player1FireWins++;
            }
            //Earth v Ice and Water
            else if (GameManager.player1Card.elements == Card.elementTypes.Earth && (GameManager.Player2Card.elements == Card.elementTypes.Ice || GameManager.Player2Card.elements == Card.elementTypes.Water)) {
                winText.text = "Player 1 wins the round)";
                //Player1WinsPanel.GetComponent<WinsPanel>().AddWin(WinsPanel.type.Fire);
                Player1WinsPanel.GetComponent<WinsPanel>().pVWisPanel.RPC("AddWin", RpcTarget.All, 0);
                GameManager.Player1EarthWins++;
            }
            //Ice v Water and Wind
            else if (GameManager.player1Card.elements == Card.elementTypes.Ice && (GameManager.Player2Card.elements == Card.elementTypes.Water || GameManager.Player2Card.elements == Card.elementTypes.Wind)) {
                winText.text = "Player 1 wins the round";
                //Player1WinsPanel.GetComponent<WinsPanel>().AddWin(WinsPanel.type.Ice);
                Player1WinsPanel.GetComponent<WinsPanel>().pVWisPanel.RPC("AddWin", RpcTarget.All, 1);
                GameManager.Player1IceWins++;
            }
            //Water v Fire and Wind
            else if (GameManager.player1Card.elements == Card.elementTypes.Water && (GameManager.Player2Card.elements == Card.elementTypes.Fire || GameManager.Player2Card.elements == Card.elementTypes.Wind)) {
                winText.text = "Player 1 wins the round";
                //Player1WinsPanel.GetComponent<WinsPanel>().AddWin(WinsPanel.type.Water);
                Player1WinsPanel.GetComponent<WinsPanel>().pVWisPanel.RPC("AddWin", RpcTarget.All, 2);
                GameManager.Player1WaterWins++;
            }
            //Wind v Fire and Earth
            else if (GameManager.player1Card.elements == Card.elementTypes.Wind && (GameManager.Player2Card.elements == Card.elementTypes.Fire || GameManager.Player2Card.elements == Card.elementTypes.Earth)) {
                winText.text = "Player 1 wins the round";
                //Player1WinsPanel.GetComponent<WinsPanel>().AddWin(WinsPanel.type.Fire);
                Player1WinsPanel.GetComponent<WinsPanel>().pVWisPanel.RPC("AddWin", RpcTarget.All, 0);
                GameManager.Player1WindWins++;
            }
            //Same element 
            else if ((GameManager.player1Card.elements == GameManager.Player2Card.elements) && GameManager.player1Card.value > GameManager.Player2Card.value) {
                winText.text = "Player 1 wins the round";
                switch (GameManager.player1Card.elements) {
                    case Card.elementTypes.Fire:
                        //Player1WinsPanel.GetComponent<WinsPanel>().AddWin(WinsPanel.type.Fire);
                        Player1WinsPanel.GetComponent<WinsPanel>().pVWisPanel.RPC("AddWin", RpcTarget.All, 0);
                        GameManager.Player1FireWins++;
                        break;
                    case Card.elementTypes.Earth:
                        //Player1WinsPanel.GetComponent<WinsPanel>().AddWin(WinsPanel.type.Water);
                        Player1WinsPanel.GetComponent<WinsPanel>().pVWisPanel.RPC("AddWin", RpcTarget.All, 2);
                        GameManager.Player1EarthWins++;
                        break;
                    case Card.elementTypes.Ice:
                        //Player1WinsPanel.GetComponent<WinsPanel>().AddWin(WinsPanel.type.Ice);
                        Player1WinsPanel.GetComponent<WinsPanel>().pVWisPanel.RPC("AddWin", RpcTarget.All, 1);
                        GameManager.Player1IceWins++;
                        break;
                    case Card.elementTypes.Water:
                        //Player1WinsPanel.GetComponent<WinsPanel>().AddWin(WinsPanel.type.Water);
                        Player1WinsPanel.GetComponent<WinsPanel>().pVWisPanel.RPC("AddWin", RpcTarget.All, 2);
                        GameManager.Player1WaterWins++;
                        break;
                    case Card.elementTypes.Wind:
                        //Player1WinsPanel.GetComponent<WinsPanel>().AddWin(WinsPanel.type.Water);
                        Player1WinsPanel.GetComponent<WinsPanel>().pVWisPanel.RPC("AddWin", RpcTarget.All, 2);
                        GameManager.Player1WindWins++;
                        break;
                }
            }
            //Draw
            else if ((GameManager.player1Card.elements == GameManager.Player2Card.elements) && GameManager.player1Card.value == GameManager.Player2Card.value) {
                winText.text = "The rounds is a draw!";
            }
            //Player 2 wins
            else {
                winText.text = "Player 2 wins the round";
                switch (GameManager.Player2Card.elements) {
                    case Card.elementTypes.Fire:
                        ///Player2WinsPanel.GetComponent<WinsPanel>().AddWin(WinsPanel.type.Fire);
                        Player2WinsPanel.GetComponent<WinsPanel>().pVWisPanel.RPC("AddWin", RpcTarget.All, 0);
                        GameManager.Player2FireWins++;
                        break;
                    case Card.elementTypes.Earth:
                        //Player2WinsPanel.GetComponent<WinsPanel>().AddWin(WinsPanel.type.Fire);
                        Player2WinsPanel.GetComponent<WinsPanel>().pVWisPanel.RPC("AddWin", RpcTarget.All, 0);
                        GameManager.Player2EarthWins++;
                        break;
                    case Card.elementTypes.Ice:
                        //Player2WinsPanel.GetComponent<WinsPanel>().AddWin(WinsPanel.type.Ice);
                        Player2WinsPanel.GetComponent<WinsPanel>().pVWisPanel.RPC("AddWin", RpcTarget.All, 1);
                        GameManager.Player2IceWins++;
                        break;
                    case Card.elementTypes.Water:
                        //Player2WinsPanel.GetComponent<WinsPanel>().AddWin(WinsPanel.type.Water);
                        Player2WinsPanel.GetComponent<WinsPanel>().pVWisPanel.RPC("AddWin", RpcTarget.All, 2);
                        GameManager.Player2WaterWins++;
                        break;
                    case Card.elementTypes.Wind:
                        //Player2WinsPanel.GetComponent<WinsPanel>().AddWin(WinsPanel.type.Water);
                        Player2WinsPanel.GetComponent<WinsPanel>().pVWisPanel.RPC("AddWin", RpcTarget.All, 2);
                        GameManager.Player2WindWins++;
                        break;
                }
            }
            #endregion

            gameObject.GetComponent<GameControllerScript>().photonView.RPC("SetWinText", RpcTarget.All, true, winText.text);
            UpdateScoreText();

            #region Check number of wins
            //Checks player 1 win state
            foreach (Transform vertPanel in Player1WinsPanel.transform) {
                if (vertPanel.childCount >= 3) {
                    Debug.Log("Player 1 wins the game from 3 of the same element");
                    gameObject.GetComponent<GameControllerScript>().photonView.RPC("WinRound", RpcTarget.All, 1);
                    //StartCoroutine("RoundWin", 1);
                    wonRound = true;
                }             
            } 
            if (Player1WinsPanel.transform.GetChild(0).childCount > 0 & Player1WinsPanel.transform.GetChild(1).childCount > 0 & Player1WinsPanel.transform.GetChild(2).childCount > 0) {
                Debug.Log("Player 1 wins the game from 3 different elements");
                gameObject.GetComponent<GameControllerScript>().photonView.RPC("WinRound", RpcTarget.All, 1);
                //StartCoroutine("RoundWin", 1);
                wonRound = true;
            }

            //Checks player 2 win state
            foreach (Transform vertPanel in Player2WinsPanel.transform) {
                if (vertPanel.childCount >= 3) {
                    Debug.Log("Player 2 wins the game from 3 of the same element");
                    gameObject.GetComponent<GameControllerScript>().photonView.RPC("WinRound", RpcTarget.All, 2);
                    //StartCoroutine("RoundWin", 2);
                    wonRound = true;
                }
            }
            if (Player2WinsPanel.transform.GetChild(0).childCount > 0 & Player2WinsPanel.transform.GetChild(1).childCount > 0 & Player2WinsPanel.transform.GetChild(2).childCount > 0) {
                Debug.Log("Player 2 wins the game from 3 different elements");
                gameObject.GetComponent<GameControllerScript>().photonView.RPC("WinRound", RpcTarget.All, 2);
                //StartCoroutine("RoundWin", 2);
                wonRound = true;
            }
            #endregion
            
            yield return new WaitForSeconds(3f);

            if (wonRound == false) {
                //FindObjectOfType<Canvas>().GetComponent<GameControllerScript>().StartCoroutine("InitGame");
                gameObject.GetComponent<GameControllerScript>().photonView.RPC("PlayGame", RpcTarget.MasterClient);
                gameObject.GetComponent<GameControllerScript>().photonView.RPC("ClearBoard", RpcTarget.All, false);

            }
        }

    }

    public void UpdateScoreText() {
        P1Fire.text = GameManager.Player1FireWins.ToString();
        P1Earth.text = GameManager.Player1EarthWins.ToString();
        P1Ice.text = GameManager.Player1IceWins.ToString();
        P1Water.text = GameManager.Player1WaterWins.ToString();
        P1Wind.text = GameManager.Player1WindWins.ToString();

        P2Fire.text = GameManager.Player2FireWins.ToString();
        P2Earth.text = GameManager.Player2EarthWins.ToString();
        P2Ice.text = GameManager.Player2IceWins.ToString();
        P2Water.text = GameManager.Player2WaterWins.ToString();
        P2Wind.text = GameManager.Player2WindWins.ToString();
    }

    [PunRPC]
    public void WinRound(int winningPlayer) {
        StartCoroutine("RoundWin", winningPlayer);
        GameManager.nuberOfRounds ++;  
    }

    public IEnumerator RoundWin(int player) {

        yield return new WaitForSeconds(3);
        bool gameOver = false;

        if (player == 1) {
            GameManager.player1RoundWins++;
            if (GameManager.player1RoundWins == 1) {
                shields[0].transform.gameObject.GetComponent<Animator>().SetTrigger("flip");
                yield return new WaitForSeconds(0.15f);
                shields[0].transform.gameObject.GetComponent<Shield>().SetShieldColor(0);
            }
            if (GameManager.player1RoundWins == 2) {
                shields[2].transform.gameObject.GetComponent<Animator>().SetTrigger("flip");
                yield return new WaitForSeconds(0.15f);
                shields[2].transform.gameObject.GetComponent<Shield>().SetShieldColor(0);
                gameOver = true;
            }
        }
        else {
            GameManager.player2RoundWins++;
            if (GameManager.player2RoundWins == 1) {
                shields[1].transform.gameObject.GetComponent<Animator>().SetTrigger("flip");
                yield return new WaitForSeconds(0.15f);
                shields[1].transform.gameObject.GetComponent<Shield>().SetShieldColor(1);
            }
            if (GameManager.player2RoundWins == 2) {
                shields[2].transform.gameObject.GetComponent<Animator>().SetTrigger("flip");
                yield return new WaitForSeconds(0.15f);
                shields[2].transform.gameObject.GetComponent<Shield>().SetShieldColor(1);
                gameOver = true;
            }
        }

        if (gameOver) {
            winningPlayerText.text = "Player " + player + " wins the Game!";
            winnerPanel.GetComponent<Animator>().SetTrigger("playerWon");
            ClearBoard(true);

            StopCoroutine("Timer");
            timePlayed.text = "Time played : " + Mathf.Floor(timeInSeconds / 60) + "m " + (timeInSeconds % 60).ToString("00") + "s";
            roundsPlayed.text = "Rounds played : " + GameManager.nuberOfRounds;

            yield return new WaitForSeconds(4);

            SendDiscordStats();
            mainMenuRef.GetComponent<MainMenuScript>().ChangePage(gameOverPanel);

            UpdateScoreText();

            //Clears the sheilds / round win counter
            GameManager.player1RoundWins = 0;
            GameManager.player2RoundWins = 0;
            foreach (GameObject shield in shields) {
                shield.GetComponent<Shield>().SetShieldColor(2);
            }
        }
        else {
            winningPlayerText.text = "Player " + player + " wins the round!";
            winnerPanel.GetComponent<Animator>().SetTrigger("playerWon");
            ClearBoard(true);

            yield return new WaitForSeconds(2.5f);
            StartCoroutine("InitGame");
            //gameObject.GetComponent<GameControllerScript>().photonView.RPC("ClearBoard", RpcTarget.All, true);
            //gameObject.GetComponent<GameControllerScript>().photonView.RPC("PlayGame", RpcTarget.MasterClient);
        }
    }

    public void SendDiscordStats() {
        if(PhotonNetwork.IsMasterClient) {
            Dishook.Send(
                GameManager.nuberOfRounds
                + "_" + 
                timeInSeconds
                , discordHook);
        }

    }

    public override void OnLobbyStatisticsUpdate(List<TypedLobbyInfo> lobbyStatistics) {
        base.OnLobbyStatisticsUpdate(lobbyStatistics);

    }

    public void RestartGame() {
        gameObject.GetComponent<GameControllerScript>().photonView.RPC("ResetTimer", RpcTarget.All);
        timerStarted = false;
            
        PlayGame();
    }

    bool timerStarted;
    [PunRPC]
    public void StartTimer() {
        StartCoroutine("Timer");
    }

    [PunRPC]
    public void ResetTimer() {
        StopCoroutine("Timer");
        timeInSeconds = 0;
        GameManager.nuberOfRounds = 0;
    }

    IEnumerator Timer() {
        timerStarted = true;
        yield return new WaitForSeconds(1f);
        timeInSeconds++;
        timerText.text = "Timer : " + Mathf.Floor(timeInSeconds / 60) + "m " + (timeInSeconds % 60).ToString("00") + "s";
        StartCoroutine("Timer");
    }


}
