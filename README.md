# Quest2-VRC
[![CI](https://github.com/Sergey004/Quest2-VRC/actions/workflows/main.yml/badge.svg)](https://github.com/Sergey004/Quest2-VRC/actions/workflows/main.yml)

![image](https://user-images.githubusercontent.com/11889498/230911077-48b58669-f37f-433d-b6ae-17bf2af58db9.png)

This program sending Quest 2 (As well as other headsets in the Quest line) battery (and not only) information (Now also Wifi signal strength, especially for you, AirLink or VD users) to the VRChtat via the OSC protocol, also receive osc address to control OpenRGB

If you like this program, please put a star or better yet spread the word about this program

Zeroconf discovery tested on v55 (Android 12 based) [Zeroconf discovery addon](https://github.com/Sergey004/Quest2-VRC/releases/tag/New_Addon)

OpenRGB functions tested only on MSI Mystic Light (AKA MSI-RGB *I think any RGB controller that is supported in OpenRGB will work...*

[Avatars with support for this program](Avatars%20with%20Quest2-VRC%20support.md)

## Building from source
To build this application you will need:
- Visual Studio 2019 or later
- .NET Core 6+

To start building the application, simply launch the solution in Visual Studio and run "Restore NuGet packages" to download dependencies

## Or you can dowload preconpile version fron Github Actions

There are two versions:

- Core version where the basic methods and the possibility to use them in other projects (ADB and OSC functions, remote connectivity)

- GUI version with additional functionality (Мanaging Oculus services and settings on PC, crash watch dog for Oculus Dash)

## Using

- Connect the Quest 2 (or another Quest headset) to your computer using USB or Wi-Fi in developer mode
  
  [How to enadle](https://www.wikihow.com/Enable-Developer-Mode-Oculus-Quest-2)
 
- Replace 127.0.0.1 in the vars.json file with the IP address of your headset if you are using VRChat in standalone mode (AKA VRC on Quest) or AirLink (Or VD) 

Add a parameter to the ExpressionParameters of your avatar by assigning (Or use [Quest2-VRC OSC bindings](Bindings/Quest2-VRC%20OSC%20bindings.unitypackage) for faster integration or to test the app functions in Unity)

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

*some vars in code maked as Int, as workaround I use Int to Float convertion for work as intended*

```CPUtemp``` , ```GPUtemp``` and ```WifiRSSI``` vars is optional

For receiving:
- Replace in var.txt ```Receive_addr```, ```Receive_addr_test``` according to your specific parameters



## About interaction between Quest and ADB

``dumpsys`` in 100% safe

> dumpsys is a tool that runs on Android devices and provides information about system services. Call dumpsys from the command line using the Android Debug Bridge (ADB) to get diagnostic output for all system services running on a connected device.

[Source](https://developer.android.com/tools/dumpsys)

## Dependencies

- [ADB](https://developer.android.com/studio/releases/platform-tools)
- [AdvancedSharpAdbClient](https://github.com/yungd1plomat/AdvancedSharpAdbClient)
- [Bespoke.Osc](https://bitbucket.org/pvarcholik/bespoke.osc)
- [OpenRGB.NET](https://github.com/diogotr7/OpenRGB.NET)
- [MaterialSkin.2](https://github.com/leocb/MaterialSkin)
- [GitInfo](https://github.com/devlooped/GitInfo)
- [NetBeauty2](https://github.com/nulastudio/NetBeauty2)
- [ZeroConf](https://github.com/novotnyllc/Zeroconf)
- [vrc-oscquery-lib](https://github.com/vrchat-community/vrc-oscquery-lib)

Sending code based on modified source code from https://github.com/KaleidonKep99/VRChat_CS_OSCTest

Audio files were generated with ~~[xVASynth](https://github.com/DanRuta/xVA-Synth)~~ [ElevenLabs](https://elevenlabs.io/speech-synthesis) + [RVC](https://github.com/Mangio621/Mangio-RVC-Fork)

(If you know whose voice it is and which character it represents, then you're good, you found a mini Easter Egg :) )
