# BatchInput.Collect method

Collect each line of the command line output.

```csharp
public override ICollection<ICollectedEvent> Collect(TriggerEventArgs fromTriggerEventArgs)
```

| parameter | description |
| --- | --- |
| fromTriggerEventArgs | Trigger contextual event arg |

## Return Value

a collection of CollectedEvent with the Raw property filled with a line of the command line output, excluding empty lines

## See Also

* class [BatchInput](../BatchInput.md)
* namespace [Canopee.StandardLibrary.Inputs.Batch](../../Canopee.StandardLibrary.md)

<!-- DO NOT EDIT: generated by xmldocmd for Canopee.StandardLibrary.dll -->
