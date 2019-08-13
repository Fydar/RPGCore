# The RPGCore Framework

## Setup

### Opening `.bpkg` files

If you would like to be able to open `.bpkg` files, you can tell your OS to open them like compressed folders.

#### Windows

Open up Command Prompt as Administrator and run the following command:

```assoc .bpkg=CompressedFolder```

## RPGCore.CLI

`RPGCore.CLI` is a command line tool that enables building, formating and otherwise modifying `.bproj` and `.bpkg`'s.

### Installation

Run the following command to install `RPGCore.CLI`:

```
dotnet tool install -g RPGCore.CLI
```

### Commands


#### Build

Builds any `.bproj`'s at the current directory.

```
bpack build
```

