# MINI KeyBoard Configuration Tool

Windows application for configuring programmable mini keyboards (macro pads) from AliExpress.

## Supported Devices

| Device | VID | PID | Keys | Knobs |
|--------|-----|-----|------|-------|
| MINI KeyBoard | 0x1189 (4489) | 0x8890 (34960) | 3-16 | 0-3 |

### Known Compatible Models
- 3-key + 1 knob macro pad
- 6-key macro pad
- 9-key + 1 knob macro pad
- 12-key + 2 knob macro pad
- 16-key + 3 knob macro pad

These devices are commonly sold on AliExpress, Amazon, and other marketplaces under various brand names. They share the same USB HID protocol and can be configured with this tool.

## Features

- **Basic Keys**: Standard keyboard keys (A-Z, 0-9, symbols)
- **Function Keys**: F1-F24, modifiers (Ctrl, Alt, Shift, Win)
- **Multimedia Keys**: Play/Pause, Volume, Next/Previous track, Mute
- **Mouse Keys**: Left/Right/Middle click, scroll wheel
- **LED Control**: RGB lighting modes
- **Layer Functions**: Multiple key layers support

## Key Configuration Limits

- **Maximum 6 characters per key** (firmware limitation)
- Modifier keys (Shift, Ctrl, Alt) count toward the character limit
- Example: "Shift+E+x+a+m+p" = 6 slots used (types "Exampl")

## Requirements

- Windows 7/8/10/11
- .NET Framework 4.8
- USB port

## Building from Source

```bash
# Using Visual Studio 2022
"C:\Program Files\Microsoft Visual Studio\2022\Professional\MSBuild\Current\Bin\MSBuild.exe" "MINI KeyBoard.sln" /p:Configuration=Release /p:Platform="Any CPU"
```

Output: `bin\Release\MINI KeyBoard.exe`

## Usage

1. Connect your mini keyboard via USB
2. Run `MINI KeyBoard.exe`
3. Wait for device detection (status shows "Device Connected")
4. Click on a key button (KEY1-KEY16, or knob positions)
5. Select the key type and value from the panels
6. Click "Download" to write configuration to device

## Protocol

The device uses USB HID with 65-byte reports:
- Report ID: 0x03 (configuration write)
- Bytes 0-3: Key position and type flags
- Bytes 4-63: Key data (up to 6 character slots)

## Pre-built Release

Pre-compiled binaries are available in the `Release/` folder.

## License

This project is licensed under the [MIT License](LICENSE).

This project contains decompiled source code for educational and interoperability purposes.
