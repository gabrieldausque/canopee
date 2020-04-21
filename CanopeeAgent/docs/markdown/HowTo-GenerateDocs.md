prerequire site
xmlmarlkdown tool (see [doc](https://ejball.com/XmlDocMarkdown/))

to install : 

``` bash
dotnet tool install xmldocmd -g 
```

To generate doc 

1. publish agent and or server
2. execute from doc directory following command 

``` bash
xmldocmd ../bin/Agent/Published/Canopee.Common.dll ./markdown/References/
xmldocmd ../bin/Agent/Published/Canopee.Core.dll ./markdown/References/

```
