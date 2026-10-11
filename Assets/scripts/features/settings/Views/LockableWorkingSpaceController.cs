using UnityEngine.UIElements;

public class LockableWorkingSpaceController
{
    private readonly VisualElement lockOverlay;
    private readonly Button unlockButton;

    public LockableWorkingSpaceController(VisualElement instantiatedContent)
    {
        lockOverlay = instantiatedContent.Q<VisualElement>("lock-overlay");
        unlockButton = lockOverlay.Q<Button>("unlock-button");
        unlockButton.clicked += Unlock;
    }

    private void Unlock()
    {
        Reveal();
    }

    public void Reveal()
    {
        lockOverlay.style.display = DisplayStyle.None;
    }

    public void Lock()
    {
        lockOverlay.style.display = DisplayStyle.Flex;
    }
}