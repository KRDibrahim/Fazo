using System;
using UnityEngine;
using UnityEngine.UIElements;

[RequireComponent(typeof(UIDocument))]
public class LandingView : MonoBehaviour
{

[SerializeField] private CurrentKidSO currentKid;
    private void OnEnable()
    {
        VisualElement root = GetComponent<UIDocument>().rootVisualElement;


        Button playButton = root.Q<Button>("playBtn"), settingBtn = root.Q<Button>("settingBtn");

        VisualElement kidAccountButton = root.Q<VisualElement>("kidAccountBtn");

        kidAccountButton.RegisterCallback<ClickEvent>(evt => OnKidAccountButtonClicked());

       
        playButton.clicked += OnPlayButtonClicked;
        settingBtn.clicked += onSettingButtonClicked;

        ApplyCurrentKidToBadge(kidAccountButton);

    }

    private void ApplyCurrentKidToBadge(VisualElement badgeInstance)
    {
        Label nameLabel = badgeInstance.Q<Label>("kid-name-label");
        Image avatarImage = badgeInstance.Q<Image>("kid-avatar-image");

        if (currentKid != null && currentKid.HasSelection)
        {
            nameLabel.text = currentKid.SelectedKid.Name;
            avatarImage.sprite = currentKid.SelectedKid.Avatar;
        }
        else
        {
            nameLabel.text = "Kid Name";
            avatarImage.sprite = null;
        }
    }

    private void onSettingButtonClicked()
    {
        SceneNavigator.Instance.Navigate(GameScene.Settings);
    }

    private void OnVideoButtonClicked()
    {
        Debug.Log("[LandingView] Video button clicked, navigating to Video");
    }

    private void OnPlayButtonClicked()
    {
        SceneNavigator.Instance.Navigate(GameScene.SectionGate);
    }

    private void OnKidAccountButtonClicked()
    {
        SceneNavigator.Instance.Navigate(GameScene.WhosPlaying);
    }
}
