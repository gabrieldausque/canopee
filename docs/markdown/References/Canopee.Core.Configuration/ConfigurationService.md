# ConfigurationService class

Object in charge of managing the configuration : - read from local - write modification to local file - synchronize from a CanopeeServer that exposes the centralized configuration (Experimental feature)

```csharp
public class ConfigurationService
```

## Public Members

| name | description |
| --- | --- |
| [ConfigurationService](ConfigurationService/ConfigurationService.md)() | Default constructor |
| static [Instance](ConfigurationService/Instance.md) { get; } | Get the singleton instance |
| [AgentId](ConfigurationService/AgentId.md) { get; } | Get the agent id configured. If no agent id is configured, will set up a new one as a Guid |
| [Configuration](ConfigurationService/Configuration.md) { get; } | All configurations sections |
| event [OnNewConfiguration](ConfigurationService/OnNewConfiguration.md) | Raised if a new configuration is obtained through the synchronization process |
| [GetCanopeeConfiguration](ConfigurationService/GetCanopeeConfiguration.md)() | Get the whole Canopee Section |
| [GetConfigurationAsJsonObject](ConfigurationService/GetConfigurationAsJsonObject.md)() | Get the whole configuration file as a JsonObject. Used for write operation |
| [GetConfigurationServiceConfiguration](ConfigurationService/GetConfigurationServiceConfiguration.md)() | Get the Configuration section of the Canopee section |
| [GetLoggingConfiguration](ConfigurationService/GetLoggingConfiguration.md)() | Get the logging configuration section |
| [GetPipelinesConfiguration](ConfigurationService/GetPipelinesConfiguration.md)() | Get the pipelines configuration section |
| [IsUniqueInstance](ConfigurationService/IsUniqueInstance.md)() | Get the UniqueInstance parameter value. Default : true |
| [Start](ConfigurationService/Start.md)() | Start the synchronization of configuration if needed |

## See Also

* namespace [Canopee.Core.Configuration](../Canopee.Core.md)

<!-- DO NOT EDIT: generated by xmldocmd for Canopee.Core.dll -->