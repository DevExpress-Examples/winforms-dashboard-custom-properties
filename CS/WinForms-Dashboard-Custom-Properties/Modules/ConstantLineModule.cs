using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using DevExpress.DashboardCommon;
using DevExpress.DashboardCommon.ViewerData;
using DevExpress.DashboardWin;
using DevExpress.Utils.Svg;
using DevExpress.XtraBars;
using DevExpress.XtraBars.Ribbon;
using DevExpress.XtraCharts;
using DevExpress.XtraDataLayout;
using DevExpress.XtraEditors;
using DevExpress.XtraEditors.Controls;
using DevExpress.XtraEditors.Repository;

namespace MyTestSample {
    public class ConstantLineModuleData {
        public string SeriesName { get; set; }
        public string PaneName { get; set; }
        public bool IsSecondaryAxis { get; set; }
        public string MeasureId { get; set; }
        private static char divider = '_';
        public static ConstantLineModuleData GetData(string customPropertyValue) {
            ConstantLineModuleData constantLine = new ConstantLineModuleData();
            if(!string.IsNullOrEmpty(customPropertyValue)) {
                var array = customPropertyValue.Split(divider);
                constantLine.SeriesName = array[0];
                constantLine.PaneName = array[1];
                constantLine.IsSecondaryAxis = bool.Parse(array[2]);
                constantLine.MeasureId = array[3];
            }
            return constantLine;
        }
        public override string ToString() {
            return SeriesName + divider + PaneName + divider + IsSecondaryAxis.ToString() + divider + MeasureId;
        }
    }
    public class ConstantLineModule {
        public static readonly string PropertyName = "ConstantLine";
        readonly DashboardDesigner designer;
        readonly RibbonControl ribbon;

        public ConstantLineModule(DashboardDesigner designer, SvgImage barImage = null) {
            this.designer = designer;
            ribbon = designer.Ribbon;
            var barItem = AddBarItem(PropertyName, barImage);
            barItem.ItemClick += DoAction;
            SubscribeEvents();
        }
        BarButtonItem AddBarItem(string caption, SvgImage barImage) {
            var page = ribbon.GetDashboardRibbonPage(DashboardBarItemCategory.ChartTools, DashboardRibbonPage.Design);
            RibbonPageGroup group = page.GetGroupByName("Custom Properties");
            if(group == null) {
                group = new RibbonPageGroup("Custom Properties") { Name = "Custom Properties" };
                page.Groups.Add(group);
            }
            var barItem = new BarButtonItem(ribbon.Manager, "Select Measure");
            barItem.Caption = caption;
            if(barImage != null)
                barItem.ImageOptions.SvgImage = barImage;
            group.ItemLinks.Add(barItem);
            return barItem;
        }
        void SubscribeEvents() {
            designer.DashboardItemControlUpdated += Designer_DashboardItemControlUpdated;
        }
        void DoAction(object sender, ItemClickEventArgs e) {
            var chartItem = designer.SelectedDashboardItem as ChartDashboardItem;
            if(chartItem != null) {
                var data = GetConstantLineModuleData(chartItem);
                if(data != null) {
                    designer.AddToHistory(new CustomPropertyHistoryItem(chartItem, PropertyName, data.ToString(), $"{data.PaneName} {data.SeriesName} constant line is set"));
                }
            }
        }
        ConstantLineModuleData GetConstantLineModuleData(ChartDashboardItem dashboardItem) {
            ConstantLineModuleData data = null;
            var chartSeriesNames = designer.GetItemData(dashboardItem.ComponentName).GetAxisPoints("Series").Select(x => x.DisplayText).ToList();
            var measures = dashboardItem.GetMeasures();
            measures.AddRange(dashboardItem.HiddenMeasures);
            MeasureSelectorControl selector = new MeasureSelectorControl(chartSeriesNames, dashboardItem.Panes.Select(p => p.Name).ToList(), measures);
            if(XtraDialog.Show(selector, "Select the required series") == DialogResult.OK) {
                data = selector.ConstantLineModuleData;
            }
            selector.Dispose();
            return data;
        }
        void Designer_DashboardItemControlUpdated(object sender, DashboardItemControlEventArgs e) {
            if(e.ChartControl != null) {
                UpdateChart(designer.Dashboard.Items[e.DashboardItemName] as ChartDashboardItem, e.ChartContext);
            }
        }
        void UpdateChart(ChartDashboardItem chartDashboardItem, ChartContext chartContext) {
            var moduleData = ConstantLineModuleData.GetData(chartDashboardItem.CustomProperties.GetValue(PropertyName));
            var pane = chartDashboardItem.Panes.FirstOrDefault(x => x.Name == moduleData.PaneName);
            if(pane != null) {
                var constLineValue = GetValueByModuleData(designer.GetItemData(chartDashboardItem.ComponentName), moduleData);
                ChartSeries dashboardSeries = pane.Series.FirstOrDefault(s => s.PlotOnSecondaryAxis == moduleData.IsSecondaryAxis);

                Series chartSeries = chartContext.GetControlSeries(dashboardSeries).FirstOrDefault();
                var chartAxis = GetChartAxix(chartSeries);
                if(chartAxis != null) {
                    ConstantLine line = new ConstantLine() { AxisValue = constLineValue };
                    chartAxis.ConstantLines.Clear();
                    chartAxis.ConstantLines.Add(line);
                }
            }
        }
        Axis GetChartAxix(Series chartSeries) {
            Axis axis = null;
            var view = chartSeries.View as XYDiagramSeriesViewBase;
            if(view != null)
                axis = view.AxisY;
            return axis;
        }
        object GetValueByModuleData(MultiDimensionalData data, ConstantLineModuleData moduleData) {
            var axisPoint = data.GetAxisPointByUniqueValues(DashboardDataAxisNames.ChartSeriesAxis, new[] { moduleData.SeriesName });
            var dataSlice = data.GetSlice(axisPoint);
            var measureDescriptor = data.GetMeasures().FirstOrDefault(x => x.ID == moduleData.MeasureId);
            var mdata = dataSlice.GetValue(measureDescriptor).Value;
            return mdata;
        }
    }
    public class MeasureSelectorControl : XtraUserControl {
        public MeasureSelectorControl(List<string> chartControlSeriesNames, List<string> paneNames, List<Measure> measures) {
            DataLayoutControl dataLayoutControl = new DataLayoutControl();
            BindingSource source = new BindingSource();
            ConstantLineModuleData = new ConstantLineModuleData();
            source.DataSource = ConstantLineModuleData;
            dataLayoutControl.DataSource = source;
            dataLayoutControl.FieldRetrieving += (s, e) => {
                if(e.FieldName != nameof(ConstantLineModuleData.IsSecondaryAxis)) {
                    e.EditorType = typeof(LookUpEdit);
                    e.Handled = true;
                }
            };
            dataLayoutControl.FieldRetrieved += (s, e) => {
                if(e.FieldName == nameof(ConstantLineModuleData.SeriesName))
                    InitRepositoryItem(e.RepositoryItem, chartControlSeriesNames);
                if(e.FieldName == nameof(ConstantLineModuleData.PaneName))
                    InitRepositoryItem(e.RepositoryItem, paneNames);
                if(e.FieldName == nameof(ConstantLineModuleData.MeasureId))
                    InitMeasuresRepositoryItem(e.RepositoryItem, measures);
            };

            dataLayoutControl.RetrieveFields();
            dataLayoutControl.Dock = DockStyle.Fill;
            this.Controls.Add(dataLayoutControl);
            this.Dock = DockStyle.Top;
        }
        void InitRepositoryItem<T>(RepositoryItem ri, List<T> list) {
            var lookUpEdit = ri as RepositoryItemLookUpEdit;
            lookUpEdit.TextEditStyle = TextEditStyles.DisableTextEditor;
            lookUpEdit.AllowNullInput = DevExpress.Utils.DefaultBoolean.False;
            lookUpEdit.DataSource = list;
        }
        void InitMeasuresRepositoryItem(RepositoryItem ri, List<Measure> list) {
            var measures = list.Select(x => new MeasureHelper(x)).ToList();
            var lookUpEdit = ri as RepositoryItemLookUpEdit;
            lookUpEdit.ValueMember = "ID";
            lookUpEdit.DisplayMember = "Name";
            lookUpEdit.Columns.Add(new LookUpColumnInfo("Name"));
            InitRepositoryItem(ri, measures);
        }
        public class MeasureHelper {
            public string ID { get; set; }
            public string Name { get; set; }
            public MeasureHelper(Measure measure) {
                ID = measure.UniqueId;
                Name = measure.ToString();
            }
        }
        public ConstantLineModuleData ConstantLineModuleData { get; }
    }
}
