using UnityEngine;
using System.Collections.Generic;

namespace ProjectLopHoc.Core.ScriptableObjects
{
    [CreateAssetMenu(fileName = "NewClassroomConfig", menuName = "ProjectLopHoc/Configs/Classroom Config")]
    public class ClassroomConfigSO : ScriptableObject
    {
        [Header("thông tin Lớp Học")]
        public string classroomId = "ROOM_101";
        public string classroomName = "Phòng học Lý thuyết & Thực hành Đường sắt";
        public int maxStudents = 30;

        [Header("Cấu hình Bài giảng (PPT Slides)")]
        [Tooltip("Danh sách các hình ảnh đã xuất ra từ file PowerPoint")]
        public List<Sprite> slideImages = new List<Sprite>();

        [Header("Mật khẩu Giảng viên (Tạm thời lưu tại đây)")]
        public string lecturerSecretPin = "123456";
    }
}