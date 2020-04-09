using System;
using System.Collections.Generic;
using DevExpress.DashboardCommon;
using DevExpress.DashboardWin;
using DevExpress.Utils.Svg;
using DevExpress.XtraBars;
using DevExpress.XtraBars.Ribbon;
using DevExpress.XtraCharts;
using DevExpress.XtraReports.UI;

namespace WindowsFormsAppCustomProperties {
    public class ChartScaleBreakModule {
        public static readonly string PropertyName = "ScaleBreak";
        readonly DashboardDesigner designer;
        BarCheckItem barItem;

        public ChartScaleBreakModule(DashboardDesigner designer, SvgImage barImage = null) {
            this.designer = designer;
            RibbonControl ribbon = (RibbonControl)designer.MenuManager;
            RibbonPage page = ribbon.GetDashboardRibbonPage(DashboardBarItemCategory.ChartTools, DashboardRibbonPage.Design);
            RibbonPageGroup group = page.GetGroupByName("Custom Properties");
            if(group == null) {
                group = new RibbonPageGroup("Custom Properties") { Name = "Custom Properties" };
                page.Groups.Add(group);
            }
            barItem = CreateBarItem("Scale Break", barImage);
            group.ItemLinks.Add(barItem);
            barItem.ItemClick += ChangeCustomPropertyValue;
            designer.DashboardItemControlUpdated += Designer_DashboardItemControlUpdated;
            designer.DashboardItemSelected += Designer_DashboardItemSelected;
            designer.DashboardCustomPropertyChanged += Designer_DashboardCustomPropertyChanged;
            designer.CustomExport += Designer_CustomExport;
        }
        BarCheckItem CreateBarItem(string caption, SvgImage barImage) {
            BarCheckItem barItem = new BarCheckItem();
            barItem.Caption = caption;
            barItem.ImageOptions.SvgImage = barImage;
            return barItem;
        }
        void ChangeCustomPropertyValue(object sender, ItemClickEventArgs e) {
            DashboardItem dashboardItem = designer.SelectedDashboardItem;
            bool newValue = !Convert.ToBoolean(dashboardItem.CustomProperties.GetValue(PropertyName));
            string status = newValue ? "enabled" : "disabled";
            CustomPropertyHistoryItem historyItem = new CustomPropertyHistoryItem(dashboardItem, PropertyName, newValue.ToString(), $"Scale break for {dashboardItem.ComponentName} is {status}");
            designer.AddToHistory(historyItem);
        }
        void Designer_DashboardCustomPropertyChanged(object sender, CustomPropertyChangedEventArgs e) {
            if(e.Name == PropertyName)
                UpdateBarItem();
        }
        void Designer_DashboardItemSelected(object sender, DashboardItemSelectedEventArgs e) {
            UpdateBarItem();
        }
        void UpdateBarItem() {
            barItem.Checked = Convert.ToBoolean(designer.SelectedDashboardItem.CustomProperties.GetValue(PropertyName));
        }
        void Designer_CustomExport(object sender, CustomExportEventArgs e) {
            Dictionary<string, XRControl> controls = e.GetPrintableControls();
            foreach(var control in controls) {
                XRChart xrChart = control.Value as XRChart;
                if(xrChart != null) {
                    string itemComponentName = control.Key;
                    UpdateChartScaleBreaks(designer.Dashboard.Items[itemComponentName], xrChart.Diagram);
                }
            }
        }
        void Designer_DashboardItemControlUpdated(object sender, DashboardItemControlEventArgs e) {
            if(e.ChartControl != null)
                UpdateChartScaleBreaks(designer.Dashboard.Items[e.DashboardItemName], e.ChartControl.Diagram);
        }
        void UpdateChartScaleBreaks(DashboardItem dashboardItem, Diagram chartDiagram) {
            if(chartDiagram != null) {
                bool scaleBreakEnabled = Convert.ToBoolean(dashboardItem.CustomProperties.GetValue(PropertyName));
                ((XYDiagram)chartDiagram).SecondaryAxesY[0].AutoScaleBreaks.Enabled = scaleBreakEnabled;
            }
        }
    }
}
