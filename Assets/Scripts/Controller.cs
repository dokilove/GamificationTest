using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class Controller : MonoBehaviour
{
    public float speed = 10.0f;
    public void MoveToTarget(VisualElement icon, Vector2 direction)
    {
        StartCoroutine(MoveToTargetCoroutine(icon, direction));
    }

    IEnumerator MoveToTargetCoroutine(VisualElement icon, Vector2 diff)
    {
        Vector2 initialPos = new Vector2(icon.layout.x, icon.layout.y);
        Vector2 targetPos = new Vector2(icon.layout.x - diff.x, icon.layout.y - diff.y);

        Vector2 direction = new Vector2(-diff.x, -diff.y).normalized;

        Vector2 currentPos = initialPos;

        while ((currentPos - targetPos).sqrMagnitude > speed)
        {
            currentPos += direction * speed * Time.deltaTime;

            icon.style.left = currentPos.x;
            icon.style.top = currentPos.y;

            yield return null;
        }

        icon.style.top = targetPos.y;
        icon.style.left = targetPos.x;
    }
    
}
