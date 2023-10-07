using System.Collections;
using UnityEngine;

public class BezierMove : MonoBehaviour
{
    public Transform startPoint;
    public Transform firstControlPoint; // 기존의 controlPoint를 firstControlPoint로 이름 변경
    public Transform secondControlPoint; // 새로운 제어점
    public Transform endPoint;

    public float duration = 2f; // 이동에 걸리는 시간
    private bool isMoving = false;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) && !isMoving)
        {
            StartCoroutine(MoveAlongBezier());
        }
    }

    IEnumerator MoveAlongBezier()
    {
        isMoving = true;
        float t = 0;

        while (t < 1)
        {
            t += Time.deltaTime / duration;

            Vector3 position = CalculateBezierPoint(t, startPoint.position, firstControlPoint.position, secondControlPoint.position, endPoint.position);
            transform.position = position;

            // 로테이션도 베지어 곡선으로 보간
            Quaternion rotation = CalculateBezierRotation(t, startPoint.rotation, firstControlPoint.rotation, secondControlPoint.rotation, endPoint.rotation);
            transform.rotation = rotation;

            yield return null;
        }

        isMoving = false;
    }

    Quaternion CalculateBezierRotation(float t, Quaternion q0, Quaternion q1, Quaternion q2, Quaternion q3)
    {
        Quaternion i = Quaternion.Lerp(q0, q1, t);
        Quaternion j = Quaternion.Lerp(q1, q2, t);
        Quaternion k = Quaternion.Lerp(q2, q3, t);
        Quaternion l = Quaternion.Lerp(i, j, t);
        Quaternion m = Quaternion.Lerp(j, k, t);
        return Quaternion.Lerp(l, m, t);
    }




    Vector3 CalculateBezierPoint(float t, Vector3 p0, Vector3 p1, Vector3 p2, Vector3 p3)
    {
        float u = 1 - t;
        float tt = t * t;
        float uu = u * u;
        float uuu = uu * u;
        float ttt = tt * t;

        Vector3 p = uuu * p0;
        p += 3 * uu * t * p1;
        p += 3 * u * tt * p2;
        p += ttt * p3;

        return p;
    }
}
