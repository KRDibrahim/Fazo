using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

[RequireComponent(typeof(UIDocument))]
public class WhosPlayingView : MonoBehaviour
{
    [SerializeField] private VisualTreeAsset kidCardTemplate;
    [SerializeField] private CurrentKidSO currentKid; // assign the shared asset here

    [SerializeField] private Sprite responsiblePhoto; // assign your PNG here

    // NEW: assign your avatar sprites in the Inspector
    [SerializeField] private List<Sprite> availableAvatars;

    // NEW: a small pool of placeholder names to pick from randomly
    [SerializeField]
    private List<string> availableNames = new List<string>
    {
        "Amir", "Lina", "Youssef", "Nora", "Adam", "Sara"
    };

    private VisualElement root;
    private VisualElement kidsContainer;
    private VisualElement emptyState;

    // In-memory only for now — swap for a real repository call later
    // without touching anything below this list itself.
    private readonly List<KidData> kids = new List<KidData>();

    private void OnEnable()
    {
        Debug.Log("[WhosPlayingView] Enabled");
        root = GetComponent<UIDocument>().rootVisualElement;

        kidsContainer = root.Q<VisualElement>("kids-container");
        emptyState = root.Q<VisualElement>("empty-state");

        Button homeButton = root.Q<Button>("home-button");


        Button addKidButton = root.Q<Button>("add-kid-button");


        VisualElement headerRightGroup = root.Q<VisualElement>("header-right-group");
        ConfigureProfileBadge();

        addKidButton.clicked += OnAddKidButtonClicked;

        RenderKidList();
    }

    private void OnAddKidButtonClicked()
    {

        KidData newKid = new KidData(
             id: System.Guid.NewGuid().ToString(),
             name: GetRandomName(),
             avatar: GetRandomAvatar()
         );

        kids.Add(newKid);
        RenderKidList();
    }


    private string GetRandomName()
    {
        if (availableNames == null || availableNames.Count == 0)
            return $"Kid {kids.Count + 1}";

        int index = Random.Range(0, availableNames.Count);
        return availableNames[index];
    }

    private Sprite GetRandomAvatar()
    {
        if (availableAvatars == null || availableAvatars.Count == 0)
            return null;

        int index = Random.Range(0, availableAvatars.Count);
        return availableAvatars[index];
    }


    private void RenderKidList()
    {
        kidsContainer.Clear();

        if (kids.Count == 0)
        {
            kidsContainer.Add(emptyState);
            return;
        }

        foreach (KidData kid in kids)
        {
            VisualElement cardInstance = kidCardTemplate.Instantiate();
            KidCardController cardController = new KidCardController(cardInstance);

            cardController.Configure(
                kid,
                onSelect: OnKidSelected,
                onDetails: OnKidDetailsRequested,
                onSettings: OnKidSettingsRequested
            );

            kidsContainer.Add(cardInstance);
        }
    }

    private void OnKidSelected(KidData kid)
    {
        currentKid.Select(kid);
        Debug.Log($"[WhosPlayingView] Selected kid: {kid.Name}");
        SceneNavigator.Instance.Navigate(GameScene.Landing);
    }

    private void OnKidDetailsRequested(KidData kid)
    {
        currentKid.Select(kid); // so the Details screen knows which kid to show
        Debug.Log($"[WhosPlayingView] Details requested for: {kid.Name}");
        SceneNavigator.Instance.Navigate(GameScene.KidDetails); // wire once Details screen exists
    }

    private void OnKidSettingsRequested(KidData kid)
    {
        Debug.Log($"[WhosPlayingView] Kid-specific settings requested for: {kid.Name} — not yet designed");
    }


    private void ConfigureProfileBadge()
    {

        VisualElement responsibleBadgeInstance = root.Q<VisualElement>("responsible-badge-instance");
        var responsibleBadgeController = new ResponsibleProfileBadgeController(responsibleBadgeInstance, onClicked: () =>
        {
            Debug.Log("[WhosPlayingView] Responsible profile clicked — show account dialog (placeholder)");
        });
        responsibleBadgeController.SetPhoto(responsiblePhoto);

    }

}
