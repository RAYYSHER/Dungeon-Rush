using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameResultUI : MonoBehaviour
{
    [SerializeField] private GameObject _gameResultPanel;
    [SerializeField] private TMP_Text resultText;
    [SerializeField] private Button restartButton;

    [SerializeField] private GameObject _gameResultFirst;

    void Start()
    {
        _gameResultPanel.SetActive(false);
    }

    public void ShowResult(bool isWin)
    {
        // Time.timeScale = 0f;

        _gameResultPanel.SetActive(true);

        if (isWin)
        {
            resultText.text = "VICTORY";
        }
        else
        {
            resultText.text = "DEFEAT";
        }

        // Controller(Joystick) automatically highlights RESTART button
        // EventSystem.current.SetSelectedGameObject(_gameResultFirst);
        StartCoroutine(SelectAfterFrame(_gameResultFirst));
    }

    public void RestartGame()
    {
        ZombieGlobalStat.Reset();
        BossGlobalStat.Reset();
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    private IEnumerator SelectAfterFrame(GameObject target)
    {
        EventSystem.current.SetSelectedGameObject(null);
        yield return null;
        EventSystem.current.SetSelectedGameObject(target);
    }

}
