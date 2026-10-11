using UnityEngine;

// Plain data — no Unity dependency beyond Sprite, kept simple on purpose
// since this is placeholder/in-memory data for now, not a database entity.
[System.Serializable]
public class KidData
{
    public string Id;
    public string Name;
    public Sprite Avatar;

    public KidData(string id, string name, Sprite avatar)
    {
        Id = id;
        Name = name;
        Avatar = avatar;
    }
}

// Shared, cross-scene "current kid" state — this is exactly the kind of
// genuinely singleton, multi-screen-observed value ScriptableObjects are
// for, per the earlier discussion. Create ONE instance of this asset
// (Assets/Data/SharedState/CurrentKid.asset) and every screen that needs
// to know "who's playing right now" reads from that same asset.
[CreateAssetMenu(fileName = "CurrentKid", menuName = "Fazo/Current Kid")]
public class CurrentKidSO : ScriptableObject
{
    public KidData SelectedKid;

    public bool HasSelection => SelectedKid != null;

    public void Select(KidData kid)
    {
        SelectedKid = kid;
    }

    public void Clear()
    {
        SelectedKid = null;
    }
}
