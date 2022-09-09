using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CMControls.Layer
{
    public class LayerForm:Form
    {
        private Form _onLayerForm;

        public LayerForm(Form LayeredForm,Form onLayerForm)
        {
            ControlBox = false;
            FormBorderStyle = FormBorderStyle.FixedSingle;
            //StartPosition = FormStartPosition.CenterParent;
            StartPosition = FormStartPosition.Manual;
            
            BackColor = Color.Black;
            Opacity = 0.5;

            ShowInTaskbar = false;

            Location = LayeredForm.Location;
            Size = LayeredForm.Size;

            _onLayerForm = onLayerForm;

            //Load += LayerForm_Load; // Load中处理不显示onLayerForm —— _onLayerForm.ShowDialog()
            Shown += LayerForm_Shown;
        }

        private void LayerForm_Shown(object sender, EventArgs e)
        {
            _onLayerForm.ShowInTaskbar = false;
            _onLayerForm.StartPosition = FormStartPosition.CenterParent;
            _onLayerForm.FormClosed += OnLayerForm_FormClosed;
            _onLayerForm.ShowDialog();
        }

        private void OnLayerForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            this.Close();
        }
    }
}
