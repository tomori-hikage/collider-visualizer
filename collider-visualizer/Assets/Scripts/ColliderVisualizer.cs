using UnityEngine;
using UnityEngine.UI;


namespace HC.Debug
{
    /// <summary>
    /// Colliderの可視化をするデバッグ機能
    /// </summary>
    [DisallowMultipleComponent]
    public class ColliderVisualizer : MonoBehaviour
    {
        #region フィールド / プロパティ

        /// <summary>
        /// 可視コライダー
        /// </summary>
        private GameObject _visibleCollider;

        private static GameObject _colliderVisualizerCanvas;
        private static GameObject ColliderVisualizerCanvas
        {
            get
            {
                if (_colliderVisualizerCanvas == null)
                {
                    _colliderVisualizerCanvas = new GameObject("ColliderVisualizerCanvas");
                    var canvas = _colliderVisualizerCanvas.AddComponent<Canvas>();
                    canvas.renderMode = RenderMode.ScreenSpaceOverlay;
                    _colliderVisualizerCanvas.AddComponent<CanvasScaler>();
                    _colliderVisualizerCanvas.AddComponent<GraphicRaycaster>();
                }

                return _colliderVisualizerCanvas;
            }
        }

        private static Font _font;

        private static Font Font { get { return _font ?? (_font = Resources.GetBuiltinResource<Font>("Arial.ttf")); } }

        #endregion


        #region イベントメソッド

        private void Start()
        {
            var collider = GetComponent<Collider>();

            switch (collider.GetType().Name)
            {
                case "BoxCollider":
                    _visibleCollider = CreateVisibleCollider(PrimitiveType.Cube);
                    SetVisibleColliderTransform((BoxCollider)collider);
                    break;

                case "SphereCollider":
                    _visibleCollider = CreateVisibleCollider(PrimitiveType.Sphere);
                    SetVisibleColliderTransform((SphereCollider)collider);
                    break;

                case "CapsuleCollider":
                    _visibleCollider = CreateVisibleCollider(PrimitiveType.Capsule);
                    SetVisibleColliderTransform((CapsuleCollider)collider);
                    break;

                default:
                    UnityEngine.Debug.LogWarning("BoxCollider, SphereCollider, CapsuleColliderのみサポートしています。");
                    return;
            }

            // 可視コライダーのマテリアルを設定する
            var material = _visibleCollider.GetComponent<Renderer>().material;
            SetVisibleColliderMaterial(material);

            CreateLabel("Hello");
        }

        #endregion


        #region メソッド

        /// <summary>
        /// ラベルを生成してメッセージを表示する
        /// </summary>
        /// <param name="message">message.</param>
        public void CreateLabel(string message)
        {
            var label = new GameObject("Label");
            label.transform.SetParent(ColliderVisualizerCanvas.transform);

            // TODO: 適切な位置に表示する
            var text = label.AddComponent<Text>();
            text.font = Font;
            text.alignment = TextAnchor.MiddleCenter;
            text.raycastTarget = false;
            text.text = message;
        }

        /// <summary>
        /// 可視コライダーを生成する
        /// </summary>
        /// <param name="primitiveType">primitiveType.</param>
        /// <returns>可視コライダー</returns>
        private GameObject CreateVisibleCollider(PrimitiveType primitiveType)
        {
            var visibleCollider = GameObject.CreatePrimitive(primitiveType);
            visibleCollider.transform.SetParent(transform, worldPositionStays: false);
            // Colliderは不要なので削除する
            Destroy(visibleCollider.GetComponent<Collider>());

            return visibleCollider;
        }

        /// <summary>
        /// 可視コライダーのTransformを設定する
        /// </summary>
        /// <param name="boxCollider">boxCollider.</param>
        private void SetVisibleColliderTransform(BoxCollider boxCollider)
        {
            // BoxColliderのプロパティを加味したTransformにする
            var visibleColliderTransform = _visibleCollider.transform;
            visibleColliderTransform.localPosition += boxCollider.center;
            visibleColliderTransform.localScale = Vector3.Scale(visibleColliderTransform.localScale, boxCollider.size);
        }

        /// <summary>
        /// 可視コライダーのTransformを設定する
        /// </summary>
        /// <param name="sphereCollider">sphereCollider.</param>
        private void SetVisibleColliderTransform(SphereCollider sphereCollider)
        {
            // SphereColliderのプロパティを加味したTransformにする
            var visibleColliderTransform = _visibleCollider.transform;
            visibleColliderTransform.localPosition += sphereCollider.center;
            visibleColliderTransform.localScale *= sphereCollider.radius * 2f;
        }

        /// <summary>
        /// 可視コライダーのTransformを設定する
        /// </summary>
        /// <param name="capsuleCollider">capsuleCollider.</param>
        private void SetVisibleColliderTransform(CapsuleCollider capsuleCollider)
        {
            // CapsuleColliderのプロパティを加味したTransformにする
            var visibleColliderTransform = _visibleCollider.transform;
            visibleColliderTransform.localPosition += capsuleCollider.center;

            switch (capsuleCollider.direction)
            {
                // X-Axis
                case 0:
                    visibleColliderTransform.Rotate(Vector3.forward * 90f);
                    break;

                // Y-Axis
                case 1:
                    break;

                // Z-Axis
                case 2:
                    visibleColliderTransform.Rotate(Vector3.right * 90f);
                    break;
            }

            Vector3 capsuleLocalScale = visibleColliderTransform.localScale;
            float radius = capsuleCollider.radius;
            float newCapsuleLocalScaleX = capsuleLocalScale.x * radius * 2f;
            float newCapsuleLocalScaleY = capsuleLocalScale.y * capsuleCollider.height * 0.5f;
            float newCapsuleLocalScaleZ = capsuleLocalScale.z * radius * 2f;
            visibleColliderTransform.localScale = new Vector3(newCapsuleLocalScaleX, newCapsuleLocalScaleY, newCapsuleLocalScaleZ);
        }

        /// <summary>
        /// 可視コライダーのマテリアルを設定する
        /// </summary>
        /// <param name="material">material.</param>
        private void SetVisibleColliderMaterial(Material material)
        {
            material.shader = Shader.Find("Sprites/Default");
            material.color = new Color(1f, 0f, 0f, 0.2f);
        }

        #endregion
    }
}