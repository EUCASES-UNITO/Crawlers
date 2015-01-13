using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Unito.EUCases.Base.Helpers;

namespace Unito.EUCases.Workers
{
    public  class WorkerForm<W,P,R>:Form
        where W : IWorker<P, R>, new()
        where P : class, INotifyPropertyChanged, new()
        where R : class, INotifyPropertyChanged, new()
    {
        // TODO load of results
        // TODO on form close check for running worker and call cancel

        public WorkerForm(W worker)
        {
            InitializeComponent();

            var wType = typeof(W);
            var displayNameAttribute = wType.GetCustomAttribute<DisplayNameAttribute>();
            if (displayNameAttribute == null)
                this.Text = wType.Name;
            else
                this.Text = displayNameAttribute.DisplayName;


            workerUserControl.SetWorker(worker);
            
        }
       

        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.workerUserControl = new Unito.EUCases.Workers.WorkerUserControl<W,P,R>();
            this.SuspendLayout();
            // 
            // workerUserControl1
            // 
            this.workerUserControl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.workerUserControl.Location = new System.Drawing.Point(0, 0);
            this.workerUserControl.Name = "workerUserControl1";
            this.workerUserControl.Size = new System.Drawing.Size(479, 362);
            this.workerUserControl.TabIndex = 0;
            // 
            // WorkerForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(479, 362);
            this.Controls.Add(this.workerUserControl);
            this.Name = "WorkerForm";
            this.Text = "WorkerForm";
            this.ResumeLayout(false);

        }

        #endregion

        internal WorkerUserControl<W,P,R> workerUserControl;
    }
}
