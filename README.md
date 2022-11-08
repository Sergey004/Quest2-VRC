# Quest2-VRC
This program transmits Quest 2 battery information to the VRChtat via the OSC protocol

## Building from source
To build this application you will need:
- Visual Studio 2019 
- [ADB](https://developer.android.com/studio/releases/platform-tools)

To start building the application, simply launch the solution in Visual Studio and run " Restore NuGet packages" to download dependencies

## Using
Add a parameter to the ExpressionParameters of your avatar by assigning ```HMDBat```, ```ControllerBatL```,  ```ControllerBatR```  with a value ```-1 float```

Connect your Quest 2 to your computer in developer mode

And hope for the best that this program will work for you

## Dependencies

- [AdvancedSharpAdbClient](https://github.com/yungd1plomat/AdvancedSharpAdbClient)
- [Bespoke Osc](https://github.com/emilytrau/Bespoke.Osc)

Based on modified source code from https://github.com/KaleidonKep99/VRChat_CS_OSCTest
