# whatmouse

Small Windows mouse jiggler with a Start/Stop button.

## Run

Download and run `whatmouse.exe`.

The executable is published as a self-contained Windows x64 single file, so it does not require installing .NET or PowerShell scripts on the target machine.

## Build

Requires .NET SDK 7 or newer on the build machine:

```powershell
dotnet publish .\WhatMouse.csproj -c Release -r win-x64 --self-contained true -p:PublishSingleFile=true -p:EnableCompressionInSingleFile=true -p:IncludeNativeLibrariesForSelfExtract=true -p:DebugType=None -p:DebugSymbols=false -o .\publish
```

The output executable is `publish\whatmouse.exe`.
