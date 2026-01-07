using UnityEngine;

public class LanderYellowLight2D : MonoBehaviour
{
    //light pos
    private Vector3 startPos;

    public float movementDistance = 0.1f;
    public float moveSpeed = 2.0f;

    private void Start()
    {
        //where it is
        startPos = transform.localPosition;
    }

    private void Update()
    {
        //up down loop
        float newY = Mathf.Sin(Time.time * moveSpeed) * movementDistance;

        transform.localPosition = new Vector3(startPos.x, startPos.y + newY, startPos.z);
    }
}