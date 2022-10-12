using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public static class Tools
{

    public static bool ScreenRayHit(out RaycastHit hit, Camera camera, LayerMask layers, GameObject gameObject = null)
    {
        Vector2 mousePosition = Input.mousePosition;

        Ray ray = camera.ScreenPointToRay(mousePosition);

        if (Physics.Raycast(ray, out hit, Mathf.Infinity, layers))
        {
            if (hit.transform.gameObject == gameObject || gameObject == null)
            {
                return true;
            }
        }
        return false;
    }

    public static void Timer(MonoBehaviour monoBehaviour, float time = 1, UnityAction callback = null)
    {
        monoBehaviour.StartCoroutine(_Timer(time, callback));
    }

    private static IEnumerator _Timer(float time, UnityAction callback)
    {
        yield return new WaitForSeconds(time);
        callback?.Invoke();
    }

    //

    public static bool HasLayer(LayerMask layerMask, int layer)
    {
        if (layerMask == (layerMask | (1 << layer)))
        {
            return true;
        }

        return false;
    }

    public static void LaunchIEnum(MonoBehaviour monoBehaviour, IEnumerator coroutine, ref IEnumerator memorizedIenum)
    {
        StopIEnum(monoBehaviour, ref memorizedIenum);
        memorizedIenum = coroutine;
        monoBehaviour.StartCoroutine(memorizedIenum);
    }

    public static void StopIEnum(MonoBehaviour monoBehaviour, ref IEnumerator coroutine)
    {
        if (coroutine != null)
        {
            monoBehaviour.StopCoroutine(coroutine);
            coroutine = null;
        }
    }

    public static bool Cross(Vector2 p1, Vector2 p2, Vector2 p3, Vector2 p4, out Vector2 result)
    {
        float n;
        if (p2.y - p1.y != 0)
        {
            float q = (p2.x - p1.x) / (p1.y - p2.y);
            float sn = (p3.x - p4.x) + (p3.y - p4.y) * q;
            if (sn == 0)
            {
                result = default;
                return false;
            }
            float fn = (p3.x - p1.x) + (p3.y - p1.y) * q;
            n = fn / sn;
        }
        else
        {
            if ((p3.y - p4.y) == 0)
            {
                result = default;
                return false;
            }
            n = (p3.y - p1.y) / (p3.y - p4.y);
        }

        float X = p3.x + (p4.x - p3.x) * n;
        float Y = p3.y + (p4.y - p3.y) * n;
        result = new Vector2(X, Y);
        return true;
    }

    public enum Direction
    {
        Up,
        Forward,
        Right
    }

    public static void DrawCircle(Vector3 position, float radius, float density = 20, Direction direction = default)
    {
        Vector3 lastPos = Vector3.zero;
        Vector3 currentPos = Vector3.zero;

        for (float i = 0; i <= (int)density; i++)
        {
            float t = (float)i / density;
            float x = Mathf.Sin(t * 2 * Mathf.PI) * radius / 2;
            float y = Mathf.Cos(t * 2 * Mathf.PI) * radius / 2;

            float targetX = 0;
            float targetY = 0;
            float targetZ = 0;

            switch (direction)
            {
                case Direction.Forward:
                    {
                        targetX = x;
                        targetY = y;
                        break;
                    }
                case Direction.Up:
                    {
                        targetX = x;
                        targetZ = y;
                        break;
                    }
                case Direction.Right:
                    {
                        targetY = y;
                        targetZ = x;
                        break;
                    }
            }

            currentPos = position +
                new Vector3(
                        targetX,
                        targetY,
                        targetZ
                    );
            if (lastPos != Vector3.zero)
            {
                Gizmos.DrawLine(lastPos, currentPos);
            }
            lastPos = currentPos;
        }
    }

    public static Vector3 RandomV3(Vector3 first, Vector3 second)
    {
        float minX = Mathf.Min(first.x, second.x);
        float minY = Mathf.Min(first.y, second.y);
        float minZ = Mathf.Min(first.z, second.z);

        float maxX = Mathf.Max(first.x, second.x);
        float maxY = Mathf.Max(first.y, second.y);
        float maxZ = Mathf.Max(first.z, second.z);

        return new Vector3(Random.Range(minX, maxX), Random.Range(minY, maxY), Random.Range(minZ, maxZ));
    }

    //

    public static void SlowDownTime(float times)
    {
        Time.timeScale /= times;
        Time.fixedDeltaTime *= times;
    }

    public static void SpeedUpTime(float times)
    {
        Time.timeScale *= times;
        Time.fixedDeltaTime /= times;
    }

    public static void ResumeTime()
    {
        Time.timeScale = 1;
        Time.fixedDeltaTime = 0.02f;
    }
}