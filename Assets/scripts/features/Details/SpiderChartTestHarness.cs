using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using FazoGame.Features.KidDetails;

// Attach to a UIDocument GameObject in a blank test scene, with an empty
// UXML (just a root VisualElement, no content needed) as its Source Asset.
[RequireComponent(typeof(UIDocument))]
public class SpiderChartTestHarness : MonoBehaviour
{
    private void OnEnable()
    {
        VisualElement root = GetComponent<UIDocument>().rootVisualElement;

        var chart = new SpiderChartElement();
        chart.style.width = 400;
        chart.style.height = 400;
        chart.style.alignSelf = Align.Center;
        chart.style.marginTop = 40;

        root.Add(chart);

        // Placeholder data — 6 domains, varying values, to confirm the
        // polygon draws correctly with an uneven, realistic-looking shape.
        var testData = new List<SpiderAxis>
        {
            new SpiderAxis("Shapes", 0.85f),
            new SpiderAxis("Animals", 0.9f),
            new SpiderAxis("Colors", 0.5f),
            new SpiderAxis("Patterns", 0.35f),
            new SpiderAxis("Emotions", 0.65f),
            new SpiderAxis("Fruits", 0.7f),
        };

        chart.SetData(testData);
    }
}
