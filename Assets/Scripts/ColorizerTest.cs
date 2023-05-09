using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class ColorizerTest : MonoBehaviour
{
    public Controller controller;
    private void OnEnable()
    {
        VisualElement root = GetComponent<UIDocument>().rootVisualElement;

        Label debugLabel = root.Q<Label>("label_debug");

        List<VisualElement> otherObjects = new List<VisualElement>();
        VisualElement test1 = root.Q<VisualElement>("TestElem1");
        test1.AddManipulator(new MouseManipulatorTest(debugLabel));
        otherObjects.Add(test1);
        VisualElement test2 = root.Q<VisualElement>("TestElem2");
        test2.AddManipulator(new MouseManipulatorTest(debugLabel));
        otherObjects.Add(test2);
        VisualElement test3 = root.Q<VisualElement>("TestElem3");
        test3.AddManipulator(new MouseManipulatorTest(debugLabel));
        otherObjects.Add(test3);

        List<BreakableItem> otherCircles = new List<BreakableItem>();
        VisualElement item1 = root.Q<VisualElement>("TestItem1");
        BreakableItem breakableItem1 = new BreakableItem(item1, controller);
        otherCircles.Add(breakableItem1);
        VisualElement item2 = root.Q<VisualElement>("TestItem2");
        BreakableItem breakableItem2 = new BreakableItem(item2, controller);
        otherCircles.Add(breakableItem2);

        VisualElement stage = root.Q<VisualElement>("stage");
        controller.SetStage(stage, otherObjects, otherCircles,  debugLabel);


        VisualElement elasticBand = root.Q<VisualElement>("elastic_band");
        elasticBand.AddManipulator(new SlingshotController(root, controller, debugLabel));
    }
}
