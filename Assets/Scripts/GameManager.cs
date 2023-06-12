using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public static class GameManager
{
    public static bool isLocal;

    public static bool canUseCards;

    public static Card player1Card;
    public static Card Player2Card;
    public static List<GameObject> playedCards = new List<GameObject>();
    public static bool easyMode = false;
    public static int player1RoundWins;
    public static int player2RoundWins;

    public static int Player1FireWins;
    public static int Player1EarthWins;
    public static int Player1IceWins;
    public static int Player1WaterWins;
    public static int Player1WindWins;

    public static int Player2FireWins;
    public static int Player2EarthWins;
    public static int Player2IceWins;
    public static int Player2WaterWins;
    public static int Player2WindWins;

    public static int nuberOfRounds;
}
