using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DCPInfo {
    public partial class Form1 : Form {
        public Form1() {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e) {
            reset();
        }

        private void reset() {
            grp_cpl_uuid.Text = "";
            grp_cpl_annotationText.Text = "";
            grp_cpl_contentTitle.Text = "";
            grp_cpl_issueDate.Value = DateTime.MinValue;
            grp_cpl_facility.Text = "";
            grp_cpl_creator.Text = "";
            grp_cpl_issuer.Text = "";
            grp_cpl_contentType.Text = "";
            grp_cpl_contentVersion.Value = 0;
            grp_cpl_ratings.Items.Clear();
            grp_cpl_reels.Items.Clear();

            grp_rights_isEncrypted.Text = "";
            grp_rights_kdmState.Text = "No KDM loaded";
            grp_rights_kdmState.ForeColor = Color.Silver;
            grp_rights_uuid.Text = "";
            grp_rights_signer.Text = "";
            grp_rights_issueDate.Value = DateTime.MinValue;
            grp_rights_cplUuid.Text = "";
            grp_rights_contentTitle.Text = "";
            grp_rights_notValidBefore.Value = DateTime.MinValue;
            grp_rights_notValidAfter.Value = DateTime.MinValue;
            grp_rights_properties.Items.Clear();

            grp_assetmap_assets.Items.Clear();
        }
    }
}
