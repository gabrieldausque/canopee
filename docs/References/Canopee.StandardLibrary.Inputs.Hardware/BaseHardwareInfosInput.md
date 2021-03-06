# BaseHardwareInfosInput class

This is the base class for hardware collect informations. As the collect is specific for each supported OS, this class needs to be implemented for each OS. The current library contains two implementations : - windows [`WindowsHardwareInfosInput`](WindowsHardwareInfosInput.md) - linux [`LinuxHardwareInfosInput`](LinuxHardwareInfosInput.md) This base class is responsible for the global sequence which is the same for each OS : - collect Cpu - collect memory - collect disks - collect display info (graphical cards and display) - collect usb peripherals Configuration for this input will be :

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
                   "InputType": "Hardware",
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

the InputType is Hardware The OSSpecific argument must be set to true.

```csharp
public abstract class BaseHardwareInfosInput : BatchInput
```

## Public Members

| name | description |
| --- | --- |
| override [Collect](BaseHardwareInfosInput/Collect.md)(…) | Collect one [`HardwareInfos`](HardwareInfos.md) to be treated in the executing pipeline |

## Protected Members

| name | description |
| --- | --- |
| [BaseHardwareInfosInput](BaseHardwareInfosInput/BaseHardwareInfosInput.md)() | Default constructor. Initialize the units mapping repository. |
| [GetOptimizedSizeAndUnit](BaseHardwareInfosInput/GetOptimizedSizeAndUnit.md)(…) | Get a human readable bytes value in the optimized unit. |
| [GetSizeUnit](BaseHardwareInfosInput/GetSizeUnit.md)(…) | Map a customUnit to a standard unit. Used to standardize unit used in some batch. |
| abstract [SetCpuInfos](BaseHardwareInfosInput/SetCpuInfos.md)(…) | Set cpu infos in the [`CpuArchitecture`](HardwareInfos/CpuArchitecture.md),[`CpuModel`](HardwareInfos/CpuModel.md) and [`CpusAvailable`](HardwareInfos/CpusAvailable.md) in the [`HardwareInfos`](HardwareInfos.md) arg |
| abstract [SetDiskInfos](BaseHardwareInfosInput/SetDiskInfos.md)(…) | Set one or more [`DiskInfos`](DiskInfos.md) in the [`Disks`](HardwareInfos/Disks.md) in the [`HardwareInfos`](HardwareInfos.md) arg |
| abstract [SetDisplayInfos](BaseHardwareInfosInput/SetDisplayInfos.md)(…) | Set one or more [`DisplayInfos`](DisplayInfos.md) in the [`Displays`](HardwareInfos/Displays.md) and one or more [`GraphicalCardInfos`](GraphicalCardInfos.md) in the [`GraphicalCards`](HardwareInfos/GraphicalCards.md) in the [`HardwareInfos`](HardwareInfos.md) arg |
| abstract [SetMemoryInfos](BaseHardwareInfosInput/SetMemoryInfos.md)(…) | Set memory infos in the [`MemorySize`](HardwareInfos/MemorySize.md) and [`MemoryUnit`](HardwareInfos/MemoryUnit.md) |
| abstract [SetUsbPeripherals](BaseHardwareInfosInput/SetUsbPeripherals.md)(…) | Set one or more [`UsbPeripheralInfos`](UsbPeripheralInfos.md) in the [`USBPeripherals`](HardwareInfos/USBPeripherals.md) in the [`HardwareInfos`](HardwareInfos.md) arg |

## See Also

* class [BatchInput](../Canopee.StandardLibrary.Inputs.Batch/BatchInput.md)
* namespace [Canopee.StandardLibrary.Inputs.Hardware](../Canopee.StandardLibrary.md)

<!-- DO NOT EDIT: generated by xmldocmd for Canopee.StandardLibrary.dll -->
