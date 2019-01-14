[![license](https://img.shields.io/github/license/tomori-hikage/collider-visualizer.svg?style=flat-square)](https://github.com/tomori-hikage/collider-visualizer/blob/master/LICENSE)
[![release](https://img.shields.io/github/release/tomori-hikage/collider-visualizer.svg?style=flat-square)](https://github.com/tomori-hikage/collider-visualizer/releases)
[![GitHub](https://img.shields.io/github/followers/tomori-hikage.svg?label=@tomori-hikage&style=social)](https://github.com/tomori-hikage)
[![Twitter](https://img.shields.io/twitter/follow/tomori_hikage.svg?label=@tomori_hikage&style=social)](https://twitter.com/tomori_hikage)

# collider-visualizer

## Description

collider-visualizerは当たり判定の表示デバッグ機能です

## Demo

![実行結果](https://github.com/tomoriaki/collider-visualizer/blob/readme_images/Images/ss1.gif)

## Install

[releases](https://github.com/tomoriaki/collider-visualizer/releases)からcollider-visualizer.unitypackageをダウンロードしてプロジェクトにインポートしてください

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

## Use License

[© Unity Technologies Japan/UCL](http://unity-chan.com/contents/license_jp/)
