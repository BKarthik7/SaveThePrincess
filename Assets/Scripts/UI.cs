using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UI : MonoBehaviour
{

    public static UI instance;
    [SerializeField] private GameObject gameOverUI;

    [SerializeField] private TextMeshProUGUI timerText;
    [SerializeField] private TextMeshProUGUI killCountText;

    private int killCount;

    private float elapsedTime;

    private void Update()
    {
        elapsedTime += Time.deltaTime;
        timerText.text = elapsedTime.ToString("F1") + "s";
    }

    public void EnableGameOverUI()
    {
        StartCoroutine(EnableGameOverAfterDelay(1f));
    }

    private System.Collections.IEnumerator EnableGameOverAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        Time.timeScale = 0f;
        killCount = 0;
        elapsedTime = 0f;
        timerText.text = "0.0s";
        gameOverUI.SetActive(true);
    }

    private void Awake()
    {
        instance = this;
        Time.timeScale = 1f;
    }

    public void UpdateKillCount()
    {
        killCount++;
        killCountText.text = killCount.ToString();
    }

    public void RestartLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
