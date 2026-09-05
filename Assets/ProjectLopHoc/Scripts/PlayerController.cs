using Fusion;
using UnityEngine;

public class PlayerController : NetworkBehaviour
{
    private NetworkCharacterController _ncc;
    private Animator _animator;
    private Camera _mainCamera;

    [Header("--- Cấu hình Camera ---")]
    public Vector3 cameraOffset = new Vector3(0, 2.5f, -4f); // Camera nằm sau lưng và hơi cao lên
    public float cameraFollowSpeed = 5f;

    private void Awake()
    {
        _ncc = GetComponent<NetworkCharacterController>();
        _animator = GetComponentInChildren<Animator>();
    }

    public override void Spawned()
    {
        // Khi nhân vật sinh ra, nếu đây là nhân vật của BẠN (HasStateAuthority)
        if (HasStateAuthority)
        {
            // Tìm Camera chính trong Scene để chuẩn bị bám theo
            _mainCamera = Camera.main;
        }
    }

    // Bắt buộc dùng FixedUpdateNetwork trong Fusion thay cho Update bình thường
    public override void FixedUpdateNetwork()
    {
        // Chỉ người chủ của nhân vật này mới được quyền điều khiển nó
        if (!HasStateAuthority) return;

        // 1. Đọc tín hiệu phím WASD
        float moveHorizontal = Input.GetAxisRaw("Horizontal"); // A, D
        float moveVertical = Input.GetAxisRaw("Vertical");     // W, S

        Vector3 moveDirection = new Vector3(moveHorizontal, 0, moveVertical).normalized;

        // 2. Lệnh di chuyển nhân vật (Tốc độ đi mặc định)
        _ncc.Move(moveDirection * 4000f * Runner.DeltaTime);

        // 3. Xoay mặt nhân vật về hướng đang đi
        if (moveDirection.sqrMagnitude > 0)
        {
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(moveDirection), Runner.DeltaTime * 10f);
        }

        // 4. Kích hoạt Animator Blend Tree (Truyền vào biến Speed đã tạo)
        if (_animator != null)
        {
            _animator.SetFloat("Speed", moveDirection.magnitude);
        }
    }

    private void LateUpdate()
    {
        // 5. Xử lý Camera bám theo đuôi nhân vật một cách mượt mà (chỉ máy mình thấy)
        if (HasStateAuthority && _mainCamera != null)
        {
            Vector3 targetPosition = transform.position + cameraOffset;
            _mainCamera.transform.position = Vector3.Lerp(_mainCamera.transform.position, targetPosition, Time.deltaTime * cameraFollowSpeed);
            
            // Ép camera luôn quay mặt nhìn vào nhân vật
            _mainCamera.transform.LookAt(transform.position + Vector3.up * 400f);
        }
    }
}