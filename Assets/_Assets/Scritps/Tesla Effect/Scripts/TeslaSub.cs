using UnityEngine;
using System.Collections.Generic;
using System.Linq;

[RequireComponent(typeof(LineRenderer))]
public class TeslaSub : MonoBehaviour
{
    public LineRenderer lineRenderer;
    public Transform startPoint;
    public Transform endPoint;

    [Range(0, 8)]
    public int generations = 6;

    [Range(0.01f, 1.0f)]
    public float duration = 0.05f;

    [Range(0.0f, 1.0f)]
    public float chaosFactor = 0.15f;
    public bool manualMode;

    private float timer;
    private System.Random randomGenerator = new System.Random();
    private List<KeyValuePair<Vector3, Vector3>> segments = new List<KeyValuePair<Vector3, Vector3>>();
    private int startIndex;
    private Vector2 size;
    private Vector2[] offsets;
    private int animationOffsetIndex;
    private int animationPingPongDirection = 1;
    private bool orthographic;

    [Header("")]
    public BaseUnit victim;
    public Transform hitEffect;

    private RaycastHit2D hit;

    void Start()
    {
        if (lineRenderer == null)
        {
            enabled = false;
            return;
        }

        orthographic = (Camera.main != null && Camera.main.orthographic);
        lineRenderer.positionCount = 0;
    }

    void OnDisable()
    {
        lineRenderer.positionCount = 0;
    }

    void Update()
    {
        orthographic = (Camera.main != null && Camera.main.orthographic);

        if (timer <= 0.0f)
        {
            if (manualMode)
            {
                timer = duration;
                lineRenderer.positionCount = 0;
            }
            else
            {
                Trigger();
            }
        }

        timer -= Time.deltaTime;
    }

    private void GetPerpendicularVector(ref Vector3 directionNormalized, out Vector3 side)
    {
        if (directionNormalized == Vector3.zero)
        {
            side = Vector3.right;
        }
        else
        {
            // use cross product to find any perpendicular vector around directionNormalized:
            // 0 = x * px + y * py + z * pz
            // => pz = -(x * px + y * py) / z
            // for computational stability use the component farthest from 0 to divide by
            float x = directionNormalized.x;
            float y = directionNormalized.y;
            float z = directionNormalized.z;
            float px, py, pz;
            float ax = Mathf.Abs(x), ay = Mathf.Abs(y), az = Mathf.Abs(z);

            if (ax >= ay && ay >= az)
            {
                // x is the max, so we can pick (py, pz) arbitrarily at (1, 1):
                py = 1.0f;
                pz = 1.0f;
                px = -(y * py + z * pz) / x;
            }
            else if (ay >= az)
            {
                // y is the max, so we can pick (px, pz) arbitrarily at (1, 1):
                px = 1.0f;
                pz = 1.0f;
                py = -(x * px + z * pz) / y;
            }
            else
            {
                // z is the max, so we can pick (px, py) arbitrarily at (1, 1):
                px = 1.0f;
                py = 1.0f;
                pz = -(x * px + y * py) / z;
            }

            side = new Vector3(px, py, pz).normalized;
        }
    }

    private void GenerateLightningBolt(Vector3 start, Vector3 end, int generation, int totalGenerations, float offsetAmount)
    {
        if (generation < 0 || generation > 8)
        {
            return;
        }
        else if (orthographic)
        {
            start.z = end.z = Mathf.Min(start.z, end.z);
        }

        segments.Add(new KeyValuePair<Vector3, Vector3>(start, end));

        if (generation == 0)
        {
            return;
        }

        Vector3 randomVector;

        if (offsetAmount <= 0.0f)
        {
            offsetAmount = (end - start).magnitude * chaosFactor;
        }

        while (generation-- > 0)
        {
            int previousStartIndex = startIndex;
            startIndex = segments.Count;

            for (int i = previousStartIndex; i < startIndex; i++)
            {
                start = segments[i].Key;
                end = segments[i].Value;

                // determine a new direction for the split
                Vector3 midPoint = (start + end) * 0.5f;

                // adjust the mid point to be the new location
                RandomVector(ref start, ref end, offsetAmount, out randomVector);
                midPoint += randomVector;

                // add two new segments
                segments.Add(new KeyValuePair<Vector3, Vector3>(start, midPoint));
                segments.Add(new KeyValuePair<Vector3, Vector3>(midPoint, end));
            }

            // halve the distance the lightning can deviate for each generation down
            offsetAmount *= 0.5f;
        }
    }

    public void RandomVector(ref Vector3 start, ref Vector3 end, float offsetAmount, out Vector3 result)
    {
        if (orthographic)
        {
            Vector3 directionNormalized = (end - start).normalized;
            Vector3 side = new Vector3(-directionNormalized.y, directionNormalized.x, directionNormalized.z);
            float distance = ((float)randomGenerator.NextDouble() * offsetAmount * 2.0f) - offsetAmount;
            result = side * distance;
        }
        else
        {
            Vector3 directionNormalized = (end - start).normalized;
            Vector3 side;
            GetPerpendicularVector(ref directionNormalized, out side);

            // generate random distance
            float distance = (((float)randomGenerator.NextDouble() + 0.1f) * offsetAmount);

            // get random rotation angle to rotate around the current direction
            float rotationAngle = ((float)randomGenerator.NextDouble() * 360.0f);

            // rotate around the direction and then offset by the perpendicular vector
            result = Quaternion.AngleAxis(rotationAngle, directionNormalized) * side * distance;
        }
    }

    private void UpdateLineRenderer()
    {
        int segmentCount = (segments.Count - startIndex) + 1;
        lineRenderer.positionCount = segmentCount;

        if (segmentCount < 1)
        {
            return;
        }

        int index = 0;
        lineRenderer.SetPosition(index++, segments[startIndex].Key);

        for (int i = startIndex; i < segments.Count; i++)
        {
            lineRenderer.SetPosition(index++, segments[i].Value);
        }

        segments.Clear();
    }

    public void Trigger()
    {
        if (victim)
        {
            if (victim.isDead)
            {
                Deactive();
                return;
            }

            startPoint.position = transform.position;
            endPoint.position = victim.BodyCenterPoint.position;
            hitEffect.transform.position = endPoint.position;

            timer = duration + Mathf.Min(0.0f, timer);
            startIndex = 0;
            GenerateLightningBolt(startPoint.position, endPoint.position, generations, generations, 0.0f);
            UpdateLineRenderer();
        }
        else
        {
            Deactive();
        }
    }

    public void Active(Vector3 startPoint, BaseUnit victim)
    {
        this.victim = victim;
        this.startPoint.position = startPoint;
        this.endPoint.position = victim.BodyCenterPoint.position;

        gameObject.SetActive(true);
    }

    public void Deactive()
    {
        victim = null;

        gameObject.SetActive(false);
    }

    public void ApplyDamage(AttackData atkData)
    {
        if (victim && victim.isDead == false && victim.isImmortal == false)
        {
            victim.TakeDamage(atkData);
        }
    }
}