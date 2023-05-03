using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class ColorizerTest : MonoBehaviour
{
    private void OnEnable()
    {
        VisualElement root = GetComponent<UIDocument>().rootVisualElement;

        VisualElement test = root.Q<VisualElement>("TestElem");

        test.AddManipulator(new MouseManipulatorTest());

        VisualElement elasticBand = root.Q<VisualElement>("elastic_band");
        elasticBand.AddManipulator(new SlingshotController(root));
    }
}
