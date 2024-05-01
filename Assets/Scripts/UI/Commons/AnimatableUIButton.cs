using UnityEngine.UI;
using UnityEngine;

public class AnimatableUIButton : AnimatableUIItem<Button>
{
    protected void OnEnable() 
    {
        if (PlayOnEnabled)
        {
            PlayAnim(0);
        }
    }
}
