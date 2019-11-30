@echo off
rem OpenCover.bat
rem �\�����[�V�������[�g�Ŏ��s���邱��

rem MSTEST �̃C���X�g�[����
set MSTEST="C:\Program Files (x86)\Microsoft Visual Studio\2019\Community\Common7\IDE\CommonExtensions\Microsoft\TestWindow\vstest.console.exe"

rem OpenCover �̃C���X�g�[����
set OPENCOVER=".\packages\OpenCover.4.7.922\tools\OpenCover.Console.exe"

rem ���s����e�X�g�̃A�Z���u��
rem set TARGET_TEST="DoteNetOwinWebApiSample.Api.Test.dll /Logger:trx /TestCaseFilter:"TestCategory=e2e"" e2e�̃e�X�g�������{����ꍇ
set TARGET_TEST="DoteNetOwinWebApiSample.Api.Test.dll /Logger:trx;LogFileName=Result.trx"

rem ���s����e�X�g�̃A�Z���u���̊i�[��
set TARGET_DIR=".\DoteNetOwinWebApiSample.Api.Test\bin\Debug"

rem OpenCover �̏o�̓t�@�C��
set OUTPUT="coverage.xml"

rem �J�o���b�W�v���Ώۂ̎w��
rem set FILTERS="+[OpenCoverSample]*"
set FILTERS="+[DoteNetOwinWebApiSample*]* -[*.Test.*]*"

rem OpenCover �̎��s
%OPENCOVER% -register:user -target:%MSTEST% -targetargs:%TARGET_TEST% -targetdir:%TARGET_DIR% -filter:%FILTERS% -output:%OUTPUT%
pause
