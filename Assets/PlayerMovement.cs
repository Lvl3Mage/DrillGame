using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] Transform controller;
    [SerializeField] Transform playerCamera;

    [SerializeField] Vector2 controllerSpeedRange;
    [SerializeField] Vector2 playerSpeedRange;
    [SerializeField] float rotationSpeed = 10f;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        targetDirection = playerCamera.forward;
        
    }

    Vector3 pastControllerPosition;
    Vector3 targetDirection;
    void Update()
    {
        targetDirection = Vector3.Slerp(targetDirection, playerCamera.forward, rotationSpeed*Time.deltaTime);
        Vector3 controllerVelocity = (controller.localPosition - pastControllerPosition) / Time.deltaTime;
        float t = Mathf.InverseLerp(controllerSpeedRange.x, controllerSpeedRange.y, controllerVelocity.magnitude);
        t = Mathf.Clamp01(t);
        float playerSpeed = Mathf.Lerp(playerSpeedRange.x, playerSpeedRange.y, t);
        transform.position += targetDirection * (playerSpeed * Time.deltaTime);
        // Debug.Log(playerSpeed);
        pastControllerPosition = controller.localPosition;

        Debug.DrawRay(playerCamera.position, targetDirection*playerSpeed, Color.green);
        Debug.DrawRay(playerCamera.position, playerCamera.forward, Color.red);
    }
}
