using UnityEngine;
using System.Collections; // Add this line

public abstract class AnimatableUIItem<T> : MonoBehaviour where T : Component
{
    public AnimationClip[] animClips;
    protected T targetComponent;
    protected Animation animComponent;
    public bool PlayOnEnabled = false;

     void Awake()
    {
        // Get the target component attached to the same GameObject
        targetComponent = GetComponent<T>();
        animComponent = GetComponent<Animation>();
        if (animComponent != null && animClips != null)
            foreach (AnimationClip clip in animClips)
                animComponent.AddClip(clip, clip.name);
        if (targetComponent == null)
            Debug.LogError("No target component found.");
    }

    protected virtual void Start() { }

    public void PlayAnim(int index)
    {
        if (index >= 0 && index < animClips.Length && targetComponent != null)
        {
            // Play the animation clip on the target component
            AnimationClip clip = animClips[index];
            Animation anim = targetComponent.gameObject.GetComponent<Animation>();
            if (anim == null)
            {
                anim = targetComponent.gameObject.AddComponent<Animation>();
            }
            anim.AddClip(clip, clip.name);
            anim.Play(clip.name);
            StartCoroutine(ResetClipAfterDelay(clip.length));
        }
    }

    private IEnumerator ResetClipAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);

        // Reset the animation clip
        if (animComponent != null && animClips.Length > 0)
        {
            AnimationClip clip = animClips[0];
            animComponent.Stop();
        }
    }
}
