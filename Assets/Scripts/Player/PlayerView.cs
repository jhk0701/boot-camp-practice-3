using UnityEngine;

public class PlayerView : MonoBehaviour
{
    [SerializeField] Transform cameraAxis;
    Vector2 direction;
    [SerializeField] float rotateSensitive = 15f;
    [SerializeField] Vector2 clampForFirstPerson;
    float camRotateX = 0f;

    bool cursorIsLocked = false;


    void Start()
    {
        Player.Instance.inputController.OnLookEvent += OnLook;
        Player.Instance.inputController.OnOpenSettingEvent += Toggle;

        camRotateX = cameraAxis.localEulerAngles.x;

        cursorIsLocked = true;
        Cursor.lockState = CursorLockMode.Locked;
    }

    void LateUpdate()
    {
        if(!cursorIsLocked) return;

        float speed = rotateSensitive * Time.deltaTime;

        transform.Rotate(Vector3.up * direction.x * speed);

        camRotateX += speed * -direction.y;
        camRotateX = Mathf.Clamp(camRotateX, clampForFirstPerson.x, clampForFirstPerson.y);

        cameraAxis.localEulerAngles = new Vector3(camRotateX, 0f, 0f);
    }


    void OnLook(Vector2 mouseDelta)
    {
        direction = mouseDelta;
    }

    void Toggle()
    {
        cursorIsLocked = !cursorIsLocked;

        if (cursorIsLocked)
            Cursor.lockState = CursorLockMode.Locked;
        else
            Cursor.lockState = CursorLockMode.None;
    }
}