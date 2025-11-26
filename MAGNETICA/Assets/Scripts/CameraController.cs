using UnityEngine;

public class CameraController : MonoBehaviour
{
    [Header("Camera Settings")]
    public Transform target; // 플레이어
    public float cameraFollowSpeed = 5.0f; // 카메라 부드러운 이동 속도
    public Vector2 offset = new Vector2(-2f, 0f); // 플레이어 기준 카메라 위치

    [Header("X-axis Movement Range")]
    public float minX = -10f; // 카메라 최소 X값
    public float maxX = 6000f;  // 카메라 최대 X값

    [Header("Y Fixed Position")]
    public float fixedY = 12f; // Y 위치를 고정할 값

    void LateUpdate()
    {
        if (target == null) return;

        Vector3 targetPos = transform.position;

        // X는 플레이어 위치 + 오프셋을 따라감 (Clamp로 제한)
        float targetX = Mathf.Clamp(target.position.x + offset.x, minX, maxX);

        targetPos.x = targetX;
        targetPos.y = fixedY;     // Y 고정
        targetPos.z = -10f;       // 카메라 Z 고정

        // 부드러운 이동
        transform.position = Vector3.Lerp(transform.position, targetPos, cameraFollowSpeed * Time.deltaTime);
    }
}
