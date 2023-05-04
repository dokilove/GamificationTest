using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class Controller : MonoBehaviour
{
    public float duration = 0.1f;
    public void MoveToTarget(VisualElement icon, Vector2 direction)
    {
        StartCoroutine(MoveToTargetCoroutine(icon, direction));
    }

    IEnumerator MoveToTargetCoroutine(VisualElement icon, Vector2 diff)
    {
        float initialPosY = icon.layout.y;
        float initialPosX = icon.layout.x;

        float elapsedTime = 0.0f;
        while (elapsedTime < duration)
        {
            float t = elapsedTime / duration;

            icon.style.top = Mathf.Lerp(initialPosY, initialPosY - diff.y, t);
            icon.style.left = Mathf.Lerp(initialPosX, initialPosX - diff.x, t);

            elapsedTime += Time.deltaTime;

            yield return null;
        }

        icon.style.top = initialPosY - diff.y;
        icon.style.left = initialPosX - diff.x;
    }
    
}
