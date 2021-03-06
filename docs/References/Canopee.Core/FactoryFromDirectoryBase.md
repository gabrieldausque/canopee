# FactoryFromDirectoryBase class

Base class for all factories that will load a catalog of specific operational contract based on a directory

```csharp
public class FactoryFromDirectoryBase
```

## Protected Members

| name | description |
| --- | --- |
| [FactoryFromDirectoryBase](FactoryFromDirectoryBase/FactoryFromDirectoryBase.md)(…) | Default constructor that initialize the [`Container`](FactoryFromDirectoryBase/Container.md) from a directory by loading all assembly in it |
| readonly [Container](FactoryFromDirectoryBase/Container.md) | The MEF container |
| [GetCurrentPlatform](FactoryFromDirectoryBase/GetCurrentPlatform.md)() | Helper that return the OSPlatform of the current workstation where the hosting process is running |

## See Also

* namespace [Canopee.Core](../Canopee.Core.md)

<!-- DO NOT EDIT: generated by xmldocmd for Canopee.Core.dll -->
