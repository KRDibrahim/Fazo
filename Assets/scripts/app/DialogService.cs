using UnityEngine;
using UnityEngine.UIElements;
[RequireComponent(typeof(UIDocument))]
public class DialogService : MonoBehaviour
{
    public static DialogService Instance { get; private set; }

    [SerializeField] private VisualTreeAsset authComingSoonTemplate;

    private VisualElement overlayRoot;

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;

        overlayRoot = GetComponent<UIDocument>().rootVisualElement;


        overlayRoot.style.position = Position.Relative;
        overlayRoot.style.flexGrow = 1;

        overlayRoot.style.display = DisplayStyle.None;


        overlayRoot = GetComponent<UIDocument>().rootVisualElement;


    }

    public void ShowAuthComingSoon(System.Action onFinished)
    {
        overlayRoot.Clear();
        VisualElement dialog = authComingSoonTemplate.Instantiate();
        overlayRoot.Add(dialog);
        overlayRoot.style.display = DisplayStyle.Flex;

        VisualElement card = dialog.Q<VisualElement>("dialog-card");
        dialog.schedule.Execute(() => card.AddToClassList("visible")).ExecuteLater(10);

        dialog.schedule.Execute(() =>
        {
            Hide();
            onFinished?.Invoke();
        }).ExecuteLater(4000);
    }

    private void Hide()
    {
        overlayRoot.style.display = DisplayStyle.None;
        overlayRoot.Clear();
    }
}
