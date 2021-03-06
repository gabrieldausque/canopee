# LinuxNetworkInput class

Collect all [`NetworkInfo`](NetworkInfo.md) of the current workstation/server for LINUX OS

```csharp
{
    ...
    "Canopee": {
        ...
            "Pipelines": [
             ...   
              {
                "Name": "Products",
                ...
                "Input": {
                   "InputType": "Network",
                   "OSSpecific": true
                },
             ...
            }
            ...   
            ]
        ...
    }
}
```

the InputType is Network The OSSpecific argument must be set to true.

```csharp
public class LinuxNetworkInput : BatchInput
```

## Public Members

| name | description |
| --- | --- |
| [LinuxNetworkInput](LinuxNetworkInput/LinuxNetworkInput.md)() | Default constructor. Set [`CommandLine`](../Canopee.StandardLibrary.Inputs.Batch/BatchInput/CommandLine.md) to ifconfig |
| override [Collect](LinuxNetworkInput/Collect.md)(…) | Get one or more [`NetworkInfo`](NetworkInfo.md) that represents a network card. extract interface name, ipv4 and mac address. |

## See Also

* class [BatchInput](../Canopee.StandardLibrary.Inputs.Batch/BatchInput.md)
* namespace [Canopee.StandardLibrary.Inputs.Network](../Canopee.StandardLibrary.md)

<!-- DO NOT EDIT: generated by xmldocmd for Canopee.StandardLibrary.dll -->
