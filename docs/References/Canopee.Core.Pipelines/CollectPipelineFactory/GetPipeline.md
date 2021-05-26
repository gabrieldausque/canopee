# CollectPipelineFactory.GetPipeline method

Get the pipeline corresponding to the pipeline configuration section passed

```csharp
public ICollectPipeline GetPipeline(IConfigurationSection pipelineConfiguration, 
    IConfigurationSection loggingConfiguration)
```

| parameter | description |
| --- | --- |
| pipelineConfiguration | The pipeline configuration that defines the wanted pipeline. If no Type of pipeline defined explicitely, use the "Default" type |
| loggingConfiguration | The logger configuration |

## Return Value

a ICollectPipeline

## See Also

* class [CollectPipelineFactory](../CollectPipelineFactory.md)
* namespace [Canopee.Core.Pipelines](../../Canopee.Core.md)

<!-- DO NOT EDIT: generated by xmldocmd for Canopee.Core.dll -->