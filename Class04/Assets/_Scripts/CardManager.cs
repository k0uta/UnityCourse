using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CardManager : MonoBehaviour {

    public CardDisplay player1CardDisplay;

    public CardDisplay player2CardDisplay;

    public Card[] cardPool;

    public TextMeshProUGUI winnerText;

    private void Start()
    {
        Play();
    }

    Card GetRandomCard()
    {
        int randomIndex = Random.Range(0, cardPool.Length);
        return cardPool[randomIndex];
    }

    public void Play()
    {
        var player1Card = GetRandomCard();
        player1CardDisplay.SetCard(player1Card);

        var player2Card = GetRandomCard();
        player2CardDisplay.SetCard(player2Card);

        string winner;
        if(player1Card.attack > player2Card.attack)
        {
            winner = "Player 1";
        }
        else
        {
            winner = "Player 2";
        }

        winnerText.text = string.Format("{0} Wins!", winner);
    }
}
