@echo off
Echo Set Running Environment
SET /P ASPNETCORE_ENVIRONMENT=
SET CANOPEE_ENVIRONMENT=%ASPNETCORE_ENVIRONMENT%

CanopeeAgent