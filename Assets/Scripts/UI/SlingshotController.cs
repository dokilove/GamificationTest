using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
public class SlingshotController : MouseManipulator
{
    Controller controller;

    public Vector2 startGlobalPos;
    public Vector2 targetLayoutPos;
    Vector2 diff;

    VisualElement root;
    public SlingshotController(VisualElement root, Controller controller)
    {
        this.root = root;
        this.controller = controller;
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

        diff = evt.mousePosition - targetLayoutPos;
        //diff = diff.normalized;
        float angleRadian = -Mathf.Atan2(diff.x, diff.y);
        //Debug.Log(diffFromTarget.magnitude);

        float targetHalfHeight = target.layout.height * 0.5f;

        float scale = (diff.magnitude + targetHalfHeight) / targetHalfHeight;

        target.transform.scale = scale < 1.0f ? Vector3.one : new Vector3(1.0f, scale, 1.0f);

        target.transform.rotation =  Quaternion.AngleAxis(Mathf.Rad2Deg * angleRadian, Vector3.forward);

        //target.transform.position += new Vector3(diff.x, diff.y, 0.0f);

        //Debug.Log(target.style.top + " " + target.style.left + " " + target.transform.position);
    }

    void OnMouseUp(MouseUpEvent evt)
    {
        MoveController();
    }

    void OnMouseLeave(MouseLeaveEvent evt)
    {
        MoveController();
    }

    void MoveController()
    {
        if (!target.HasMouseCapture())
            return;

        target.ReleaseMouse();
        target.transform.scale = Vector3.one;

        controller.MoveToTarget(target, diff);

        //target.style.top = target.layout.y - direction.y;
        //target.style.left = target.layout.x - direction.x;
    }
}
