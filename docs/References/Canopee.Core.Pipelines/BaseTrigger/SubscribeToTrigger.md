# BaseTrigger.SubscribeToTrigger method

Subscribe to the Event

```csharp
public virtual void SubscribeToTrigger(EventHandler<TriggerEventArgs> eventHandler, 
    TriggerSubscriptionContext context)
```

| parameter | description |
| --- | --- |
| eventHandler | The EventHandler to execute when trigger is raised |
| context | The context object of the subscription. Will contain at least owner id and name |

## See Also

* class [BaseTrigger](../BaseTrigger.md)
* namespace [Canopee.Core.Pipelines](../../Canopee.Core.md)

<!-- DO NOT EDIT: generated by xmldocmd for Canopee.Core.dll -->
