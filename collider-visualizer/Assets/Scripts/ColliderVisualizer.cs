using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections.Generic;


namespace HC.Debug
{
    /// <summary>
    /// Colliderの可視化をするデバッグ機能
    /// ※ 必ずInitializeメソッドを呼ぶこと
    /// </summary>
    [DisallowMultipleComponent]
    public class ColliderVisualizer : MonoBehaviour
    {
        #region 列挙型

        /// <summary>
        /// 可視コライダーの色の種類
        /// </summary>
        public enum VisualizerColorType
        {
            Red,
            Green,
            Blue
        }

        /// <summary>
        /// オブジェクトの等価比較をサポートするクラス
        /// ※ 列挙型をキーにしたDictionaryを高速化するために実装
        /// </summary>
        public class VisualizerColorTypeComparer : IEqualityComparer<VisualizerColorType>
        {
            public bool Equals(VisualizerColorType x, VisualizerColorType y)
            {
                return x == y;
            }

            public int GetHashCode(VisualizerColorType obj)
            {
                return (int)obj;
            }
        }

        #endregion


        #region 定数

        /// <summary>
        /// レイアウトの基準解像度
        /// </summary>
        private static readonly Vector2 ReferenceResolution = new Vector2(800f, 600f);

        /// <summary>
        /// 可視コライダーの色の種類
        /// </summary>
        private static readonly Dictionary<VisualizerColorType, Color> VisualizerColorDictionary =
            new Dictionary<VisualizerColorType, Color>(new VisualizerColorTypeComparer())
            {
                { VisualizerColorType.Red, new Color32(255, 0, 0, 50) },
                { VisualizerColorType.Green, new Color32(0, 255, 0, 50) },
                { VisualizerColorType.Blue, new Color32(0, 0, 255, 50) }
            };

        #endregion


        #region フィールド / プロパティ

        /// <summary>
        /// 可視コライダー
        /// </summary>
        private GameObject _visualizer;

        /// <summary>
        /// ラベル
        /// </summary>
        private Text _label;

        /// <summary>
        /// 可視コライダーのラベルを表示するためのCanvas
        /// </summary>
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
                    var canvasScaler = _colliderVisualizerCanvas.AddComponent<CanvasScaler>();
                    canvasScaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
                    canvasScaler.referenceResolution = ReferenceResolution;
                    canvasScaler.matchWidthOrHeight = 1f;
                    _colliderVisualizerCanvas.AddComponent<GraphicRaycaster>();
                }

                return _colliderVisualizerCanvas;
            }
        }

        /// <summary>
        /// ラベルのフォント
        /// </summary>
        private static Font _font;
        private static Font Font { get { return _font ?? (_font = Resources.GetBuiltinResource<Font>("Arial.ttf")); } }

        #endregion


        #region イベントメソッド

        private void LateUpdate()
        {
            if (_visualizer == null || _label == null) return;

            // ラベルを可視コライダーの位置に追従する
            _label.rectTransform.position = RectTransformUtility.WorldToScreenPoint(Camera.main, _visualizer.transform.position);
        }

        private void OnDestroy()
        {
            if (_label == null) return;

            Destroy(_label.gameObject);
            Destroy(_visualizer);
        }

        #endregion


        #region メソッド

        /// <summary>
        /// 初期化する
        /// </summary>
        /// <param name="visualizerColor">visualizerColor.</param>
        /// <param name="message">message.</param>
        /// <param name="fontSize">fontSize.</param>
        public void Initialize(VisualizerColorType visualizerColor, string message, int fontSize)
        {
            Initialize(VisualizerColorDictionary[visualizerColor], message, fontSize);
        }

        /// <summary>
        /// 初期化する
        /// </summary>
        /// <param name="color">color.</param>
        /// <param name="message">message.</param>
        /// <param name="fontSize">fontSize.</param>
        public void Initialize(Color color, string message, int fontSize)
        {
            var targetCollider = GetComponent<Collider>();

            if (targetCollider is BoxCollider)
            {
                _visualizer = CreateVisualizer(PrimitiveType.Cube);
                SetVisualizerTransform((BoxCollider)targetCollider);
            }
            else if (targetCollider is SphereCollider)
            {
                _visualizer = CreateVisualizer(PrimitiveType.Sphere);
                SetVisualizerTransform((SphereCollider)targetCollider);
            }
            else if (targetCollider is CapsuleCollider)
            {
                _visualizer = CreateVisualizer(PrimitiveType.Capsule);
                SetVisualizerTransform((CapsuleCollider)targetCollider);
            }
            else
            {
                UnityEngine.Debug.LogAssertion("BoxCollider, SphereCollider, CapsuleColliderのみサポートしています。");
                return;
            }

            // 可視コライダーのマテリアルを設定する
            var material = _visualizer.GetComponent<Renderer>().material;
            material.shader = Shader.Find("Sprites/Default");
            // 色を設定する
            material.color = color;

            // ラベルを生成する
            CreateLabel(message, fontSize);
        }

        /// <summary>
        /// 可視コライダーを生成する
        /// </summary>
        /// <param name="primitiveType">primitiveType.</param>
        /// <returns>可視コライダー</returns>
        private GameObject CreateVisualizer(PrimitiveType primitiveType)
        {
            GameObject visualizer = GameObject.CreatePrimitive(primitiveType);
            visualizer.transform.SetParent(transform, worldPositionStays: false);

            // Colliderは不要なので削除する
            var visibleCollider = visualizer.GetComponent<Collider>();
            visibleCollider.enabled = false;
            Destroy(visibleCollider);

            return visualizer;
        }

        /// <summary>
        /// 可視コライダーのTransformを設定する
        /// </summary>
        /// <param name="boxCollider">boxCollider.</param>
        private void SetVisualizerTransform(BoxCollider boxCollider)
        {
            // BoxColliderのプロパティを加味したTransformにする
            Transform visualizerTransform = _visualizer.transform;
            visualizerTransform.localPosition += boxCollider.center;
            visualizerTransform.localScale = Vector3.Scale(visualizerTransform.localScale, boxCollider.size);
        }

        /// <summary>
        /// 可視コライダーのTransformを設定する
        /// </summary>
        /// <param name="sphereCollider">sphereCollider.</param>
        private void SetVisualizerTransform(SphereCollider sphereCollider)
        {
            // SphereColliderのプロパティを加味したTransformにする
            Transform visualizerTransform = _visualizer.transform;
            visualizerTransform.localPosition += sphereCollider.center;
            visualizerTransform.localScale *= sphereCollider.radius * 2f;
        }

        /// <summary>
        /// 可視コライダーのTransformを設定する
        /// </summary>
        /// <param name="capsuleCollider">capsuleCollider.</param>
        private void SetVisualizerTransform(CapsuleCollider capsuleCollider)
        {
            // CapsuleColliderのプロパティを加味したTransformにする
            Transform visualizerTransform = _visualizer.transform;
            visualizerTransform.localPosition += capsuleCollider.center;

            switch (capsuleCollider.direction)
            {
                // X-Axis
                case 0:
                    visualizerTransform.Rotate(Vector3.forward * 90f);
                    break;

                // Y-Axis
                case 1:
                    break;

                // Z-Axis
                case 2:
                    visualizerTransform.Rotate(Vector3.right * 90f);
                    break;

                default:
                    throw new ArgumentOutOfRangeException();
            }

            Vector3 capsuleLocalScale = visualizerTransform.localScale;
            float radius = capsuleCollider.radius;
            float newCapsuleLocalScaleX = capsuleLocalScale.x * radius * 2f;
            float newCapsuleLocalScaleY = capsuleLocalScale.y * capsuleCollider.height * 0.5f;
            float newCapsuleLocalScaleZ = capsuleLocalScale.z * radius * 2f;
            visualizerTransform.localScale = new Vector3(newCapsuleLocalScaleX, newCapsuleLocalScaleY, newCapsuleLocalScaleZ);
        }

        /// <summary>
        /// ラベルを生成する
        /// </summary>
        /// <param name="message">message.</param>
        /// <param name="fontSize">fontSize.</param>
        private void CreateLabel(string message, int fontSize)
        {
            var label = new GameObject("Label");
            label.transform.SetParent(ColliderVisualizerCanvas.transform, worldPositionStays: false);

            _label = label.AddComponent<Text>();
            _label.font = Font;
            _label.fontSize = fontSize;
            _label.alignment = TextAnchor.MiddleCenter;
            _label.raycastTarget = false;
            _label.text = message;

            var contentSizeFitter = label.AddComponent<ContentSizeFitter>();
            contentSizeFitter.horizontalFit = ContentSizeFitter.FitMode.PreferredSize;
            contentSizeFitter.verticalFit = ContentSizeFitter.FitMode.PreferredSize;
        }

        #endregion
    }
}