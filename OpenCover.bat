@echo off
rem OpenCover.bat
rem コードカバレッジを測定して実行する

rem MSTEST の実行ファイル
set MSTEST=C:\Program Files\Microsoft Visual Studio\2022\Community\Common7\IDE\CommonExtensions\Microsoft\TestWindow\vstest.console.exe

rem OpenCover の実行ファイル
set OPENCOVER=.\packages\OpenCover.4.7.1221\tools\OpenCover.Console.exe

rem 実行するテストのアセンブリ名
set TARGET_TEST=DotNetOwinWebApiSample.Api.Test.dll /Logger:trx
rem set TEST_CATEGORY=/TestCaseFilter:TestCategory=Logic
set TEST_CATEGORY=
if not "%TEST_CATEGORY%"=="" set TARGET_TEST=%TARGET_TEST% %TEST_CATEGORY%

rem 実行するテストのアセンブリ名の追加
set TARGET_DIR=DotNetOwinWebApiSample.Api.Test\bin\Debug

rem OpenCover の出力ファイル
set OUTPUT=coverage.xml

rem カバレッジ対象の指定
rem set FILTERS=+[OpenCoverSample]*
rem set FILTERS=+[DotNetOwinWebApiSample.*]* -[*.Test.*]*
set FILTERS=+[DotNetOwinWebApiSample*]* -[Test*]*

rem OpenCover の実行
"%OPENCOVER%" -register:user -target:"%MSTEST%" -targetargs:"%TARGET_TEST%" -filter:"%FILTERS%" -output:"%OUTPUT%" -targetdir:"%TARGET_DIR%"

rem レポート生成
.\packages\ReportGenerator.5.1.10\tools\net47\ReportGenerator.exe -reports:%OUTPUT% -targetdir:report

echo.
echo カバレッジレポートが生成されました: report\index.html
pause
