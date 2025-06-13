using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DefenderRemove
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (!IsAdministrator())
            {
                MessageBox.Show("Esta accion requiere de permisos de administrador", "Permisos Insuficientes", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                // disable Windows Defender
                RegistryKey key = Registry.LocalMachine.CreateSubKey(@"SOFTWARE\Policies\Microsoft\Windows Defender");
                key.SetValue("DisableAntiSpyware",
                    1, RegistryValueKind.DWord);
                key.Close();

                RegistryKey realTime = Registry.LocalMachine.CreateSubKey(@"SOFTWARE\Policies\Microsoft\Windows Defender\Real-Time Protection");

                realTime.SetValue("DisableRealtimeMonitoring", 1, RegistryValueKind.DWord);
                realTime.Close();

                MessageBox.Show("Windows Defender ha sido desactivado correctamente.\nReinicia para aplicar todos los cambios.", "Disable Success.", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al modificar el registro: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private bool IsAdministrator()
        {
            WindowsIdentity identity = WindowsIdentity.GetCurrent();
            WindowsPrincipal principal = new WindowsPrincipal(identity);
            return principal.IsInRole(WindowsBuiltInRole.Administrator);
        }
    }
}
