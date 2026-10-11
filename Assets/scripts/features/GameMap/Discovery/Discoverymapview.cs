using UnityEngine;
using UnityEngine.UIElements;

[RequireComponent(typeof(UIDocument))]
public class DiscoveryMapView : MonoBehaviour
{
    private VisualElement root;

    private void OnEnable()
    {
        root = GetComponent<UIDocument>().rootVisualElement;

        Button backButton = root.Q<Button>("back-button");
        Button silenceButton = root.Q<Button>("silence-button");

        backButton.clicked += OnBackButtonClicked;
        silenceButton.clicked += OnSilenceButtonClicked;
    }

    private void OnBackButtonClicked()
    {
        // Discovery Map is a needsBackdrop:false destination, so returning
        // to WhosPlaying should bring the ambient Backdrop back with it.
        SceneNavigator.Instance.Navigate(GameScene.WhosPlaying, needsBackdrop: true);
    }

    private void OnSilenceButtonClicked()
    {
        Debug.Log("[DiscoveryMapView] Silence toggled — wire to AudioSystem");
    }
}