using UnityEngine;
using System.Collections;

public class MathUtils
{
    private const float DOUBLE_PI = Mathf.PI * 2;
    private const float HALF_PI = Mathf.PI / 2;

    public static float FastSin(float angle)
    {
        if (angle < -Mathf.PI)
        {
            angle += DOUBLE_PI;
        }
        else if (angle > Mathf.PI)
        {
            angle -= DOUBLE_PI;
        }

        float sin;
        if (angle < 0)
        {
            sin = angle * (1.27323954f + 0.405284735f * angle);
            if (sin < 0)
            {
                sin = 0.225f * (sin * -sin - sin) + sin;
            }
            else
            {
                sin = 0.225f * (sin * sin - sin) + sin;
            }
        }
        else
        {
            sin = angle * (1.27323954f - 0.405284735f * angle);
            if (sin < 0)
            {
                sin = 0.225f * (sin * -sin - sin) + sin;
            }
            else
            {
                sin = 0.225f * (sin * sin - sin) + sin;
            }
        }

        return sin;
    }

    public static float FastCos(float angle)
    {
        if (angle < -Mathf.PI)
        {
            angle += DOUBLE_PI;
        }
        else if (angle > Mathf.PI)
        {
            angle -= DOUBLE_PI;
        }

        angle += HALF_PI;
        if (angle > Mathf.PI)
        {
            angle -= DOUBLE_PI;
        }

        float cos;
        if (angle < 0)
        {
            cos = angle * (1.27323954f + 0.405284735f * angle);
            if (cos < 0)
            {
                cos = 0.225f * (cos * -cos - cos) + cos;
            }
            else
            {
                cos = 0.225f * (cos * cos - cos) + cos;
            }
        }
        else
        {
            cos = angle * (1.27323954f - 0.405284735f * angle);
            if (cos < 0)
            {
                cos = 0.225f * (cos * -cos - cos) + cos;
            }
            else
            {
                cos = 0.225f * (cos * cos - cos) + cos;
            }
        }

        return cos;
    }

    public static float CalculateLaunchSpeed(float distance, float yOffset, float gravity, float angle)
    {
        float speed = (distance * Mathf.Sqrt(gravity) * Mathf.Sqrt(1 / Mathf.Cos(angle))) / Mathf.Sqrt(2 * distance * Mathf.Sin(angle) + 2 * yOffset * Mathf.Cos(angle));

        return speed;
    }

    public static Vector3 ProjectVectorOnPlane(Vector3 planeNormal, Vector3 vector)
    {
        return vector - (Vector3.Dot(vector, planeNormal) * planeNormal);
    }
}
