# Canopee Applications

## Introduction

The Canopee Applications is a simple, configurable and easily telemetry tool to collect, transform and push datas to any datalake you want.

It has been initially designed to collect local info on workstations and push them to Elastic Search, but can be extended to any collect, transform and output you want !

It contains two components : 

- The Agent : responsible for collecting event on a local workstation or server and send them to the output, that can be CanopeeServer for further data transformations, or directly your datalake.
> There is two implementations of agent : the console agent, which is more for demo and test purpose, and the electron agent which is more for production. The difference between both agent are the host process.
- The Server : expose a pipeline engine through REST and help you ingest and transform collected datas from agent with datas that are not available on agents. Used also to centralize configurations of agents by group or agentid (Experimental features for now).

## General Architecture

 Below a global scheme of a standard Canopee architecture : 
 
![](Images/GeneralArchitecture.png)

  

## Resources

- [Api References](References/ReferencesHome.md) 