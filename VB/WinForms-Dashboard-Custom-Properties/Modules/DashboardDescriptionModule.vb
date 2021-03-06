﻿Imports System
Imports System.Windows.Forms
Imports DevExpress.DashboardCommon
Imports DevExpress.DashboardWin
Imports DevExpress.DashboardWin.Localization
Imports DevExpress.Utils
Imports DevExpress.Utils.Svg
Imports DevExpress.XtraBars
Imports DevExpress.XtraBars.Ribbon
Imports DevExpress.XtraEditors
Imports DevExpress.XtraLayout
Imports DevExpress.XtraLayout.Utils

Namespace LayoutChange.Modules
	Public Class DashboardDescriptionModule
		Public Shared ReadOnly PropertyName As String = "DashboardDescription"
		Private ReadOnly designer As DashboardDesigner
		Private ReadOnly titleDescriptionImage As SvgImage

		Public Sub New(ByVal designer As DashboardDesigner, Optional ByVal barImage As SvgImage = Nothing, Optional ByVal titleDescriptionImage As SvgImage = Nothing)
			Me.designer = designer
			Me.titleDescriptionImage = titleDescriptionImage
			Dim ribbon As RibbonControl = CType(designer.MenuManager, RibbonControl)
			Dim page As RibbonPage = ribbon.GetDashboardRibbonPage(DashboardBarItemCategory.None, DashboardRibbonPage.Home)
			Dim group As RibbonPageGroup = page.Groups.GetGroupByText(DashboardWinLocalizer.GetString(DashboardWinStringId.RibbonPageDashboardCaption))
			Dim barItem As BarButtonItem = CreateBarItem("Dashboard Description", barImage)
			group.ItemLinks.Add(barItem)
			AddHandler barItem.ItemClick, AddressOf DescriptionBarItem_Click
			AddHandler designer.CustomizeDashboardTitle, AddressOf Designer_CustomizeDashboardTitle
			AddHandler designer.DashboardCustomPropertyChanged, AddressOf Designer_DashboardCustomPropertyChanged
		End Sub
		Private Function CreateBarItem(ByVal caption As String, ByVal barImage As SvgImage) As BarButtonItem
			Dim barItem As New BarButtonItem()
			barItem.Caption = caption
			barItem.ImageOptions.SvgImage = barImage
			Return barItem
		End Function
		Private Sub DescriptionBarItem_Click(ByVal sender As Object, ByVal e As EventArgs)
			ShowDescriptionForm()
		End Sub
		Private Sub ShowDescriptionForm()
			Dim currentCaption As String = designer.Dashboard.CustomProperties.GetValue(PropertyName)
			Dim layoutControl As New LayoutControl()
			layoutControl.Dock = DockStyle.Fill
			layoutControl.Name = "layoutControl1"
			Dim memoEditLayoutItem As LayoutControlItem = layoutControl.Root.AddItem()
			memoEditLayoutItem.TextLocation = Locations.Top
			memoEditLayoutItem.Text = "Text:"
			Dim memoEdit As New MemoEdit()
			memoEdit.Text = currentCaption
			memoEditLayoutItem.Control = memoEdit
			Dim buttonLayoutItem As LayoutControlItem = layoutControl.Root.AddItem()
			buttonLayoutItem.SizeConstraintsType = SizeConstraintsType.Custom
			buttonLayoutItem.TextVisible = False
			Dim buttonOk As New SimpleButton()
			buttonOk.Text = "OK"
			buttonOk.DialogResult = DialogResult.OK
			AddHandler buttonOk.Click, Sub(sender1, e1)
				Dim modifiedCaption As String = memoEdit.Text
				If currentCaption <> modifiedCaption Then
					Dim status As String = If(Not String.IsNullOrEmpty(modifiedCaption), "Remove", "Add")
					Dim historyItem As New CustomPropertyHistoryItem(designer.Dashboard, PropertyName, modifiedCaption, status & " the dashboard caption")
					designer.AddToHistory(historyItem)
				End If
			End Sub
			buttonLayoutItem.Control = buttonOk
			layoutControl.Root.AddItem(New EmptySpaceItem(), buttonLayoutItem, InsertType.Left)
			Using form As New XtraForm()
				form.Text = "Description"
				form.Size = New System.Drawing.Size(250, 250)
				form.StartPosition = FormStartPosition.CenterParent
				form.IconOptions.ShowIcon = False
				form.Controls.Add(layoutControl)
				form.ShowDialog()
			End Using
		End Sub
		Private Sub Designer_CustomizeDashboardTitle(ByVal sender As Object, ByVal e As CustomizeDashboardTitleEventArgs)
			Dim text As String = designer.Dashboard.CustomProperties.GetValue(PropertyName)
			If Not String.IsNullOrEmpty(text) Then
				Dim showDataItem As New DashboardToolbarItem("Description", New Action(Of DashboardToolbarItemClickEventArgs)(Sub(args)
					MessageBox.Show(text, "Description")
				End Sub))
				showDataItem.SvgImage = titleDescriptionImage
				e.Items.Insert(0, showDataItem)
			End If
		End Sub
		Private Sub Designer_DashboardCustomPropertyChanged(ByVal sender As Object, ByVal e As CustomPropertyChangedEventArgs)
			If e.Name = PropertyName Then
				designer.UpdateDashboardTitle()
			End If
		End Sub
	End Class
End Namespace

