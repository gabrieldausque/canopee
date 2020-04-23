dotnet publish --configuration Release --output ../bin/Server/Published --self-contained --runtime linux-x64
cp -Rv ../bin/Server/Release/linux-x64/Pipelines/ ../bin/Server/Published/
cp -Rv ../bin/Server/Release/linux-x64/Canopee*.xml ../bin/Server/Published/
chmod 777 ../bin/Server/Published/CanopeeServer