using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOverFullScreenPopup : MonoBehaviour
{
    public static GameOverFullScreenPopup CreateInstance(GameObject prefab, Transform parent)
    {
        GameObject instance = Instantiate(prefab, parent);
        instance.SetActive(false); // Start inactive
        return instance.GetComponent<GameOverFullScreenPopup>();
    }

    public void Show()
    {
        this.gameObject.SetActive(true);
    }

    public void Hide()
    {
        this.gameObject.SetActive(false);
        Destroy(this.gameObject); // Optionally destroy if you don't plan to reuse
    }


    public void OnPlayAgainClick()
    {
        RestartGame();
    }

    private void RestartGame()
    {
        // Get the current scene name using the SceneManager
        string sceneName = SceneManager.GetActiveScene().name;

        // Reload the current scene
        SceneManager.LoadScene(sceneName);
    }
}
