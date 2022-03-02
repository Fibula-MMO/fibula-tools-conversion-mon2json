# Fibula MMO - MON to JSON Converter
A tool that converts monster files from the Cip format to Fibula's JSON format.

## Prerequisites:

For reasons**, this tool uses the item names supplied by an [IItemTypesLoader](https://github.com/Fibula-MMO/fibula-data-access/blob/main/src/Fibula.DataAccess.Contracts/Abstractions/IItemTypesLoader.cs). 

One implementation for this loader (and the one this tool [is using](https://github.com/Fibula-MMO/fibula-tools-conversion-mon2json/blob/3cc880e0eda4683dfedf3309a15f8d08fa59c455/src/Fibula.Tools.Mon2Json.Standalone/Program.cs#L65)), is [ObjectsFileItemTypeLoader](https://github.com/Fibula-MMO/fibula-plugins-cip-objects-file/blob/main/src/Fibula.Plugins.ItemLoaders.CipObjectsFile/ObjectsFileItemTypeLoader.cs).

You don't really have to use this types loader, but since you are converting from `.mon` files, a wild guess is that you already have in your hands CipSoft's old `objects.srv` files, which is the only input this item loader needs. It's also conveniently in the data folder of this repo.

Anyhow, if you are in fact using this loader, just make sure the right path to this file is being referenced in `appsettings.json`. 

Something like this:

```
{
  "ObjectsFileItemTypeLoaderOptions": {
    "FilePath": "../data/objects.srv"
  }
}
```

##### ** we can probably break this coupling by just passing down a mapping of `itemId`->`itemName` instead of the abstraction, but for now this works.

## Usage:

- Clone this repo
- Run `dotnet build src --configuration Release` to build the project.
- `cd` to the `/src/mon2json/bin/Release/net6.0` folder, and you can run the app from there using `dotnet mon2json.dll` 

### Use the `convert` command:
```
convert <from> <to> [overwrite]
```


