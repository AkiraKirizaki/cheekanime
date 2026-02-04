# Unity WebGL Deployment Guide

このプロジェクトはGitHub Actionsを使用して、Unity WebGLビルドを自動的にGitHub Pagesへデプロイするように設定されています。

## 初期設定 (Secretsの登録)

自動ビルドを実行するには、GitHubリポジトリにUnityのライセンス情報を登録する必要があります。

1. **GitHubリポジトリ** > **Settings** > **Secrets and variables** > **Actions** に移動します。
2. **New repository secret** をクリックし、以下の3つを追加します。

| Secret Name | Value | 説明 |
| :--- | :--- | :--- |
| `UNITY_LICENSE` | `.ulf` ファイルの内容 | Unity Personalなどのライセンスファイル (`Unity_lic.ulf`) の中身全て。 |
| `UNITY_EMAIL` | メールアドレス | Unityアカウントのメールアドレス。 |
| `UNITY_PASSWORD` | パスワード | Unityアカウントのパスワード。 |

### ライセンスファイルの取得方法 (Windows)
`C:\ProgramData\Unity\Unity_lic.ulf` をテキストエディタで開き、内容をコピーします。
※ファイルが見つからない場合は、[GameCI Activation Guide](https://game-ci.com/docs/github/activation) を参照してください。

## デプロイの流れ

1. `main` ブランチにコードをプッシュします。
2. GitHub Actionsの **Deploy to GitHub Pages** ワークフローが自動的に開始されます。
   - **Actions** タブで進行状況を確認できます。
3. ビルドが成功すると、成果物が `gh-pages` ブランチにプッシュされます。
4. GitHub Pagesの設定で、Sourceを `gh-pages` ブランチに指定すると（通常は自動設定されます）、公開URLでゲームが遊べるようになります。

## 注意事項

- **ビルド時間**: 初回のビルドには数分〜十数分かかる場合があります（Libraryのキャッシュがないため）。
- **圧縮設定**: `WebGLBuilder.cs` で `decompressionFallback = true` に設定してあるため、特別なサーバー設定なしでも動作するはずです。

## トラブルシューティング

**ビルドが失敗する場合:**
- Secretsの内容（特に `UNITY_LICENSE`）が正しいか確認してください。
- Actionsのログを見て、エラーメッセージを確認してください。

**ページが表示されない場合:**
- リポジトリの **Settings** > **Pages** で、Build and deploymentのSourceが **Deploy from a branch** になっているか、Branchが **gh-pages** になっているか確認してください。
