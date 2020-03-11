dotnet publish --configuration Release --output ../bin/Agent/Published --self-contained --runtime win-x64
xcopy /S /E /I /Y ..\bin\Agent\Release\Pipelines ..\bin\Agent\Published\Pipelines
xcopy /S /E /I /Y ..\bin\Agent\Release\Canopee*.xml ..\bin\Agent\Published