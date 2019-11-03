rem OpenCover.bat
rem ソリューションルートで実行すること

rem MSTEST のインストール先
set MSTEST="C:\Program Files (x86)\Microsoft Visual Studio\2019\Community\Common7\IDE\CommonExtensions\Microsoft\TestWindow\vstest.console.exe"

rem OpenCover のインストール先
set OPENCOVER=".\packages\OpenCover.4.7.922\tools\OpenCover.Console.exe"

rem 実行するテストのアセンブリ
set TARGET_TEST="DoteNetOwinWebApiSample.Api.Test.dll"

rem 実行するテストのアセンブリの格納先
set TARGET_DIR=".\DoteNetOwinWebApiSample.Api.Test\bin\Debug"

rem OpenCover の出力ファイル
set OUTPUT="coverage.xml"

rem カバレッジ計測対象の指定
rem set FILTERS="+[OpenCoverSample]*"
set FILTERS="+[*]*"

rem OpenCover の実行
%OPENCOVER% -register:user -target:%MSTEST% -targetargs:%TARGET_TEST% -targetdir:%TARGET_DIR% -filter:%FILTERS% -output:%OUTPUT%