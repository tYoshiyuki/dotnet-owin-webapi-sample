@echo off
rem OpenCover.bat
rem �\�����[�V�������[�g�Ŏ��s���邱��

rem MSTEST �̃C���X�g�[����
set MSTEST="C:\Program Files (x86)\Microsoft Visual Studio\2019\Community\Common7\IDE\CommonExtensions\Microsoft\TestWindow\vstest.console.exe"

rem OpenCover �̃C���X�g�[����
set OPENCOVER=".\packages\OpenCover.4.7.922\tools\OpenCover.Console.exe"

rem ���s����e�X�g�̃A�Z���u��
set TARGET_TEST="DotNetOwinWebApiSample.Api.Test.dll /Logger:trx"
rem set TEST_CATEGORY="/TestCaseFilter:TestCategory=Logic"
set TEST_CATEGORY=""
if not TEST_CATEGORY=="" set TEST_CATEGORY="/TestCaseFilter:TestCategory=%TARGET_TEST_CATEGORY%"


rem e2e�̃e�X�g�������{����ꍇ
rem set TARGET_TEST="DotNetOwinWebApiSample.Api.Test.dll /Logger:trx;LogFileName=DotNetOwinWebApiSample.Api.Test.trx"

rem ���s����e�X�g�̃A�Z���u���̊i�[��
set TARGET_DIR=".\DotNetOwinWebApiSample.Api.Test\bin\Debug"

rem OpenCover �̏o�̓t�@�C��
set OUTPUT="coverage.xml"

rem �J�o���b�W�v���Ώۂ̎w��
rem set FILTERS="+[OpenCoverSample]*"
rem set FILTERS="+[DotNetOwinWebApiSample.*]* -[*.Test.*]*"
set FILTERS="+[DotNetOwinWebApiSample*]* -[Test*]*"

rem OpenCover �̎��s
%OPENCOVER% -register:user -target:%MSTEST% -targetargs:%TARGET_TEST% -targetargs:%TARGET_TEST_CATEGORY% -targetdir:%TARGET_DIR% -filter:%FILTERS% -output:%OUTPUT%

.\packages\ReportGenerator.4.3.6\tools\net47\ReportGenerator.exe -reports:%OUTPUT% -targetdir:.\report
pause
