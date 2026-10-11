using System;
using UnityEngine;
using UnityEngine.UIElements;

namespace FazoGame.Features.KidDetails
{
    public class ResponsibleProfileBadgeController
    {
        private readonly VisualElement root;
        private readonly Image profileImage;

        public ResponsibleProfileBadgeController(VisualElement instantiatedBadge, Action onClicked)
        {
            root = instantiatedBadge;
            profileImage = root.Q<Image>("responsible-photo");

            root.RegisterCallback<ClickEvent>(evt => onClicked?.Invoke());
        }

        public void SetPhoto(Sprite photo)
        {
            profileImage.sprite = photo;
        }
    }
}
