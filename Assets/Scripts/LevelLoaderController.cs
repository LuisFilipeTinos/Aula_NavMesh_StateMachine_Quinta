using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelLoaderController : MonoBehaviour
{
    [SerializeField] float transitionTime = 2f;

    public async Task LoadLevel(int levelIndex)
    {
        await Awaitable.WaitForSecondsAsync(transitionTime);
        SceneManager.LoadScene(levelIndex);
    }
}
