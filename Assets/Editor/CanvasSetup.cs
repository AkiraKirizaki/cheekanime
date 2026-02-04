using UnityEngine;
using UnityEditor;
using UnityEngine.UI;
using UnityEditor.SceneManagement;

public class CanvasSetup
{
    // メニューからも実行可能にする
    [MenuItem("Tools/Setup Canvas for Mobile")]
    public static void Setup()
    {
        // 編集対象のシーンを開く (現在のシーンが対象だが、念のためSampleSceneを指定)
        // ※実際のシーン名に合わせて調整してください
        string scenePath = "Assets/Scenes/SampleScene.unity";
        EditorSceneManager.OpenScene(scenePath);

        Canvas[] canvases = Object.FindObjectsByType<Canvas>(FindObjectsSortMode.None);
        bool changed = false;

        foreach (var canvas in canvases)
        {
            CanvasScaler scaler = canvas.GetComponent<CanvasScaler>();
            if (scaler == null)
            {
                scaler = canvas.gameObject.AddComponent<CanvasScaler>();
            }

            // Scale With Screen Size に設定
            if (scaler.uiScaleMode != CanvasScaler.ScaleMode.ScaleWithScreenSize)
            {
                scaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
                changed = true;
            }

            // 解像度設定 (スマホ標準的な 1080x1920 or 1920x1080)
            // ここでは縦横どちらでも対応しやすいよう、短辺を基準にするか、
            // あるいはMatchを変えることで対応します。
            Vector2 referenceRes = new Vector2(1920, 1080);
            if (scaler.referenceResolution != referenceRes)
            {
                scaler.referenceResolution = referenceRes;
                changed = true;
            }

            // 幅・高さのどちらに合わせるか (0=Width, 1=Height)
            // 0.5にするとバランスよく追従する
            if (scaler.matchWidthOrHeight != 0.5f)
            {
                scaler.matchWidthOrHeight = 0.5f;
                changed = true;
            }
            
            Debug.Log($"Setup CanvasScaler for {canvas.name}");
        }

        if (changed)
        {
            EditorSceneManager.SaveScene(EditorSceneManager.GetActiveScene());
            Debug.Log("Scene saved with CanvasScaler updates.");
        }
    }
}
