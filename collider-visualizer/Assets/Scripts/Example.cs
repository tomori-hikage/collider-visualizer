using UnityEngine;
using HC.Debug;


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
        private GameObject _cube;

        [SerializeField]
        private GameObject _sphere;

        [SerializeField]
        private GameObject _capsule;

        #endregion


        #region イベントメソッド

        private void Start()
        {
            int fontSize = 88;
            _cube.AddComponent<ColliderVisualizer>().Initialize(ColliderVisualizer.VisualizerColorType.Red, "Cube", fontSize);
            _sphere.AddComponent<ColliderVisualizer>().Initialize(ColliderVisualizer.VisualizerColorType.Green, "Sphere", fontSize);
            _capsule.AddComponent<ColliderVisualizer>().Initialize(ColliderVisualizer.VisualizerColorType.Blue, "Capsule", fontSize);
        }

        #endregion
    }
}