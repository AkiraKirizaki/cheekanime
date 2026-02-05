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

        // GitHub Pages向けに圧縮設定を調整
        // CI環境での安定性のために圧縮を無効化
        PlayerSettings.WebGL.compressionFormat = WebGLCompressionFormat.Disabled;
        PlayerSettings.WebGL.decompressionFallback = false;

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
                    if (message.type == UnityEngine.LogType.Error || 
                        message.type == UnityEngine.LogType.Exception)
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
