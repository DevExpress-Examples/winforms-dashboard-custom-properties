*Files to look at*:

* [Form1.cs](./CS/WinForms-Dashboard-Custom-Properties/Form1.cs) (VB: [Form1.vb](./VB/WinForms-Dashboard-Custom-Properties/Form1.vb))
* [DashboardDescriptionModule.cs](./CS/WinForms-Dashboard-Custom-Properties/Modules/DashboardDescriptionModule.cs) (VB: [DashboardDescriptionModule.vb](./VB/WinForms-Dashboard-Custom-Properties/Modules/DashboardDescriptionModule.vb))
* [GridFixedColumnModule.cs](./CS/WinForms-Dashboard-Custom-Properties/Modules/GridFixedColumnModule.cs) (VB: [GridFixedColumnModule.vb](./VB/WinForms-Dashboard-Custom-Properties/Modules/GridFixedColumnModule.vb))
* [ChartScaleBreakModule.cs](./CS/WinForms-Dashboard-Custom-Properties/Modules/ChartScaleBreakModule.cs) (VB: [ChartScaleBreakModule.vb](./VB/WinForms-Dashboard-Custom-Properties/Modules/ChartScaleBreakModule.vb))
* [ConstantLineUserValueModule.cs](./CS/WinForms-Dashboard-Custom-Properties/Modules/ConstantLineUserValueModule.cs) (VB: [ConstantLineUserValueModule.vb](./VB/WinForms-Dashboard-Custom-Properties/Modules/ConstantLineUserValueModule.vb))


# WinForms Dashboard - Custom Properties

The following example shows how to create [custom properties](http://docs.devexpress.com/Dashboard/401595/designer-and-viewer-applications/winforms-designer/custom-properties) for the WinForms Dashboard. 

![](img/win-custom-properties.png)

The example contains the following custom properties that organized as separate modules:

## Dashboard Description
This module enables you to set a dashboard description. The **Dashboard Description** button is located in the _Dashboard_ Ribbon's group on the _Home_ page. The dashboard description is displayed when you hover the info button in the dashboard title. 
    
**Overview:**
* Adds a string custom property for a dashboard.
* Provides editors to edit a dashboard description.
* Shows how to add a new item to the existing Ribbon group.

## Grid Fixed (Pinned) Columns
This module enables you to pin columns in the Grid item. The **Fix Columns** button is located in the _Custom Properties_ Ribbon's group on the Grid's _Design_ page. The button invokes an editor where you can set which columns to pin.

**Overview:**
* Adds a boolean custom property for a specific data item container (Grid columns).
* Provides an editor that displays a list of grid columns to be pinned.
* Shows how to create a new group in the Ribbon and add a new item.

## Chart Scale Break
This module enables or disables scale breaks for the Chart dashboard item. The **Scale Break** button is located in the _Custom Properties_ Ribbon's group on the Chart's _Design_ page.

**Overview:**
* Adds a boolean custom property for a specific dashboard item (Chart).
* Shows how to create a new group in the Ribbon and add a new item.
* Customizes export to display the result in the exported document.

## Chart Constant Line
This module allows you to draw a constant line for the selected Chart item. The **Constant Line** button is located in the _Custom Properties_ ribbon group on the Chart's _Design_ page. The button invokes an editor where you can set a constant line's options.

**Overview:**
* Adds a complex custom property for a specific dashboard item (Chart).
* Shows how to create a new group in the Ribbon and add a new item.
* Customizes export to display the result in the exported document.

## See Also

- [Custom Properties](https://docs.devexpress.com/Dashboard/401595/designer-and-viewer-applications/winforms-designer/custom-properties?v=20.1)
