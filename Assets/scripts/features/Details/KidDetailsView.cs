using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using FazoGame.Features.KidDetails;

[RequireComponent(typeof(UIDocument))]
public class KidDetailsView : MonoBehaviour
{
    [SerializeField] private VisualTreeAsset kidSummaryCardTemplate;
    [SerializeField] private VisualTreeAsset responsibleBadgeTemplate;
    [SerializeField] private VisualTreeAsset domainTabButtonTemplate;
    [SerializeField] private VisualTreeAsset activityNodeCardTemplate; // new field

    private readonly List<ActivityNodeCardController> currentCards = new List<ActivityNodeCardController>();

    private VisualElement root;
    private VisualElement kidCardSlot;
    private VisualElement spiderChartSlot;
    private VisualElement domainTabRow;
    private VisualElement activitySection;

    private DomainTabController domainTabController;

    private void OnEnable()
    {
        root = GetComponent<UIDocument>().rootVisualElement;

        Button backButton = root.Q<Button>("back-button");
        backButton.clicked += () => SceneNavigator.Instance.Navigate(GameScene.WhosPlaying);

        kidCardSlot = root.Q<VisualElement>("kid-card-slot");
        spiderChartSlot = root.Q<VisualElement>("spider-chart-slot");
        domainTabRow = root.Q<VisualElement>("domain-tab-row");
        activitySection = root.Q<VisualElement>("activity-section");

        BuildHeaderProfile();
        BuildKidSummaryCard();
        BuildSpiderChart();
        BuildDomainTabs();
    }

    private void BuildHeaderProfile()
    {
        VisualElement headerRoot = root.Q<VisualElement>("header");
        VisualElement badgeInstance = responsibleBadgeTemplate.Instantiate();
       

        var controller = new ResponsibleProfileBadgeController(badgeInstance, () =>
        {
            Debug.Log("[KidDetailsView] Responsible profile clicked — show logout dialog (placeholder)");
        });
    }

    private void BuildKidSummaryCard()
    {
        VisualElement cardInstance = kidSummaryCardTemplate.Instantiate();
        kidCardSlot.Add(cardInstance);

        var controller = new KidSummaryCardController(cardInstance);
        controller.Configure(name: "Youssef", age: 8, level: 3, photo: null); // placeholder data
    }

    private void BuildSpiderChart()
    {
        var chart = new SpiderChartElement();
        chart.style.flexGrow = 1;
        chart.style.minHeight = 280;
        spiderChartSlot.Add(chart);

        var testData = new List<SpiderAxis>
        {
            new SpiderAxis("Shapes", 0.85f),
            new SpiderAxis("Animals", 0.9f),
            new SpiderAxis("Colors", 0.5f),
            new SpiderAxis("Patterns", 0.35f),
            new SpiderAxis("Emotions", 0.65f),
        };
        chart.SetData(testData);
    }

    private void BuildDomainTabs()
    {
        domainTabController = new DomainTabController(domainTabRow, domainTabButtonTemplate);

        var domains = new List<DomainTabData>
        {
            new DomainTabData("shapes", "Shapes", "icon-shapes"),
            new DomainTabData("animals", "Animals", "icon-animals"),
            new DomainTabData("colors", "Colors", "icon-colors"),
            new DomainTabData("patterns", "Patterns", "icon-patterns"),
            new DomainTabData("emotions", "Emotions", "icon-emotions"),
        };

        domainTabController.BuildTabs(domains, OnDomainSelected);
    }


    private void OnDomainSelected(string domainId)
    {
        activitySection.Clear();
        currentCards.Clear();

        // Placeholder data per domain — replace with real repository data later.
        var placeholderActivities = new[]
        {
        ("Square", "88%", SupportLevel.Low, FocusLevel.Ready),
        ("Triangle", "64%", SupportLevel.Medium, FocusLevel.KeepPracticing),
        ("Circle", "47%", SupportLevel.High, FocusLevel.NeedsSupport),
    };

        foreach (var (name, mastery, support, focus) in placeholderActivities)
        {
            VisualElement cardInstance = activityNodeCardTemplate.Instantiate();
            var controller = new ActivityNodeCardController(cardInstance);
            controller.Configure(name, mastery, support, focus);

            // Collapse every other card when one expands — accordion behavior.
            controller.OnExpandedRequested = expandedController =>
            {
                foreach (var c in currentCards)
                    if (c != expandedController) c.Collapse();
            };

            currentCards.Add(controller);
            activitySection.Add(cardInstance);
        }


    }
}
