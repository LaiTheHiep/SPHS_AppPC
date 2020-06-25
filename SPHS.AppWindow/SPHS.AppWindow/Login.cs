using SPHS.AppWindow.actions;
using SPHS.AppWindow.models;
using SPHS.AppWindow.parameters;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SPHS.AppWindow
{
    public partial class Login : Form
    {
        public Login()
        {
            InitializeComponent();
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            var confim = MessageBox.Show("Do you want exit?", "Confirm", MessageBoxButtons.YesNo);
            if (confim == DialogResult.Yes)
                this.Close();
        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
            try
            {
                users user = UserAPI.login(txtAccount.Text, txtPassword.Text);
                if (user.accessToken != null && user.role != ROLES.user.ToString() && user.role != ROLES.manager.ToString())
                {
                    Parameter_Special.USER_PRESENT = user;
                    MainApp app = new MainApp();
                    app.FormClosed += App_FormClosed;
                    app.Show();
                    txtPassword.Text = null;
                    this.Hide();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void App_FormClosed(object sender, FormClosedEventArgs e)
        {
            Parameter_Special.USER_PRESENT = new users();
            this.Show();
        }
    }
}
