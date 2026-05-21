# whatmouse

Небольшой джиглер мышки для Windows с кнопкой включения и выключения.

## Запуск

Скачайте и запустите `whatmouse.exe`.

Кнопка `Start` включает джиглер, кнопка `Stop` выключает его. Когда джиглер включен, курсор двигается на 10 пикселей и возвращается обратно каждые 10 секунд.

Файл `whatmouse.exe` собран как самостоятельное приложение для Windows x64. На другом компьютере не нужно устанавливать .NET, PowerShell-модули или дополнительные файлы.

## Сборка

Для сборки нужен .NET SDK 7 или новее:

```powershell
dotnet publish .\WhatMouse.csproj -c Release -r win-x64 --self-contained true -p:PublishSingleFile=true -p:EnableCompressionInSingleFile=true -p:IncludeNativeLibrariesForSelfExtract=true -p:DebugType=None -p:DebugSymbols=false -o .\publish
```

Готовый файл появится здесь: `publish\whatmouse.exe`.
