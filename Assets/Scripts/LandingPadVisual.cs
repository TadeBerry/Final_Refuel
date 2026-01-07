using TMPro;
using UnityEngine;

public class LandingPadVisual : MonoBehaviour
{
    [SerializeField] private TextMeshPro scooreMultiplierTextMesh;





    private void Awake()
    {
        LandingPad landingPad = GetComponent<LandingPad>();
        scooreMultiplierTextMesh.text = "x" + landingPad.GetScoreMultiplier();
    }
}
