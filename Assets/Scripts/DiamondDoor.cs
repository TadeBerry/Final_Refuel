using UnityEngine;

public class DiamondDoor : MonoBehaviour
{
    [SerializeField] private string diamondDoorID; 
    [SerializeField] private GameObject doorClosedVisual;
    [SerializeField] private GameObject doorOpenVisual;

    private void Start()
    {
        DiamondKey.OnKeyPickedUp += DiamondKey_OnKeyPickedUp;
        
        doorClosedVisual.SetActive(true);
        doorOpenVisual.SetActive(false);
    }

    private void DiamondKey_OnKeyPickedUp(string keyID)
    {
        if (keyID == diamondDoorID)
        {
            OpenDoor();
        }
    }

    private void OpenDoor()
    {
        doorClosedVisual.SetActive(false);
        doorOpenVisual.SetActive(true);
        
        if (TryGetComponent(out Collider2D col))
        {
            col.enabled = false;
        }
    }

    private void OnDestroy() 
    {
        DiamondKey.OnKeyPickedUp -= DiamondKey_OnKeyPickedUp;
    }
}