# Canopee Applications

## Introduction

The Canopee Applications is a simple, configurable and easily telemetry tool to collect, transform and push datas to any datalake you want.

It has been initially designed to collect local info on workstations and push them to Elastic Search, but can be extended to any collect, transform and output you want !

It contains two components : 

- The Agent : responsible for collecting event on a local workstation or server and send them to the output, that can be CanopeeServer for further data transformations, or directly your datalake.
> There is two implementations of agent : the console agent, which is more for demo and test purpose, and the electron agent which is more for production. The difference between both agent are the host process.
- The Server : expose a pipeline engine through REST and help you ingest and transform collected datas from agent with datas that are not available on agents. Used also to centralize configurations of agents by group or agentid (Experimental features for now).

## Prerequiresite

The Canopee Applications are dotnet core 3.1 standalone applications

## Installation 

> Work In Progress

## General 

### Architecture

 Below a global scheme of a standard Canopee architecture : 
 
![](Images/GeneralArchitecture.png)

 As you can see, agents will collect data locally on client workstations, send them through REST call to a Canopee Server Node, that will enrich data through specific pipelines, and then store data to an ElasticSearch server. Operationals can then consult data through a Kibana server, using reports designed specifically for them.
 
 ### Objects Relationships
 
 Below a schema that describes relations between each type of object in Canopee Framework :
 
 ![](Images/ObjectArchitecture.png)
  
 ### How does it works 
 
 How does it works ? Below a schema of the pipeline process that describes the way a pipeline is working : 
 
 ![](Images/PipelineProcessDiagram.png) 

To sumup : 

> Work in progress : NEXT TODO

## Resources

- [Api References](References/ReferencesHome.md) 