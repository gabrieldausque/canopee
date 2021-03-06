# OutputFactory class

The factory in charge of creating IOutput

```csharp
public class OutputFactory : FactoryFromDirectoryBase
```

## Public Members

| name | description |
| --- | --- |
| [OutputFactory](OutputFactory/OutputFactory.md)(…) | Default constructor |
| static [Instance](OutputFactory/Instance.md)(…) | Get and create if needed the singleton instance |
| [GetOutput](OutputFactory/GetOutput.md)(…) | Create an instance of IOutput with the specified configuration |
| [GetOutputs](OutputFactory/GetOutputs.md)(…) |  |
| static [SetGlobalInstance](OutputFactory/SetGlobalInstance.md)(…) | Set the global factory instance with a new one. |

## See Also

* class [FactoryFromDirectoryBase](../Canopee.Core/FactoryFromDirectoryBase.md)
* namespace [Canopee.Core.Pipelines](../Canopee.Core.md)

<!-- DO NOT EDIT: generated by xmldocmd for Canopee.Core.dll -->
