using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ProjEncontraPlaca
{
    public partial class frmPrincipal : Form
    {
        private Image image;
        private Bitmap imageBitmap;
        private Otsu otsu;
        public frmPrincipal()
        {
            InitializeComponent();
            otsu = new Otsu();
        }

        private void btnAbrir_Click(object sender, EventArgs e)
        {
            openFileDialog.FileName = "";
            openFileDialog.Filter = "Arquivos de Imagem (*.jpg;*.png)|*.jpg;*.png";
            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                image = Image.FromFile(openFileDialog.FileName);
                pictBoxImg.Image = image;
                pictBoxImg.SizeMode = PictureBoxSizeMode.Normal;
            }
        }

        private void btnLimpar_Click(object sender, EventArgs e)
        {
            pictBoxImg.Image = null;
        }

        private void btnOTSU_Click(object sender, EventArgs e)
        {
            Bitmap temp = (Bitmap)image.Clone();
            otsu.Convert2GrayScaleFast(temp);
            int otsuThreshold = otsu.getOtsuThreshold((Bitmap)temp);
            otsu.threshold(temp, otsuThreshold);
            textBox1.Text = otsuThreshold.ToString();
            pictBoxImg.Image = temp;
        }

        private void btnSegmenta8_Click(object sender, EventArgs e)
        {
            imageBitmap = (Bitmap)image.Clone();
            Bitmap imgDest = (Bitmap)image.Clone();
            String placa = "";
            Filtros.encontra_placa(imageBitmap, ref imgDest, ref placa);

            pictBoxImg.Image = imgDest;
        }

        private void btnReconheDigito_Click(object sender, EventArgs e)
        {
            imageBitmap = (Bitmap)image.Clone();
            Bitmap imgDest = (Bitmap)image.Clone();
            String placa = "";
            Filtros.encontra_placa(imageBitmap, ref imgDest, ref placa);

            this.textBox1.Text = "Placa: " + placa;

        }

        private void pictBoxImg_Click(object sender, EventArgs e)
        {

        }
    }
}
