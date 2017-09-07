using UnityEngine;


namespace HC.Extensions
{
    /// <summary>
    /// Materialの拡張メソッドクラス
    /// </summary>
    public static class MaterialExtensions
    {
        #region 列挙型

        /// <summary>
        /// StandardShaderのRenderingMode
        /// </summary>
        public enum RenderingMode
        {
            Opaque,
            Cutout,
            Fade,
            Transparent
        }

        #endregion


        #region メソッド

        /// <summary>
        /// RenderingModeを設定する
        /// </summary>
        /// <param name="self">self.</param>
        /// <param name="renderingMode">StandardShaderのRenderingMode</param>
        public static void SetRenderingMode(this Material self, RenderingMode renderingMode)
        {
            switch (renderingMode)
            {
                case RenderingMode.Opaque:
                    self.SetFloat("_Mode", 0);
                    self.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.One);
                    self.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.Zero);
                    self.SetInt("_ZWrite", 1);
                    self.DisableKeyword("_ALPHATEST_ON");
                    self.DisableKeyword("_ALPHABLEND_ON");
                    self.DisableKeyword("_ALPHAPREMULTIPLY_ON");
                    self.renderQueue = -1;
                    break;

                case RenderingMode.Cutout:
                    self.SetFloat("_Mode", 1);
                    self.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.One);
                    self.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.Zero);
                    self.SetInt("_ZWrite", 1);
                    self.EnableKeyword("_ALPHATEST_ON");
                    self.DisableKeyword("_ALPHABLEND_ON");
                    self.DisableKeyword("_ALPHAPREMULTIPLY_ON");
                    self.renderQueue = 2450;
                    break;

                case RenderingMode.Fade:
                    self.SetFloat("_Mode", 2);
                    self.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.SrcAlpha);
                    self.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.OneMinusSrcAlpha);
                    self.SetInt("_ZWrite", 0);
                    self.DisableKeyword("_ALPHATEST_ON");
                    self.EnableKeyword("_ALPHABLEND_ON");
                    self.DisableKeyword("_ALPHAPREMULTIPLY_ON");
                    self.renderQueue = 3000;
                    break;

                case RenderingMode.Transparent:
                    self.SetFloat("_Mode", 3);
                    self.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.One);
                    self.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.OneMinusSrcAlpha);
                    self.SetInt("_ZWrite", 0);
                    self.DisableKeyword("_ALPHATEST_ON");
                    self.DisableKeyword("_ALPHABLEND_ON");
                    self.EnableKeyword("_ALPHAPREMULTIPLY_ON");
                    self.renderQueue = 3000;
                    break;
            }
        }

        #endregion
    }
}