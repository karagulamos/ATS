namespace ATS.Desktop.Service
{
    partial class ProjectInstaller
    {
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

        #region Component Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.AtsProcessInstaller = new System.ServiceProcess.ServiceProcessInstaller();
            this.AtsInstaller = new System.ServiceProcess.ServiceInstaller();
            // 
            // AtsProcessInstaller
            // 
            this.AtsProcessInstaller.Account = System.ServiceProcess.ServiceAccount.LocalService;
            this.AtsProcessInstaller.Password = null;
            this.AtsProcessInstaller.Username = null;
            // 
            // AtsInstaller
            // 
            this.AtsInstaller.Description = "Applicant Tracker that retrieve job applications from designated emails and proce" +
    "sses them.";
            this.AtsInstaller.DisplayName = "Applicant Tracking System by Netrifik";
            this.AtsInstaller.ServiceName = "ApplicantTrackingSystem";
            // 
            // ProjectInstaller
            // 
            this.Installers.AddRange(new System.Configuration.Install.Installer[] {
            this.AtsProcessInstaller,
            this.AtsInstaller});

        }

        #endregion

        private System.ServiceProcess.ServiceProcessInstaller AtsProcessInstaller;
        private System.ServiceProcess.ServiceInstaller AtsInstaller;
    }
}