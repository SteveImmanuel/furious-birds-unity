using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CameraController : MonoBehaviour
{
    public float minZoom = 1f;
    public float maxZoom = 2f;
    public float scrollSpeed = 2f;
    public float dragSpeed = 2;
    public GameObject dummyEmpty;

    private CinemachineVirtualCamera cam;
    private CinemachineFramingTransposer frame;
    private float defaultOrthographicSize = 13;
    private bool isZooming = false;
    private bool isDragging = false;
    private Vector2 dragOrigin;
    private float deadZoneWidth;
    private float deadZoneHeight;
    private float xDamping;
    private float yDamping;
    [HideInInspector]
    public bool isAiming = false;

    [HideInInspector]
    public static CameraController instance;
    
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
            return;
        }
    }
    void Start()
    {
        cam = GetComponent<CinemachineVirtualCamera>();
        frame = cam.GetCinemachineComponent<CinemachineFramingTransposer>();
        deadZoneHeight = frame.m_DeadZoneHeight;
        deadZoneWidth = frame.m_DeadZoneWidth;
        xDamping = frame.m_XDamping;
        yDamping = frame.m_YDamping;
    }

    public void SetFollowTarget(Transform target)
    {
        frame.m_LookaheadTime = 0.75f;
        cam.Follow = target;
    }
    
    void Update()
    {
        float scrollInput = Input.mouseScrollDelta.y;

        if (scrollInput != 0)
        {
            if (isZooming)
            {
                StopAllCoroutines();
            }
            
            Vector2 p = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            dummyEmpty.transform.position = p;
            cam.Follow = dummyEmpty.transform;
            frame.m_LookaheadTime = 0;

            float newSize = cam.m_Lens.OrthographicSize - scrollInput * scrollSpeed;
            newSize = Mathf.Clamp(newSize, defaultOrthographicSize / maxZoom, defaultOrthographicSize / minZoom);
            StartCoroutine(SlowZoom(cam.m_Lens.OrthographicSize, newSize, .5f));
            isZooming = true;

        }

        if (Input.GetMouseButtonDown(0) && !isAiming)
        {
            isDragging = true;
            dragOrigin = Camera.main.ScreenToViewportPoint(Input.mousePosition);
            SetDragCamera();
            SetDummyToCenterAndFollow();
        }

        if (Input.GetMouseButton(0) && isDragging && !isAiming)
        {
            Vector2 currentMousePos = Camera.main.ScreenToViewportPoint(Input.mousePosition);
            Vector2 move = currentMousePos - dragOrigin;
            dummyEmpty.transform.Translate(move * dragSpeed * -1, Space.World);
        }

        if (Input.GetMouseButtonUp(0))
        {
            ResetDragCamera();
            isDragging = false;
        }

    }

    public void SetDummyToCenterAndFollow()
    {
        Vector2 centerCam = Camera.main.ScreenToWorldPoint(new Vector2(Screen.width / 2, Screen.height / 2));
        dummyEmpty.transform.position = centerCam;
        cam.Follow = dummyEmpty.transform;
        frame.m_LookaheadTime = 0;
    }

    void SetDragCamera()
    {
        frame.m_DeadZoneWidth = 0.02f;
        frame.m_DeadZoneHeight = 0.02f;
        frame.m_XDamping = 0;
        frame.m_YDamping = 0;
    }

    void ResetDragCamera()
    {
        frame.m_DeadZoneWidth = deadZoneWidth;
        frame.m_DeadZoneHeight = deadZoneHeight;
        frame.m_XDamping = xDamping;
        frame.m_YDamping = yDamping;
    }

    IEnumerator SlowZoom(float start, float end, float duration)
    {
        float elapsedTime = 0;
        while(elapsedTime < duration)
        {
            float newSize = Mathf.Lerp(start, end, elapsedTime / duration);
            cam.m_Lens.OrthographicSize = newSize;
            elapsedTime += Time.deltaTime;
            yield return new WaitForSeconds(Time.deltaTime);
        }
        isZooming = false;
    }
}
