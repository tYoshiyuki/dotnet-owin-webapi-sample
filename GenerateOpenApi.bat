@echo off
.\packages\NSwag.MSBuild.13.11.1\tools\Win\nswag webapi2openapi /assembly:.\DotNetOwinWebApiSample.Api\bin\DotNetOwinWebApiSample.Api.dll /output:DotNetOwinWebApiSample.Api.json

rem nswag.json を用いて出力する場合は、以下のコマンドを実施します。
rem npx nswag run ./nswag.json /runtime:WinX64