using System;
using UnityEngine;
using UnityEngine.UIElements;
[RequireComponent(typeof(UIDocument))]
public class AuthView : MonoBehaviour
{


    private void OnEnable()
    {
        VisualElement root = GetComponent<UIDocument>().rootVisualElement;
        Debug.Log("[AuthView] Auth scene loaded and UI rendered");

        Button appleButton = root.Q<Button>("apple"), googleAuth = root.Q<Button>("google"), guestButton = root.Q<Button>("guest");

        guestButton.clicked += OnGuestButtonClicked;

        googleAuth.clicked += OnGoogleAuthClicked;
        appleButton.clicked += OnAppleAuthClicked;
    }

    private void OnAppleAuthClicked()
    {
             DialogService.Instance.ShowAuthComingSoon(() =>
   {
       SceneNavigator.Instance.Navigate(GameScene.WhosPlaying);
   });
    }

    private void OnGuestButtonClicked()
    {
        Debug.Log("[AuthView] Guest button clicked, navigating to Landing");
        SceneNavigator.Instance.Navigate(GameScene.Landing);
    }

    private void OnGoogleAuthClicked()
    {
        DialogService.Instance.ShowAuthComingSoon(() =>
   {
       SceneNavigator.Instance.Navigate(GameScene.WhosPlaying);
   });
    }


}
