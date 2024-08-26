using UnityEngine;

public class LeaderboardFullScreenPopup : MonoBehaviour
{

    [@SerializeField]
    private LeaderboardContent leaderboardContent;
    public static LeaderboardFullScreenPopup CreateInstance(GameObject prefab, Transform parent)
    {
        GameObject instance = Instantiate(prefab, parent);
        instance.SetActive(false); // Start inactive
        return instance.GetComponent<LeaderboardFullScreenPopup>();
    }

    public void Show()
    {
        leaderboardContent.Initialize();
        this.gameObject.SetActive(true);
    }

    public void Hide()
    {
        this.gameObject.SetActive(false);
        Destroy(this.gameObject); // Optionally destroy if you don't plan to reuse
    }
}
