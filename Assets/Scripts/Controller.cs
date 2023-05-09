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
    Label debugLabel;

    public void SetStage(VisualElement stage, List<VisualElement> otherObjects, Label debugLabel)
    {
        this.stage = stage;
        this.otherObjects = otherObjects;
        this.debugLabel = debugLabel;
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
        float startVelocity = velocity;

        Vector2 currentPos = initialPos;

        float diffX = -diff.x;
        float diffY = -diff.y;
        Vector2 direction = new Vector2(diffX, diffY).normalized;

        while (velocity > 0.0f)
        {
            currentPos += direction * velocity * speed * Time.deltaTime;

            bool hasCollision = false;
            bool isInsideStage = stage.layout.Contains(currentPos);

            if (isInsideStage)
            {
                for (int i = 0; i < otherObjects.Count; ++i)
                {
                    Rect obstacleRect = otherObjects[i].layout;

                    if (PointRectCollision(currentPos, obstacleRect))
                    {
                        hasCollision = true;

                        // Calculate the normal vector of the obstacle
                        // Calculate the normal vector of the obstacle
                        const float tolerance = 100.0f;
                        Vector2 normal = new Vector2(
                            Mathf.Abs(currentPos.x - obstacleRect.xMin) < tolerance ? -1.0f :
                            Mathf.Abs(currentPos.x - obstacleRect.xMax) < tolerance ? 1.0f : 0.0f,
                            Mathf.Abs(currentPos.y - obstacleRect.yMin) < tolerance ? -1.0f :
                            Mathf.Abs(currentPos.y - obstacleRect.yMax) < tolerance ? 1.0f : 0.0f);

                        // Calculate the reflection vector
                        Vector2 reflection = Vector2.Reflect(direction, normal);

                        // Update the direction
                        direction = reflection;

                        // Move the current position to the closest point on the obstacle boundary
                        float distanceToBoundary = Mathf.Min(Mathf.Abs(currentPos.x - obstacleRect.xMin), Mathf.Abs(currentPos.x - obstacleRect.xMax), Mathf.Abs(currentPos.y - obstacleRect.yMin), Mathf.Abs(currentPos.y - obstacleRect.yMax));
                        currentPos += reflection.normalized * (distanceToBoundary + 0.1f);

                    }
                }
            }

            if (!hasCollision && !isInsideStage)
            {
                // If the current position is outside the stage, move it to the closest point on the stage boundary
                //currentPos = ClosestPointOnRectBoundary(currentPos, stage.layout);

                Vector2 closestPoint = ClosestPointOnRectBoundary(currentPos, stage.layout);

                // Calculate the reflection vector
                Vector2 normal = (currentPos - closestPoint).normalized;
                Vector2 reflection = Vector2.Reflect(direction, normal);

                // Update the direction
                direction = reflection;

                currentPos = closestPoint + direction.normalized * (velocity * speed * Time.deltaTime - Vector2.Distance(currentPos, closestPoint));
            }

            velocity -= friction;

            icon.style.left = currentPos.x - icon.layout.width * 0.5f;
            icon.style.top = currentPos.y - icon.layout.height * 0.5f;

            yield return new WaitForFixedUpdate();

            debugLabel.text = "start Velocity: " + startVelocity + " friction: " + friction +  "\nvelocity: " + velocity;
        }
    }


    //IEnumerator MoveToTargetCoroutine(VisualElement icon, Vector2 diff)
    //{
    //    Vector2 initialPos = new Vector2(icon.layout.center.x, icon.layout.center.y);
    //    Vector2 targetPos = new Vector2(icon.layout.center.x - diff.x, icon.layout.center.y - diff.y);

    //    float velocity = (initialPos - targetPos).magnitude;

    //    Vector2 currentPos = initialPos;

    //    float diffX = -diff.x;
    //    float diffY = -diff.y;
    //    Vector2 direction = new Vector2(diffX, diffY).normalized;

    //    while (velocity > 0.0f)
    //    {

    //        currentPos += direction * velocity * speed * Time.deltaTime;

    //        // Rect �ȿ� ���� ������ ������ �ٲ��ݴϴ�.
    //        if (!stage.layout.Contains(currentPos))
    //        {
    //            // Find the closest point on the rect boundary
    //            Vector2 closestPoint = ClosestPointOnRectBoundary(currentPos, stage.layout);

    //            // Calculate the reflection vector
    //            Vector2 normal = (currentPos - closestPoint).normalized;
    //            Vector2 reflection = Vector2.Reflect(direction, normal);

    //            // Update the direction
    //            direction = reflection;
                
    //            currentPos = closestPoint + direction.normalized * (velocity * speed * Time.deltaTime - Vector2.Distance(currentPos, closestPoint));
    //        }
    //        else if (null != otherObjects)
    //        {
    //            for (int i = 0; i < otherObjects.Count; ++i)
    //            {
    //                VisualElement otherObject = otherObjects[i];
    //                if (PointRectCollision(currentPos, otherObject.layout))
    //                {
    //                    // Find the closest point on the rect boundary
    //                    Vector2 closestPoint = ClosestPointOnRectBoundary(currentPos, otherObject.layout);

    //                    // Calculate the reflection vector
    //                    Vector2 normal = (currentPos - closestPoint).normalized;
    //                    Vector2 reflection = Vector2.Reflect(direction, normal);
    //                    //Debug.Log("normal " + normal);
    //                    // Update the direction
    //                    direction = reflection;

    //                    currentPos = closestPoint + direction.normalized * (velocity * speed * Time.deltaTime - Vector2.Distance(currentPos, closestPoint));
    //                }
    //            }
    //        }

    //        //Debug.Log(stage.layout.Contains(icon.layout.center));

    //        velocity -= friction;

    //        icon.style.left = currentPos.x - icon.layout.width * 0.5f;
    //        icon.style.top = currentPos.y - icon.layout.height * 0.5f;
      
    //        yield return null;
    //    }
    //}

    private Vector2 ClosestPointOnRectBoundary(Vector2 point, Rect rect)
    {
        //if (rect.Contains(point))
        //{
        //    return point;
        //}

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
    bool PointRectCollision(Vector2 point, Rect rect)
    {
        if (point.x < rect.xMin || point.x > rect.xMax ||
            point.y < rect.yMin || point.y > rect.yMax)
        {
            // point is outside rect bounds
            return false;
        }

        return true;
    }

}
