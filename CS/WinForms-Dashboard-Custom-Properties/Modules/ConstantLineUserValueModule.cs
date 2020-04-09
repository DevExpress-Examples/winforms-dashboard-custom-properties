using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using DevExpress.DashboardCommon;
using DevExpress.DashboardWin;
using DevExpress.Utils;
using DevExpress.Utils.Svg;
using DevExpress.XtraBars;
using DevExpress.XtraBars.Ribbon;
using DevExpress.XtraCharts;
using DevExpress.XtraDataLayout;
using DevExpress.XtraEditors;
using DevExpress.XtraEditors.Controls;
using DevExpress.XtraEditors.Repository;
using DevExpress.XtraReports.UI;

namespace WindowsFormsAppCustomProperties {
    public class ConstantLineUserValueModuleData {
        public string PaneName { get; set; }
        public bool IsSecondaryAxis { get; set; }
        public double Value { get; set; }

        public string GetStringFromData() {
            return PaneName + "_" + IsSecondaryAxis.ToString() + "_" + Value;
        }
    }
    public class ConstantLineUserValueModule {
        public static readonly string PropertyName = "ConstantLine";
        readonly DashboardDesigner designer;

        public ConstantLineUserValueModule(DashboardDesigner designer, SvgImage barImage = null) {
            this.designer = designer;
            BarButtonItem barItem = AddBarItem("Constant line", barImage, designer.Ribbon);
            barItem.ItemClick += BarItem_Click;
            designer.DashboardItemControlUpdated += Designer_DashboardItemControlUpdated;
            designer.CustomExport += Designer_CustomExport;
        }
        BarButtonItem AddBarItem(string caption, SvgImage barImage, RibbonControl ribbon) {
            var page = ribbon.GetDashboardRibbonPage(DashboardBarItemCategory.ChartTools, DashboardRibbonPage.Design);
            RibbonPageGroup group = page.GetGroupByName("Custom Properties");
            if(group == null) {
                group = new RibbonPageGroup("Custom Properties") { Name = "Custom Properties" };
                page.Groups.Add(group);
            }
            var barItem = new BarButtonItem(ribbon.Manager, caption);
            barItem.ImageOptions.SvgImage = barImage;
            group.ItemLinks.Add(barItem);
            return barItem;
        }
        void BarItem_Click(object sender, ItemClickEventArgs e) {
            var chartItem = designer.SelectedDashboardItem as ChartDashboardItem;
            if(chartItem != null) {
                var data = GetConstantLineModuleData(chartItem);
                if(data != null)
                    designer.AddToHistory(new CustomPropertyHistoryItem(chartItem, PropertyName, data.GetStringFromData(), $"{data.PaneName} constant line is set"));
            }
        }
        ConstantLineUserValueModuleData GetConstantLineModuleData(ChartDashboardItem dashboardItem) {
            using(ValueSelectorControl selector = new ValueSelectorControl(dashboardItem.Panes.Select(p => p.Name).ToList())) {
                if(XtraDialog.Show(selector, "Select the required series") == DialogResult.OK) {
                    return selector.ConstantLineModuleData;
                }
            }
            return null;
        }
        void Designer_CustomExport(object sender, CustomExportEventArgs e) {
            Dictionary<string, XRControl> controls = e.GetPrintableControls();
            foreach(var control in controls) {
                XRChart xrChart = control.Value as XRChart;
                if(xrChart != null) {
                    string itemComponentName = control.Key;
                    ChartDashboardItem chart = designer.Dashboard.Items[itemComponentName] as ChartDashboardItem;
                    ChartContext chartContext = e.GetChartContext(itemComponentName);
                    UpdateChart(chart, chartContext);
                }
            }
        }
        void Designer_DashboardItemControlUpdated(object sender, DashboardItemControlEventArgs e) {
            if(e.ChartControl != null)
                UpdateChart(designer.Dashboard.Items[e.DashboardItemName] as ChartDashboardItem, e.ChartContext);
        }
        void UpdateChart(ChartDashboardItem chartDashboardItem, ChartContext chartContext) {
            var moduleData = GetDataFromString(chartDashboardItem.CustomProperties.GetValue(PropertyName));
            var pane = chartDashboardItem.Panes.FirstOrDefault(x => x.Name == moduleData.PaneName);
            if(pane != null) {
                ChartSeries dashboardSeries = pane.Series.FirstOrDefault(s => s.PlotOnSecondaryAxis == moduleData.IsSecondaryAxis);
                if(dashboardSeries != null) {
                    Series chartSeries = chartContext.GetControlSeries(dashboardSeries).FirstOrDefault();
                    var chartAxis = (chartSeries.View as XYDiagramSeriesViewBase)?.AxisY;
                    if(chartAxis != null) {
                        ConstantLine line = new ConstantLine() { AxisValue = moduleData.Value };
                        chartAxis.ConstantLines.Clear();
                        chartAxis.ConstantLines.Add(line);
                        line.ShowInLegend = false;
                        line.Color = Color.Green;
                        line.LineStyle.Thickness = 2;
                        line.LineStyle.DashStyle = DashStyle.Dash;
                        line.Title.Text = "Value: " + moduleData.Value.ToString();
                        line.Title.TextColor = line.Color;
                    }
                }
            }
        }
        ConstantLineUserValueModuleData GetDataFromString(string customPropertyValue) {
            if(!string.IsNullOrEmpty(customPropertyValue)) {
                var array = customPropertyValue.Split('_');
                return new ConstantLineUserValueModuleData() {
                    PaneName = array[0],
                    IsSecondaryAxis = bool.Parse(array[1]),
                    Value = Convert.ToDouble(array[2])
                };
            }
            return new ConstantLineUserValueModuleData();
        }
    }
    public class ValueSelectorControl: XtraUserControl {
        public ConstantLineUserValueModuleData ConstantLineModuleData { get; }

        public ValueSelectorControl(List<string> paneNames) {
            DataLayoutControl dataLayoutControl = new DataLayoutControl();
            BindingSource source = new BindingSource();
            ConstantLineModuleData = new ConstantLineUserValueModuleData();
            source.DataSource = ConstantLineModuleData;
            dataLayoutControl.DataSource = source;
            dataLayoutControl.FieldRetrieving += (s, e) => {
                if(e.FieldName == nameof(ConstantLineModuleData.PaneName)) {
                    e.EditorType = typeof(LookUpEdit);
                    e.Handled = true;
                }
            };
            dataLayoutControl.FieldRetrieved += (s, e) => {
                if(e.FieldName == nameof(ConstantLineModuleData.PaneName))
                    InitRepositoryItem(e.RepositoryItem, paneNames);
            };
            dataLayoutControl.RetrieveFields();
            dataLayoutControl.Dock = DockStyle.Fill;
            Controls.Add(dataLayoutControl);
            Dock = DockStyle.Top;
        }
        void InitRepositoryItem<T>(RepositoryItem ri, List<T> list) {
            var lookUpEdit = ri as RepositoryItemLookUpEdit;
            lookUpEdit.TextEditStyle = TextEditStyles.DisableTextEditor;
            lookUpEdit.AllowNullInput = DefaultBoolean.False;
            lookUpEdit.DataSource = list;
        }
    }
}
