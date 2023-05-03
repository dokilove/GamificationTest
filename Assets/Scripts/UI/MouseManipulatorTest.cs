using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class MouseManipulatorTest : MouseManipulator
{
    public MouseManipulatorTest()
    {
        activators.Add(new ManipulatorActivationFilter { modifiers = EventModifiers.Control });
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

        if (CanStartManipulation(evt))
        {
            target.style.backgroundColor = new Color(0, 1, 0);
        }
        else
        {
            target.style.backgroundColor = new Color(1, 0, 0);
        }
    }

    void OnMouseMove(MouseMoveEvent evt)
    {

        if (CanStartManipulation(evt))
        {
            target.style.backgroundColor = new Color(0, 1, 1);
        }
        else
        {
            target.style.backgroundColor = new Color(1, 1, 0);
        }
    }

    void OnMouseUp(MouseUpEvent evt)
    {
        target.style.backgroundColor = new Color(0.5f, 0.5f, 0.7f);

    }
}
