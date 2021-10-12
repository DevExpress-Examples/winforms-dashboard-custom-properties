Imports System
Imports System.Collections.Generic
Imports DevExpress.DashboardCommon
Imports DevExpress.DashboardWin
Imports DevExpress.Utils.Svg
Imports DevExpress.XtraBars
Imports DevExpress.XtraBars.Ribbon
Imports DevExpress.XtraCharts
Imports DevExpress.XtraReports.UI

Namespace WindowsFormsAppCustomProperties

    Public Class ChartScaleBreakModule

        Public Shared ReadOnly PropertyName As String = "ScaleBreak"

        Private ReadOnly designer As DashboardDesigner

        Private barItem As BarCheckItem

        Public Sub New(ByVal designer As DashboardDesigner, ByVal Optional barImage As SvgImage = Nothing)
            Me.designer = designer
            Dim ribbon As RibbonControl = CType(designer.MenuManager, RibbonControl)
            Dim page As RibbonPage = ribbon.GetDashboardRibbonPage(DashboardBarItemCategory.ChartTools, DashboardRibbonPage.Design)
            Dim group As RibbonPageGroup = page.GetGroupByName("Custom Properties")
            If group Is Nothing Then
                group = New RibbonPageGroup("Custom Properties") With {.Name = "Custom Properties"}
                page.Groups.Add(group)
            End If

            barItem = Me.CreateBarItem("Scale Break", barImage)
            group.ItemLinks.Add(barItem)
            Me.barItem.ItemClick += AddressOf ChangeCustomPropertyValue
            designer.DashboardItemControlUpdated += AddressOf Designer_DashboardItemControlUpdated
            designer.DashboardItemSelected += AddressOf Designer_DashboardItemSelected
            designer.DashboardCustomPropertyChanged += AddressOf Designer_DashboardCustomPropertyChanged
            designer.CustomExport += AddressOf Designer_CustomExport
        End Sub

        Private Function CreateBarItem(ByVal caption As String, ByVal barImage As SvgImage) As BarCheckItem
            Dim barItem As BarCheckItem = New BarCheckItem()
            barItem.Caption = caption
            barItem.ImageOptions.SvgImage = barImage
            Return barItem
        End Function

        Private Sub ChangeCustomPropertyValue(ByVal sender As Object, ByVal e As ItemClickEventArgs)
            Dim dashboardItem As DashboardItem = designer.SelectedDashboardItem
            Dim newValue As Boolean = Not Convert.ToBoolean(dashboardItem.CustomProperties.GetValue(PropertyName))
            Dim status As String = If(newValue, "enabled", "disabled")
            Dim historyItem As CustomPropertyHistoryItem = New CustomPropertyHistoryItem(dashboardItem, PropertyName, newValue.ToString(), $"Scale break for {dashboardItem.ComponentName} is {status}")
            designer.AddToHistory(historyItem)
        End Sub

        Private Sub Designer_DashboardCustomPropertyChanged(ByVal sender As Object, ByVal e As CustomPropertyChangedEventArgs)
            If e.Name Is PropertyName Then UpdateBarItem()
        End Sub

        Private Sub Designer_DashboardItemSelected(ByVal sender As Object, ByVal e As DashboardItemSelectedEventArgs)
            UpdateBarItem()
        End Sub

        Private Sub UpdateBarItem()
            If designer.SelectedDashboardItem IsNot Nothing Then barItem.Checked = Convert.ToBoolean(designer.SelectedDashboardItem.CustomProperties.GetValue(PropertyName))
        End Sub

        Private Sub Designer_CustomExport(ByVal sender As Object, ByVal e As CustomExportEventArgs)
            Dim controls As Dictionary(Of String, XRControl) = e.GetPrintableControls()
            For Each control In controls
                Dim xrChart As XRChart = TryCast(control.Value, XRChart)
                If xrChart IsNot Nothing Then
                    Dim itemComponentName As String = control.Key
                    UpdateChartScaleBreaks(designer.Dashboard.Items(itemComponentName), xrChart.Diagram)
                End If
            Next
        End Sub

        Private Sub Designer_DashboardItemControlUpdated(ByVal sender As Object, ByVal e As DashboardItemControlEventArgs)
            If e.ChartControl IsNot Nothing Then UpdateChartScaleBreaks(designer.Dashboard.Items(e.DashboardItemName), e.ChartControl.Diagram)
        End Sub

        Private Sub UpdateChartScaleBreaks(ByVal dashboardItem As DashboardItem, ByVal chartDiagram As Diagram)
            Dim diagram As XYDiagram = Nothing
            If CSharpImpl.__Assign(diagram, TryCast(chartDiagram, XYDiagram)) IsNot Nothing Then
                Dim scaleBreakEnabled As Boolean = Convert.ToBoolean(dashboardItem.CustomProperties.GetValue(PropertyName))
                diagram.SecondaryAxesY(0).AutoScaleBreaks.Enabled = scaleBreakEnabled
            End If
        End Sub

        Private Class CSharpImpl

            <Obsolete("Please refactor calling code to use normal Visual Basic assignment")>
            Shared Function __Assign(Of T)(ByRef target As T, value As T) As T
                target = value
                Return value
            End Function
        End Class
    End Class
End Namespace
