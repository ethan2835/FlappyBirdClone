using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LogicScript : MonoBehaviour
{
    public int playerScore;
    public Text scoreText;
    public GameObject gameOverScreen;
    // Track which pipe roots have already been scored to avoid duplicates
    private System.Collections.Generic.HashSet<int> scoredPipeRoots = new System.Collections.Generic.HashSet<int>();

    [ContextMenu("Increase Score")]
    public void addScore(int scoretoAdd)
    {
        playerScore = playerScore + scoretoAdd;
        scoreText.text = playerScore.ToString();
    }

    // Add score for a pipe root object only once
    public void addScoreOnce(GameObject pipeRoot)
    {
        if (pipeRoot == null) return;
        int id = pipeRoot.GetInstanceID();
        if (scoredPipeRoots.Contains(id)) return;
        scoredPipeRoots.Add(id);
        addScore(1);
    }

    public void restartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void gameOver()
    {
        gameOverScreen.SetActive(true);
    }


}
