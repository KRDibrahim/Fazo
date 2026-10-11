using UnityEngine;
using UnityEngine.UIElements;

[RequireComponent(typeof(UIDocument))]
public class SettingsView : MonoBehaviour
{
    [SerializeField] private VisualTreeAsset audioSettingsTemplate;
    [SerializeField] private VisualTreeAsset assistanceSettingsTemplate;
    [SerializeField] private VisualTreeAsset sessionSettingsTemplate;
    [SerializeField] private VisualTreeAsset languageSettingsTemplate;
    [SerializeField] private VisualTreeAsset plainWorkingSpaceTemplate;
    [SerializeField] private VisualTreeAsset lockableWorkingSpaceTemplate;

    private VisualElement root;
    private VisualElement workingSpaceContainer;

    private Button tabAudio, tabAssistance, tabSession, tabLanguage;

    private void OnEnable()
    {
        root = GetComponent<UIDocument>().rootVisualElement;
        workingSpaceContainer = root.Q<VisualElement>("working-space");

        Button backButton = root.Q<Button>("back-button");
        backButton.clicked += () => SceneNavigator.Instance.Navigate(GameScene.Landing);

        tabAudio = root.Q<Button>("tab-audio");
        tabAssistance = root.Q<Button>("tab-assistance");
        tabSession = root.Q<Button>("tab-session");
        tabLanguage = root.Q<Button>("tab-language");

        tabAudio.clicked += () => SelectTab("Audio", tabAudio);
        tabAssistance.clicked += () => SelectTab("Assistance", tabAssistance);
        tabSession.clicked += () => SelectTab("Session", tabSession);
        tabLanguage.clicked += () => SelectTab("Language", tabLanguage);

        SelectTab("Audio", tabAudio);
    }

    private void SelectTab(string tabTitle, Button selectedButton)
    {
        tabAudio.RemoveFromClassList("active");
        tabAssistance.RemoveFromClassList("active");
        tabSession.RemoveFromClassList("active");
        tabLanguage.RemoveFromClassList("active");
        selectedButton.AddToClassList("active");

        workingSpaceContainer.Clear();

        VisualTreeAsset templateToUse = GetTemplateForTab(tabTitle);
        VisualElement content = templateToUse.Instantiate();
        workingSpaceContainer.Add(content);

        if (IsTabLockable(tabTitle))
        {
            new LockableWorkingSpaceController(content);
        }

        switch (tabTitle)
        {
            case "Audio": WireAudioPanel(content); break;
            case "Assistance": WireAssistancePanel(content); break;
            case "Session": WireSessionPanel(content); break;
            case "Language": WireLanguagePanel(content); break;
        }
    }

    private VisualTreeAsset GetTemplateForTab(string tabTitle)
    {
        switch (tabTitle)
        {
            case "Audio": return audioSettingsTemplate;
            case "Assistance": return assistanceSettingsTemplate;
            case "Session": return sessionSettingsTemplate;
            case "Language": return languageSettingsTemplate;
            default: return plainWorkingSpaceTemplate;
        }
    }

    private bool IsTabLockable(string tabTitle) => tabTitle == "Assistance" || tabTitle == "Session";

    private void WireAudioPanel(VisualElement content)
    {
        Slider musicSlider = content.Q<Slider>("music-volume-slider");
        Slider sfxSlider = content.Q<Slider>("sfx-volume-slider");
        Toggle speechToggle = content.Q<Toggle>("speech-enable-toggle");
        Slider speechSlider = content.Q<Slider>("speech-volume-slider");

        musicSlider.RegisterValueChangedCallback(evt =>
            Debug.Log($"[AudioSettings] Music volume set to {evt.newValue:F2}"));

        sfxSlider.RegisterValueChangedCallback(evt =>
            Debug.Log($"[AudioSettings] SFX volume set to {evt.newValue:F2}"));

        speechToggle.RegisterValueChangedCallback(evt =>
            Debug.Log($"[AudioSettings] Speech enabled: {evt.newValue}"));

        speechSlider.RegisterValueChangedCallback(evt =>
            Debug.Log($"[AudioSettings] Speech volume set to {evt.newValue:F2}"));
    }

    private void WireAssistancePanel(VisualElement content)
    {
        Toggle assistanceToggle = content.Q<Toggle>("assistance-enable-toggle");
        Toggle manualHintToggle = content.Q<Toggle>("manual-hint-toggle");

        assistanceToggle.RegisterValueChangedCallback(evt =>
            Debug.Log($"[AssistanceSettings] Assistance enabled: {evt.newValue}"));

        manualHintToggle.RegisterValueChangedCallback(evt =>
            Debug.Log($"[AssistanceSettings] Manual hint enabled: {evt.newValue}"));

        WireStepperGroup(content, "hint-time-10", "hint-time-20", "hint-time-60",
            label => Debug.Log($"[AssistanceSettings] Time before hint set to {label}"));
    }

    private void WireSessionPanel(VisualElement content)
    {
        WireStepperGroup(content, "activities-3", "activities-6", "activities-8",
            label => Debug.Log($"[SessionSettings] Activity count set to {label}"));

        WireStepperGroup(content, "playtime-20", "playtime-30", "playtime-60",
            label => Debug.Log($"[SessionSettings] Play time set to {label}"));
    }

   private void WireLanguagePanel(VisualElement content)
{
    Button english = content.Q<Button>("language-english");
    Button arabic = content.Q<Button>("language-arabic");

    english.clicked += () =>
    {
        english.AddToClassList("active");
        arabic.RemoveFromClassList("active");
        Debug.Log("[LanguageSettings] Language set to English");
    };

    arabic.clicked += () =>
    {
        arabic.AddToClassList("active");
        english.RemoveFromClassList("active");
        Debug.Log("[LanguageSettings] Language set to Arabic");
    };
}

    private void WireStepperGroup(VisualElement content, string idA, string idB, string idC, System.Action<string> onSelect)
    {
        Button a = content.Q<Button>(idA);
        Button b = content.Q<Button>(idB);
        Button c = content.Q<Button>(idC);

        a.clicked += () => { SetStepperActive(a, b, c); onSelect(a.text); };
        b.clicked += () => { SetStepperActive(b, a, c); onSelect(b.text); };
        c.clicked += () => { SetStepperActive(c, a, b); onSelect(c.text); };
    }

    private void SetStepperActive(Button selected, Button other1, Button other2)
    {
        selected.AddToClassList("active");
        other1.RemoveFromClassList("active");
        other2.RemoveFromClassList("active");
    }
}