using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class Controller : MonoBehaviour
{
    public float speed = 10.0f;
    public float friction = 100.0f;

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
        Vector2 initialPos = new Vector2(icon.layout.x, icon.layout.y);
        Vector2 targetPos = new Vector2(icon.layout.x - diff.x, icon.layout.y - diff.y);

        float velocity = (initialPos - targetPos).magnitude;

        Vector2 currentPos = initialPos;

        float diffX = -diff.x;
        float diffY = -diff.y;
        Vector2 direction = new Vector2(diffX, diffY).normalized;

        while (velocity > 0.0f)
        {

            //if (currentPos.x < 0.0f || currentPos.x > root.layout.width)
            //{
            //    Debug.Log(root.layout.width + " " + root.layout.height);
            //    Debug.Log(currentPos.x + " " + currentPos.y);
            //    diffX = diff.x;
            //}

            //if (currentPos.y < 0.0f || currentPos.y > root.layout.height)
            //{
            //    Debug.Log(root.layout.width + " " + root.layout.height);
            //    Debug.Log(currentPos.x + " " + currentPos.y);
            //    diffY = diff.y;
            //}

            currentPos += direction * velocity * speed * Time.deltaTime;

            icon.style.left = currentPos.x;
            icon.style.top = currentPos.y;

            Debug.Log(velocity);
            velocity -= friction;

            Debug.Log(stage.layout.x + " " + stage.layout.width + " " + stage.layout.y +  " " + stage.layout.height);

            if (currentPos.x < stage.layout.x || currentPos.x > stage.layout.width - stage.layout.x) 
            {
                direction.x = -direction.x;
            }
            if (currentPos.y < stage.layout.y || currentPos.y > stage.layout.height - stage.layout
                .y)
            {
                direction.y = -direction.y;
            }

            yield return null;
        }

        //icon.style.top = targetPos.y;
        //icon.style.left = targetPos.x;
    }
    
}
