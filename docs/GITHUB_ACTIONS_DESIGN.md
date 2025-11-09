# GitHub Actions ワークフロー設計指針

## 1. 概要

本ドキュメントは、`.NET Framework 4.8` プロジェクト（OWIN Web API）を GitHub Actions でビルド、テスト実行、カバレッジ取得するための設計指針を提供します。

## 2. プロジェクト構成の確認

### 2.1 プロジェクト構造
- **メインプロジェクト**: `DotNetOwinWebApiSample.Api` (.NET Framework 4.8)
- **テストプロジェクト1**: `DotNetOwinWebApiSample.Api.Test` (MSTest)
- **テストプロジェクト2**: `DotNetOwinWebApiSample.Api.NUnit.Test` (NUnit)
- **ソリューション**: `DotNetOwinWebApiSample.sln`

### 2.2 既存のツール
- **カバレッジツール**: OpenCover
- **レポート生成**: ReportGenerator
- **ビルドツール**: MSBuild
- **テストランナー**: VSTest.Console.exe / dotnet test

## 3. ワークフロー設計方針

### 3.1 基本方針

1. **Windows ランナーの使用**
   - .NET Framework 4.8 は Windows 環境でビルド・実行が必要
   - `windows-latest` または `windows-2022` を使用

2. **既存ツールとの互換性**
   - 既存の `OpenCover.bat` と `azure-pipelines.yml` のアプローチを参考
   - OpenCover と ReportGenerator を使用したカバレッジ取得

3. **テストフレームワークの対応**
   - MSTest プロジェクト: VSTest.Console.exe または dotnet test
   - NUnit プロジェクト: dotnet test（NUnit3TestAdapter を使用）

4. **カバレッジレポートの公開**
   - HTML レポートを artifact としてアップロード
   - カバレッジ結果を GitHub Actions の Summary に表示
   - オプション: Codecov などのサービスにアップロード

### 3.2 ワークフローの構成

#### 3.2.1 トリガー

```yaml
on:
  push:
    branches: [ main, develop ]
  pull_request:
    branches: [ main, develop ]
  workflow_dispatch:  # 手動実行も可能にする
```

#### 3.2.2 ジョブ構成

1. **ビルドジョブ**
   - ソリューションのビルド
   - NuGet パッケージの復元

2. **テストジョブ**（ビルドジョブに依存）
   - テストの実行
   - カバレッジの取得
   - カバレッジレポートの生成

3. **カバレッジレポート公開ジョブ**（テストジョブに依存）
   - レポートの artifact アップロード
   - カバレッジ結果のサマリー表示

## 4. 実装詳細

### 4.1 環境設定

```yaml
env:
  SOLUTION_FILE: DotNetOwinWebApiSample.sln
  TEST_PROJECT_MSTEST: DotNetOwinWebApiSample.Api.Test/DotNetOwinWebApiSample.Api.Test.csproj
  TEST_PROJECT_NUNIT: DotNetOwinWebApiSample.Api.NUnit.Test/DotNetOwinWebApiSample.Api.NUnit.Test.csproj
  COVERAGE_FILTER: "+[DotNetOwinWebApiSample*]* -[*.Test.*]*"
  COVERAGE_OUTPUT: coverage.xml
  COVERAGE_REPORT_DIR: coverage-report
```

### 4.2 ビルドステップ

1. **NuGet パッケージの復元**
   ```yaml
   - name: Restore NuGet packages
     run: nuget restore ${{ env.SOLUTION_FILE }}
   ```

2. **ソリューションのビルド**
   ```yaml
   - name: Build solution
     run: msbuild ${{ env.SOLUTION_FILE }} /p:Configuration=Release /p:Platform="Any CPU" /t:Build
   ```

### 4.3 テストステップ

#### オプション1: VSTest.Console.exe を使用（.NET Framework 向け）

```yaml
- name: Run tests with VSTest
  run: |
    $testAssembly = "DotNetOwinWebApiSample.Api.Test\bin\Release\DotNetOwinWebApiSample.Api.Test.dll"
    $vstest = "C:\Program Files\Microsoft Visual Studio\2022\Enterprise\Common7\IDE\CommonExtensions\Microsoft\TestWindow\vstest.console.exe"
    & $vstest $testAssembly /Logger:trx /Logger:console
```

#### オプション2: dotnet test を使用（推奨）

```yaml
- name: Run tests with dotnet test
  run: |
    dotnet test ${{ env.TEST_PROJECT_MSTEST }} --configuration Release --logger trx --logger "console;verbosity=normal"
    dotnet test ${{ env.TEST_PROJECT_NUNIT }} --configuration Release --logger trx --logger "console;verbosity=normal"
```

### 4.4 カバレッジ取得ステップ

#### OpenCover のインストール

```yaml
- name: Install OpenCover
  run: |
    nuget install OpenCover -Version 4.7.1221 -OutputDirectory packages
```

#### カバレッジの実行

```yaml
- name: Run code coverage with OpenCover
  run: |
    $openCover = ".\packages\OpenCover.4.7.1221\tools\OpenCover.Console.exe"
    $testAssembly = "DotNetOwinWebApiSample.Api.Test\bin\Release\DotNetOwinWebApiSample.Api.Test.dll"
    $vstest = "C:\Program Files\Microsoft Visual Studio\2022\Enterprise\Common7\IDE\CommonExtensions\Microsoft\TestWindow\vstest.console.exe"
    
    & $openCover `
      -register:user `
      -target:"$vstest" `
      -targetargs:"`"$testAssembly`" /Logger:trx" `
      -filter:"${{ env.COVERAGE_FILTER }}" `
      -output:"${{ env.COVERAGE_OUTPUT }}" `
      -targetdir:"DotNetOwinWebApiSample.Api.Test\bin\Release"
```

### 4.5 レポート生成ステップ

#### ReportGenerator のインストール

```yaml
- name: Install ReportGenerator
  run: |
    nuget install ReportGenerator -Version 5.1.10 -OutputDirectory packages
```

#### レポートの生成

```yaml
- name: Generate coverage report
  run: |
    $reportGenerator = ".\packages\ReportGenerator.5.1.10\tools\net6.0\ReportGenerator.exe"
    & $reportGenerator `
      -reports:"${{ env.COVERAGE_OUTPUT }}" `
      -targetdir:"${{ env.COVERAGE_REPORT_DIR }}" `
      -reporttypes:"Html;Cobertura"
```

### 4.6 カバレッジ結果の公開

#### Artifact としてアップロード

```yaml
- name: Upload coverage report
  uses: actions/upload-artifact@v3
  with:
    name: coverage-report
    path: ${{ env.COVERAGE_REPORT_DIR }}
    retention-days: 30
```

#### GitHub Actions Summary に表示

```yaml
- name: Add coverage to summary
  run: |
    $coverageXml = [xml](Get-Content "${{ env.COVERAGE_OUTPUT }}")
    $sequenceCoverage = $coverageXml.CoverageSession.Summary.sequenceCoverage
    $branchCoverage = $coverageXml.CoverageSession.Summary.branchCoverage
    
    Add-Content -Path $env:GITHUB_STEP_SUMMARY -Value "## Code Coverage Results"
    Add-Content -Path $env:GITHUB_STEP_SUMMARY -Value "| Metric | Coverage |"
    Add-Content -Path $env:GITHUB_STEP_SUMMARY -Value "|--------|----------|"
    Add-Content -Path $env:GITHUB_STEP_SUMMARY -Value "| Sequence Coverage | $sequenceCoverage% |"
    Add-Content -Path $env:GITHUB_STEP_SUMMARY -Value "| Branch Coverage | $branchCoverage% |"
```

#### テスト結果の公開

```yaml
- name: Publish test results
  uses: EnricoMi/publish-unit-test-result-action@v2
  if: always()
  with:
    files: |
      **/*.trx
    check_name: Test Results
```

## 5. ワークフロー全体の構造

### 5.1 単一ジョブ構成（推奨開始点）

- ビルド、テスト、カバレッジを1つのジョブで実行
- シンプルで理解しやすい
- 実行時間が短い

### 5.2 マトリックス構成（将来的な拡張）

- 複数の .NET Framework バージョンでテスト
- 複数の構成（Debug/Release）でテスト
- 並列実行による高速化

## 6. 考慮事項

### 6.1 パフォーマンス

1. **キャッシュの活用**
   - NuGet パッケージのキャッシュ
   - ビルド成果物のキャッシュ（必要に応じて）

2. **並列実行**
   - 複数のテストプロジェクトを並列実行
   - マトリックス戦略の採用

### 6.2 エラーハンドリング

1. **テスト失敗時の処理**
   - テストが失敗してもカバレッジレポートを生成
   - `if: always()` を使用してレポートをアップロード

2. **ビルド失敗時の処理**
   - 早期に失敗を検出
   - エラーメッセージを明確に表示

### 6.3 セキュリティ

1. **シークレット管理**
   - 必要なシークレットは GitHub Secrets で管理
   - Codecov などのトークンは環境変数で設定

2. **依存関係の検証**
   - Dependabot の有効化
   - セキュリティ脆弱性のスキャン

## 7. 実装の優先順位

### フェーズ1: 基本実装（必須）

1. ✅ ビルドジョブの実装
2. ✅ テスト実行の実装
3. ✅ カバレッジ取得の実装
4. ✅ レポート生成の実装
5. ✅ Artifact アップロードの実装

### フェーズ2: 拡張機能（推奨）

1. ⬜ GitHub Actions Summary への表示
2. ⬜ テスト結果の公開
3. ⬜ カバレッジバッジの追加
4. ⬜ カバレッジ閾値の設定（カバレッジが低下した場合の警告）

### フェーズ3: 最適化（任意）

1. ⬜ キャッシュの実装
2. ⬜ 並列実行の実装
3. ⬜ マトリックス戦略の実装
4. ⬜ Codecov などの外部サービスとの統合

## 8. 参考資料

- [GitHub Actions ドキュメント](https://docs.github.com/ja/actions)
- [OpenCover ドキュメント](https://github.com/OpenCover/opencover)
- [ReportGenerator ドキュメント](https://github.com/danielpalme/ReportGenerator)
- [.NET Framework ビルドガイド](https://docs.microsoft.com/ja-jp/dotnet/framework/)

## 9. 次のステップ

1. **ワークフローファイルの作成**
   - `.github/workflows/ci.yml` を作成
   - フェーズ1の機能を実装

2. **テスト実行**
   - プッシュしてワークフローを実行
   - エラーがあれば修正

3. **カバレッジレポートの確認**
   - 生成されたレポートを確認
   - カバレッジ率を確認

4. **拡張機能の追加**
   - フェーズ2、フェーズ3の機能を段階的に追加

