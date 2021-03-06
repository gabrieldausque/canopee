# Log4NetCanopeeLogger class

A Log4net logger wrapper, by default will use the log4net.config file that will exists in the working directory. The configuration will be :

```csharp
{
    ...
   "Canopee": {
   ...
       "Logging": {
           "LoggerType": "Log4Net",
           "configurationFile":"mylog4netconfigfile.conf"     
       },
   ...
  }
...
}
```

The LoggerType is Log4Net configurationFile attribute is optional. it define the name of the log4net configuration file. By default it is log4net.config.

```csharp
public class Log4NetCanopeeLogger : BaseCanopeeLogger
```

## Public Members

| name | description |
| --- | --- |
| [Log4NetCanopeeLogger](Log4NetCanopeeLogger/Log4NetCanopeeLogger.md)() | The default constructor. |
| override [Initialize](Log4NetCanopeeLogger/Initialize.md)(…) |  |
| override [Log](Log4NetCanopeeLogger/Log.md)(…) |  |

## See Also

* namespace [Canopee.StandardLibrary.Loggers](../Canopee.StandardLibrary.md)

<!-- DO NOT EDIT: generated by xmldocmd for Canopee.StandardLibrary.dll -->
