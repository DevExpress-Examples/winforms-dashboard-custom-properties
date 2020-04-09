using System;
using System.Windows.Forms;
using DevExpress.DashboardCommon;
using DevExpress.DashboardWin;
using DevExpress.DashboardWin.Localization;
using DevExpress.Utils;
using DevExpress.Utils.Svg;
using DevExpress.XtraBars;
using DevExpress.XtraBars.Ribbon;
using DevExpress.XtraEditors;
using DevExpress.XtraLayout;
using DevExpress.XtraLayout.Utils;

namespace LayoutChange.Modules {
    public class DashboardDescriptionModule {
        public static readonly string PropertyName = "DashboardDescription";
        readonly DashboardDesigner designer;
        readonly SvgImage titleDescriptionImage;

        public DashboardDescriptionModule(DashboardDesigner designer, SvgImage barImage = null, SvgImage titleDescriptionImage = null) {
            this.designer = designer;
            this.titleDescriptionImage = titleDescriptionImage;
            RibbonControl ribbon = (RibbonControl)designer.MenuManager;
            RibbonPage page = ribbon.GetDashboardRibbonPage(DashboardBarItemCategory.None, DashboardRibbonPage.Home);
            RibbonPageGroup group = page.Groups.GetGroupByText(DashboardWinLocalizer.GetString(DashboardWinStringId.RibbonPageDashboardCaption));
            BarButtonItem barItem = CreateBarItem("Dashboard Description", barImage);
            group.ItemLinks.Add(barItem);
            barItem.ItemClick += DescriptionBarItem_Click;
            designer.CustomizeDashboardTitle += Designer_CustomizeDashboardTitle;
            designer.DashboardCustomPropertyChanged += Designer_DashboardCustomPropertyChanged;
        }
        BarButtonItem CreateBarItem(string caption, SvgImage barImage) {
            BarButtonItem barItem = new BarButtonItem();
            barItem.Caption = caption;
            barItem.ImageOptions.SvgImage = barImage;
            return barItem;
        }
        void DescriptionBarItem_Click(object sender, EventArgs e) {
            ShowDescriptionForm();
        }
        void ShowDescriptionForm() {
            string currentCaption = designer.Dashboard.CustomProperties.GetValue(PropertyName);
            LayoutControl layoutControl = new LayoutControl();
            layoutControl.Dock = DockStyle.Fill;
            layoutControl.Name = "layoutControl1";
            LayoutControlItem memoEditLayoutItem = layoutControl.Root.AddItem();
            memoEditLayoutItem.TextLocation = Locations.Top;
            memoEditLayoutItem.Text = "Text:";
            MemoEdit memoEdit = new MemoEdit();
            memoEdit.Text = currentCaption;
            memoEditLayoutItem.Control = memoEdit;
            LayoutControlItem buttonLayoutItem = layoutControl.Root.AddItem();
            buttonLayoutItem.SizeConstraintsType = SizeConstraintsType.Custom;
            buttonLayoutItem.TextVisible = false;
            SimpleButton buttonOk = new SimpleButton();
            buttonOk.Text = "OK";
            buttonOk.DialogResult = DialogResult.OK;
            buttonOk.Click += (sender1, e1) => {
                string modifiedCaption = memoEdit.Text;
                if (currentCaption != modifiedCaption) {
                    string status = !string.IsNullOrEmpty(modifiedCaption) ? "Remove" : "Add";
                    CustomPropertyHistoryItem historyItem = new CustomPropertyHistoryItem(designer.Dashboard, PropertyName, modifiedCaption, status + " the dashboard caption");
                    designer.AddToHistory(historyItem);
                }
            };
            buttonLayoutItem.Control = buttonOk;
            layoutControl.Root.AddItem(new EmptySpaceItem(), buttonLayoutItem, InsertType.Left);
            using(XtraForm form = new XtraForm()) {
                form.Text = "Description";
                form.Size = new System.Drawing.Size(250, 250);
                form.StartPosition = FormStartPosition.CenterParent;
                form.IconOptions.ShowIcon = false;
                form.Controls.Add(layoutControl);
                form.ShowDialog();
            }
        }
        void Designer_CustomizeDashboardTitle(object sender, CustomizeDashboardTitleEventArgs e) {
            string text = designer.Dashboard.CustomProperties.GetValue(PropertyName);
            if(!string.IsNullOrEmpty(text)) {
                DashboardToolbarItem showDataItem = new DashboardToolbarItem("Description",
                    new Action<DashboardToolbarItemClickEventArgs>((args) => {
                        MessageBox.Show(text, "Description");
                    }));
                showDataItem.SvgImage = titleDescriptionImage;
                e.Items.Insert(0, showDataItem);
            }
        }
        void Designer_DashboardCustomPropertyChanged(object sender, CustomPropertyChangedEventArgs e) {
            if(e.Name == PropertyName)
                designer.UpdateDashboardTitle();
        }
    }
}

