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

        VisualElement stage = root.Q<VisualElement>("stage");
        controller.SetStage(stage);

        VisualElement test = root.Q<VisualElement>("TestElem");

        test.AddManipulator(new MouseManipulatorTest());

        VisualElement elasticBand = root.Q<VisualElement>("elastic_band");
        elasticBand.AddManipulator(new SlingshotController(root, controller));
    }
}
