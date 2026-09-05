using UnityEngine;

namespace ProjectLopHoc.Core.Interfaces
{
    /// <summary>
    /// Interface chuẩn cho tất cả các đối tượng có thể tương tác trong thế giới 3D/VR.
    /// </summary>
    public interface IInteractable
    {
        /// <summary>
        /// Gọi khi người chơi nhấn nút tương tác (Trigger VR hoặc Click chuột).
        /// </summary>
        /// <param name="interactor">GameObject của người thực hiện tương tác</param>
        void OnInteract(GameObject interactor);

        /// <summary>
        /// Gọi khi tia Laser VR vừa chiếu vào vật thể (dùng để làm sáng/highlight nút).
        /// </summary>
        void OnHoverEnter();

        /// <summary>
        /// Gọi khi tia Laser VR rời khỏi vật thể.
        /// </summary>
        void OnHoverExit();
    }
}