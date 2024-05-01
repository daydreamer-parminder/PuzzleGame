using UnityEngine;
using UnityEngine.UI;

public class Card : MonoBehaviour
{
    public CardData cardData;
    private Image image;
    private bool isPlayable = false;

    private void Awake()
    {
        image = GetComponent<Image>();
    }

    public virtual void SetPlayable(bool pplayable) => isPlayable = pplayable;
    public bool IsPlayable() => isPlayable;

    public virtual void SetData(CardData pCardData)  => cardData = pCardData;

    public virtual void Reveal()
    {
        image.sprite = cardData.symbol;
    }
    public virtual void Hide()
    {
        // Assuming you have a card back sprite
        // Replace "cardBack" with the name of your card back sprite
        image.sprite = cardData.back;
    }
}
