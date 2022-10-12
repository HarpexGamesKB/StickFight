using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public static class ExtensionMethods
{
    // ������������ �� � �� b, ��� t � �������� 0-1
    private static float Lerp(float a, float b, float t)
    {
        return (1.0f - t) * a + b * t;
    }

    // ��������������� ������������ �� � �� b, ��� v � �������� 0-1
    private static float InvLerp(float a, float b, float v)
    {
        return (v - a) / (b - a);
    }

    // ������������ �������� � �������� ��������� iMin - iMax � ������� oMin - oMax
    public static float Remap(this float value, float iMin, float iMax, float oMin, float oMax)
    {
        float t = InvLerp(iMin, iMax, value);
        return Lerp(oMin, oMax, t);
    }

    // ������� ��������� ���� �� ����
    public static void DestroyComponent<T>(this GameObject gameObject) where T : Object
    {
        if (gameObject.GetComponent<T>() != null)
        {
            Object.Destroy(gameObject.GetComponent<T>());
        }
    }

    // ���������� ������� ���� �������� ���������� ���� bool = true
    public static bool Use(this bool isTrue, UnityAction fuctionIfTrue)
    {
        if (isTrue)
        {
            fuctionIfTrue?.Invoke();
        }
        return isTrue;
    }

    // ���������� ������� ���� �������� ���������� ���� bool != true
    public static void Else(this bool isTrue, UnityAction fuctionIfFalse)
    {
        if (!isTrue)
        {
            fuctionIfFalse?.Invoke();
        }
    }

    // ������� transform � ������������, ���������
    public static void Move(this Transform transform, Vector3 direction, float speed, bool fixedUpdate = false)
    {
        transform.position += direction * speed * (fixedUpdate ? Time.fixedDeltaTime : Time.deltaTime);
    }

    public static void SmoothRotation(this Transform transform, Vector3 direction, float speed)
    {
        transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(direction), speed);
    }

    public static void SmoothRotation(this Transform transform, Quaternion quaternion, float speed)
    {
        transform.rotation = Quaternion.Lerp(transform.rotation, quaternion, speed);
    }

    // ��������, ��������� �� ���� � ������� �� ����������
    public static bool InDistance(this Transform transform, Transform target, float radius)
    {
        return Mathf.Pow(radius, 2) > (target.position - transform.position).sqrMagnitude;
    }

    // ��������, ��������� �� ������� ������� � ������� �� �������������� �������
    public static bool InDistance(this Vector3 position, Vector3 targetPosition, float radius)
    {
        return Mathf.Pow(radius, 2) > (targetPosition - position).sqrMagnitude;
    }

    // ��������, ��������� �� ���� ��� ������� �� ����������
    public static bool OutDistance(this Transform transform, Transform target, float radius)
    {
        return Mathf.Pow(radius, 2) < (target.position - transform.position).sqrMagnitude;
    }

    // ��������, ��������� �� ������� ������� ��� ������� �� �������������� �������
    public static bool OutDistance(this Vector3 position, Vector3 targetPosition, float radius)
    {
        return Mathf.Pow(radius, 2) < (targetPosition - position).sqrMagnitude;
    }

    // ��������� ���������� �� �������
    public static void Split(this Vector3 vector, out float x, out float y, out float z)
    {
        x = vector.x;
        y = vector.y;
        z = vector.z;
    }

    // ���������� ���������� ���� bool � true �������� � �������������� ��������, ���� ���������
    public static bool True(this ref bool b, UnityAction action = null)
    {
        if (!b)
        {
            b = true;
            action?.Invoke();
        }
        return b;
    }

    // ���������� ���������� ���� bool � false �������� � �������������� ��������, ���� ���������
    public static bool False(this ref bool b, UnityAction action = null)
    {
        if (b)
        {
            b = false;
            action?.Invoke();
        }
        return b;
    }

    // ��������� ������� � ������ � ���������� ���
    public static T AddR<T>(this List<T> list, T member)
    {
        list.Add(member);
        return member;
    }

    // ������� ��������� ������� � ���������� ���
    public static T RemoveRandom<T>(this List<T> list)
    {
        T item = list[Random.Range(0, list.Count - 1)];
        list.Remove(item);
        return item;
    }

    // ������� ���������� ����� ���������
    public static Material OwnMaterial(this Renderer renderer)
    {
        Material material = new Material(renderer.material);
        renderer.material = material;
        return material;
    }

    // ��������� �������������� ������� ����� � ���� �����
    public static bool HasLayer(this Component component, LayerMask layerMask)
    {
        if (layerMask == (layerMask | (1 << component.gameObject.layer)))
        {
            return true;
        }

        return false;
    }
}