dotnet publish --configuration Release --output ../bin/Agent/Published --self-contained --runtime linux-x64
cp -Rv ../bin/Agent/Release/Pipelines/ ../bin/Agent/Published/
cp -Rv ../bin/Agent/Release/Canopee*.xml ../bin/Agent/Published/
chmod 777 ../bin/Agent/Published/CanopeeAgent