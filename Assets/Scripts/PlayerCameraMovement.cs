using UnityEngine;

public class PlayerCameraMovement : MonoBehaviour
{
    [SerializeField] private Transform playerRoot, cameraRoot;
    [SerializeField] private bool invert;
    [SerializeField] private float sensitivity = 5f;
    [SerializeField] private Vector2 defaultLookLimits = new Vector2(-70f, 80f);
    [SerializeField] private Vector2 lookAngles;
    [SerializeField] private Vector2 currentMouseLook;
    [SerializeField] private float currentRollAngle;

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }

    private void Update()
    {
        LockAndUnlockCursor();

        if (Cursor.lockState == CursorLockMode.Locked)
        {
            LookAround();
        }
    }

    private void LockAndUnlockCursor()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (Cursor.lockState == CursorLockMode.Locked)
            {
                Cursor.lockState = CursorLockMode.None;
            }
            else if (Cursor.lockState == CursorLockMode.None)
            {
                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = false;
            }
        }
    }

    private void LookAround()
    {
        currentMouseLook = new Vector2(Input.GetAxis("Mouse Y"), Input.GetAxis("Mouse X"));

        lookAngles.x += currentMouseLook.x * sensitivity * (invert ? 1f : -1f);
        lookAngles.y += currentMouseLook.y * sensitivity;

        lookAngles.x = Mathf.Clamp(lookAngles.x, defaultLookLimits.x, defaultLookLimits.y);

        cameraRoot.localRotation = Quaternion.Euler(lookAngles.x, 0f, currentRollAngle);
        playerRoot.localRotation = Quaternion.Euler(0f, lookAngles.y, 0f);
    }
}
