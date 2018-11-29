# How to set up your C# and .NET Core Web-API local development environment

In this guide we’ll cover how to set up your .NET Core development environment for an Web-API project. 

> This tutorial is intended for Visual Studio on Mac with .NET Core and [`Homebrew`](http://brew.sh/) also Java.

# Required tools, databases, frameworks, libs

1. .Net Core
2. PostgreSQL
3. Kibana
4. Elasticsearch

# Install .NET Core

You can check if you already have .NET Core installed on your machine by opening up a command prompt or terminal and running the following command:

```
$ dotnet --version 
```

You should see something like  `2.1.3`. If you receive an error message, you can  [download .NET Core from Microsoft](https://www.microsoft.com/net/)  and install it.


# Install PostgreSQL

This is a quick guide for installing [PostgreSQL](http://www.postgresql.org/) (`Postgres` for short) on a Mac with [Homebrew](http://brew.sh/)

Before you install anything with Homebrew, you should always make sure it's up to date and that it's healthy:

## Update Homebrew
```
$ brew update
```
## Install Postgres

```
$ brew install postgresql
```
## Start/Stop Postgres
```
$ brew services start postgresql
$ brew services stop postgresql
```
# Install Kibana

```
$ brew install kibana
```
## Start Kibana
```
$ kibana
```
## Access

[http://localhost:5601/app/kibana](http://localhost:5601/app/kibana)

# Install Elasticsearch

```
$ brew update
$ brew install elasticsearch
```
## Use Elasticsearch

Finally, you’re able to run Elasticsearch from the terminal window by simply executing the  `elasticsearch`  command:

```
$ elasticsearch
```
## Access

[http://localhost:9200](http://localhost:9200/)