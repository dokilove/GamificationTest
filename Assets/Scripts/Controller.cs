using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class Controller : MonoBehaviour
{
    public float speed = 20.0f;
    public float friction = 10.0f;
    public float overlapThreshold = 10.0f;

    VisualElement stage;

    IEnumerator moveCoroutine;

    public void SetStage(VisualElement stage)
    {
        this.stage = stage;
    }

    public void MoveToTarget(VisualElement icon, Vector2 direction)
    {
        if (null != moveCoroutine)
        {
            StopCoroutine(moveCoroutine);
        }

        moveCoroutine = MoveToTargetCoroutine(icon, direction);
        StartCoroutine(moveCoroutine);
    }

    IEnumerator MoveToTargetCoroutine(VisualElement icon, Vector2 diff)
    {
        Vector2 initialPos = new Vector2(icon.layout.center.x, icon.layout.center.y);
        Vector2 targetPos = new Vector2(icon.layout.center.x - diff.x, icon.layout.center.y - diff.y);

        float velocity = (initialPos - targetPos).magnitude;

        Vector2 currentPos = initialPos;

        float diffX = -diff.x;
        float diffY = -diff.y;
        Vector2 direction = new Vector2(diffX, diffY).normalized;

        while (velocity > 0.0f)
        {
            currentPos += direction * velocity * speed * Time.deltaTime;

            velocity -= friction;

            if (currentPos.x < stage.layout.x || currentPos.x > stage.layout.width + stage.layout.x) 
            {
                direction.x = -direction.x;
                AdjustPositionX(currentPos);
            }
            if (currentPos.y < stage.layout.y || currentPos.y > stage.layout.height + stage.layout.y)
            {
                direction.y = -direction.y;
                AdjustPositionY(currentPos);
            }

            icon.style.left = currentPos.x - icon.layout.width * 0.5f;
            icon.style.top = currentPos.y - icon.layout.height * 0.5f;

            yield return null;
        }
    }

    void AdjustPositionX(Vector2 currentPos)
    {
        float offset = 0;
        if (currentPos.x < stage.layout.x)
        {
            offset = stage.layout.x - currentPos.x;
        }
        else if (currentPos.x > stage.layout.width + stage.layout.x)
        {
            offset = stage.layout.width + stage.layout.x - currentPos.x;
        }

        if (Mathf.Abs(offset) > overlapThreshold)
        {
            currentPos.x += offset;
        }
    }

    void AdjustPositionY(Vector2 currentPos)
    {
        float offset = 0;
        if (currentPos.y < stage.layout.y)
        {
            offset = stage.layout.y - currentPos.y;
        }
        else if (currentPos.y > stage.layout.height + stage.layout.y)
        {
            offset = stage.layout.height + stage.layout.y - currentPos.y;
        }

        if (Mathf.Abs(offset) > overlapThreshold)
        {
            currentPos.y += offset;
        }
    }

}
