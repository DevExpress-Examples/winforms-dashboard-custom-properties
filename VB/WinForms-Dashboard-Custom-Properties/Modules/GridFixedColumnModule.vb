Imports System
Imports System.Collections.Generic
Imports System.Linq
Imports System.Windows.Forms
Imports DevExpress.DashboardCommon
Imports DevExpress.DashboardWin
Imports DevExpress.Utils.Svg
Imports DevExpress.XtraBars
Imports DevExpress.XtraBars.Ribbon
Imports DevExpress.XtraEditors
Imports DevExpress.XtraGrid.Columns

Namespace WindowsFormsAppCustomProperties
	Public Class GridFixedColumnModule
		Public Shared ReadOnly PropertyName As String = "FixedColumns"
		Private ReadOnly designer As DashboardDesigner

		Public Sub New(ByVal designer As DashboardDesigner, Optional ByVal barImage As SvgImage = Nothing)
			Me.designer = designer
			Dim ribbon As RibbonControl = CType(designer.MenuManager, RibbonControl)
			Dim page As RibbonPage = ribbon.GetDashboardRibbonPage(DashboardBarItemCategory.GridTools, DashboardRibbonPage.Design)
			Dim group As RibbonPageGroup = page.GetGroupByName("Custom Properties")
			If group Is Nothing Then
				group = New RibbonPageGroup("Custom Properties") With {.Name = "Custom Properties"}
				page.Groups.Add(group)
			End If
			Dim barItem As BarButtonItem = CreateBarItem("Fix Columns", barImage)
			group.ItemLinks.Add(barItem)
			AddHandler barItem.ItemClick, AddressOf ChangeCustomPropertyValue
			AddHandler designer.DashboardItemControlUpdated, AddressOf Designer_DashboardItemControlUpdated
		End Sub
		Private Function CreateBarItem(ByVal caption As String, ByVal barImage As SvgImage) As BarButtonItem
			Dim barItem As New BarButtonItem()
			barItem.Caption = caption
			barItem.ImageOptions.SvgImage = barImage
			Return barItem
		End Function
		Private Sub Designer_DashboardItemControlUpdated(ByVal sender As Object, ByVal e As DashboardItemControlEventArgs)
			If e.GridControl IsNot Nothing Then
				Dim gridItem As GridDashboardItem = TryCast(designer.Dashboard.Items(e.DashboardItemName), GridDashboardItem)
				gridItem.GridOptions.ColumnWidthMode = GridColumnWidthMode.AutoFitToContents
				For Each itemColumn As GridColumnBase In gridItem.Columns
					Dim customProperty As String = itemColumn.CustomProperties.GetValue(PropertyName)
					If Not String.IsNullOrEmpty(customProperty) Then
						Dim gridColumn As GridColumn = e.GridContext.GetControlColumn(itemColumn)
						If gridColumn IsNot Nothing Then
							Dim fixedWidthEnabled As Boolean = Convert.ToBoolean(itemColumn.CustomProperties.GetValue(PropertyName))
							gridColumn.Fixed = If(fixedWidthEnabled, FixedStyle.Left, FixedStyle.None)
						End If
					End If
				Next itemColumn
			End If
		End Sub
		Private Sub ChangeCustomPropertyValue(ByVal sender As Object, ByVal e As ItemClickEventArgs)
			Dim gridItem As GridDashboardItem = TryCast(designer.SelectedDashboardItem, GridDashboardItem)
			Dim checkedColumnsList As List(Of CheckedComboBoxItem) = gridItem.Columns.Select(Function(x) New CheckedComboBoxItem() With {
				.Column = x,
				.Checked = Convert.ToBoolean(x.CustomProperties.GetValue(PropertyName))
			}).ToList()
			Dim control As New ColumnSelectorControl(checkedColumnsList)
			If XtraDialog.Show(control, "Select columns to fix:") = DialogResult.OK Then
				For Each item In checkedColumnsList
					If Convert.ToBoolean(item.Column.CustomProperties.GetValue(PropertyName)) <> item.Checked Then
						Dim status As String = If(item.Checked = True, "Pin", "Unpin")
						Dim historyItem As New CustomPropertyHistoryItem(item.Column, PropertyName, item.Checked.ToString(), $"{status} the {item} column")
						designer.AddToHistory(historyItem)
					End If
				Next item
			End If
		End Sub
	End Class
	Public Class CheckedComboBoxItem
		Public Property Column() As GridColumnBase
		Public Property Checked() As Boolean

		Public Overrides Function ToString() As String
			Return Column.GetDisplayName()
		End Function
	End Class
	Public Class ColumnSelectorControl
		Inherits XtraUserControl

		Private checkedCombo As New CheckedListBoxControl()

		Public Sub New(ByVal gridColumns As List(Of CheckedComboBoxItem))
			For Each col As CheckedComboBoxItem In gridColumns
				checkedCombo.Items.Add(col, col.Checked)
			Next col
			AddHandler checkedCombo.ItemCheck, AddressOf CheckedCombo_ItemCheck
			checkedCombo.Dock = DockStyle.Fill
			Controls.Add(checkedCombo)
			Dock = DockStyle.Top
		End Sub
		Private Sub CheckedCombo_ItemCheck(ByVal sender As Object, ByVal e As DevExpress.XtraEditors.Controls.ItemCheckEventArgs)
			Dim checkList As CheckedListBoxControl = TryCast(sender, CheckedListBoxControl)
			Dim item As CheckedComboBoxItem = TryCast(checkList.Items(e.Index).Value, CheckedComboBoxItem)
			item.Checked = checkList.Items(e.Index).CheckState = CheckState.Checked
		End Sub
	End Class
End Namespace

