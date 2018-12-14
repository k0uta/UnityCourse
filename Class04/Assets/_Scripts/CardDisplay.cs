using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CardDisplay : MonoBehaviour {

    public Card card;

    public TextMeshProUGUI titleText;

    public TextMeshProUGUI descriptionText;

    public TextMeshProUGUI attackText;

    public TextMeshProUGUI lifeText;

    public Image cardImage;

    // Use this for initialization
    void Start () {
        if(card != null)
        {
            UpdateCard();
        }
    }
	
	public void SetCard(Card _card)
    {
        card = _card;

        UpdateCard();
    }

    void UpdateCard()
    {
        cardImage.sprite = card.cardImage;
        titleText.text = card.cardName;
        descriptionText.text = card.cardDescription;
        attackText.text = card.attack.ToString();
        lifeText.text = card.life.ToString();
    }
}
