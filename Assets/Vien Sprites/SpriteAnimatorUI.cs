using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class SpriteAnimatorUI : MonoBehaviour {
    public Image targetImage;       // UI Image component displaying the sprite animation.
    public Sprite[] frames;         // Array of sprites for the animation frames.
    public float frameRate = 10f;   // Frame rate for the animation.

    private bool isAnimating = false;

    /// <summary>
    /// Trigger the shoot animation if (and only if) one isn’t already playing.
    /// </summary>
    public void ResetToFirstFrame()
    {
        StopAllCoroutines();
        isAnimating = false;
        if (frames != null && frames.Length > 0)
            targetImage.sprite = frames[0];
    }

    public void PlayShootAnimation() {
        if (isAnimating) return;      // ← guard against spamming
        StartCoroutine(Animate());
    }

    private IEnumerator Animate() {
        isAnimating = true;
        // Cycle through each frame once.
        for (int i = 0; i < frames.Length; i++) {
            targetImage.sprite = frames[i];
            yield return new WaitForSeconds(1f / frameRate);
        }
        isAnimating = false;
    }
}
