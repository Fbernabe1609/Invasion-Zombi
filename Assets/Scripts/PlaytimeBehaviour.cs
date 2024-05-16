using TMPro;
using UnityEngine;

public class PlaytimeBehaviour : MonoBehaviour
{
    private TextMeshProUGUI playtimeText;
    private void Awake()
    {
        playtimeText = GetComponent<TextMeshProUGUI>();
        CalculateTime();
        GameObject.Find("GameManager").GetComponent<GameManager>().DestroyGameManager();
    }

    private void CalculateTime()
    {
        GameManager gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        int minutes = Mathf.FloorToInt(gameManager.playTime / 60);
        int seconds = Mathf.FloorToInt(gameManager.playTime % 60);

        playtimeText.text = "Tiempo: " + minutes + " mins " + seconds + " s \nEnemigos eliminados: " + HumanBehaviour.deadHumans;
    }
}
