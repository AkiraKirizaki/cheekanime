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



        Console.WriteLine(":: Starting WebGL Build ::");

        // シーンリストの取得
        string[] scenes = EditorBuildSettings.scenes
            .Where(s => s.enabled)
            .Select(s => s.path)
            .ToArray();

        Console.WriteLine($":: Scenes to build: {scenes.Length} ::");
        foreach (var scene in scenes)
        {
            Console.WriteLine($" - {scene}");
        }

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
            
            // 詳細なエラー情報を出力
            Debug.LogError($"Total errors: {report.summary.totalErrors}");
            Debug.LogError($"Total warnings: {report.summary.totalWarnings}");
            
            // ビルドステップの詳細を出力
            foreach (var step in report.steps)
            {
                Debug.Log($"Build step: {step.name} - Duration: {step.duration}");
                foreach (var message in step.messages)
                {
                    if (message.type == LogType.Error || 
                        message.type == LogType.Exception)
                    {
                        Debug.LogError($"  Error: {message.content}");
                    }
                }
            }
            
            if (Application.isBatchMode)
            {
                EditorApplication.Exit(1);
            }
        }
    }
}
