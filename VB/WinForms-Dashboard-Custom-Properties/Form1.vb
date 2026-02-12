Imports System.Windows.Forms
Imports LayoutChange.Modules
Imports WindowsFormsAppCustomProperties

Namespace WinForms_Dashboard_Custom_Properties

    Public Partial Class Form1
        Inherits Form

        Public Sub New()
            InitializeComponent()
            dashboardDesigner1.CreateRibbon()
            Dim tmp_GridFixedColumnModule = New GridFixedColumnModule(dashboardDesigner1, svgImageCollection1("alignverticalleft"))
            Dim tmp_ChartScaleBreakModule = New ChartScaleBreakModule(dashboardDesigner1, svgImageCollection1("changechartseriestype"))
            Dim tmp_ConstantLineUserValueModule = New ConstantLineUserValueModule(dashboardDesigner1, svgImageCollection1("charttype_rangearea"))
            Dim tmp_DashboardDescriptionModule = New DashboardDescriptionModule(dashboardDesigner1, svgImageCollection1("State_Validation_Information"), svgImageCollection1("Description"))
            dashboardDesigner1.LoadDashboard("../../Dashboard/Dashboard.xml")
        End Sub
    End Class
End Namespace
