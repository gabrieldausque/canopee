dotnet publish --configuration Release --output ../bin/Agent/Published --self-contained --runtime linux-x64
cp -Rv ../bin/Agent/Release/linux-x64/Pipelines/ ../bin/Agent/Published/
cp -Rv ../bin/Agent/Release/linux-x64/Canopee*.xml ../bin/Agent/Published/
chmod 777 ../bin/Agent/Published/CanopeeAgent
chmod 777 ../bin/Agent/Published/LaunchAgentLinux.sh
