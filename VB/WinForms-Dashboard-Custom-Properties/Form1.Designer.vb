Namespace WinForms_Dashboard_Custom_Properties

    Partial Class Form1

        ''' <summary>
        ''' Required designer variable.
        ''' </summary>
        Private components As System.ComponentModel.IContainer = Nothing

        ''' <summary>
        ''' Clean up any resources being used.
        ''' </summary>
        ''' <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        Protected Overrides Sub Dispose(ByVal disposing As Boolean)
            If disposing AndAlso (Me.components IsNot Nothing) Then
                Me.components.Dispose()
            End If

            MyBase.Dispose(disposing)
        End Sub

#Region "Windows Form Designer generated code"
        ''' <summary>
        ''' Required method for Designer support - do not modify
        ''' the contents of this method with the code editor.
        ''' </summary>
        Private Sub InitializeComponent()
            Me.components = New System.ComponentModel.Container()
            Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(WinForms_Dashboard_Custom_Properties.Form1))
            Me.dashboardDesigner1 = New DevExpress.DashboardWin.DashboardDesigner()
            Me.svgImageCollection1 = New DevExpress.Utils.SvgImageCollection(Me.components)
            CType((Me.dashboardDesigner1), System.ComponentModel.ISupportInitialize).BeginInit()
            CType((Me.svgImageCollection1), System.ComponentModel.ISupportInitialize).BeginInit()
            Me.SuspendLayout()
            ' 
            ' dashboardDesigner1
            ' 
            Me.dashboardDesigner1.Appearance.BackColor = System.Drawing.Color.FromArgb((CInt(((CByte((240)))))), (CInt(((CByte((240)))))), (CInt(((CByte((240)))))))
            Me.dashboardDesigner1.Appearance.Options.UseBackColor = True
            Me.dashboardDesigner1.Dock = System.Windows.Forms.DockStyle.Fill
            Me.dashboardDesigner1.Location = New System.Drawing.Point(0, 0)
            Me.dashboardDesigner1.Name = "dashboardDesigner1"
            Me.dashboardDesigner1.Size = New System.Drawing.Size(1125, 659)
            Me.dashboardDesigner1.TabIndex = 0
            ' 
            ' svgImageCollection1
            ' 
            Me.svgImageCollection1.Add("alignverticalleft", "image://svgimages/align/alignverticalleft.svg")
            Me.svgImageCollection1.Add("changechartseriestype", "image://svgimages/dashboards/changechartseriestype.svg")
            Me.svgImageCollection1.Add("info", CType((resources.GetObject("svgImageCollection1.info")), DevExpress.Utils.Svg.SvgImage))
            Me.svgImageCollection1.Add("State_Validation_Information", CType((resources.GetObject("svgImageCollection1.State_Validation_Information")), DevExpress.Utils.Svg.SvgImage))
            Me.svgImageCollection1.Add("charttype_rangearea", "image://svgimages/chart/charttype_rangearea.svg")
            Me.svgImageCollection1.Add("Description", CType((resources.GetObject("svgImageCollection1.Description")), DevExpress.Utils.Svg.SvgImage))
            ' 
            ' Form1
            ' 
            Me.AutoScaleDimensions = New System.Drawing.SizeF(6F, 13F)
            Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
            Me.ClientSize = New System.Drawing.Size(1125, 659)
            Me.Controls.Add(Me.dashboardDesigner1)
            Me.Name = "Form1"
            Me.Text = "Custom Properties"
            CType((Me.dashboardDesigner1), System.ComponentModel.ISupportInitialize).EndInit()
            CType((Me.svgImageCollection1), System.ComponentModel.ISupportInitialize).EndInit()
            Me.ResumeLayout(False)
        End Sub

#End Region
        Private dashboardDesigner1 As DevExpress.DashboardWin.DashboardDesigner

        Private svgImageCollection1 As DevExpress.Utils.SvgImageCollection
    End Class
End Namespace
