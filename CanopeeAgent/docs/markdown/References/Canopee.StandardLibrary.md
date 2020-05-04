# Canopee.StandardLibrary assembly

## Canopee.StandardLibrary.Configuration namespace

| public type | description |
| --- | --- |
| class [CanopeeServerConfigurationSynchronizer](Canopee.StandardLibrary.Configuration/CanopeeServerConfigurationSynchronizer.md) | The object in charge of checking and notifying if a new configuration is available from an ASPNet CanopeeServer. It will : - get the local configuration - get default configuration from distant source - get the associated group configurations for agent Id sorted by ascending priority, merged with the default - get the agent id specific configuration and merged with the previous one - compare the local configuration to the merged configurations If the new configuration is different from the local one, it raised an event with the new configuration the behavior of the configuration is set through the configuration : |

## Canopee.StandardLibrary.Configuration.AspNet namespace

| public type | description |
| --- | --- |
| class [AspNetCanopeeServerConfigurationReader](Canopee.StandardLibrary.Configuration.AspNet/AspNetCanopeeServerConfigurationReader.md) | The client of the CanopeeServer instance that will give access to : - the list of group for a specified agent id - the configuration for a specified agent id and group Example of the configuration : /// the behavior of the configuration is set through the configuration : |

## Canopee.StandardLibrary.Inputs.Batch namespace

| public type | description |
| --- | --- |
| class [BatchInput](Canopee.StandardLibrary.Inputs.Batch/BatchInput.md) | This IInput collect each line of the output of a batch command. By default it manage : - dosshell command for windows - bash command for linux You can override the shell executor by configuration :  For a powershell command you will need this configuration for input in a pipeline : |

## Canopee.StandardLibrary.Inputs.Databases.Firebird namespace

| public type | description |
| --- | --- |
| class [FirebirdADOInput](Canopee.StandardLibrary.Inputs.Databases.Firebird/FirebirdADOInput.md) | Create a collected event for each record returned by the select statement Configuration example : |

## Canopee.StandardLibrary.Inputs.File namespace

| public type | description |
| --- | --- |
| class [CSVInput](Canopee.StandardLibrary.Inputs.File/CSVInput.md) | Work In Progress. Collect all line of a CSV file and get them. Configuration example : |
| class [RawFileLineInfo](Canopee.StandardLibrary.Inputs.File/RawFileLineInfo.md) | A ICollectedEvent that represents a line of a file |

## Canopee.StandardLibrary.Inputs.Hardware namespace

| public type | description |
| --- | --- |
| abstract class [BaseHardwareInfosInput](Canopee.StandardLibrary.Inputs.Hardware/BaseHardwareInfosInput.md) | This is the base class for hardware collect informations. As the collect is specific for each supported OS, this class needs to be implemented for each OS. The current library contains two implementations : - windows [`WindowsHardwareInfosInput`](Canopee.StandardLibrary.Inputs.Hardware/WindowsHardwareInfosInput.md) - linux [`LinuxHardwareInfosInput`](Canopee.StandardLibrary.Inputs.Hardware/LinuxHardwareInfosInput.md) This base class is responsible for the global sequence which is the same for each OS : - collect Cpu - collect memory - collect disks - collect display info (graphical cards and display) - collect usb peripherals Configuration for this input will be : |
| class [DiskInfos](Canopee.StandardLibrary.Inputs.Hardware/DiskInfos.md) | Represents all information for a physical disk |
| class [DisplayInfos](Canopee.StandardLibrary.Inputs.Hardware/DisplayInfos.md) | This ICollectedEvent represents a Display |
| class [GraphicalCardInfos](Canopee.StandardLibrary.Inputs.Hardware/GraphicalCardInfos.md) |  |
| class [HardwareInfos](Canopee.StandardLibrary.Inputs.Hardware/HardwareInfos.md) | Represents all hardware infos of the current workstation/server |
| class [LinuxHardwareInfosInput](Canopee.StandardLibrary.Inputs.Hardware/LinuxHardwareInfosInput.md) | This class is the [`BaseHardwareInfosInput`](Canopee.StandardLibrary.Inputs.Hardware/BaseHardwareInfosInput.md) for all LINUX OS |
| class [UsbPeripheralInfos](Canopee.StandardLibrary.Inputs.Hardware/UsbPeripheralInfos.md) | Represent a Usb peripheral |
| class [WindowsHardwareInfosInput](Canopee.StandardLibrary.Inputs.Hardware/WindowsHardwareInfosInput.md) | This class is the [`BaseHardwareInfosInput`](Canopee.StandardLibrary.Inputs.Hardware/BaseHardwareInfosInput.md) for all Windows OS |

## Canopee.StandardLibrary.Inputs.Network namespace

| public type | description |
| --- | --- |
| class [LinuxNetworkInput](Canopee.StandardLibrary.Inputs.Network/LinuxNetworkInput.md) | Collect all [`NetworkInfo`](Canopee.StandardLibrary.Inputs.Network/NetworkInfo.md) of the current workstation/server for LINUX OS |
| class [NetworkInfo](Canopee.StandardLibrary.Inputs.Network/NetworkInfo.md) | Represent a Network card |
| class [WindowsNetworkInput](Canopee.StandardLibrary.Inputs.Network/WindowsNetworkInput.md) | Collect all [`NetworkInfo`](Canopee.StandardLibrary.Inputs.Network/NetworkInfo.md) of the current workstation/server for Windows OS |

## Canopee.StandardLibrary.Inputs.OperatingSystem namespace

| public type | description |
| --- | --- |
| class [LinuxOperatingSystemInput](Canopee.StandardLibrary.Inputs.OperatingSystem/LinuxOperatingSystemInput.md) | collect one [`OperatingSystemInfo`](Canopee.StandardLibrary.Inputs.OperatingSystem/OperatingSystemInfo.md) for LINUX OS. Configuration will be : |
| class [OperatingSystemInfo](Canopee.StandardLibrary.Inputs.OperatingSystem/OperatingSystemInfo.md) | Represent the current OS |
| class [WindowsOperatingSystemInput](Canopee.StandardLibrary.Inputs.OperatingSystem/WindowsOperatingSystemInput.md) | collect one [`OperatingSystemInfo`](Canopee.StandardLibrary.Inputs.OperatingSystem/OperatingSystemInfo.md) for Windows OS. Configuration will be : |

## Canopee.StandardLibrary.Inputs.Trigger namespace

| public type | description |
| --- | --- |
| class [FromTriggerInput](Canopee.StandardLibrary.Inputs.Trigger/FromTriggerInput.md) | This IInput is used to take the ICollectedEvent from the Raw send by the ITrigger that raise the executing CollectPipeline /// Configuration will be |

## Canopee.StandardLibrary.Loggers namespace

| public type | description |
| --- | --- |
| class [ConsoleCanopeeLogger](Canopee.StandardLibrary.Loggers/ConsoleCanopeeLogger.md) | This is the default logger. Log all message to the console with a color code based on the LogLevel. Configuration will be : |
| class [Log4NetCanopeeLogger](Canopee.StandardLibrary.Loggers/Log4NetCanopeeLogger.md) | A Log4net logger wrapper, by default will use the log4net.config file that will exists in the working directory. The configuration will be : |
| class [MultiCanopeeLogger](Canopee.StandardLibrary.Loggers/MultiCanopeeLogger.md) | This logger allow you to have multiple logger define for the current host Configuration will be : |

## Canopee.StandardLibrary.Outputs namespace

| public type | description |
| --- | --- |
| class [CanopeeOutput](Canopee.StandardLibrary.Outputs/CanopeeOutput.md) |  |
| class [ConsoleOutput](Canopee.StandardLibrary.Outputs/ConsoleOutput.md) |  |
| class [ElasticOutput](Canopee.StandardLibrary.Outputs/ElasticOutput.md) |  |

## Canopee.StandardLibrary.Transforms namespace

| public type | description |
| --- | --- |
| class [APIGeoGouvTransform](Canopee.StandardLibrary.Transforms/APIGeoGouvTransform.md) |  |
| abstract class [BatchTransform](Canopee.StandardLibrary.Transforms/BatchTransform.md) |  |
| class [ElasticLookupTransform](Canopee.StandardLibrary.Transforms/ElasticLookupTransform.md) |  |
| class [LinuxOperatingSystemTransform](Canopee.StandardLibrary.Transforms/LinuxOperatingSystemTransform.md) |  |
| class [NoTransform](Canopee.StandardLibrary.Transforms/NoTransform.md) |  |
| class [TransformFieldMapping](Canopee.StandardLibrary.Transforms/TransformFieldMapping.md) |  |
| class [WindowsOperatingSystemTransform](Canopee.StandardLibrary.Transforms/WindowsOperatingSystemTransform.md) |  |

## Canopee.StandardLibrary.Transforms.Databases.Firebird namespace

| public type | description |
| --- | --- |
| class [FirebirdInsertFieldsTransform](Canopee.StandardLibrary.Transforms.Databases.Firebird/FirebirdInsertFieldsTransform.md) |  |
| class [FirebirdLookupTransform](Canopee.StandardLibrary.Transforms.Databases.Firebird/FirebirdLookupTransform.md) |  |

## Canopee.StandardLibrary.Transforms.Hardware namespace

| public type | description |
| --- | --- |
| abstract class [BaseHardwareTransform](Canopee.StandardLibrary.Transforms.Hardware/BaseHardwareTransform.md) |  |
| class [HardwareTransform](Canopee.StandardLibrary.Transforms.Hardware/HardwareTransform.md) |  |
| class [WindowsHardwareTransform](Canopee.StandardLibrary.Transforms.Hardware/WindowsHardwareTransform.md) |  |

## Canopee.StandardLibrary.Triggers namespace

| public type | description |
| --- | --- |
| class [OnceTrigger](Canopee.StandardLibrary.Triggers/OnceTrigger.md) |  |

## Canopee.StandardLibrary.Triggers.Cron namespace

| public type | description |
| --- | --- |
| class [CronTrigger](Canopee.StandardLibrary.Triggers.Cron/CronTrigger.md) |  |
| class [CronTriggerEventArgs](Canopee.StandardLibrary.Triggers.Cron/CronTriggerEventArgs.md) |  |

## Canopee.StandardLibrary.Triggers.Hub namespace

| public type | description |
| --- | --- |
| class [HubTrigger](Canopee.StandardLibrary.Triggers.Hub/HubTrigger.md) |  |

<!-- DO NOT EDIT: generated by xmldocmd for Canopee.StandardLibrary.dll -->
