# ElasticLookupTransform class

This transforms will make a lookup for a specific key field in an ElasticSearch recordset and add expected field in the ICollectedEvent The configuration will be :

```csharp

     {
         ...
         "Canopee": {
             ...
                 "Pipelines": [
                  ...   
                   {
                     "Name": "OS",
                     ...
                     "Transforms" : [
                         {
                            "TransformType": "ElasticVLookup",
                            "Url": "https://myelasticsearchserver:9200",
                            "SearchedIndex": "MySearchIndex", 
                            "SelectStatement": "SELECT * FROM userinfos",
                            "Key" : {
                                 "LocalName":"KeyNameInCollectedEvent",
                                 "SearchedName":"KeyNameInRecordSet"
                             }, 
                            "Fields": [
                            {
                                "LocalName": "Field1NewName",
                                "SearchedName": "Field1"
                            },
                            {
                                "LocalName": "Field2NewName",
                                "SearchedName": "Field2"
                            }
                         }
                     ]
                  ...
                 }
                 ...   
                 ]
             ...
         }
     }    /// 
```

The TransformType attribute will be ElasticVLookup The Url attribute will contain the url of the elasticsearch server The SearchedIndex attribute will contain the name of the index to search recordset The Key attribute will contain a mapping definition for the key field : LocalName is the name of the key in the ICollectedEvent, SearchedName is the name of the key in the record set The Fields attribute will contain all mappings for each wanted field : LocalName attribute will define the name of the field in the ICollectedEvent, SearchedName will define the name of the field in the recordset from the select statement

```csharp
public class ElasticLookupTransform : BaseTransform
```

## Public Members

| name | description |
| --- | --- |
| [ElasticLookupTransform](ElasticLookupTransform/ElasticLookupTransform.md)() | Default constructor. Initialize _requestedFieldMappings |
| override??[Initialize](ElasticLookupTransform/Initialize.md)(???) | Initialize this ITransform with configurations. Add all defined [`TransformFieldMapping`](TransformFieldMapping.md) from the configuration. |
| override??[Transform](ElasticLookupTransform/Transform.md)(???) | Add new fields from the SelectStatement to a collected event |

## See Also

* namespace??[Canopee.StandardLibrary.Transforms](../Canopee.StandardLibrary.md)

<!-- DO NOT EDIT: generated by xmldocmd for Canopee.StandardLibrary.dll -->
