using UnityEngine;

public class CameraMove : MonoBehaviour
{
    public Camera cam;
    public BuidManager bm;

    public float clamp_x;
    public float clamp_y;
    public float min_size;
    public float max_size;

    private bool drag = false;
    private bool zoom = false;

    private Vector3 initialTouchPosition;
    private Vector3 initialCameraPosition;

    private Vector3 initialTouch0Position;
    private Vector3 initialTouch1Position;
    private Vector3 initialMidPointScreen;
    private float initialOrthographicSize;

    void Update()
    {
        if (Input.touchCount == 1 && (!bm.towerEnabled))
        {
            zoom = false;
            Touch touch0 = Input.GetTouch(0);

            if (IsTouching(touch0))
            {
                if (!drag)
                {
                    initialTouchPosition = touch0.position;
                    initialCameraPosition = transform.position;

                    drag = true;
                }
                else
                {
                    Vector2 delta = cam.ScreenToWorldPoint(touch0.position) -
                                    cam.ScreenToWorldPoint(initialTouchPosition);

                    Vector3 newPos = initialCameraPosition;
                    newPos.x -= delta.x;
                    newPos.x = Mathf.Clamp(newPos.x, -clamp_x, clamp_x);
                    newPos.y -= delta.y;
                    newPos.y = Mathf.Clamp(newPos.y, -clamp_y, clamp_y);

                    transform.position = newPos;
                }
            }
            else
            {
                drag = false;
            }
        }
        else
        {
            drag = false;
        }

        if (Input.touchCount == 2)
        {
            drag = false;

            Touch touch0 = Input.GetTouch(0);
            Touch touch1 = Input.GetTouch(1);

            if (!zoom)
            {
                initialTouch0Position = touch0.position;
                initialTouch1Position = touch1.position;
                initialCameraPosition = transform.position;
                initialOrthographicSize = cam.orthographicSize;
                initialMidPointScreen = (touch0.position + touch1.position) / 2;

                zoom = true;
            }
            else
            {
                transform.position = initialCameraPosition;
                cam.orthographicSize = initialOrthographicSize;

                float scaleFactor = GetScaleFactor(touch0.position,
                                                   touch1.position,
                                                   initialTouch0Position,
                                                   initialTouch1Position);

                Vector2 currentMidPoint = (touch0.position + touch1.position) / 2;
                Vector3 initialMidPointWorldBeforeZoom = cam.ScreenToWorldPoint(initialMidPointScreen);

                cam.orthographicSize = Mathf.Clamp(initialOrthographicSize / scaleFactor, min_size, max_size);

                Vector3 initialMidPointWorldAfterZoom = cam.ScreenToWorldPoint(initialMidPointScreen);
                Vector2 initialMidPointDelta = initialMidPointWorldBeforeZoom - initialMidPointWorldAfterZoom;

                Vector2 oldAndNewMidPointDelta =
                    cam.ScreenToWorldPoint(currentMidPoint) -
                    cam.ScreenToWorldPoint(initialMidPointScreen);

                Vector3 newPos = initialCameraPosition;
                newPos.x -= oldAndNewMidPointDelta.x - initialMidPointDelta.x;
                newPos.x = Mathf.Clamp(newPos.x, -clamp_x, clamp_x);
                newPos.y -= oldAndNewMidPointDelta.y - initialMidPointDelta.y;
                newPos.y = Mathf.Clamp(newPos.y, -clamp_y, clamp_y);

                transform.position = newPos;
            }
        }
        else
        {
            zoom = false;
        }
    }

    static bool IsTouching(Touch touch)
    {
        return touch.phase == TouchPhase.Began ||
                touch.phase == TouchPhase.Moved ||
                touch.phase == TouchPhase.Stationary;
    }

    public static float GetScaleFactor(Vector2 position1, Vector2 position2, Vector2 oldPosition1, Vector2 oldPosition2)
    {
        float distance = Vector2.Distance(position1, position2);
        float oldDistance = Vector2.Distance(oldPosition1, oldPosition2);

        if (oldDistance == 0 || distance == 0)
        {
            return 1.0f;
        }

        return distance / oldDistance;
    }
}
