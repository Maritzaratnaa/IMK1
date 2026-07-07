using UnityEngine;

public class MouseLookAround : MonoBehaviour
{
    float rotationX = 0f;
    float rotationY = 0f;

    public Vector2 sensitivity = new Vector2(360f, 360f);
    
    public float minimumX = -90f;
    public float maximumX = 90f;

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void Update()
    {
        rotationY += Input.GetAxis("Mouse X") * Time.deltaTime * sensitivity.x;
        rotationX += Input.GetAxis("Mouse Y") * Time.deltaTime * -1 * sensitivity.y;
        
        rotationX = Mathf.Clamp(rotationX, minimumX, maximumX);
        
        transform.localEulerAngles = new Vector3(rotationX, rotationY, 0);
    }
}