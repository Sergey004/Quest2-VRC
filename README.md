# Quest2-VRC
This program sending Quest 2 battery information to the VRChtat via the OSC protocol, also receive osc address to control OpenRGB

OpenRGB functions tested only on MSI Mystic Light (AKA MSI-RGB)

## Wireless ADB is broken for now
Ignore the switch and enter
- ```platform-tools\\adb tcpip 5555```
- ```platform-tools\\adb connect  QUEST_IP:5555```

## Building from source
To build this application you will need:
- Visual Studio 2019 or later

To start building the application, simply launch the solution in Visual Studio and run "Restore NuGet packages" to download dependencies

## Using
Add a parameter to the ExpressionParameters of your avatar by assigning

For sending:
- You can replace ```HMDBat``` , ```ControllerBatL```, ```ControllerBatR``` with your own parameters is vars.txt

Default vars
- ```HMDBat``` = ```-1 float```
- ```ControllerBatL``` =```-1 float```
- ```ControllerBatR``` = ```-1 float```
- ```LowHMDBat``` = ```bool false```

For receiving
- Replace in var.txt ```Receive_addr``` and ```Receive_addr_test``` according to your specific parameters

Connect your Quest 2 to your computer in developer mode

And hope for the best that this program will work for you


## Dependencies

- [ADB](https://developer.android.com/studio/releases/platform-tools)
- [AdvancedSharpAdbClient](https://github.com/yungd1plomat/AdvancedSharpAdbClient)
- [Bespoke.Osc](https://bitbucket.org/pvarcholik/bespoke.osc)
- [OpenRGB.NET](https://github.com/diogotr7/OpenRGB.NET)
- [MaterialSkin.2](https://github.com/leocb/MaterialSkin)

Sending code based on modified source code from https://github.com/KaleidonKep99/VRChat_CS_OSCTest

Audio files were generated with [xVASynth](https://github.com/DanRuta/xVA-Synth)
