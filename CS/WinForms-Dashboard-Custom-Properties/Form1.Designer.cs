namespace WinForms_Dashboard_Custom_Properties {
    partial class Form1 {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing) {
            if(disposing && (components != null)) {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent() {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            this.dashboardDesigner1 = new DevExpress.DashboardWin.DashboardDesigner();
            this.svgImageCollection1 = new DevExpress.Utils.SvgImageCollection(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.dashboardDesigner1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.svgImageCollection1)).BeginInit();
            this.SuspendLayout();
            // 
            // dashboardDesigner1
            // 
            this.dashboardDesigner1.Appearance.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(240)))), ((int)(((byte)(240)))), ((int)(((byte)(240)))));
            this.dashboardDesigner1.Appearance.Options.UseBackColor = true;
            this.dashboardDesigner1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dashboardDesigner1.Location = new System.Drawing.Point(0, 0);
            this.dashboardDesigner1.Name = "dashboardDesigner1";
            this.dashboardDesigner1.Size = new System.Drawing.Size(1125, 659);
            this.dashboardDesigner1.TabIndex = 0;
            // 
            // svgImageCollection1
            // 
            this.svgImageCollection1.Add("alignverticalleft", "image://svgimages/align/alignverticalleft.svg");
            this.svgImageCollection1.Add("changechartseriestype", "image://svgimages/dashboards/changechartseriestype.svg");
            this.svgImageCollection1.Add("info", ((DevExpress.Utils.Svg.SvgImage)(resources.GetObject("svgImageCollection1.info"))));
            this.svgImageCollection1.Add("State_Validation_Information", ((DevExpress.Utils.Svg.SvgImage)(resources.GetObject("svgImageCollection1.State_Validation_Information"))));
            this.svgImageCollection1.Add("charttype_rangearea", "image://svgimages/chart/charttype_rangearea.svg");
            this.svgImageCollection1.Add("Description", ((DevExpress.Utils.Svg.SvgImage)(resources.GetObject("svgImageCollection1.Description"))));
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1125, 659);
            this.Controls.Add(this.dashboardDesigner1);
            this.Name = "Form1";
            this.Text = "Custom Properties";
            ((System.ComponentModel.ISupportInitialize)(this.dashboardDesigner1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.svgImageCollection1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private DevExpress.DashboardWin.DashboardDesigner dashboardDesigner1;
        private DevExpress.Utils.SvgImageCollection svgImageCollection1;
    }
}

