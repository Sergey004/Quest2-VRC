# Quest2-VRC
[![CI](https://github.com/Sergey004/Quest2-VRC/actions/workflows/main.yml/badge.svg)](https://github.com/Sergey004/Quest2-VRC/actions/workflows/main.yml)

![image](https://user-images.githubusercontent.com/11889498/230911077-48b58669-f37f-433d-b6ae-17bf2af58db9.png)

This program sending Quest 2 (As well as other headsets in the Quest line) battery (and not only) information (Now also Wifi signal strength, especially for you, AirLink or VD users) to the VRChtat via the OSC protocol, also receive osc address to control OpenRGB


Zeroconf discovery tested on v55 (Android 12 based)

[Zeroconf discovery addon](https://github.com/Sergey004/Quest2-VRC/releases/tag/Addon)

OpenRGB functions tested only on MSI Mystic Light (AKA MSI-RGB)

[Avatars with support for this program](Avatars%20with%20Quest2-VRC%20support.md)

Available also in [CLI](https://github.com/Sergey004/Quest2-VRC/tree/cli_ver) form (unmaintained)

Also for Quest Native [Xamarin](https://github.com/Sergey004/Quest2-VRC/tree/xamarin) (POC State)

## Building from source
To build this application you will need:
- Visual Studio 2019 or later

To start building the application, simply launch the solution in Visual Studio and run "Restore NuGet packages" to download dependencies

## Or you can dowload preconpile version fron Github Actions

There are two versions:

Core version where the basic methods and the possibility to use them in other projects (Debug and Release)

GUI version where you can use this version right out of the box

## Using
Add a parameter to the ExpressionParameters of your avatar by assigning

- Replace 127.0.0.1 in the vars.json file with the IP address of your headset if you are using VRChat in standalone mode

For sending:
- You can replace ```HMDBat```, ```ControllerBatL```, ```ControllerBatR```, ```SendPort``` with your own parameters is vars.txt

Default values transferred via OSC

|Var name|Type|Value|
|---|---|---|
|HMDBat|Float|-1|
|ControllerBatL|Float|-1|
|ControllerBatLR|Float|-1|
|LowHMDBat|Bool|False|
|WifiRSSI|Float|-1|
|CPUtemp|Int|0|
|GPUtemp|Int|0|
 

(About RSSI 0.0 is best, -1 is worst)

For receiving
- Replace in var.txt ```Receive_addr```, ```Receive_addr_test``` and ```ReceivePort``` according to your specific parameters

Connect your Quest 2 (Or another headset from the Quest range) to your using USB or Wi-Fi computer in developer mode

And hope for the best that this program will work for you



## Dependencies

- [ADB](https://developer.android.com/studio/releases/platform-tools)
- [AdvancedSharpAdbClient](https://github.com/yungd1plomat/AdvancedSharpAdbClient)
- [Bespoke.Osc](https://bitbucket.org/pvarcholik/bespoke.osc)
- [OpenRGB.NET](https://github.com/diogotr7/OpenRGB.NET)
- [MaterialSkin.2](https://github.com/leocb/MaterialSkin)
- [GitInfo](https://github.com/devlooped/GitInfo)
- [NetBeauty2](https://github.com/nulastudio/NetBeauty2)
- [ZeroConf](https://github.com/novotnyllc/Zeroconf)

Sending code based on modified source code from https://github.com/KaleidonKep99/VRChat_CS_OSCTest

Audio files were generated with [xVASynth](https://github.com/DanRuta/xVA-Synth) 

(If you know whose voice it is and which character it represents, then you're good, you found a mini Easter Egg :) )
