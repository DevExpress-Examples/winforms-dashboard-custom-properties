Imports Microsoft.VisualBasic
Imports System
Imports System.Collections.Generic
Imports System.Drawing
Imports System.Linq
Imports System.Windows.Forms
Imports DevExpress.DashboardCommon
Imports DevExpress.DashboardWin
Imports DevExpress.Utils
Imports DevExpress.Utils.Svg
Imports DevExpress.XtraBars
Imports DevExpress.XtraBars.Ribbon
Imports DevExpress.XtraCharts
Imports DevExpress.XtraDataLayout
Imports DevExpress.XtraEditors
Imports DevExpress.XtraEditors.Controls
Imports DevExpress.XtraEditors.Repository
Imports DevExpress.XtraReports.UI

Namespace WindowsFormsAppCustomProperties
	Public Class ConstantLineUserValueModuleData
		Private privatePaneName As String
		Public Property PaneName() As String
			Get
				Return privatePaneName
			End Get
			Set(ByVal value As String)
				privatePaneName = value
			End Set
		End Property
		Private privateIsSecondaryAxis As Boolean
		Public Property IsSecondaryAxis() As Boolean
			Get
				Return privateIsSecondaryAxis
			End Get
			Set(ByVal value As Boolean)
				privateIsSecondaryAxis = value
			End Set
		End Property
		Private privateValue As Double
		Public Property Value() As Double
			Get
				Return privateValue
			End Get
			Set(ByVal value As Double)
				privateValue = value
			End Set
		End Property

		Public Function GetStringFromData() As String
			Return PaneName & "_" & IsSecondaryAxis.ToString() & "_" & Value
		End Function
	End Class
	Public Class ConstantLineUserValueModule
		Public Shared ReadOnly PropertyName As String = "ConstantLine"
		Private ReadOnly designer As DashboardDesigner

		Public Sub New(ByVal designer As DashboardDesigner, Optional ByVal barImage As SvgImage = Nothing)
			Me.designer = designer
			Dim barItem As BarButtonItem = AddBarItem("Constant line", barImage, designer.Ribbon)
			AddHandler barItem.ItemClick, AddressOf BarItem_Click
			AddHandler designer.DashboardItemControlUpdated, AddressOf Designer_DashboardItemControlUpdated
			AddHandler designer.CustomExport, AddressOf Designer_CustomExport
		End Sub
		Private Function AddBarItem(ByVal caption As String, ByVal barImage As SvgImage, ByVal ribbon As RibbonControl) As BarButtonItem
			Dim page = ribbon.GetDashboardRibbonPage(DashboardBarItemCategory.ChartTools, DashboardRibbonPage.Design)
			Dim group As RibbonPageGroup = page.GetGroupByName("Custom Properties")
			If group Is Nothing Then
				group = New RibbonPageGroup("Custom Properties") With {.Name = "Custom Properties"}
				page.Groups.Add(group)
			End If
			Dim barItem = New BarButtonItem(ribbon.Manager, caption)
			barItem.ImageOptions.SvgImage = barImage
			group.ItemLinks.Add(barItem)
			Return barItem
		End Function
		Private Sub BarItem_Click(ByVal sender As Object, ByVal e As ItemClickEventArgs)
			Dim chartItem = TryCast(designer.SelectedDashboardItem, ChartDashboardItem)
			If chartItem IsNot Nothing Then
				Dim data = GetConstantLineModuleData(chartItem)
				If data IsNot Nothing Then
					designer.AddToHistory(New CustomPropertyHistoryItem(chartItem, PropertyName, data.GetStringFromData(), $"{data.PaneName} constant line is set"))
				End If
			End If
		End Sub
		Private Function GetConstantLineModuleData(ByVal dashboardItem As ChartDashboardItem) As ConstantLineUserValueModuleData
			Using selector As New ValueSelectorControl(dashboardItem.Panes.Select(Function(p) p.Name).ToList())
				If XtraDialog.Show(selector, "Select the required series") = DialogResult.OK Then
					Return selector.ConstantLineModuleData
				End If
			End Using
			Return Nothing
		End Function
		Private Sub Designer_CustomExport(ByVal sender As Object, ByVal e As CustomExportEventArgs)
			Dim controls As Dictionary(Of String, XRControl) = e.GetPrintableControls()
			For Each control In controls
				Dim xrChart As XRChart = TryCast(control.Value, XRChart)
				If xrChart IsNot Nothing Then
					Dim itemComponentName As String = control.Key
					Dim chart As ChartDashboardItem = TryCast(designer.Dashboard.Items(itemComponentName), ChartDashboardItem)
					Dim chartContext As ChartContext = e.GetChartContext(itemComponentName)
					UpdateChart(chart, chartContext)
				End If
			Next control
		End Sub
		Private Sub Designer_DashboardItemControlUpdated(ByVal sender As Object, ByVal e As DashboardItemControlEventArgs)
			If e.ChartControl IsNot Nothing Then
				UpdateChart(TryCast(designer.Dashboard.Items(e.DashboardItemName), ChartDashboardItem), e.ChartContext)
			End If
		End Sub
		Private Sub UpdateChart(ByVal chartDashboardItem As ChartDashboardItem, ByVal chartContext As ChartContext)
			Dim moduleData = GetDataFromString(chartDashboardItem.CustomProperties.GetValue(PropertyName))
			Dim pane = chartDashboardItem.Panes.FirstOrDefault(Function(x) x.Name = moduleData.PaneName)
			If pane IsNot Nothing Then
				Dim dashboardSeries As ChartSeries = pane.Series.FirstOrDefault(Function(s) s.PlotOnSecondaryAxis = moduleData.IsSecondaryAxis)
				If dashboardSeries IsNot Nothing Then
					Dim chartSeries As Series = chartContext.GetControlSeries(dashboardSeries).FirstOrDefault()
					Dim chartAxis = (TryCast(chartSeries.View, XYDiagramSeriesViewBase))?.AxisY
					If chartAxis IsNot Nothing Then
						Dim line As New ConstantLine() With {.AxisValue = moduleData.Value}
						chartAxis.ConstantLines.Clear()
						chartAxis.ConstantLines.Add(line)
						line.ShowInLegend = False
						line.Color = Color.Green
						line.LineStyle.Thickness = 2
						line.LineStyle.DashStyle = DashStyle.Dash
						line.Title.Text = "Value: " & moduleData.Value.ToString()
						line.Title.TextColor = line.Color
					End If
				End If
			End If
		End Sub
		Private Function GetDataFromString(ByVal customPropertyValue As String) As ConstantLineUserValueModuleData
			If (Not String.IsNullOrEmpty(customPropertyValue)) Then
				Dim array = customPropertyValue.Split("_"c)
				Return New ConstantLineUserValueModuleData() With {.PaneName = array(0), .IsSecondaryAxis = Boolean.Parse(array(1)), .Value = Convert.ToDouble(array(2))}
			End If
			Return New ConstantLineUserValueModuleData()
		End Function
	End Class
	Public Class ValueSelectorControl
		Inherits XtraUserControl
		Private privateConstantLineModuleData As ConstantLineUserValueModuleData
		Public Property ConstantLineModuleData() As ConstantLineUserValueModuleData
			Get
				Return privateConstantLineModuleData
			End Get
			Set(value As ConstantLineUserValueModuleData)
			End Set
		End Property

		Public Sub New(ByVal paneNames As List(Of String))
			Dim dataLayoutControl As New DataLayoutControl()
			Dim source As New BindingSource()
			ConstantLineModuleData = New ConstantLineUserValueModuleData()
			source.DataSource = ConstantLineModuleData
			dataLayoutControl.DataSource = source
			AddHandler dataLayoutControl.FieldRetrieving, Function(s, e) AnonymousMethod1(s, e)
			AddHandler dataLayoutControl.FieldRetrieved, Function(s, e) AnonymousMethod2(s, e, paneNames)
			dataLayoutControl.RetrieveFields()
			dataLayoutControl.Dock = DockStyle.Fill
			Controls.Add(dataLayoutControl)
			Dock = DockStyle.Top
		End Sub
		
		Private Function AnonymousMethod1(ByVal s As Object, ByVal e As Object) As Boolean
			If e.FieldName Is NameOf(ConstantLineModuleData.PaneName) Then
				e.EditorType = GetType(LookUpEdit)
				e.Handled = True
			End If
			Return True
		End Function
		
		Private Function AnonymousMethod2(ByVal s As Object, ByVal e As Object, ByVal paneNames As List(Of String)) As Boolean
			If e.FieldName Is NameOf(ConstantLineModuleData.PaneName) Then
				InitRepositoryItem(e.RepositoryItem, paneNames)
			End If
			Return True
		End Function
		Private Sub InitRepositoryItem(Of T)(ByVal ri As RepositoryItem, ByVal list As List(Of T))
			Dim lookUpEdit = TryCast(ri, RepositoryItemLookUpEdit)
			lookUpEdit.TextEditStyle = TextEditStyles.DisableTextEditor
			lookUpEdit.AllowNullInput = DefaultBoolean.False
			lookUpEdit.DataSource = list
		End Sub
	End Class
End Namespace
