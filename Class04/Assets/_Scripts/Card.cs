using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Card", menuName = "Scriptable Object/Card")]
public class Card : ScriptableObject {
    public string cardName;
    public string cardDescription;
    public int manaCost;
    public int life;
    public int attack;
    public Sprite cardImage;
    public CardType cardType;

    public enum CardType { Unit, Hero, Magic, Equipment, Mana }
}
