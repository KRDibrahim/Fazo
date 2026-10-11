using UnityEngine.UIElements;


    public class ActivityNodeCardController
    {
        public VisualElement Root { get; }

        private readonly VisualElement cardHeader;
        private readonly VisualElement activityIcon;
        private readonly Label activityNameLabel;
        private readonly Label masteryLabel;
        private readonly VisualElement supportIndicator;
        private readonly VisualElement focusDot;
        private readonly VisualElement expandedContent;
        private readonly VisualElement chartPlaceholderSlot;

        private bool isExpanded;

        // Set by the parent list so only one card is expanded at a time —
        // matches the "collapse others on expand" rule from the original
        // stats-screen design discussion.
        public System.Action<ActivityNodeCardController> OnExpandedRequested;

        public ActivityNodeCardController(VisualElement instantiatedCard)
        {
            Root = instantiatedCard;

            cardHeader = Root.Q<VisualElement>("card-header");
            activityIcon = Root.Q<VisualElement>("activity-icon");
            activityNameLabel = Root.Q<Label>("activity-name");
            masteryLabel = Root.Q<Label>("mastery-value");
            supportIndicator = Root.Q<VisualElement>("support-indicator");
            focusDot = Root.Q<VisualElement>("focus-dot");
            expandedContent = Root.Q<VisualElement>("card-expanded-content");
            chartPlaceholderSlot = Root.Q<VisualElement>("chart-placeholder-slot");

            cardHeader.RegisterCallback<ClickEvent>(evt => ToggleExpanded());

            // Placeholder content until the real line chart is wired in.
            var placeholderLabel = new Label("Line chart goes here");
            placeholderLabel.style.color = new UnityEngine.Color(0.54f, 0.52f, 0.45f);
            chartPlaceholderSlot.Add(placeholderLabel);
        }

        public void Configure(string activityName, string masteryText, SupportLevel support, FocusLevel focus)
        {
            activityNameLabel.text = activityName;
            masteryLabel.text = masteryText;
            supportIndicator.style.backgroundColor = ColorForSupport(support);
            focusDot.style.backgroundColor = ColorForFocus(focus);
        }

        public void Collapse()
        {
            isExpanded = false;
            expandedContent.style.display = DisplayStyle.None;
        }

        private void ToggleExpanded()
        {
            if (isExpanded)
            {
                Collapse();
            }
            else
            {
                isExpanded = true;
                expandedContent.style.display = DisplayStyle.Flex;
                OnExpandedRequested?.Invoke(this);
            }
        }

        private UnityEngine.Color ColorForSupport(SupportLevel level)
        {
            switch (level)
            {
                case SupportLevel.Low: return new UnityEngine.Color(0.5f, 0.72f, 0.29f);   // green
                case SupportLevel.Medium: return new UnityEngine.Color(0.94f, 0.64f, 0.24f); // amber
                default: return new UnityEngine.Color(0.89f, 0.38f, 0.24f);                  // red
            }
        }

        private UnityEngine.Color ColorForFocus(FocusLevel level)
        {
            switch (level)
            {
                case FocusLevel.Ready: return new UnityEngine.Color(0.5f, 0.72f, 0.29f);
                case FocusLevel.KeepPracticing: return new UnityEngine.Color(0.94f, 0.64f, 0.24f);
                default: return new UnityEngine.Color(0.89f, 0.38f, 0.24f);
            }
        }
    }

    public enum SupportLevel { Low, Medium, High }
    public enum FocusLevel { Ready, KeepPracticing, NeedsSupport }
