# ICanopeeLogger.LogError method

Log a message with error level

```csharp
public void LogError(string message, [CallerMemberName] string memberName = "", 
    [CallerFilePath] string sourceFilePath = "", [CallerLineNumber] int sourceLineNumber = 0)
```

| parameter | description |
| --- | --- |
| message | the message to log |
| memberName | the member name specified on runtime |
| sourceFilePath | the source path specified on runtime |
| sourceLineNumber | the source line number specified on runtime |

## See Also

* interface [ICanopeeLogger](../ICanopeeLogger.md)
* namespace [Canopee.Common.Logging](../../Canopee.Common.md)

<!-- DO NOT EDIT: generated by xmldocmd for Canopee.Common.dll -->
