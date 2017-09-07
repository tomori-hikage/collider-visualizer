using UnityEngine;
using HC.Extensions;


namespace HC.Debug
{
    /// <summary>
    /// Colliderの可視化をするデバッグ機能
    /// </summary>
    [DisallowMultipleComponent]
    public class ColliderVisualizer : MonoBehaviour
    {
        #region イベントメソッド

        private void Start()
        {
            var collider = GetComponent<Collider>();

            switch (collider.GetType().Name)
            {
                case "BoxCollider":
                    VisualizeBoxCollider((BoxCollider)collider);
                    break;

                case "SphereCollider":
                    VisualizeSphereCollider((SphereCollider)collider);
                    break;

                case "CapsuleCollider":
                    VisualizeCapsuleCollider((CapsuleCollider)collider);
                    break;

                default:
                    UnityEngine.Debug.LogWarning("BoxCollider, SphereCollider, CapsuleColliderのみサポートしています。");
                    return;
            }
        }

        #endregion


        #region メソッド

        /// <summary>
        /// BoxColliderを可視化する
        /// </summary>
        /// <param name="boxCollider">boxCollider.</param>
        private void VisualizeBoxCollider(BoxCollider boxCollider)
        {
            // キューブを子として生成する
            var cube = GameObject.CreatePrimitive(PrimitiveType.Cube);
            var cubeTransform = cube.transform;
            cubeTransform.SetParent(transform, worldPositionStays: false);
            // BoxColliderは不要なので削除する
            Destroy(cube.GetComponent<Collider>());

            // BoxColliderのプロパティを加味したTransformにする
            cubeTransform.localPosition += boxCollider.center;
            cubeTransform.localScale = Vector3.Scale(cubeTransform.localScale, boxCollider.size);

            // 可視コライダーのマテリアルを設定する
            var cubeMaterial = cube.GetComponent<Renderer>().material;
            SetVisibleColliderMaterial(cubeMaterial);
        }

        /// <summary>
        /// SphereColliderを可視化する
        /// </summary>
        /// <param name="sphereCollider">sphereCollider.</param>
        private void VisualizeSphereCollider(SphereCollider sphereCollider)
        {
            // スフィアを子として生成する
            var sphere = GameObject.CreatePrimitive(PrimitiveType.Sphere);
            var sphereTransform = sphere.transform;
            sphereTransform.SetParent(transform, worldPositionStays: false);
            // SphereColliderは不要なので削除する
            Destroy(sphere.GetComponent<Collider>());

            // SphereColliderのプロパティを加味したTransformにする
            sphereTransform.localPosition += sphereCollider.center;
            sphereTransform.localScale *= sphereCollider.radius * 2f;

            // 可視コライダーのマテリアルを設定する
            var sphereMaterial = sphere.GetComponent<Renderer>().material;
            SetVisibleColliderMaterial(sphereMaterial);
        }

        /// <summary>
        /// CapsuleColliderを可視化する
        /// </summary>
        /// <param name="capsuleCollider">capsuleCollider.</param>
        private void VisualizeCapsuleCollider(CapsuleCollider capsuleCollider)
        {
            // カプセルを子として生成する
            var capsule = GameObject.CreatePrimitive(PrimitiveType.Capsule);
            var capsuleTransform = capsule.transform;
            capsuleTransform.SetParent(transform, worldPositionStays: false);
            // CapsuleColliderは不要なので削除する
            Destroy(capsule.GetComponent<Collider>());


            // CapsuleColliderのプロパティを加味したTransformにする
            capsuleTransform.localPosition += capsuleCollider.center;

            switch (capsuleCollider.direction)
            {
                // X-Axis
                case 0:
                    capsuleTransform.Rotate(Vector3.forward * 90f);
                    break;

                // Y-Axis
                case 1:
                    break;

                // Z-Axis
                case 2:
                    capsuleTransform.Rotate(Vector3.right * 90f);
                    break;
            }

            Vector3 capsuleLocalScale = capsuleTransform.localScale;
            float radius = capsuleCollider.radius;
            float newCapsuleLocalScaleX = capsuleLocalScale.x * radius * 2f;
            float newCapsuleLocalScaleY = capsuleLocalScale.y * capsuleCollider.height * 0.5f;
            float newCapsuleLocalScaleZ = capsuleLocalScale.z * radius * 2f;
            capsuleTransform.localScale = new Vector3(newCapsuleLocalScaleX, newCapsuleLocalScaleY, newCapsuleLocalScaleZ);


            // 可視コライダーのマテリアルを設定する
            var capsuleMaterial = capsule.GetComponent<Renderer>().material;
            SetVisibleColliderMaterial(capsuleMaterial);
        }

        /// <summary>
        /// 可視コライダーのマテリアルを設定する
        /// </summary>
        /// <param name="material">material.</param>
        private void SetVisibleColliderMaterial(Material material)
        {
            material.SetRenderingMode(MaterialExtensions.RenderingMode.Fade);
            material.color = new Color(1f, 0f, 0f, 0.6f);
        }

        #endregion
    }
}