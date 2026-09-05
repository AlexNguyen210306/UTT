using Fusion;
using Fusion.Sockets;
using System;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSpawner : MonoBehaviour, INetworkRunnerCallbacks
{
    [Header("--- Cấu hình Nhân vật ---")]
    [Tooltip("Prefab nhân vật dành cho Sinh viên")]
    public NetworkPrefabRef studentPrefab;
    [Tooltip("Prefab nhân vật dành cho Giảng viên (nếu có riêng)")]
    public NetworkPrefabRef lecturerPrefab;

    [Header("--- Vị trí Spawn ---")]
    [Tooltip("Kéo vật thể đại diện cho Bục Giảng Viên vào đây")]
    public Transform lecturerSpawnPoint;
    [Tooltip("Kéo vật thể đại diện cho Khu vực Sinh Viên vào đây")]
    public Transform studentSpawnPoint;

    private void Start()
    {
        // Tự động tìm hệ thống mạng đang chạy và đăng ký lắng nghe sự kiện
        NetworkRunner runner = FindAnyObjectByType<NetworkRunner>();
        if (runner != null)
        {
            runner.AddCallbacks(this);
        }
    }

    public void OnPlayerJoined(NetworkRunner runner, PlayerRef player)
    {
        // Trong chế độ Shared Mode, mỗi máy tự gọi lệnh spawn nhân vật cho chính mình
        if (player == runner.LocalPlayer)
        {
            // 1. Đọc dữ liệu quyền từ lúc Đăng nhập
            bool isLecturer = PlayerPrefs.GetInt("IsLecturer", 0) == 1;

            // 2. Phân loại Prefab (Nếu dùng chung 1 prefab thì gán cả 2 ô trên Inspector giống nhau)
            NetworkPrefabRef prefabToSpawn = isLecturer ? lecturerPrefab : studentPrefab;
            
            // 3. Phân loại Vị trí: Giảng viên lên bục, Sinh viên ở dưới
            Vector3 spawnPosition = isLecturer ? lecturerSpawnPoint.position : studentSpawnPoint.position;
            Quaternion spawnRotation = isLecturer ? lecturerSpawnPoint.rotation : studentSpawnPoint.rotation;

            // 4. Sinh ra nhân vật
            runner.Spawn(prefabToSpawn, spawnPosition, spawnRotation, player);
            
            Debug.Log($"[PlayerSpawner] Đã đưa {(isLecturer ? "Giảng viên lên bục" : "Sinh viên vào lớp")} thành công!");
        }
    }

    private void OnDestroy()
    {
        // Hủy lắng nghe khi Scene này bị đóng
        NetworkRunner runner = FindAnyObjectByType<NetworkRunner>();
        if (runner != null)
        {
            runner.RemoveCallbacks(this);
        }
    }

    // ==========================================
    // CÁC HÀM BẮT BUỘC CỦA GIAO DIỆN FUSION
    // (Để trống để tránh lỗi kịch bản)
    // ==========================================
    public void OnPlayerLeft(NetworkRunner runner, PlayerRef player) { }
    public void OnInput(NetworkRunner runner, NetworkInput input) { }
    public void OnInputMissing(NetworkRunner runner, PlayerRef player, NetworkInput input) { }
    public void OnShutdown(NetworkRunner runner, ShutdownReason shutdownReason) { }
    public void OnConnectedToServer(NetworkRunner runner) { }
    public void OnDisconnectedFromServer(NetworkRunner runner, NetDisconnectReason reason) { }
    public void OnConnectRequest(NetworkRunner runner, NetworkRunnerCallbackArgs.ConnectRequest request, byte[] token) { }
    public void OnConnectFailed(NetworkRunner runner, NetAddress remoteAddress, NetConnectFailedReason reason) { }
    public void OnUserSimulationMessage(NetworkRunner runner, SimulationMessagePtr message) { }
    public void OnSessionListUpdated(NetworkRunner runner, List<SessionInfo> sessionList) { }
    
    public void OnCustomAuthenticationResponse(NetworkRunner runner, Dictionary<string, object> data) { }
    public void OnHostMigration(NetworkRunner runner, HostMigrationToken hostMigrationToken) { }
    public void OnReliableDataReceived(NetworkRunner runner, PlayerRef player, ReliableKey key, ReadOnlySpan<byte> data) { }    public void OnReliableDataProgress(NetworkRunner runner, PlayerRef player, ReliableKey key, float progress) { }
    public void OnSceneLoadDone(NetworkRunner runner) { }
    public void OnSceneLoadStart(NetworkRunner runner) { }
    public void OnObjectExitAOI(NetworkRunner runner, NetworkObject obj, PlayerRef player) { }
    public void OnObjectEnterAOI(NetworkRunner runner, NetworkObject obj, PlayerRef player) { }
}