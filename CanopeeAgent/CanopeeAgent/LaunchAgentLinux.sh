echo "Set Running Environment"
read RunningEnvironment
export ASPNETCORE_ENVIRONMENT=$RunningEnvironment
export CANOPEE_ENVIRONMENT=$RunningEnvironment
./CanopeeAgent