using System;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class LandedUI : MonoBehaviour
{


    [SerializeField] private TextMeshProUGUI titleTextMesh;
    [SerializeField] private TextMeshProUGUI statsTextMesh;
    [SerializeField] private TextMeshProUGUI nextButtonTextMesh;
    [SerializeField] Button nextButton;
    [SerializeField] Button retryButton;


    private Action nextButtonClickAction;
    private Action retryButtonClickAction;

    private void Awake()
{
    nextButton.onClick.AddListener(() =>
    {
        Hide();
        nextButtonClickAction();
    });

    retryButton.onClick.AddListener(() =>
    {
        Hide();
        retryButtonClickAction();
    });
}

    private void Start()
    {
        Lander.Instance.OnLanded += Lander_OnLanded;

        Hide();
    }

    private void Lander_OnLanded(object sender, Lander.OnLandedEventArgs e)
    {
        retryButtonClickAction = GameManager.Instance.RetryLevel;
        if (e.landingType == Lander.LandingType.Success)
        {
        titleTextMesh.text = "SUCCESSFUL LANDING!";
        nextButtonTextMesh.text = "CONTINUE";
        nextButtonClickAction = GameManager.Instance.GoToNextLevel;
        retryButton.gameObject.SetActive(true);
        }
        else
        {
            titleTextMesh.text = "<color=#ff0000>CRASH!</color>";
            nextButtonTextMesh.text = "RETRY";
            nextButtonClickAction = GameManager.Instance.RetryLevel;
            retryButton.gameObject.SetActive(false);
        }

        statsTextMesh.text =
        Mathf.Round(e.LandingSpeed * 2f) + "\n" +
        Mathf.Round(e.dotVector* 100f) + "\n" +
        "x" + e.scoreMultiplier + "\n" +
        e.score;

        Show();
    }

    private void Show()
    {
        gameObject.SetActive(true);
        if (UnityEngine.EventSystems.EventSystem.current != null)
    {
        UnityEngine.EventSystems.EventSystem.current.SetSelectedGameObject(null);
    }
    }

    private void Hide()
    {
        gameObject.SetActive(false);
    }
}
