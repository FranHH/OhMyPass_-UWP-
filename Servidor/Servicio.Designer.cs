
namespace Servidor
{
    partial class Servicio
    {
        /// <summary>
        /// Variable del diseñador necesaria.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Limpiar los recursos que se estén usando.
        /// </summary>
        /// <param name="disposing">true si los recursos administrados se deben desechar; false en caso contrario.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Código generado por el Diseñador de Windows Forms

        /// <summary>
        /// Método necesario para admitir el Diseñador. No se puede modificar
        /// el contenido de este método con el editor de código.
        /// </summary>
        private void InitializeComponent()
        {
            this.lb = new System.Windows.Forms.Label();
            this.panel1 = new System.Windows.Forms.Panel();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // lb
            // 
            this.lb.AutoSize = true;
            this.lb.Location = new System.Drawing.Point(337, 115);
            this.lb.Name = "lb";
            this.lb.Size = new System.Drawing.Size(51, 13);
            this.lb.TabIndex = 0;
            this.lb.Text = "PRUEBA";
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.lb);
            this.panel1.Location = new System.Drawing.Point(326, 156);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(727, 295);
            this.panel1.TabIndex = 1;
            // 
            // Servicio
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1421, 597);
            this.Controls.Add(this.panel1);
            this.Name = "Servicio";
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion
        private System.ComponentModel.BackgroundWorker backgroundWorker1;
        private GMap.NET.WindowsForms.GMapControl gmap;
        private System.Windows.Forms.Button btnBuscar;
        private System.Windows.Forms.Label lb;
        private System.Windows.Forms.Panel panel1;
    }
}

