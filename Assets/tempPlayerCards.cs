using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class tempPlayerCards : MonoBehaviour
{
    public TMP_Text p1;
    public TMP_Text p2;

    private void Update() {
        if (GameManager.player1Card != null)
            p1.text = "Player 1 card : " + GameManager.player1Card.name;

        if (GameManager.Player2Card != null)
            p2.text = "Player 2 card : " + GameManager.Player2Card.name;

    }

}
