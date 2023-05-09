using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class MouseManipulatorTest : MouseManipulator
{
    public Vector2 startPos;
    Label debugLabel;

    public MouseManipulatorTest(Label debugLabel)
    {
        activators.Add(new ManipulatorActivationFilter { clickCount = 2 });
        this.debugLabel = debugLabel;
    }
    protected override void RegisterCallbacksOnTarget()
    {
        target.RegisterCallback<MouseDownEvent>(OnMouseDown);
        target.RegisterCallback<MouseMoveEvent>(OnMouseMove);
        target.RegisterCallback<MouseUpEvent>(OnMouseUp);        
    }

    protected override void UnregisterCallbacksFromTarget()
    {
        target.UnregisterCallback<MouseDownEvent>(OnMouseDown);
        target.UnregisterCallback<MouseMoveEvent>(OnMouseMove);
        target.UnregisterCallback<MouseUpEvent>(OnMouseUp);
    }

    void OnMouseDown(MouseDownEvent evt)
    {

        //if (CanStartManipulation(evt))
        //{
        //    target.style.backgroundColor = new Color(0, 1, 0);
        //}
        //else
        //{
        //    target.style.backgroundColor = new Color(1, 0, 0);
        //}

        startPos = evt.localMousePosition;

        target.CaptureMouse();
    }

    void OnMouseMove(MouseMoveEvent evt)
    {
        if (!target.HasMouseCapture())
            return;

        Vector2 diff = evt.localMousePosition - startPos;

        //target.transform.position += new Vector3(diff.x, diff.y, 0.0f);

        target.style.left = target.layout.x + diff.x;
        target.style.top = target.layout.y + diff.y;
        debugLabel.text = "mousePosition.x: " + evt.mousePosition.x + "\nmousePosition.y: " + evt.mousePosition.y;
    }

    void OnMouseUp(MouseUpEvent evt)
    {
        if (!target.HasMouseCapture())
            return;

        target.ReleaseMouse();

    }
}
