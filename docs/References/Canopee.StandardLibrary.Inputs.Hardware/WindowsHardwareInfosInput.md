# WindowsHardwareInfosInput class

This class is the [`BaseHardwareInfosInput`](BaseHardwareInfosInput.md) for all Windows OS

```csharp
public class WindowsHardwareInfosInput : BaseHardwareInfosInput
```

## Public Members

| name | description |
| --- | --- |
| [WindowsHardwareInfosInput](WindowsHardwareInfosInput/WindowsHardwareInfosInput.md)() | Default constructor. Set the shell executor to cmd. |

## Protected Members

| name | description |
| --- | --- |
| override [SetCpuInfos](WindowsHardwareInfosInput/SetCpuInfos.md)(…) | Get cpus infos. use echo and wmic command. |
| override [SetDiskInfos](WindowsHardwareInfosInput/SetDiskInfos.md)(…) | Get all [`DiskInfos`](DiskInfos.md). use wmic command |
| override [SetDisplayInfos](WindowsHardwareInfosInput/SetDisplayInfos.md)(…) | Get all [`DisplayInfos`](DisplayInfos.md). Use wmic command. |
| override [SetMemoryInfos](WindowsHardwareInfosInput/SetMemoryInfos.md)(…) | Set memory infos. use wmic command |
| override [SetUsbPeripherals](WindowsHardwareInfosInput/SetUsbPeripherals.md)(…) | Get all [`UsbPeripheralInfos`](UsbPeripheralInfos.md). Use wmic command. |

## See Also

* class [BaseHardwareInfosInput](BaseHardwareInfosInput.md)
* namespace [Canopee.StandardLibrary.Inputs.Hardware](../Canopee.StandardLibrary.md)

<!-- DO NOT EDIT: generated by xmldocmd for Canopee.StandardLibrary.dll -->