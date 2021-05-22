@echo off
.\packages\NSwag.MSBuild.13.11.1\tools\Win\nswag webapi2openapi /assembly:.\DotNetOwinWebApiSample.Api\bin\DotNetOwinWebApiSample.Api.dll /output:DotNetOwinWebApiSample.Api.json
