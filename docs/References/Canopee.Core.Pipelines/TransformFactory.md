# TransformFactory class

The factory in charge of creating ITransform

```csharp
public class TransformFactory : FactoryFromDirectoryBase
```

## Public Members

| name | description |
| --- | --- |
| [TransformFactory](TransformFactory/TransformFactory.md)(…) | Default constructor. |
| static [Instance](TransformFactory/Instance.md)(…) | Get and create if needed the singleton instance |
| [GetTransform](TransformFactory/GetTransform.md)(…) | Create the ITransform corresponding to the configuration |
| static [SetGlobalInstance](TransformFactory/SetGlobalInstance.md)(…) | Set the global factory instance with a new one. |

## See Also

* class [FactoryFromDirectoryBase](../Canopee.Core/FactoryFromDirectoryBase.md)
* namespace [Canopee.Core.Pipelines](../Canopee.Core.md)

<!-- DO NOT EDIT: generated by xmldocmd for Canopee.Core.dll -->
