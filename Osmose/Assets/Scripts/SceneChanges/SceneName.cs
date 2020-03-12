using UnityEngine;

[CreateAssetMenu(menuName = "SceneName")]
public class SceneName : ScriptableObject {
    [SerializeField] private string sceneName;

    /// <summary>
    /// Get the scene's name
    /// </summary>
    /// <returns>Scene's name</returns>
    public string GetSceneName() {
        return sceneName;
    }
}
