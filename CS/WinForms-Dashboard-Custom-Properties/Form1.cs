using System.Windows.Forms;
using LayoutChange.Modules;
using WindowsFormsAppCustomProperties;

namespace WinForms_Dashboard_Custom_Properties {
    public partial class Form1: Form {
        public Form1() {
            InitializeComponent();
            dashboardDesigner1.CreateRibbon();
            new GridFixedColumnModule(dashboardDesigner1, svgImageCollection1["alignverticalleft"]);
            new ChartScaleBreakModule(dashboardDesigner1, svgImageCollection1["changechartseriestype"]);
            new ConstantLineUserValueModule(dashboardDesigner1, svgImageCollection1["charttype_rangearea"]);
            new DashboardDescriptionModule(dashboardDesigner1, svgImageCollection1["State_Validation_Information"], svgImageCollection1["Description"]);
            dashboardDesigner1.LoadDashboard("../../Dashboard/Dashboard.xml");
        }
    }
}
