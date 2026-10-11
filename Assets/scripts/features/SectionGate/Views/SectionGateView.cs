using UnityEngine;
using UnityEngine.UIElements;

[RequireComponent(typeof(UIDocument))]
public class SectionGateView : MonoBehaviour
{
    [SerializeField] private VisualTreeAsset kidIdentityBadgeTemplate;
    [SerializeField] private Sprite discoverRobotSprite;
    [SerializeField] private Sprite combineRobotSprite;
    [SerializeField] private Sprite applyRobotSprite;

    private VisualElement root;
    private VisualElement headerRoot;

    private int currentPoints = 120;
    private int discoverPointsThreshold = 0;
    private int combinePointsThreshold = 100;
    private int applyPointsThreshold = 300;

    private void OnEnable()
    {
        root = GetComponent<UIDocument>().rootVisualElement;
        headerRoot = root.Q<VisualElement>("header");

        Button backButton = root.Q<Button>("back-button");
        Button silenceButton = root.Q<Button>("silence-button");
        //Button videoButton = root.Q<Button>("video-button");

        backButton.clicked += () => SceneNavigator.Instance.Navigate(GameScene.Landing);


   

        silenceButton.clicked += OnSilenceClicked;
      

        ConfigureMeters();
        BuildKidIdentity();
        BuildSectionCards();
    }

    private void ConfigureMeters()
    {
        VisualElement points = root.Q<VisualElement>("points-instance");
        if (points != null)
        {
            VisualElement pointsFill = points.Q<VisualElement>("points-fill");
            float progress = Mathf.Clamp01((float)currentPoints / combinePointsThreshold);
            pointsFill.style.width = new Length(progress * 100f, LengthUnit.Percent);
        }
    }

    private void BuildKidIdentity()
    {
        if (kidIdentityBadgeTemplate != null)
        {
            VisualElement badge = kidIdentityBadgeTemplate.Instantiate();
            headerRoot.Add(badge);
        }
    }

    private void BuildSectionCards()
    {
        ConfigureCard(root.Q<VisualElement>("card-learn"), "Learn", "Discover new things!", discoverRobotSprite, discoverPointsThreshold, GameScene.DiscoveryMap);
        ConfigureCard(root.Q<VisualElement>("card-combine"), "Combine", "Mix it all up!", combineRobotSprite, combinePointsThreshold, GameScene.DiscoveryMap);
        ConfigureCard(root.Q<VisualElement>("card-apply"), "Apply", "Show what you know!", applyRobotSprite, applyPointsThreshold, GameScene.DiscoveryMap);
    }

    private void ConfigureCard(VisualElement cardInstance, string title, string subtitle, Sprite robotSprite, int threshold, GameScene destination)
    {
        if (cardInstance == null)
        {
            Debug.LogWarning($"[SectionGateView] Card instance for '{title}' not found in UXML");
            return;
        }

        SectionCardController controller = new SectionCardController(cardInstance);

        SectionState state;
        if (threshold == 0)
            state = SectionState.Unlocked;
        else if (currentPoints >= threshold)
            state = SectionState.LockedReadyToUnlock;
        else
            state = SectionState.LockedInsufficientPoints;

        float progress = state == SectionState.Unlocked ? 0.4f : 0f;

        controller.Configure(
            title, subtitle, robotSprite, state,
            pointsRequired: threshold,
            progress01: progress,
            onUnlock: () => controller.Configure(title, subtitle, robotSprite, SectionState.Unlocked, progress01: 0f)
        );

        if (state == SectionState.Unlocked)
        {
            cardInstance.RegisterCallback<ClickEvent>(evt => SceneNavigator.Instance.Navigate(GameScene.DiscoveryMap, needsBackdrop: false));
        }
    }

    private void OnSilenceClicked()
    {
        Debug.Log("[SectionGateView] Silence toggled — wire to AudioSystem");
    }

    private void OnVideoClicked()
    {
        Debug.Log("[SectionGateView] Video library requested — not yet built");
    }
}