using System;
using UnityEngine.UIElements;

public class KidCardController
{
    public VisualElement Root { get; }

    private readonly Image avatarImage;
    private readonly Label nameLabel;
    private readonly Button selectButton;
    private readonly Button detailsButton;
    private readonly Button settingsButton;

    public KidCardController(VisualElement instantiatedCard)
    {
        Root = instantiatedCard;

        avatarImage = Root.Q<Image>("kid-avatar-image");
        nameLabel = Root.Q<Label>("kid-name-label");
        selectButton = Root.Q<Button>("select-button");
        detailsButton = Root.Q<Button>("details-button");
        settingsButton = Root.Q<Button>("kid-settings-button");
    }

    public void Configure(KidData kid, Action<KidData> onSelect, Action<KidData> onDetails, Action<KidData> onSettings)
    {
        nameLabel.text = kid.Name;
        avatarImage.sprite = kid.Avatar;

        // Clear any previously registered handlers before adding new ones —
        // matters if the same card instance is ever reused/reconfigured.
        selectButton.clicked -= InvokeSelect;
        detailsButton.clicked -= InvokeDetails;
        settingsButton.clicked -= InvokeSettings;

        currentKid = kid;
        onSelectCallback = onSelect;
        onDetailsCallback = onDetails;
        onSettingsCallback = onSettings;

        selectButton.clicked += InvokeSelect;
        detailsButton.clicked += InvokeDetails;
        settingsButton.clicked += InvokeSettings;
    }

    private KidData currentKid;
    private Action<KidData> onSelectCallback;
    private Action<KidData> onDetailsCallback;
    private Action<KidData> onSettingsCallback;

    private void InvokeSelect() => onSelectCallback?.Invoke(currentKid);
    private void InvokeDetails() => onDetailsCallback?.Invoke(currentKid);
    private void InvokeSettings() => onSettingsCallback?.Invoke(currentKid);
}
