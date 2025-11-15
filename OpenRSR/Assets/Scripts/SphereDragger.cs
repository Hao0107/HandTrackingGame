using UnityEngine;

public class SphereDragger : MonoBehaviour
{
    public bool isDragging = false;
    public float maxOffset = 6.5f;
    private Rigidbody rb;
    [SerializeField] private GameManager manager;

    // Biến nội bộ để lưu trữ vị trí X đã được chuẩn hóa (-1 đến 1)
    private float currentNormalizedX = 0f;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        if (manager == null)
        {
            Debug.LogError("SphereDragger chưa được gán GameManager trong Inspector!");
        }
    }

    /// <summary>
    /// Hàm này được gọi bởi Script MediaPipe của bạn.
    /// 'isGrabbing' = true nếu tay đang nắm, false nếu đang mở.
    /// </summary>
    public void SetDragging(bool isGrabbing)
    {
        if (manager == null)
        {
            Debug.LogError("LỖI: Biến 'manager' trong SphereDragger chưa được gán!");
            return; // Dừng lại để không gây lỗi
        }

        if (manager.isGamePaused || manager.isGameOver)
        {
            this.isDragging = false;
            return;
        }

        this.isDragging = isGrabbing;
    }

    /// <summary>
    /// Hàm này được gọi bởi Script MediaPipe của bạn.
    /// 'screenNormalizedX' là tọa độ X của tay (ví dụ: ngón trỏ)
    /// với giá trị từ 0.0 (cạnh trái màn hình) đến 1.0 (cạnh phải màn hình).
    /// </summary>
    public void UpdateHandPosition(float screenNormalizedX)
    {
        // Chuyển đổi từ 0.0 -> 1.0 thành -1.0 -> 1.0
        // Giống như logic '((mousePos.x) / Screen.width) * 2f - 1f;' của bạn
        this.currentNormalizedX = (screenNormalizedX * 2f) - 1f;
    }

    private void Update()
    {
        // Tất cả logic Input.GetMouseButton đã bị xóa.
        // 'isDragging' và 'currentNormalizedX' giờ được cập nhật bởi các hàm public ở trên.

        if (isDragging && !manager.isGamePaused && !manager.isGameOver)
        {
            // Sử dụng logic di chuyển X-pos y hệt như code gốc của bạn
            float xPos = maxOffset * currentNormalizedX * Screen.width / Screen.height;

            // Di chuyển vật thể
            transform.Translate(new Vector3(xPos - transform.position.x, 0f, 0f), Space.World);
            rb.position = transform.position;
        }
    }
}