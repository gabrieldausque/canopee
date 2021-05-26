# BaseTrigger class

The base implementation of ITrigger

```csharp
public abstract class BaseTrigger : ITrigger
```

## Public Members

| name | description |
| --- | --- |
| [OwnerId](BaseTrigger/OwnerId.md) { get; set; } | The id of the owner object ( a [`CollectPipeline`](CollectPipeline.md)) |
| [OwnerName](BaseTrigger/OwnerName.md) { get; set; } | The name of the owner object (a [`CollectPipeline`](CollectPipeline.md)) |
| [Dispose](BaseTrigger/Dispose.md)() | Dispose the current ITrigger |
| virtual [Initialize](BaseTrigger/Initialize.md)(…) | Initialize the trigger with the Trigger configuration and its logger |
| virtual [RaiseEvent](BaseTrigger/RaiseEvent.md)(…) | Raise the [`EventTriggered`](BaseTrigger/EventTriggered.md) |
| abstract [Start](BaseTrigger/Start.md)() | Start the watch of the trigger |
| abstract [Stop](BaseTrigger/Stop.md)() | Stop the watch of the trigger |
| virtual [SubscribeToTrigger](BaseTrigger/SubscribeToTrigger.md)(…) | Subscribe to the Event |

## Protected Members

| name | description |
| --- | --- |
| [BaseTrigger](BaseTrigger/BaseTrigger.md)() | The default constructor. |
| [Logger](BaseTrigger/Logger.md) | The internal ICanopeeLogger |
| event [EventTriggered](BaseTrigger/EventTriggered.md) | Event raise when the trigger check that a collect needs to be done |
| virtual [Dispose](BaseTrigger/Dispose.md)(…) | Dispose the current ITrigger |
| abstract [InternalDispose](BaseTrigger/InternalDispose.md)() | Will dispose all needed internal object of the current implementation |

## See Also

* namespace [Canopee.Core.Pipelines](../Canopee.Core.md)

<!-- DO NOT EDIT: generated by xmldocmd for Canopee.Core.dll -->