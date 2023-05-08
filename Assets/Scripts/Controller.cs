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

    List<VisualElement> otherObjects;

    public void SetStage(VisualElement stage, List<VisualElement> otherObjects)
    {
        this.stage = stage;
        this.otherObjects = otherObjects;
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

            // Rect 안에 있지 않으면 방향을 바꿔줍니다.
            if (!stage.layout.Contains(currentPos))
            {
                // Find the closest point on the rect boundary
                Vector2 closestPoint = ClosestPointOnRectBoundary(currentPos, stage.layout);

                // Calculate the reflection vector
                Vector2 normal = (currentPos - closestPoint).normalized;
                Vector2 reflection = Vector2.Reflect(direction, normal);

                // Update the direction
                direction = reflection;
                
                currentPos = closestPoint + direction.normalized * (velocity * speed * Time.deltaTime - Vector2.Distance(currentPos, closestPoint));
            }

            if (null != otherObjects)
            {
                for (int i = 0; i < otherObjects.Count; ++i)
                {
                    VisualElement otherObject = otherObjects[i];
                    if (otherObject.layout.Contains(currentPos))
                    {
                        // Find the closest point on the rect boundary
                        Vector2 closestPoint = ClosestPointOnRect(currentPos, otherObject.layout);

                        // Calculate the reflection vector
                        Vector2 normal = (currentPos - closestPoint).normalized;
                        Vector2 reflection = Vector2.Reflect(direction, normal);
                        //Debug.Log("normal " + normal);
                        // Update the direction
                        direction = reflection;

                        currentPos = closestPoint + direction.normalized * (velocity * speed * Time.deltaTime - Vector2.Distance(currentPos, closestPoint));
                    }
                }
            }

            //Debug.Log(stage.layout.Contains(icon.layout.center));

            velocity -= friction;

            icon.style.left = currentPos.x - icon.layout.width * 0.5f;
            icon.style.top = currentPos.y - icon.layout.height * 0.5f;
      
            yield return null;
        }
    }

    private Vector2 ClosestPointOnRectBoundary(Vector2 point, Rect rect)
    {
        if (rect.Contains(point))
        {
            return point;
        }

        float closestX = Mathf.Clamp(point.x, rect.xMin, rect.xMax);
        float closestY = Mathf.Clamp(point.y, rect.yMin, rect.yMax);

        if (Mathf.Abs(point.x - closestX) < Mathf.Abs(point.y - closestY))
        {
            closestY = point.y < rect.yMin ? rect.yMin : rect.yMax;
        }
        else
        {
            closestX = point.x < rect.xMin ? rect.xMin : rect.xMax;
        }

        return new Vector2(closestX, closestY);
    }

    private Vector2 ClosestPointOnRect(Vector2 point, Rect rect)
    {
        float closestX = Mathf.Clamp(point.x, rect.xMin, rect.xMax);
        float closestY = Mathf.Clamp(point.y, rect.yMin, rect.yMax);

        if (Mathf.Abs(point.x - closestX) < Mathf.Abs(point.y - closestY))
        {
            closestY = point.y < rect.yMin ? rect.yMin : rect.yMax;
        }
        else
        {
            closestX = point.x < rect.xMin ? rect.xMin : rect.xMax;
        }

        return new Vector2(closestX, closestY);
    }

}
