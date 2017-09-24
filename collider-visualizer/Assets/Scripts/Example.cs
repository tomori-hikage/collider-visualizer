using UnityEngine;
using HC.Debug;


/// <summary>
/// 使用例
/// </summary>
[DisallowMultipleComponent]
public class Example : MonoBehaviour
{
    #region フィールド / プロパティ

    [SerializeField, Tooltip("可視コライダーの色")]
    private ColliderVisualizer.VisualizerColorType _visualizerColor;

    [SerializeField, Tooltip("メッセージ")]
    private string _message;

    [SerializeField, Tooltip("フォントサイズ")]
    private int _fontSize = 36;

    [SerializeField, Tooltip("左脚")]
    private GameObject _leftLeg;

    #endregion


    #region アニメーションイベントメソッド

    private void AttackStart()
    {
        _leftLeg.AddComponent<ColliderVisualizer>().Initialize(_visualizerColor, _message, _fontSize);
    }

    private void AttackEnd()
    {
        Destroy(_leftLeg.GetComponent<ColliderVisualizer>());
    }

    #endregion
}