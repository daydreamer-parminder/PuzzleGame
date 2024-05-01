using UnityEngine;
using UnityEngine.UI;

public class Card : MonoBehaviour, IClickable
{
    public CardData cardData;
    private IClickListener clickListener;
    private Image image;
    private bool isPlayable = false;

    private void Awake()
    {
        image = GetComponent<Image>();
    }

    public void SetListener(IClickListener pclickListener) => clickListener = pclickListener;

    public virtual void SetData(CardData pCardData) => cardData = pCardData;

    public virtual void SetPlayable(bool pplayable) 
    { 
        isPlayable = pplayable;
        GetComponent<Button>().enabled = isPlayable;
    }

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
    public void OnClicked() => clickListener.OnClicked(this);

    public bool IsPlayable() => isPlayable;
}
