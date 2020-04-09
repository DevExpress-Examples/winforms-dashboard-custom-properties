using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using DevExpress.DashboardCommon;
using DevExpress.DashboardWin;
using DevExpress.Utils.Svg;
using DevExpress.XtraBars;
using DevExpress.XtraBars.Ribbon;
using DevExpress.XtraEditors;
using DevExpress.XtraGrid.Columns;

namespace WindowsFormsAppCustomProperties {
    public class GridFixedColumnModule {
        public static readonly string PropertyName = "FixedColumns";
        readonly DashboardDesigner designer;

        public GridFixedColumnModule(DashboardDesigner designer, SvgImage barImage = null) {
            this.designer = designer;
            RibbonControl ribbon = (RibbonControl)designer.MenuManager;
            RibbonPage page = ribbon.GetDashboardRibbonPage(DashboardBarItemCategory.GridTools, DashboardRibbonPage.Design);
            RibbonPageGroup group = page.GetGroupByName("Custom Properties");
            if(group == null) {
                group = new RibbonPageGroup("Custom Properties") { Name = "Custom Properties" };
                page.Groups.Add(group);
            }
            BarButtonItem barItem = CreateBarItem("Fix Columns", barImage);
            group.ItemLinks.Add(barItem);
            barItem.ItemClick += ChangeCustomPropertyValue;
            designer.DashboardItemControlUpdated += Designer_DashboardItemControlUpdated;
        }
        BarButtonItem CreateBarItem(string caption, SvgImage barImage) {
            BarButtonItem barItem = new BarButtonItem();
            barItem.Caption = caption;
            barItem.ImageOptions.SvgImage = barImage;
            return barItem;
        }
        void Designer_DashboardItemControlUpdated(object sender, DashboardItemControlEventArgs e) {
            if(e.GridControl != null) {
                GridDashboardItem gridItem = designer.Dashboard.Items[e.DashboardItemName] as GridDashboardItem;
                gridItem.GridOptions.ColumnWidthMode = GridColumnWidthMode.AutoFitToContents;
                foreach(GridColumnBase itemColumn in gridItem.Columns) {
                    string customProperty = itemColumn.CustomProperties.GetValue(PropertyName);
                    if(!string.IsNullOrEmpty(customProperty)) {
                        GridColumn gridColumn = e.GridContext.GetControlColumn(itemColumn);
                        if(gridColumn != null) {
                            bool fixedWidthEnabled = Convert.ToBoolean(itemColumn.CustomProperties.GetValue(PropertyName));
                            gridColumn.Fixed = fixedWidthEnabled ? FixedStyle.Left : FixedStyle.None;
                        }
                    }
                }
            }
        }
        void ChangeCustomPropertyValue(object sender, ItemClickEventArgs e) {
            GridDashboardItem gridItem = designer.SelectedDashboardItem as GridDashboardItem;
            List<CheckedComboBoxItem> checkedColumnsList = gridItem.Columns.Select(x => new CheckedComboBoxItem() {
                Column = x,
                Checked = Convert.ToBoolean(x.CustomProperties.GetValue(PropertyName))
            }).ToList();
            ColumnSelectorControl control = new ColumnSelectorControl(checkedColumnsList);
            if(XtraDialog.Show(control, "Select columns to fix:") == DialogResult.OK) {
                foreach(var item in checkedColumnsList)
                    if(Convert.ToBoolean(item.Column.CustomProperties.GetValue(PropertyName)) != item.Checked) {
                        string status = item.Checked == true ? "Pin" : "Unpin";
                        CustomPropertyHistoryItem historyItem = new CustomPropertyHistoryItem(item.Column, PropertyName, item.Checked.ToString(), $"{status} the {item} column");
                        designer.AddToHistory(historyItem);
                    }
            }
        }
    }
    public class CheckedComboBoxItem {
        public GridColumnBase Column { get; set; }
        public bool Checked { get; set; }

        public override string ToString() {
            return Column.GetDisplayName();
        }
    }
    public class ColumnSelectorControl: XtraUserControl {
        CheckedListBoxControl checkedCombo = new CheckedListBoxControl();

        public ColumnSelectorControl(List<CheckedComboBoxItem> gridColumns) {
            foreach(CheckedComboBoxItem col in gridColumns) {
                checkedCombo.Items.Add(col, col.Checked);
            }
            checkedCombo.ItemCheck += CheckedCombo_ItemCheck;
            checkedCombo.Dock = DockStyle.Fill;
            Controls.Add(checkedCombo);
            Dock = DockStyle.Top;
        }
        void CheckedCombo_ItemCheck(object sender, DevExpress.XtraEditors.Controls.ItemCheckEventArgs e) {
            CheckedListBoxControl checkList = sender as CheckedListBoxControl;
            CheckedComboBoxItem item = checkList.Items[e.Index].Value as CheckedComboBoxItem;
            item.Checked = checkList.Items[e.Index].CheckState == CheckState.Checked;
        }
    }
}

