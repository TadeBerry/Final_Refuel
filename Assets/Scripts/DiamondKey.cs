using UnityEngine;
using System;

public class DiamondKey : MonoBehaviour
{
    [SerializeField] private string diamondKeyID; 

    public static event Action<string> OnKeyPickedUp;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.TryGetComponent(out Lander lander))
        {
            OnKeyPickedUp?.Invoke(diamondKeyID);
            Destroy(gameObject);
        }
    }
}