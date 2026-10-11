using System;
using System.Collections.Generic;
using UnityEngine.UIElements;

namespace FazoGame.Features.KidDetails
{
    public struct DomainTabData
    {
        public string DomainId;
        public string DisplayName;
        public string IconClass; // e.g. "icon-shapes", defined in Icons.uss

        public DomainTabData(string domainId, string displayName, string iconClass)
        {
            DomainId = domainId;
            DisplayName = displayName;
            IconClass = iconClass;
        }
    }

    public class DomainTabController
    {
        private readonly VisualElement tabContainer;
        private readonly VisualTreeAsset tabButtonTemplate;
        private readonly List<Button> createdButtons = new List<Button>();

        public DomainTabController(VisualElement tabContainer, VisualTreeAsset tabButtonTemplate)
        {
            this.tabContainer = tabContainer;
            this.tabButtonTemplate = tabButtonTemplate;
        }

        public void BuildTabs(List<DomainTabData> domains, Action<string> onDomainSelected)
        {
            tabContainer.Clear();
            createdButtons.Clear();

            foreach (DomainTabData domain in domains)
            {
                VisualElement instance = tabButtonTemplate.Instantiate();
                Button button = instance.Q<Button>("domain-tab-button");
                VisualElement icon = instance.Q<VisualElement>("domain-tab-icon");
                Label label = instance.Q<Label>("domain-tab-label");

                label.text = domain.DisplayName;
                icon.AddToClassList(domain.IconClass);

                button.clicked += () =>
                {
                    SetActive(button);
                    onDomainSelected?.Invoke(domain.DomainId);
                };

                tabContainer.Add(instance);
                createdButtons.Add(button);
            }

            if (createdButtons.Count > 0)
            {
                SetActive(createdButtons[0]);
                onDomainSelected?.Invoke(domains[0].DomainId);
            }
        }

        private void SetActive(Button selected)
        {
            foreach (Button button in createdButtons)
                button.RemoveFromClassList("active");
            selected.AddToClassList("active");
        }
    }
}
