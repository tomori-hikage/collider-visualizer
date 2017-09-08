using HC.Debug;
using UnityEngine;


namespace HC
{
    /// <summary>
    /// 使用例
    /// </summary>
    [DisallowMultipleComponent]
    public class Example : MonoBehaviour
    {
        #region フィールド / プロパティ

        [SerializeField]
        private ColliderVisualizer _cubeColliderVisualizer;

        [SerializeField]
        private ColliderVisualizer _sphereColliderVisualizer;

        [SerializeField]
        private ColliderVisualizer _capsuleColliderVisualizer;

        #endregion


        #region イベントメソッド

        private void Start()
        {
            _cubeColliderVisualizer.CreateLabel("Cube", 24);
            _sphereColliderVisualizer.CreateLabel("Sphere", 24);
            _capsuleColliderVisualizer.CreateLabel("Capsule", 24);
        }

        #endregion
    }
}