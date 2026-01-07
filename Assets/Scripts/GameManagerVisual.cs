using System.Collections;
using Unity.Cinemachine;
using UnityEngine;

public class GameManagerVisual : MonoBehaviour
{


    [SerializeField] private CinemachineImpulseSource crashCinemachineImpulseSource;
    [SerializeField] private ScorePopup scorePopupPrefab;



    private void Start()
    {
        Lander.Instance.OnLanded += Lander_OnLanded;
        Lander.Instance.OnCoinPickup += Lander_OnCoinPickup;
        Lander.Instance.OnFuelPickup += Lander_OnFuelPickup;
        
    }

    private void Lander_OnFuelPickup(object sender, Lander.OnFuelPickupEventArgs e)
    {
        Vector3 spawnPos = e.fuelPickup.transform.position;
        Instantiate(scorePopupPrefab, spawnPos, Quaternion.identity).SetText("+FUEL");
        // Transform pickupCollectVfxTransform = Instantiate(pickupCollectVfxPrefab, e.fuelPickup.transform.position, Quaternion.identity);
        // Destroy(pickupCollectVfxTransform.gameObject, 3f);
        // pickupCinemachineImpulseSource.GenerateImpulse(5f);
        // Instantiate(scorePopupPrefab, e.fuelPickup.transform.position, Quaternion.identity).SetText("+FUEL");
    }

    private void Lander_OnCoinPickup(object sender, Lander.OnCoinPickupEventArgs e)
    {
        Vector3 spawnPos = e.coinPickup.transform.position;
        Instantiate(scorePopupPrefab, spawnPos, Quaternion.identity).SetText("+" + GameManager.SCORE_PER_COIN);
        // Transform pickupCollectVfxTransform = Instantiate(pickupCollectVfxPrefab, e.fuelPickup.transform.position, Quaternion.identity);
        // Destroy(pickupCollectVfxTransform.gameObject, 3f);
        // pickupCinemachineImpulseSource.GenerateImpulse(5f);
        // Instantiate(scorePopupPrefab, e.fuelPickup.transform.position, Quaternion.identity).SetText("+" + GameManager.SCORE_PER_COIN);
    }

    private void Lander_OnLanded(object sender, Lander.OnLandedEventArgs e)
    {   
        switch (e.landingType) 
        {
            case Lander.LandingType.TooFastLanding:
            case Lander.LandingType.TooSteepAngle:
            case Lander.LandingType.WrongLandingArea:

            crashCinemachineImpulseSource.GenerateImpulse(1f);
            break;
        }
    }
    
}
