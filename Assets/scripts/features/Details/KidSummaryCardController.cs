using UnityEngine;
using UnityEngine.UIElements;

namespace FazoGame.Features.KidDetails
{
    public class KidSummaryCardController
    {
        private readonly Image photoImage;
        private readonly Label nameLabel;
        private readonly Label ageLabel;
        private readonly Label levelLabel;

        public KidSummaryCardController(VisualElement instantiatedCard)
        {
            photoImage = instantiatedCard.Q<Image>("kid-photo");
            nameLabel = instantiatedCard.Q<Label>("kid-name");
            ageLabel = instantiatedCard.Q<Label>("kid-age");
            levelLabel = instantiatedCard.Q<Label>("kid-level");
        }

        public void Configure(string name, int age, int level, Sprite photo)
        {
            nameLabel.text = name;
            ageLabel.text = $"{age} years";
            levelLabel.text = $"Level {level}";
            photoImage.sprite = photo;
        }
    }
}
