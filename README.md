# collider-visualizer

## Description

collider-visualizerは当たり判定の表示デバッグ機能です

## Demo

![実行結果](https://github.com/tomoriaki/collider-visualizer/blob/readme_images/Images/ss1.gif)

## Install

[release](https://github.com/tomoriaki/collider-visualizer/releases)からcollider-visualizer.unitypackageをダウンロードしてプロジェクトにインポートしてください

## Example

```csharp
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
```

![実行結果](https://github.com/tomoriaki/collider-visualizer/blob/readme_images/Images/ss1.gif)

## Author

Twitter: [@tomori_hikage](https://twitter.com/tomori_hikage)  
Qiita: [@tomori_hikage](https://qiita.com/tomori_hikage)

## Distribution License

[MIT](https://github.com/tomori-hikage/collider-visualizer/blob/master/LICENSE)

## Use License

[© Unity Technologies Japan/UCL](http://unity-chan.com/contents/license_jp/)
