using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Restart : MonoBehaviour
{
    [SerializeField]
    private Button _buttonRestart;

    private void OnEnable()
    {
        _buttonRestart.onClick.AddListener(ReloadScene);
    }

    private void OnDisable()
    {
        _buttonRestart.onClick.RemoveListener(ReloadScene);
    }

    private void ReloadScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
