using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cell : MonoBehaviour
{
    public int index;
    [SerializeField]
    protected Card card;

    public void SetCard(Card pcard) => card = pcard;
    public Card GetCard() => card;
}
