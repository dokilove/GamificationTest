using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class Colorizer : PointerManipulator
{
    public Colorizer()
    {
        activators.Add(new ManipulatorActivationFilter { modifiers = EventModifiers.Control});
    }

    Vector3 startPos;

    protected override void RegisterCallbacksOnTarget()
    {
        target.RegisterCallback<PointerOverEvent>(OnPointerOver);
        target.RegisterCallback<PointerMoveEvent>(OnPointerMove);
        //target.RegisterCallback<PointerUpEvent>(OnPointerUp);
        target.RegisterCallback<PointerOutEvent>(OnPointerOut);
            
    }

    protected override void UnregisterCallbacksFromTarget()
    {
        target.UnregisterCallback<PointerOverEvent>(OnPointerOver);
        target.UnregisterCallback<PointerMoveEvent>(OnPointerMove);
        //target.UnregisterCallback<PointerUpEvent>(OnPointerUp);
        target.UnregisterCallback<PointerOutEvent>(OnPointerOut);
    }

    void OnPointerOver(PointerOverEvent evt)
    {
        if (CanStartManipulation(evt))
        {
            target.style.backgroundColor = new Color(0, 1, 0);
        }
        else
        {
            target.style.backgroundColor = new Color(1, 0, 0);
        }

        startPos = evt.position;

        Debug.Log(startPos + " " + target.transform.position);


        //Debug.Log(startPos+ " " + target.layout.x + " " + target.layout.y + " " + target.transform.position);

        //target.transform.position += new Vector3(10.0f, 0, 0);
        target.CapturePointer(1);
    }

    void OnPointerMove(PointerMoveEvent evt)
    {
        if (CanStartManipulation(evt))
        {
            target.style.backgroundColor = new Color(0, 1, 1);
        }
        else
        {
            target.style.backgroundColor = new Color(1, 1, 0);
        }

        if (target.HasPointerCapture(1))
        {

            Vector3 diff = evt.position - startPos;

            Debug.Log(diff);

            target.transform.position += diff;
            startPos = evt.position;

            //target.transform.position = target.transform.position + diff;

            //target.transform.position = evt.position;
        }
    }

    void OnPointerUp(PointerUpEvent evt)
    {
        target.style.backgroundColor = new Color(0.5f, 0.5f, 0.7f);
    }

    void OnPointerOut(PointerOutEvent evt)
    {
        target.style.backgroundColor = new Color(0.7f, 0.5f, 0.5f);
    }
}
