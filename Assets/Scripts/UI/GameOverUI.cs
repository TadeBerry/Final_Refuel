using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GameOverUI : MonoBehaviour
{
    [SerializeField] private Button mainMenuButton;
    [SerializeField] private TextMeshProUGUI scoreTextMesh;

    private void Awake()
    {
        mainMenuButton.onClick.AddListener(() =>
        {
            GameManager.ResetStaticData();

            //if (GameManager.Instance != null)
            //{
            //    Destroy(GameManager.Instance.gameObject);
            //}

            SceneLoader.LoadScene(SceneLoader.Scene.MainMenuScene);
        }); 
    }

    private void Start()
    {
        if (GameManager.Instance != null)
        {
            scoreTextMesh.text = "FINAL SCORE: " + GameManager.Instance.GetTotalScore().ToString();
        }
        else
        {
            scoreTextMesh.text = "FINAL SCORE: 0";
        }

        //mainMenuButton.Select();
    }
}