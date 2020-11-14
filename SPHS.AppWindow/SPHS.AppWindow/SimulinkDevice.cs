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
    public partial class SimulinkDevice : Form
    {
        private List<companies> _companies = new List<companies>();
        private List<devices> _devices = new List<devices>();
        public SimulinkDevice()
        {
            InitializeComponent();
            getToken();
            getCompanies();
        }

        private void getToken()
        {
            var user = UserAPI.login(Parameter_Special.ACCOUNT_DEFAULT, Parameter_Special.PASSWORD_DEFAULT);
            if (user.accessToken != null)
            {
                Parameter_Special.USER_PRESENT = user;
            }
        }

        private void getCompanies()
        {
            if (!string.IsNullOrEmpty(Parameter_Special.USER_PRESENT.accessToken))
            {
                
                var companies = Utils.getAPI(COLLECTIONS.companies, null);
                foreach (var company in companies)
                {
                    _companies.Add((companies)company);
                }
                cbCompanies.DataSource = _companies;
                cbCompanies.DisplayMember = "name";
                cbCompanies.SelectedIndex = 0;
                getDevices();
            }
        }

        private void getDevices()
        {
            int index = cbCompanies.SelectedIndex;
            if(index >= 0)
            {
                _devices = new List<devices>();
                var devices = Utils.getAPI(COLLECTIONS.devices, $"companyId={_companies[index]._id}");
                foreach (var device in devices)
                {
                    _devices.Add((devices)device);
                }
                if(_devices.Count > 0)
                {
                    cbDevices.DataSource = _devices;
                    cbDevices.DisplayMember = "name";
                    cbDevices.SelectedIndex = 0;
                }
            }
        }

        private void cbCompanies_SelectedIndexChanged(object sender, EventArgs e)
        {
            getDevices();
        }
    }
}
