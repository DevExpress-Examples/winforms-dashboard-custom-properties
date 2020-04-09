Imports Microsoft.VisualBasic
Imports System.Windows.Forms
Imports LayoutChange.Modules
Imports WindowsFormsAppCustomProperties

Namespace WinForms_Dashboard_Custom_Properties
	Partial Public Class Form1
		Inherits Form
		Public Sub New()
			InitializeComponent()
			dashboardDesigner1.CreateRibbon()
			Dim TempGridFixedColumnModule As GridFixedColumnModule = New GridFixedColumnModule(dashboardDesigner1, svgImageCollection1("alignverticalleft"))
			Dim TempChartScaleBreakModule As ChartScaleBreakModule = New ChartScaleBreakModule(dashboardDesigner1, svgImageCollection1("changechartseriestype"))
			Dim TempConstantLineUserValueModule As ConstantLineUserValueModule = New ConstantLineUserValueModule(dashboardDesigner1, svgImageCollection1("charttype_rangearea"))
			Dim TempDashboardDescriptionModule As DashboardDescriptionModule = New DashboardDescriptionModule(dashboardDesigner1, svgImageCollection1("State_Validation_Information"), svgImageCollection1("Description"))
			dashboardDesigner1.LoadDashboard("../../Dashboard/Dashboard.xml")
		End Sub
	End Class
End Namespace
