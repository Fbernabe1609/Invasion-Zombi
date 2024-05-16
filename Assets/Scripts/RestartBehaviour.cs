using UnityEngine;
using UnityEngine.SceneManagement;

public class RestartBehaviour : MonoBehaviour
{
    private void Awake()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    public void RestartGame()
    {
        SceneManager.LoadScene("SampleScene 1");
    }
}