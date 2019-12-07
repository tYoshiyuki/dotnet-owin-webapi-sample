@echo off
rem OpenCover.bat
rem ソリューションルートで実行すること

rem MSTEST のインストール先
set MSTEST="C:\Program Files (x86)\Microsoft Visual Studio\2019\Community\Common7\IDE\CommonExtensions\Microsoft\TestWindow\vstest.console.exe"

rem OpenCover のインストール先
set OPENCOVER=".\packages\OpenCover.4.7.922\tools\OpenCover.Console.exe"

rem 実行するテストのアセンブリ
set TARGET_TEST="DotNetOwinWebApiSample.Api.Test.dll /Logger:trx"
rem set TEST_CATEGORY="/TestCaseFilter:TestCategory=Logic"
set TEST_CATEGORY=""
if not TEST_CATEGORY=="" set TEST_CATEGORY="/TestCaseFilter:TestCategory=%TARGET_TEST_CATEGORY%"


rem e2eのテストだけ実施する場合
rem set TARGET_TEST="DotNetOwinWebApiSample.Api.Test.dll /Logger:trx;LogFileName=DotNetOwinWebApiSample.Api.Test.trx"

rem 実行するテストのアセンブリの格納先
set TARGET_DIR=".\DotNetOwinWebApiSample.Api.Test\bin\Debug"

rem OpenCover の出力ファイル
set OUTPUT="coverage.xml"

rem カバレッジ計測対象の指定
rem set FILTERS="+[OpenCoverSample]*"
rem set FILTERS="+[DotNetOwinWebApiSample.*]* -[*.Test.*]*"
set FILTERS="+[DotNetOwinWebApiSample*]* -[Test*]*"

rem OpenCover の実行
%OPENCOVER% -register:user -target:%MSTEST% -targetargs:%TARGET_TEST% -targetargs:%TARGET_TEST_CATEGORY% -targetdir:%TARGET_DIR% -filter:%FILTERS% -output:%OUTPUT%

.\packages\ReportGenerator.4.3.6\tools\net47\ReportGenerator.exe -reports:%OUTPUT% -targetdir:.\report
pause
