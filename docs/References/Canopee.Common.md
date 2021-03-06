# Canopee.Common assembly

## Canopee.Common namespace

| public type | description |
| --- | --- |
| class [JsonObject](Canopee.Common/JsonObject.md) | This class represent a json structure as an object, and allow some features as load from a text file, a JsonDocument, and write a json to a file. This class was created to manage issues on managing configuration file easily and dynamically, but it is also useful for exchange between purpose. |

## Canopee.Common.Configuration namespace

| public type | description |
| --- | --- |
| interface [ICanopeeConfigurationReader](Canopee.Common.Configuration/ICanopeeConfigurationReader.md) | The contract for an object to obtain groups or configurations |
| interface [ICanopeeConfigurationSynchronizer](Canopee.Common.Configuration/ICanopeeConfigurationSynchronizer.md) | Contract for the object that will synchronize configuration from an external source. |

## Canopee.Common.Configuration.AspNet.Dtos namespace

| public type | description |
| --- | --- |
| class [AgentGroupDto](Canopee.Common.Configuration.AspNet.Dtos/AgentGroupDto.md) | Indicate a group that a specific agent belong to |
| class [CanopeeConfigurationDto](Canopee.Common.Configuration.AspNet.Dtos/CanopeeConfigurationDto.md) | A configuration associated to a agent and/or a group. |

## Canopee.Common.Configuration.Events namespace

| public type | description |
| --- | --- |
| class [NewConfigurationEventArg](Canopee.Common.Configuration.Events/NewConfigurationEventArg.md) | Args for the new configuration event. When a new configuration is detected by the [`ICanopeeConfigurationSynchronizer`](Canopee.Common.Configuration/ICanopeeConfigurationSynchronizer.md), it raise [`OnNewConfiguration`](Canopee.Common.Configuration/ICanopeeConfigurationSynchronizer/OnNewConfiguration.md) event. |

## Canopee.Common.Hosting namespace

| public type | description |
| --- | --- |
| interface [ICanopeeHost](Canopee.Common.Hosting/ICanopeeHost.md) | A host that will contain the Canope pipeline engine |

## Canopee.Common.Logging namespace

| public type | description |
| --- | --- |
| interface [ICanopeeLogger](Canopee.Common.Logging/ICanopeeLogger.md) | Contract for the object that will log information in object |

## Canopee.Common.Pipelines namespace

| public type | description |
| --- | --- |
| interface [ICollectPipeline](Canopee.Common.Pipelines/ICollectPipeline.md) | The interface for an extract, transform and load process that is the heart of Canopee platform. |
| interface [IInput](Canopee.Common.Pipelines/IInput.md) | The interface of the object that is in charge of collecting one or more event in a [`ICollectPipeline`](Canopee.Common.Pipelines/ICollectPipeline.md) collect process |
| interface [IOutput](Canopee.Common.Pipelines/IOutput.md) | The interface of the object that is in charge of pushing [`ICollectedEvent`](Canopee.Common.Pipelines.Events/ICollectedEvent.md) of a [`Collect`](Canopee.Common.Pipelines/ICollectPipeline/Collect.md) process to an external output |
| interface [ITransform](Canopee.Common.Pipelines/ITransform.md) | The interface of the object that is in charge of enrich one or more event in a [`ICollectPipeline`](Canopee.Common.Pipelines/ICollectPipeline.md) collect process after the extraction of it by a [`IInput`](Canopee.Common.Pipelines/IInput.md) object |
| interface [ITrigger](Canopee.Common.Pipelines/ITrigger.md) | The interface of the object that is in charge of starting the [`Collect`](Canopee.Common.Pipelines/ICollectPipeline/Collect.md) process |
| class [TriggerSubscriptionContext](Canopee.Common.Pipelines/TriggerSubscriptionContext.md) | The context of a subscription done on a [`ITrigger`](Canopee.Common.Pipelines/ITrigger.md) |

## Canopee.Common.Pipelines.Events namespace

| public type | description |
| --- | --- |
| class [CollectedEvent](Canopee.Common.Pipelines.Events/CollectedEvent.md) | An event collected, transformed and output by a [`ICollectPipeline`](Canopee.Common.Pipelines/ICollectPipeline.md) Implements [`ICollectedEvent`](Canopee.Common.Pipelines.Events/ICollectedEvent.md) |
| interface [ICollectedEvent](Canopee.Common.Pipelines.Events/ICollectedEvent.md) |  |
| class [TriggerEventArgs](Canopee.Common.Pipelines.Events/TriggerEventArgs.md) | The arg of an event that triggered a pipeline. |
| class [WebTriggerArg](Canopee.Common.Pipelines.Events/WebTriggerArg.md) | A [`TriggerEventArgs`](Canopee.Common.Pipelines.Events/TriggerEventArgs.md) specific to a web context |

<!-- DO NOT EDIT: generated by xmldocmd for Canopee.Common.dll -->
