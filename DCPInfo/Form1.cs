using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using DCPInfo.Controls;
using DCPInfo.Models;
using DCPInfo.Util;
using DCPUtils.Enum;
using DCPUtils.Models;
using DCPUtils.Models.KDM;
using DCPUtils.Utils;
using Newtonsoft.Json;
using Ookii.Dialogs.WinForms;

namespace DCPInfo {
    public partial class Form1 : Form {
        private DCP _loadedDCP;
        private KDM _loadedKDM;

        public Form1() {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e) {
            reset();

            updateRecents();
        }

        private void RecentItem_Click(object sender, EventArgs e) {
            var item = (RecentDCPToolStripItem)sender;

            if (item != null) {
                reset();

                _loadedDCP = DCP.Read(item.Data.DCPFolder);

                RecentDCPManager.MoveToEnd(item.Data);

                if (_loadedDCP != null) {
                    updateUI();

                    if (!string.IsNullOrEmpty(item.Data.KDMFile)) {
                        string kdmPath = item.Data.KDMFile;

                        if (_loadedDCP.FindKDM(kdmPath)) {
                            _loadedKDM = KDM.Read(kdmPath);

                            if (_loadedDCP != null) {
                                updateKDM();
                            }
                            else {
                                reset();
                                throw new Exception("Unable to load KDM");
                            }

                            updateRecents();
                        }
                        else {
                            MessageBox.Show("The KDM tied to this entry is not for the DCP you currently have loaded.", "DCPInfo", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                }
                else {
                    reset();
                    throw new Exception("Unable to load DCP");
                }
            }
        }

        #region UI functions
        private void reset() {
            _loadedDCP = null;
            _loadedKDM = null;

            grp_cpl_uuid.Text = "";
            grp_cpl_annotationText.Text = "";
            grp_cpl_contentTitle.Text = "";
            grp_cpl_issueDate.Value = DateTime.Now;
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
            grp_rights_issueDate.Value = DateTime.Now;
            grp_rights_cplUuid.Text = "";
            grp_rights_contentTitle.Text = "";
            grp_rights_notValidBefore.Value = DateTime.Now;
            grp_rights_notValidAfter.Value = DateTime.Now;
            grp_rights_properties.Items.Clear();

            grp_assetmap_assets.Items.Clear();
            grp_subtitles_ccList.Items.Clear();

            openKDMToolStripMenuItem.Enabled = false;
            closeToolStripMenuItem.Enabled = false;

            updateTitle();
        }

        private void updateUI() {
            grp_cpl_uuid.Text = _loadedDCP.CompositionPlaylist.UUID.ToString();
            grp_cpl_annotationText.Text = _loadedDCP.CompositionPlaylist.AnnotationText;
            grp_cpl_contentTitle.Text = _loadedDCP.CompositionPlaylist.ContentTitleText;
            grp_cpl_issueDate.Value = _loadedDCP.CompositionPlaylist.IssueDate;
            grp_cpl_facility.Text = _loadedDCP.CompositionPlaylist.ReelList.First().Metadata.Facility;
            grp_cpl_creator.Text = _loadedDCP.CompositionPlaylist.Creator;
            grp_cpl_issuer.Text = _loadedDCP.CompositionPlaylist.Issuer;
            grp_cpl_contentType.Text = getContentKindDisplayText(_loadedDCP.CompositionPlaylist.ContentKind);
            grp_cpl_contentVersion.Value = _loadedDCP.CompositionPlaylist.ContentVersion.Version;

            foreach (var rating in _loadedDCP.CompositionPlaylist.RatingList) {
                var item = new ListViewItem(RatingUtils.ToFriendlyName(rating.Agency));

                item.SubItems.Add(rating.Label);

                grp_cpl_ratings.Items.Add(item);
            }

            foreach (var reel in _loadedDCP.CompositionPlaylist.ReelList) {
                grp_cpl_reels.Items.Add(new ReelListViewItem(reel));

                if(reel.ClosedCaption != null) {
                    var uuid = reel.ClosedCaption.UUID;
                    var asset = _loadedDCP.GetAssetByUUID(uuid);

                    if(asset != null) {
                        foreach (var chunk in asset.Chunks) {
                            grp_subtitles_ccList.Items.Add(new AssetListViewItem(chunk, uuid));
                        }
                    }
                }
            }

            grp_rights_isEncrypted.Text = _loadedDCP.IsEncrypted ? "Yes" : "No";

            if (_loadedDCP.IsEncrypted) {
                grp_rights_kdmState.Text = "Requires KDM";
                grp_rights_kdmState.ForeColor = Color.Red;
                openKDMToolStripMenuItem.Enabled = true;
            }

            if(_loadedKDM != null) {
                updateKDM();
            }

            foreach (var asset in _loadedDCP.Assets) {
                foreach (var chunk in asset.Chunks) {
                    grp_assetmap_assets.Items.Add(new AssetListViewItem(chunk, asset.UUID));
                }
            }

            closeToolStripMenuItem.Enabled = true;

            updateTitle();
        }

        private string getContentKindDisplayText(EContentKind contentKind) {
            return contentKind.ToString();
        }

        private void updateKDM() {
            grp_rights_isEncrypted.Text = _loadedDCP.IsEncrypted ? "Yes" : "No";

            if (_loadedKDM != null) {
                var mainExt = _loadedKDM.AuthenticatedPublic.RequiredExtensions.First();

                if (!_loadedKDM.AuthenticatedPublic.RequiredExtensions.All(item => DateTime.UtcNow >= item.ContentKeysNotValidBefore && DateTime.UtcNow <= item.ContentKeysNotValidAfter)) {
                    grp_rights_kdmState.Text = "KDM expired";
                    grp_rights_kdmState.ForeColor = Color.Red;
                }
                else {
                    grp_rights_kdmState.Text = "KDM loaded";
                    grp_rights_kdmState.ForeColor = Color.Green;

                    grp_rights_isEncrypted.Text = "Yes (Unlocked)";
                }

                grp_rights_uuid.Text = _loadedKDM.AuthenticatedPublic.MessageId.ToString();
                grp_rights_signer.Text = _loadedKDM.AuthenticatedPublic.Signer.CommonName;
                grp_rights_issueDate.Value = _loadedKDM.AuthenticatedPublic.IssueDate;
                grp_rights_cplUuid.Text = mainExt.CompositionPlaylistId.ToString();
                grp_rights_contentTitle.Text = mainExt.ContentTitleText;
                grp_rights_notValidBefore.Value = mainExt.ContentKeysNotValidBefore;
                grp_rights_notValidAfter.Value = mainExt.ContentKeysNotValidAfter;

                grp_rights_properties.Items.Add(ListUtils.ToListViewItem(new string[] { "Signer Serial", _loadedKDM.AuthenticatedPublic.Signer.SerialNumber }));
                grp_rights_properties.Items.Add(ListUtils.ToListViewItem(new string[] { "Private Keys", _loadedKDM.AuthenticatedPrivate.Count.ToString() }));
            }
            else {
                grp_rights_kdmState.Text = "No KDM loaded";
                grp_rights_kdmState.ForeColor = Color.Silver;
            }

            updateTitle();
        }

        private void updateTitle() {
            if(_loadedDCP != null) {
                if(_loadedKDM != null) {
                    this.Text = $"DCPInfo - {_loadedDCP.Name} [Using KDM]";
                }
                else {
                    // no KDM
                    if(_loadedDCP.IsEncrypted) {
                        // requires KDM
                        this.Text = $"DCPInfo - {_loadedDCP.Name} [KDM required but not loaded]";
                    }
                    else {
                        // doesnt require KDM
                        this.Text = $"DCPInfo - {_loadedDCP.Name}";
                    }
                }
            }
            else {
                // no loaded DCP
                this.Text = "DCPInfo";
            }
        }

        private void updateRecents() {
            recentsToolStripMenuItem.DropDownItems.Clear();

            var list = RecentDCPManager.Load();
            list.Reverse();

            foreach (var item in list) {
                if (!File.Exists(item.KDMFile)) {
                    item.KDMFile = null;
                    RecentDCPManager.Update(item);
                }

                if (Directory.Exists(item.DCPFolder)) {
                    var entry = new RecentDCPToolStripItem(item);

                    recentsToolStripMenuItem.DropDownItems.Add(entry);
                    entry.Click += RecentItem_Click;
                }
                else {
                    RecentDCPManager.Remove(item);
                }
            }
        }
        #endregion

        private void exitToolStripMenuItem_Click(object sender, EventArgs e) {
            Application.Exit();
        }

        private void openDCPToolStripMenuItem_Click(object sender, EventArgs e) {
            reset();

            try {
                using (var dlg = new VistaFolderBrowserDialog()) {
                    dlg.Description = "Select a DCP to load";
                    dlg.UseDescriptionForTitle = true;

                    if (dlg.ShowDialog() == DialogResult.OK) {
                        string dcpPath = dlg.SelectedPath;

                        if (Directory.Exists(dcpPath)) {
                            _loadedDCP = DCP.Read(dcpPath);

                            if (_loadedDCP != null) {
                                updateUI();

                                RecentDCPManager.Add(new RecentDCP() { DCPFolder = dcpPath });
                                updateRecents();
                            }
                            else {
                                reset();
                                throw new Exception("Unable to load DCP");
                            }
                        }
                        else {
                            throw new DirectoryNotFoundException("Unable to find specified DCP path");
                        }
                    }
                }
            }
            catch (Exception ex) { 
                ExceptionHandler.Handle(ex);
            }
        }

        private void openKDMToolStripMenuItem_Click(object sender, EventArgs e) {
            try {
                using (var dlg = new VistaOpenFileDialog()) {
                    dlg.Title = "Select a KDM to load";
                    dlg.Filter = "Key Delivery Message|*.xml";

                    if (dlg.ShowDialog() == DialogResult.OK) {
                        string kdmPath = dlg.FileName;

                        if (File.Exists(kdmPath)) {
                            if(_loadedDCP.FindKDM(kdmPath)) {
                                _loadedKDM = KDM.Read(kdmPath);

                                if (_loadedDCP != null) {
                                    updateKDM();

                                    var fetch = RecentDCPManager.Get(_loadedDCP.DcpRoot);

                                    if (fetch != null) {
                                        fetch.KDMFile = kdmPath;
                                        RecentDCPManager.Update(fetch);
                                    }
                                }
                                else {
                                    reset();
                                    throw new Exception("Unable to load KDM");
                                }
                            }
                            else {
                                MessageBox.Show("The KDM you selected is not for the DCP you currently have loaded.", "DCPInfo", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            }
                        }
                        else {
                            throw new DirectoryNotFoundException("Unable to find specified KDM path");
                        }
                    }
                }
            }
            catch (Exception ex) {
                ExceptionHandler.Handle(ex);
            }
        }

        private void closeToolStripMenuItem_Click(object sender, EventArgs e) {
            reset();
        }
    }
}
