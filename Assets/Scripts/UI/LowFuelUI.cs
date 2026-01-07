using UnityEngine;

public class LowFuelUI : MonoBehaviour
{



    [SerializeField] private Transform container;


    private void Start()
    {
        Hide();
    }

    private void Update()
    {
        float LowFuelAmount = 0.3f;
        if (Lander.Instance.GetFuelAmountNormalized() < LowFuelAmount)
        {
            Show();
        }
        else
        {
            Hide();
        }
    }

    public void Show()
    {
        if (!container.gameObject.activeSelf)
        {
            container.gameObject.SetActive(true);
        }
    }

    public void Hide()
    {
        if (container.gameObject.activeSelf)
        {
            container.gameObject.SetActive(false);
        }
    }

}
