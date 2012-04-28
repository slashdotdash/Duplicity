# Duplicity

Duplicity is a tool for duplicating file system changes from one directory to another.

It's goal is to run as a Windows service and replicate one or more given directories to another location.
An example of usage would be to backup a local directory to an external hard disk. 

However, my intended usage is to allow working with source files stored on a [RAM disk](http://en.wikipedia.org/wiki/RAM_disk) with changes automatically copied to a hard disk as a safety net backup.

This is alpha software at the moment; do not use.

## Installation & Prerequisites

Dependencies are managed with NuGet. The packages are not included in source control but will be restored, if missing, when the solution is built via [NuGet Package Restore](http://docs.nuget.org/docs/workflows/using-nuget-without-committing-packages).

* [NuGet](http://nuget.org/)

* [FSWatcher](https://github.com/acken/FSWatcher) a cross platform file system watcher for .Net and Mono
  
* [Machine.Specifications](https://github.com/machine/machine.specifications)

## TODO

* Use [Topshelf](http://topshelf-project.com/documentation/getting-started/) to host as a Windows service 
  