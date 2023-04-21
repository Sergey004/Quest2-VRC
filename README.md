# Quest2-VRC but for Quest Native (POC state)

![image](https://user-images.githubusercontent.com/11889498/233682219-d7dee072-867f-429d-ba1d-f7933d27db41.png "POC")

This program sending Quest 2 (As well as other headsets in the Quest line) battery information (Now also Wifi signal strength, especially for you, AirLink or VD users) to the VRChtat via the OSC protocol ~~,also receive osc address to control OpenRGB~~ (disabled in modile version)

### Available features 

- [x] HMD Status
- [ ] L\R Conrroller status
- [ ] Wifi signal

If you see a program with approximately the same functionality, it is either a fork or a copy, this program is the only one that uses ADB and OSC at the same time

~~OpenRGB functions tested only on MSI Mystic Light (AKA MSI-RGB)~~

[Avatars with support for this program](Avatars%20with%20Quest2-VRC%20support.md)

Available also in [CLI](https://github.com/Sergey004/Quest2-VRC/tree/cli_ver) form (unmaintained)

## Building from source
To build this application you will need:
- Visual Studio 2019 or later + Xamarin

To start building the application, simply launch the solution in Visual Studio and run "Restore NuGet packages" to download dependencies



## Using
Add a parameter to the ExpressionParameters of your avatar by assigning

- ~~Replace 127.0.0.1 in the vars.txt file with the IP address of your headset if you are using VRChat in standalone mode~~ (disabled in modile version)

For sending:
- ~~You can replace ```HMDBat```, ```ControllerBatL```, ```ControllerBatR```, ```SendPort``` with your own parameters is vars.txt~~ (disabled in modile version)

Default vars

|Var name|Type|Value|
|---|---|---|
|HMDBat|Float|-1|
|ControllerBatL|Float|-1|
|ControllerBatLR|Float|-1|
|LowHMDBat|Bool|False|
|WifiRSSI|Float|-1|
 

(About RSSI 0.0 is best, -1 is worst)

For receiving
- Replace in var.txt ```Receive_addr```, ```Receive_addr_test``` and ```ReceivePort``` according to your specific parameters

Connect your Quest 2 (Or another headset from the Quest range) to your computer in developer mode

And hope for the best that this program will work for you



## Dependencies

- ~~[ADB](https://developer.android.com/studio/releases/platform-tools)~~
- ~~[AdvancedSharpAdbClient](https://github.com/yungd1plomat/AdvancedSharpAdbClient)~~
- [Bespoke.Osc](https://bitbucket.org/pvarcholik/bespoke.osc)
- ~~[OpenRGB.NET](https://github.com/diogotr7/OpenRGB.NET)~~
- ~~[MaterialSkin.2](https://github.com/leocb/MaterialSkin)~~
- ~~[GitInfo](https://github.com/devlooped/GitInfo)~~

Sending code based on modified source code from https://github.com/KaleidonKep99/VRChat_CS_OSCTest

Audio files were generated with [xVASynth](https://github.com/DanRuta/xVA-Synth) 

(If you know whose voice it is and which character it represents, then you're good, you found a mini Easter Egg :) )
