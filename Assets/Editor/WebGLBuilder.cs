using System;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEngine;

public class WebGLBuilder
{
    public static void Build()
    {
        // ビルド出力先フォルダ
        string buildPath = "build/WebGL";

        // シーンリストの取得
        string[] scenes = EditorBuildSettings.scenes
            .Where(s => s.enabled)
            .Select(s => s.path)
            .ToArray();

        // ビルドオプションの設定
        BuildPlayerOptions buildPlayerOptions = new BuildPlayerOptions
        {
            scenes = scenes,
            locationPathName = buildPath,
            target = BuildTarget.WebGL,
            options = BuildOptions.None
        };

        // GitHub Pages向けに圧縮設定を調整 (任意)
        // PlayerSettings.WebGL.compressionFormat = WebGLCompressionFormat.Brotli; // デフォルト
        // サーバー設定ができない場合は Disabled にする必要がある場合がありますが、
        // 近年のGitHub Pagesはgzip/brotliに対応していることが多いです。
        // トラブルシュート用にDecompression Fallbackを有効にしておくと安全です。
        PlayerSettings.WebGL.decompressionFallback = true;

        // ビルド実行
        UnityEditor.Build.Reporting.BuildReport report = BuildPipeline.BuildPlayer(buildPlayerOptions);

        // 結果の確認
        if (report.summary.result == UnityEditor.Build.Reporting.BuildResult.Succeeded)
        {
            Debug.Log($"Build succeeded: {report.summary.totalSize} bytes");
        }
        else
        {
            Debug.LogError($"Build failed: {report.summary.result}");
            if (Application.isBatchMode)
            {
                EditorApplication.Exit(1);
            }
        }
    }
}
