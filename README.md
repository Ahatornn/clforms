# clforms

![logo](https://github.com/Ahatornn/clforms/blob/master/Images/favico.jpg)

[![NuGet](https://img.shields.io/nuget/dt/ClForms.svg)](https://www.nuget.org/packages/ClForms)
[![NuGet](https://img.shields.io/nuget/v/ClForms.svg)](https://www.nuget.org/packages/ClForms)

**Pseudographics interface for console window by .Net Core 3.1**

<img src="Images/windowExample.gif" />

## Installation

Find the **ClForms** package through NuGet package manager inside Visual Studio or [here](https://www.nuget.org/packages/ClForms/)
```
PM> Install-Package ClForms -Version 1.0.25
```

> You can also download [ClFormsExtension](https://marketplace.visualstudio.com/items?itemName=KonoplevAnatolii.clforms) for creating pseudographics command-line application with this package
> <img src="Images/clforms-extension.png" />

## Documentation
Go to the [Wiki page](https://github.com/Ahatornn/clforms/wiki) for more information about **clforms**

## Controls
- [x] [Button](https://github.com/Ahatornn/clforms/wiki/Button)
- [x] [Canvas](https://github.com/Ahatornn/clforms/wiki/Canvas)
- [x] [CheckBox](https://github.com/Ahatornn/clforms/wiki/CheckBox)
- [x] [CheckBoxGroup](https://github.com/Ahatornn/clforms/wiki/CheckBoxGroup)
- [x] [DockPanel](https://github.com/Ahatornn/clforms/wiki/DockPanel)
- [x] [GlyphLabel](https://github.com/Ahatornn/clforms/wiki/GlyphLabel)
- [x] [Grid](https://github.com/Ahatornn/clforms/wiki/Grid)
- [x] [GroupBox](https://github.com/Ahatornn/clforms/wiki/GroupBox)
- [x] [Label](https://github.com/Ahatornn/clforms/wiki/Label)
- [x] [ListBox](https://github.com/Ahatornn/clforms/wiki/ListBox)
- [x] [MainMenu](https://github.com/Ahatornn/clforms/wiki/MainMenu)
- [x] [MessageBox](https://github.com/Ahatornn/clforms/wiki/MessageBox)
- [x] [Panel](https://github.com/Ahatornn/clforms/wiki/Panel)
- [x] [ProgressBar](https://github.com/Ahatornn/clforms/wiki/ProgressBar)
- [x] [RadioButton](https://github.com/Ahatornn/clforms/wiki/RadioButton)
- [x] [RadioGroup](https://github.com/Ahatornn/clforms/wiki/RadioGroup)
- [x] [StackPanel](https://github.com/Ahatornn/clforms/wiki/StackPanel)
- [x] [StatusBar](https://github.com/Ahatornn/clforms/wiki/StatusBar)
- [x] [TextBox](https://github.com/Ahatornn/clforms/wiki/TextBox)
- [x] [TilePanel](https://github.com/Ahatornn/clforms/wiki/TilePanel)
- [x] [Window](https://github.com/Ahatornn/clforms/wiki/Window)
- [ ] ListView
<img src="Images/panelExample.gif" />

## Release Notes
* 1.0.25
    * Fix Grid borders (More combination of spanned cells in [GridSpanBorderChars](https://github.com/Ahatornn/clforms/wiki/GridSpanBorderChars))
    * Fix MessageBox icon borders (It has rounded corners, left and right margin)
* 1.0.24
    * Added RadioGroup
    * Added CheckBoxGroup
* 1.0.23
    * Fix Window Measure with empty content
* 1.0.22
    * Fix invalidate visual elements when bg or fg are NotSet
    * Added BackgroundIsTransparent and ForegroundIsTransparent properties into Control
    * Fix Measure and Arrange of StackPanel
    * Fix Measure and Arrange of Window
* 1.0.21
    * Fix cursor position for focusing TextBox
* 1.0.20
    * Inject draw buffering
    * Refactoring ApplicationHandler
    * Refactoring IDrawingContext
* 1.0.19
    * FixPopupMenuContent autosize
    * Fix ListBoxBase methods
* 1.0.18
    * Added ProgressBar
    * Added TextBox
* 1.0.17
    * Added GlyphLabel
    * Implemented ListBox
* 1.0.16
    * Set DialogWindowBackground and DialogWindowForeground of HelpWindow
* 1.0.15
    * Fix Button render
    * Added DialogWindowBackground and DialogWindowForeground
    * Fix MessageBox size
* 1.0.14
    * Added MessageBox
* 1.0.13
    * Added GetRadioCheckChar in IEnvironment. 
    * Added PaintEventArgs
    * Added panels: Canvas, TilePanel
    * Added Buttons  

[More versions](https://www.nuget.org/packages/ClForms/)
