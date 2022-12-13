using UnityEngine;
using System.Collections;
using UnityStandardAssets.ImageEffects;

public class CameraFollow : Singleton<CameraFollow>
{
    [Header("PADDING")]
    public float paddingHorizontal;
    public float paddingVertical;

    [Header("MARGINS")]
    public Transform left;
    public Transform right;
    public Transform top;
    public Transform bottom;

    [Header("TARGET")]
    [SerializeField]
    private Transform target;
    public float followSpeed = 20f;

    [Header("SLOW MOTION")]
    public TiltShift lensBlur;
    public SlowMotion slowMotion;

    [Space(20f)]
    public Transform pointAirSpawnLeft;
    public Transform pointAirSpawnRight;
    public Transform[] patrolPoints;
    public LayerMask layerCheckSpawnGroundEnemy;
    [HideInInspector]
    public Vector2 pointGroundSpawnLeft;
    [HideInInspector]
    public Vector2 pointGroundSpawnRight;

    public Camera _camera;
    private Grayscale grayScale;

    private float marginLeft = -1000f;
    private float marginRight = 1000f;
    private float marginTop = 1000f;
    private float marginBottom = -1000f;
    private float horzExtent;
    private float vertExtent;

    private float shakeAmount;
    private float shakeDelta;

    private bool flagMoveToNewZone;
    private float nextZoneX;
    private int tmpZoneId;
    private bool flagSlowMotion;


    #region UNITY METHODS

    void Awake()
    {
        //_camera = GetComponent<Camera>();
        grayScale = GetComponent<Grayscale>();
        SetGrayScaleEffect(false);
        EventDispatcher.Instance.RegisterListener(EventID.MoveCameraToNewZone, MoveCameraToNewZone);
    }

    void LateUpdate()
    {
        //if (flagSlowMotion)
        //{
        //    Vector3 v = target.position;
        //    v.z = -10f;
        //    transform.position = Vector3.Lerp(transform.position, v, followSpeed * Time.deltaTime);
        //}
        //else
        //{
        if (flagMoveToNewZone)
        {
            Vector3 v = transform.position;
            v.x = Mathf.MoveTowards(v.x, nextZoneX, 10f * Time.deltaTime);

            transform.position = v;

            if (v.x == nextZoneX)
            {
                flagMoveToNewZone = false;

                EventDispatcher.Instance.PostEvent(EventID.ClearZone, tmpZoneId);
            }
        }
        else
        {
            if (target != null)
            {
                FollowTarget();
            }

            if (shakeAmount != 0f)
            {
                Shake();
            }
        }
        //}
    }

    #endregion


    #region PUBLIC METHODS

    public void SetCameraSize(float size)
    {
        _camera.orthographicSize = size;

        horzExtent = _camera.orthographicSize * Screen.width / Screen.height;
        vertExtent = _camera.orthographicSize;

        InitMargin();
    }

    public float GetCameraSize()
    {
        return _camera.orthographicSize;
    }

    public void SetInitialPoint(Transform point)
    {
        if (point)
        {
            Vector3 v = point.position;
            v.z = -10f;
            transform.position = v;
        }
        else
        {
            DebugCustom.Log("Initial point map NULL");
        }
    }

    public void SetTarget(Transform target)
    {
        this.target = target;
    }

    public void SetSlowMotion()
    {
        flagSlowMotion = true;
        //_camera.orthographicSize = 1.55f;
        //this.target = target;
    }

    public void ResetCameraToPlayer()
    {
        flagSlowMotion = false;
        SetTarget(GameController.Instance.Player.transform);
        _camera.orthographicSize = 4f;
    }

    public void AddShake(float shakeAmount, float duration)
    {
        this.shakeDelta = shakeAmount / duration;
        this.shakeAmount = shakeAmount;
    }

    public void SetMarginLeft(float margin)
    {
        marginLeft = margin;
    }

    public void SetMarginRight(float margin)
    {
        marginRight = margin;
    }

    public void SetMarginTop(float margin)
    {
        marginTop = margin;
    }

    public void SetMarginBottom(float margin)
    {
        marginBottom = margin;
    }

    public void SetGrayScaleEffect(bool isOn)
    {
        //grayScale.enabled = isOn;
    }

    #endregion


    #region PRIVATE METHODS

    private void InitMargin()
    {
        DebugCustom.Log(string.Format("INIT MARGIN CAMERA {0} * {1} * {2} * {3}", marginTop, marginBottom, marginLeft, marginRight));

        // Top
        Vector2 pos = Vector2.zero;
        pos.y = _camera.orthographicSize;
        top.transform.localPosition = pos;

        // Bottom
        bottom.transform.localPosition = -top.transform.localPosition;

        // Right
        pos = Vector2.zero;
        pos.x = horzExtent;
        right.transform.localPosition = pos;

        // Left
        left.transform.localPosition = -right.transform.localPosition;

        // Air spawn points
        Vector2 v = left.transform.position;
        v.x -= 2f;
        v.y = top.transform.position.y;
        v.y -= 2f;
        pointAirSpawnLeft.position = v;

        v.x *= -1f;
        pointAirSpawnRight.position = v;
    }

    private void MoveCameraToNewZone(Component sender, object param)
    {
        flagMoveToNewZone = true;

        tmpZoneId = (int)param;
        nextZoneX = GameController.Instance.Player.transform.position.x;
    }

    private void FollowTarget()
    {
        Vector3 pos = target.transform.position;
        pos.z = -10f;

        // Set padding
        pos.x += paddingHorizontal;
        pos.y += paddingVertical;

        // Clamp position
        if (pos.x - horzExtent < marginLeft)
            pos.x = marginLeft + horzExtent;
        if (pos.x + horzExtent > marginRight)
            pos.x = marginRight - horzExtent;
        if (pos.y + vertExtent > marginTop)
            pos.y = marginTop - vertExtent;
        if (pos.y - vertExtent < marginBottom)
            pos.y = marginBottom + vertExtent;

        // Set position
        transform.position = Vector3.Lerp(transform.position, pos, followSpeed * Time.deltaTime);
    }

    private void Shake()
    {
        Vector3 pos = target.transform.position;
        pos.z = -10f;

        // Set padding
        pos.x += paddingHorizontal;
        pos.y += paddingVertical;

        // Apply shake
        shakeAmount = Mathf.MoveTowards(shakeAmount, 0f, shakeDelta * Time.deltaTime);
        pos.x += Random.Range(-shakeAmount, shakeAmount);
        pos.y += Random.Range(-shakeAmount, shakeAmount);

        // Clamp position
        if (pos.x - horzExtent < marginLeft)
            pos.x = marginLeft + horzExtent;
        if (pos.x + horzExtent > marginRight)
            pos.x = marginRight - horzExtent;
        if (pos.y + vertExtent > marginTop)
            pos.y = marginTop - vertExtent;
        if (pos.y - vertExtent < marginBottom)
            pos.y = marginBottom + vertExtent;

        // Set position
        transform.position = pos;
    }

    #endregion


    #region SPAWN ENEMY FROM SIDE

    public bool IsCanSpawnGroundEnemyFromLeft()
    {
        GetPointGroundSpawnLeft();

        bool isNoObstaclesForward = !Physics2D.Linecast(pointGroundSpawnLeft, left.position, layerCheckSpawnGroundEnemy);

        Vector2 v = pointGroundSpawnLeft;
        v.y -= 3f;
        bool isGroundBelow = Physics2D.Linecast(pointGroundSpawnLeft, v, layerCheckSpawnGroundEnemy);

        return isNoObstaclesForward && isGroundBelow;
    }

    public bool IsCanSpawnGroundEnemyFromRight()
    {
        GetPointGroundSpawnRight();

        bool isNoObstaclesForward = !Physics2D.Linecast(pointGroundSpawnRight, right.position, layerCheckSpawnGroundEnemy);

        Vector2 v = pointGroundSpawnRight;
        v.y -= 3f;
        bool isGroundBelow = Physics2D.Linecast(pointGroundSpawnRight, v, layerCheckSpawnGroundEnemy);

        return isNoObstaclesForward && isGroundBelow;
    }

    private void GetPointGroundSpawnLeft()
    {
        Vector2 v = left.position;
        v.x -= 2f;
        //v.y = target.bodyCenterPoint.position.y;
        pointGroundSpawnLeft = v;
    }

    private void GetPointGroundSpawnRight()
    {
        Vector2 v = right.position;
        v.x += 2f;
        //v.y = target.bodyCenterPoint.position.y;
        pointGroundSpawnRight = v;
    }

    public Vector2 GetNextDestination(EnemyHelicopter helicopter)
    {
        helicopter.indexMove++;

        if (helicopter.indexMove > patrolPoints.Length - 1)
        {
            helicopter.indexMove = 0;
        }

        Vector2 v = patrolPoints[helicopter.indexMove].position;

        return v;
    }

    #endregion
}
