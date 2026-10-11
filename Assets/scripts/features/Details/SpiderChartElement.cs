using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

namespace FazoGame.Features.KidDetails
{
    // One labeled axis on the chart, with a value from 0 to 1.
    public struct SpiderAxis
    {
        public string Label;
        public float Value01;

        public SpiderAxis(string label, float value01)
        {
            Label = label;
            Value01 = Mathf.Clamp01(value01);
        }
    }

    // A custom-drawn VisualElement. Register it as a UXML factory so it
    // can also be placed via the Builder later, once proven.
    public class SpiderChartElement : VisualElement
    {
        public new class UxmlFactory : UxmlFactory<SpiderChartElement, UxmlTraits> { }

        private List<SpiderAxis> axes = new List<SpiderAxis>();

        private readonly Color gridColor = new Color(0.85f, 0.82f, 0.76f); // matches EDE0D0-ish
        private readonly Color fillColor = new Color(0.5f, 0.47f, 0.87f, 0.35f); // #7F77DD, translucent
        private readonly Color strokeColor = new Color(0.33f, 0.29f, 0.68f); // #534AB7

        public SpiderChartElement()
        {
            generateVisualContent += OnGenerateVisualContent;
        }

        // Call this whenever the underlying data changes, then trigger a redraw.
        public void SetData(List<SpiderAxis> newAxes)
        {
            axes = newAxes;
            MarkDirtyRepaint();
        }

        private void OnGenerateVisualContent(MeshGenerationContext context)
        {
            if (axes == null || axes.Count < 3)
                return; // a polygon needs at least 3 points to be meaningful

            Rect rect = contentRect;
            if (rect.width <= 0 || rect.height <= 0)
                return;

            Vector2 center = new Vector2(rect.width / 2f, rect.height / 2f);
            float radius = Mathf.Min(rect.width, rect.height) / 2f * 0.8f; // 0.8 leaves room for labels
            int count = axes.Count;
            float angleStep = 360f / count;

            var painter = context.painter2D;

            // --- Draw grid rings (25%, 50%, 75%, 100%) ---
            for (float ring = 0.25f; ring <= 1f; ring += 0.25f)
            {
                painter.strokeColor = gridColor;
                painter.lineWidth = 1f;
                painter.BeginPath();
                for (int i = 0; i <= count; i++)
                {
                    float angle = Mathf.Deg2Rad * (angleStep * i - 90f);
                    Vector2 point = center + new Vector2(Mathf.Cos(angle), Mathf.Sin(angle)) * radius * ring;
                    if (i == 0) painter.MoveTo(point);
                    else painter.LineTo(point);
                }
                painter.Stroke();
            }

            // --- Draw axis lines from center to edge ---
            for (int i = 0; i < count; i++)
            {
                float angle = Mathf.Deg2Rad * (angleStep * i - 90f);
                Vector2 point = center + new Vector2(Mathf.Cos(angle), Mathf.Sin(angle)) * radius;
                painter.strokeColor = gridColor;
                painter.lineWidth = 1f;
                painter.BeginPath();
                painter.MoveTo(center);
                painter.LineTo(point);
                painter.Stroke();
            }

            // --- Draw the data polygon (filled + outlined) ---
            painter.BeginPath();
            for (int i = 0; i <= count; i++)
            {
                int idx = i % count;
                float angle = Mathf.Deg2Rad * (angleStep * idx - 90f);
                float value = axes[idx].Value01;
                Vector2 point = center + new Vector2(Mathf.Cos(angle), Mathf.Sin(angle)) * radius * value;
                if (i == 0) painter.MoveTo(point);
                else painter.LineTo(point);
            }
            painter.fillColor = fillColor;
            painter.Fill();

            painter.strokeColor = strokeColor;
            painter.lineWidth = 2.5f;
            painter.BeginPath();
            for (int i = 0; i <= count; i++)
            {
                int idx = i % count;
                float angle = Mathf.Deg2Rad * (angleStep * idx - 90f);
                float value = axes[idx].Value01;
                Vector2 point = center + new Vector2(Mathf.Cos(angle), Mathf.Sin(angle)) * radius * value;
                if (i == 0) painter.MoveTo(point);
                else painter.LineTo(point);
            }
            painter.Stroke();

            // Data point dots
            for (int i = 0; i < count; i++)
            {
                float angle = Mathf.Deg2Rad * (angleStep * i - 90f);
                float value = axes[i].Value01;
                Vector2 point = center + new Vector2(Mathf.Cos(angle), Mathf.Sin(angle)) * radius * value;
                painter.fillColor = strokeColor;
                painter.BeginPath();
                painter.Arc(point, 4f, 0, 360);
                painter.Fill();
            }
        }
    }
}
