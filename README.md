# Klutter Tools

![Screenshot](https://github.com/user-attachments/assets/6d8d9604-6a8e-49a9-8676-1bc9190bd550)

**Klutter Tools** is a collection of Unity Editor extensions offering various
utilities to enhance the editor workflow.

## Features

Currently, the package includes the following features:

### FPS capper

**FPS Capper** allows you to set preferences for limiting the frame rate in the
Unity Editor. This is particularly useful when editing VFX Graphs in Edit Mode,
as V-Sync settings are ignored in this mode, which can lead to unnecessary
energy consumption, heat and fan noise.

### Zoom step scaling in VFX Graph Editor

This feature provides a preference setting to customize the zoom step scaling
in the VFX Graph Editor. It's especially helpful for Mac users with a trackpad,
where scroll gestures can feel overly sensitive and make precise editing
difficult.

### Fixed-pitch font in VFX Graph Text Editor

When enabled, this feature switches the font used in the VFX Graph text editor
(custom HLSL code) to a fixed-pitch typeface and lets you adjust the font size.

### .cube LUT file import

Adds support for importing `.cube` LUT (Look-Up Table) files, commonly used in
video editing software. While this feature is natively available in HDRP, it’s
beneficial for users who want to use it in URP.

### Inspector Note

**Inspector Note** is an editor-only component that allows attaching comments
or notes directly to GameObjects in the Inspector. This can be useful for
leaving reminders, instructions, or context for other developers -- or just
jotting down your own thoughts.

## System Requirements

- Unity 6 or later

## How to Install

The Klutter Tools package (`jp.keijiro.klutter-tools`) can be installed via the
"Keijiro" scoped registry using Package Manager. To add the registry to your
project, please follow [these instructions].

[these instructions]:
  https://gist.github.com/keijiro/f8c7e8ff29bfe63d86b888901b82644c
