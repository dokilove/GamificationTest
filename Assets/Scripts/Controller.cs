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
            // Rect 안에 있는지 체크합니다.
            bool isInside = stage.layout.Contains(currentPos);

            // Rect 안에 있지 않으면 방향을 바꿔줍니다.
            if (!isInside)
            {
                // 현재 위치를 기준으로 Rect와 가장 가까운 점을 구합니다.
                Vector2 closestPoint = ClosestPoint(stage.layout, currentPos);

                // 가장 가까운 점과 현재 위치의 차이 벡터를 구합니다.
                Vector2 difference = currentPos - closestPoint;

                // 차이 벡터를 reflection vector로 변환합니다.
                Vector2 reflection = Vector2.Reflect(difference, stage.layout.center - closestPoint).normalized;

                // 새로운 방향을 reflection vector로 설정합니다.
                direction = reflection;
            }

            currentPos += direction * velocity * speed * Time.deltaTime;

            Debug.Log(stage.layout.Contains(icon.layout.center));

            velocity -= friction;

            icon.style.left = currentPos.x - icon.layout.width * 0.5f;
            icon.style.top = currentPos.y - icon.layout.height * 0.5f;
      
            yield return null;
        }
    }
    public static Vector2 ClosestPoint(Rect rect, Vector2 point)
    {
        Vector2 closestPoint = point;
        closestPoint.x = Mathf.Clamp(closestPoint.x, rect.xMin, rect.xMax);
        closestPoint.y = Mathf.Clamp(closestPoint.y, rect.yMin, rect.yMax);
        if (!rect.Contains(point))
        {
            if (point.x < rect.xMin)
                closestPoint.x = rect.xMin;
            else if (point.x > rect.xMax)
                closestPoint.x = rect.xMax;
            if (point.y < rect.yMin)
                closestPoint.y = rect.yMin;
            else if (point.y > rect.yMax)
                closestPoint.y = rect.yMax;
        }
        return closestPoint;
    }

}
