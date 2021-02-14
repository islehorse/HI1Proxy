using System;
using System.Drawing;
using System.Windows.Forms;

namespace Hi1Proxy
{
    public partial class MapForm : Form
    {

        public MapForm()
        {
            InitializeComponent();
        }
        
        public void OpenBitmap()
        {
            if (draw_map.BackgroundImage != null)
            {
                draw_map.BackgroundImage.Dispose();
            }
            try
            {
                Bitmap TotalMap = new Bitmap("MapData.bmp");
                draw_map.BackgroundImage = TotalMap;
            }
            catch(Exception)
            {

            }
        }
        private void MapForm_Load(object sender, EventArgs e)
        {
            try
            {
                OpenBitmap();
            }
            catch(Exception)
            {

            }
           
        }

        private void draw_map_Click(object sender, EventArgs e)
        {

        }
    }
}
