using UnityEngine;
using UnityEngine.EventSystems;

public class ButtonHoverSound : MonoBehaviour, IPointerEnterHandler
{


    // [SerializeField] private AudioClip hoverSound;



    // private static AudioSource localSource;

    // public void OnPointerEnter(PointerEventData eventData)
    // {
    //     EventSystem.current.SetSelectedGameObject(null);

    //     if (hoverSound != null)
    //     {
    //         AudioSource.PlayClipAtPoint(hoverSound, Camera.main.transform.position, 0.3f);
    //     }
    // }


    public void OnPointerEnter(PointerEventData eventData)
    {
        if (EventSystem.current != null) 
        {
            EventSystem.current.SetSelectedGameObject(null);
        }

        if (SoundManager.Instance != null)
        {
            SoundManager.Instance.PlayButtonHoverSound();
        }
    }

}