using System;
using UnityEngine;
using UnityEngine.UIElements;

public enum SectionState
{
    LockedInsufficientPoints,
    LockedReadyToUnlock,
    Unlocked
}

public class SectionCardController
{
    public VisualElement Root { get; }

    private readonly VisualElement lockBadge;
    private readonly Image robotArt;
    private readonly Label titleLabel;
    private readonly Label subtitleLabel;
    private readonly Label unlockRequirementLabel;
    private readonly Button unlockButton;
    private readonly VisualElement progressTrack;
    private readonly VisualElement progressFill;

    private Action onUnlockClicked;

    public SectionCardController(VisualElement instantiatedCard)
    {
        Root = instantiatedCard;

        lockBadge = Root.Q<VisualElement>("lock-badge");
        robotArt = Root.Q<Image>("robot-art");
        titleLabel = Root.Q<Label>("section-title");
        subtitleLabel = Root.Q<Label>("section-subtitle");
        unlockRequirementLabel = Root.Q<Label>("unlock-requirement-label");
        unlockButton = Root.Q<Button>("unlock-button");
        progressTrack = Root.Q<VisualElement>("section-progress-track");
        progressFill = Root.Q<VisualElement>("section-progress-fill");

        unlockButton.clicked += () => onUnlockClicked?.Invoke();
    }

    public void Configure(
        string title,
        string subtitle,
        Sprite robotSprite,
        SectionState state,
        int pointsRequired = 0,
        float progress01 = 0f,
        Action onUnlock = null)
    {
        titleLabel.text = title;
        subtitleLabel.text = subtitle;
        robotArt.sprite = robotSprite;

        onUnlockClicked = onUnlock;

        switch (state)
        {
            case SectionState.LockedInsufficientPoints:
                Root.AddToClassList("locked");
                lockBadge.style.display = DisplayStyle.Flex;
                unlockRequirementLabel.style.display = DisplayStyle.Flex;
                unlockRequirementLabel.text = $"Reach {pointsRequired} points to unlock";
                unlockButton.style.display = DisplayStyle.None;
                progressTrack.style.display = DisplayStyle.None;
                break;

            case SectionState.LockedReadyToUnlock:
                Root.AddToClassList("locked");
                lockBadge.style.display = DisplayStyle.Flex;
                unlockRequirementLabel.style.display = DisplayStyle.None;
                unlockButton.style.display = DisplayStyle.Flex;
                progressTrack.style.display = DisplayStyle.None;
                break;

            case SectionState.Unlocked:
                Root.RemoveFromClassList("locked");
                lockBadge.style.display = DisplayStyle.None;
                unlockRequirementLabel.style.display = DisplayStyle.None;
                unlockButton.style.display = DisplayStyle.None;
                progressTrack.style.display = DisplayStyle.Flex;
                progressFill.style.width = new Length(progress01 * 100f, LengthUnit.Percent);
                break;
        }
    }
}