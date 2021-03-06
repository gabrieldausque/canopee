# ICanopeeLogger.Log method

Log a message with the specified log level

```csharp
public void Log(string message, LogLevel level = LogLevel.Information, 
    [CallerMemberName] string memberName = "", [CallerFilePath] string sourceFilePath = "", 
    [CallerLineNumber] int sourceLineNumber = 0)
```

| parameter | description |
| --- | --- |
| message | the message to log |
| level | the LogLevel of the message |
| memberName | the member name specified on runtime |
| sourceFilePath | the source path specified on runtime |
| sourceLineNumber | the source line number specified on runtime |

## See Also

* interface [ICanopeeLogger](../ICanopeeLogger.md)
* namespace [Canopee.Common.Logging](../../Canopee.Common.md)

<!-- DO NOT EDIT: generated by xmldocmd for Canopee.Common.dll -->
