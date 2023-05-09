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

        float radius = icon.layout.width * 0.5f;

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

                    if (RectCircleCollision(currentPos, radius, obstacleRect))
                    {
                        hasCollision = true;

                        Vector2 normal = GetCollisionNormal(currentPos, radius, obstacleRect);

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

    //        // Rect 안에 있지 않으면 방향을 바꿔줍니다.
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
    bool PointRectCollision(float radius, Vector2 point, Rect rect)
    {
        if (point.x + radius < rect.xMin || point.x - radius > rect.xMax ||
            point.y + radius < rect.yMin || point.y - radius > rect.yMax)
        {
            // point is outside rect bounds
            return false;
        }

        return true;
    }

    bool RectCircleCollision(Vector2 circleCenter, float circleRadius, Rect rect)
    {
        Vector2 closestPoint = new Vector2(
            Mathf.Clamp(circleCenter.x, rect.xMin, rect.xMax),
            Mathf.Clamp(circleCenter.y, rect.yMin, rect.yMax)
        );

        float distance = Vector2.Distance(circleCenter, closestPoint);

        return distance < circleRadius;
    }
    public Vector2 GetCollisionNormal(Vector2 circlePos, float circleRadius, Rect rect)
    {
        Vector2 rectCenter = rect.center;
        float rectWidth = rect.width;
        float rectHeight = rect.height;

        // calculate distance between circle center and rect center
        float distX = Mathf.Abs(circlePos.x - rectCenter.x);
        float distY = Mathf.Abs(circlePos.y - rectCenter.y);

        // if the distance between the centers is less than half the width and half the height
        // of the rect, then there is a collision
        if (distX <= rectWidth / 2 && distY <= rectHeight / 2)
        {
            // calculate the normal vector based on the side of the rect that the circle collided with
            float deltaX = circlePos.x - rectCenter.x;
            float deltaY = circlePos.y - rectCenter.y;

            float absDeltaX = Mathf.Abs(deltaX);
            float absDeltaY = Mathf.Abs(deltaY);

            float xSign = Mathf.Sign(deltaX);
            float ySign = Mathf.Sign(deltaY);

            Vector2 normal = Vector2.zero;

            if (absDeltaX > absDeltaY)
            {
                normal.x = xSign;
            }
            else if (absDeltaX < absDeltaY)
            {
                normal.y = ySign;
            }
            else
            {
                normal.x = xSign;
                normal.y = ySign;
            }

            return normal.normalized;
        }

        return Vector2.zero;
    }


}
