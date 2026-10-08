using UnityEngine;
using UnityEngine.Tilemaps;

public class CameraFollow : MonoBehaviour
{
    [SerializeField] private Transform player;
    [SerializeField] private Tilemap levelTilemap;
    [SerializeField] private float smoothSpeed = 5f;

    private Camera cam;

    private float minX;
    private float maxX;

    private void Awake()
    {
        cam = GetComponent<Camera>();

        levelTilemap.CompressBounds();

        Bounds bounds = levelTilemap.localBounds;

        
        float left = levelTilemap.transform.TransformPoint(bounds.min).x;
        float right = levelTilemap.transform.TransformPoint(bounds.max).x;

        float halfWidth = cam.orthographicSize * cam.aspect;

        minX = left + halfWidth;
        maxX = right - halfWidth;
    }

    private void LateUpdate()
    {
        float targetX = Mathf.Clamp(player.position.x, minX, maxX);

        Vector3 targetPosition = new Vector3(targetX, transform.position.y, transform.position.z);

        transform.position = Vector3.Lerp(transform.position, targetPosition, smoothSpeed * Time.deltaTime);
    }


}