using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
public class SlingshotController : MouseManipulator
{
    public Vector2 startGlobalPos;
    public Vector2 targetLayoutPos;
    Vector2 diffFromTarget;

    VisualElement root;
    public SlingshotController(VisualElement root)
    {
        this.root = root;
    }

    
    protected override void RegisterCallbacksOnTarget()
    {
        target.RegisterCallback<MouseDownEvent>(OnMouseDown);
        target.RegisterCallback<MouseMoveEvent>(OnMouseMove);
        target.RegisterCallback<MouseUpEvent>(OnMouseUp);
        target.RegisterCallback<MouseLeaveEvent>(OnMouseLeave);
    }

    protected override void UnregisterCallbacksFromTarget()
    {
        target.UnregisterCallback<MouseDownEvent>(OnMouseDown);
        target.UnregisterCallback<MouseMoveEvent>(OnMouseMove);
        target.UnregisterCallback<MouseUpEvent>(OnMouseUp);
        target.UnregisterCallback<MouseLeaveEvent>(OnMouseLeave);
    }

    void OnMouseDown(MouseDownEvent evt)
    {

        startGlobalPos = evt.mousePosition;
        targetLayoutPos = target.layout.center;
        //Debug.Log(targetLayoutPos);

        target.CaptureMouse();
    }

    void OnMouseMove(MouseMoveEvent evt)
    {
        if (!target.HasMouseCapture())
            return;

        //Vector2 diff = evt.mousePosition - startGlobalPos;

        diffFromTarget = evt.mousePosition - targetLayoutPos;
        //diff = diff.normalized;
        float angleRadian = -Mathf.Atan2(diffFromTarget.x, diffFromTarget.y);
        //Debug.Log(diffFromTarget.magnitude);

        float targetHalfHeight = target.layout.height * 0.5f;

        float scale = (diffFromTarget.magnitude + targetHalfHeight) / targetHalfHeight;

        target.transform.scale = scale < 1.0f ? Vector3.one : new Vector3(1.0f, scale, 1.0f);

        target.transform.rotation =  Quaternion.AngleAxis(Mathf.Rad2Deg * angleRadian, Vector3.forward);

        //target.transform.position += new Vector3(diff.x, diff.y, 0.0f);

        Debug.Log(diffFromTarget.x + " " + diffFromTarget.y + " " + target.transform.position);
    }

    void OnMouseUp(MouseUpEvent evt)
    {
        if (!target.HasMouseCapture())
            return;


        target.ReleaseMouse();
        target.transform.scale = Vector3.one;
        //target.transform.rotation = Quaternion.identity;

        //target.transform.position += new Vector3(-diffFromTarget.x, -diffFromTarget.y, 0.0f);
        target.style.top = target.layout.y - diffFromTarget.y;
        target.style.left = target.layout.x - diffFromTarget.x;
    }

    void OnMouseLeave(MouseLeaveEvent evt)
    {
        if (!target.HasMouseCapture())
            return;


        target.ReleaseMouse();
        target.transform.scale = Vector3.one;

        //target.transform.position += new Vector3(-diffFromTarget.x, -diffFromTarget.y, 0.0f);

        target.style.top = target.layout.y - diffFromTarget.y;
        target.style.left = target.layout.x - diffFromTarget.x;
    }
}
