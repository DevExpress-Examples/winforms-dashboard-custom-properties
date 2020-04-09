using DevExpress.DashboardCommon;
using DevExpress.DashboardWin;
using DevExpress.Utils.Svg;
using DevExpress.XtraBars;
using DevExpress.XtraBars.Ribbon;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DevExpress.XtraGrid;
using DevExpress.XtraGrid.Views.Grid;
using DevExpress.XtraGrid.Views.Base;
using WindowsFormsAppCustomProperties;

namespace LayoutChange {
    class GridSearchPanelModule {
        public static readonly string PropertyName = "SearchPanel";
        readonly DashboardDesigner designer;
        BarCheckItem barItem;
        readonly RibbonControl ribbon;

        public GridSearchPanelModule(DashboardDesigner designer, SvgImage barImage = null) {
            this.designer = designer;
            ribbon = (RibbonControl)designer.MenuManager;
            AddBarItem(PropertyName, barImage);
            SubscribeEvents();
        }

        void AddBarItem(string caption, SvgImage barImage) {
            var page = ribbon.GetDashboardRibbonPage(DashboardBarItemCategory.GridTools, DashboardRibbonPage.Design);
            RibbonPageGroup group = page.GetGroupByName("Custom Properties");
            if(group == null) {
                group = new RibbonPageGroup("Custom Properties") { Name = "Custom Properties" };
                page.Groups.Add(group);
            }
            barItem = new BarCheckItem(ribbon.Manager, false);
            barItem.Caption = caption;
            if(barImage != null)
                barItem.ImageOptions.SvgImage = barImage;
            group.ItemLinks.Add(barItem);
        }

        void SubscribeEvents() {
            barItem.ItemClick += DoAction;
            designer.DashboardItemControlUpdated += Designer_DashboardItemControlUpdated;
        }

        void Designer_DashboardItemControlUpdated(object sender, DashboardItemControlEventArgs e) {
            if(e.GridControl != null) {
                var gridItem = designer.Dashboard.Items[e.DashboardItemName];
                if(gridItem.CustomProperties.GetValue(PropertyName) != null) {
                    bool isVisible = gridItem.CustomProperties.GetValue<bool>(PropertyName);
                    var view = e.GridControl.MainView as GridView;
                    if(isVisible)
                        view.ShowFindPanel();
                    else
                        view.HideFindPanel();
                }
            }
        }



        void DoAction(object sender, ItemClickEventArgs e) {
            var gridItem = designer.SelectedDashboardItem as GridDashboardItem;
            if(gridItem != null) {
                bool newValuebool = !gridItem.CustomProperties.GetValue<bool>(PropertyName);
                designer.AddToHistory(new CustomPropertyHistoryItem(gridItem, PropertyName, newValuebool.ToString(), $"Find Panel visible changed"));
            }

        }
    }
}

