# CLAUDE.md

This file provides guidance to Claude Code (claude.ai/code) when working with code in this repository.

## Project Overview

This is a Windows Forms application for configuring a programmable mini keyboard (macro pad). The application communicates with the keyboard hardware via USB HID to program custom key mappings, including standard keys, modifier combinations, multimedia keys, LED modes, and mouse actions.

**Important:** This codebase was reverse-engineered/decompiled from a compiled executable using JetBrains decompiler. The code may have unusual patterns or naming conventions as a result.

## Build Commands

```bash
# Build the solution (requires Visual Studio or MSBuild)
msbuild "MINI KeyBoard.sln" /p:Configuration=Debug

# Build release version
msbuild "MINI KeyBoard.sln" /p:Configuration=Release
```

The project targets .NET Framework 4.0 and outputs a Windows executable.

## Architecture

### HID Communication Layer (`HID/` folder)
- `Hid.cs` - Core HID device communication using Windows API (setupapi.dll, hid.dll, kernel32.dll)
  - Device discovery via `SetupDiGetClassDevs` and related APIs
  - Async read/write operations with 65-byte HID reports
  - Device connection/removal event handling
- `HidLib.cs` (`HIDTester/`) - Alternative HID wrapper using HidLibrary.dll (third-party library in `lib/`)
  - Filters for specific device interface (`mi_01`)
- Supporting structs: `SP_DEVICE_INTERFACE_DATA`, `HIDD_ATTRIBUTES`, `HIDP_CAPS`, etc.

### UI Layer (`HIDTester/` folder)
- `FormMain.cs` - Main window with 16 programmable key buttons (KEY1-KEY16) plus 9 rotary encoder positions (K1/K2/K3 Left/Centre/Right)
  - Polls USB connection status every 30ms via timer
  - Uses `FormMain.KeyParam` static class for shared state across all UI components
- `BasicKeys.cs` - Standard keyboard key selection (A-Z, 0-9, F1-F12, special keys)
- `FunKey.cs` - Modifier key combinations (Ctrl, Shift, Alt)
- `MULKey.cs` - Multimedia keys (Play, Volume, Next/Previous track, Mute)
- `LEDkey.cs` - LED mode configuration (3 modes)
- `MouseKey.cs` - Mouse button and scroll wheel actions
- `LayerFun.cs` - Layer switching functionality

### Key Configuration Protocol
Key configurations are sent as 65-byte HID reports with this structure:
- Byte 0: Key number (1-18 for keys, 0xB0 for LED)
- Byte 1: Key type (1=keyboard, 2=multimedia, 3=mouse, 8=LED)
- Bytes 2+: Key-specific data (scan codes, modifiers, etc.)

The device uses Vendor ID `0x1189` (4489) and Product ID `0x8890` (34960).

## Key State Management

All key configuration state flows through `FormMain.KeyParam`:
- `Data_Send_Buff[65]` - Buffer for HID report data
- `KeySet_KeyNum` (byte 0) - Currently selected key
- `KeyType_Num` (byte 1) - Type of key action
- `KEY_Cur_Layer` - Current layer (1-based)
- `ReportID` - HID report ID (0, 2, or 3 depending on device version)

## Localization

The application supports English and Chinese via .resx resource files and `CultureInfo` switching.
