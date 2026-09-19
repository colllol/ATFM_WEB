using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Text;

namespace prjApplication
{
    public partial class Capcha : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            string[] fonts = { "Arial", "Lucida Sans Unicode" };
            const byte LENGTH = 5;
            // chuỗi để lấy các kí tự sẽ sử dụng cho captcha
            const string chars = "0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz";
            using (Bitmap bmp = new Bitmap(120, 30))
            {
                using (Graphics g = Graphics.FromImage(bmp))
                {
                    // Tạo nền cho ảnh dạng sóng
                    HatchBrush brush = new HatchBrush(HatchStyle.Wave, Color.White, Color.Aqua);

                    g.FillRegion(brush, g.Clip);
                    // Lưu chuỗi captcha trong quá trình tạo
                    StringBuilder strCaptcha = new StringBuilder();
                    Random rand = new Random();
                    for (int i = 0; i < LENGTH; i++)
                    {
                        // Lấy kí tự ngẫu nhiên từ mảng chars
                        string str = chars[rand.Next(chars.Length)].ToString();
                        strCaptcha.Append(str);

                        // Tạo font với tên font ngẫu nhiên chọn từ mảng fonts
                        Font font = new Font(fonts[rand.Next(fonts.Length)], 13, FontStyle.Bold | FontStyle.Italic);

                        // Lấy kích thước của kí tự
                        SizeF size = g.MeasureString(str, font);

                        // Vẽ kí tự đó ra ảnh tại vị trí tăng dần theo i, vị trí top ngẫu nhiên
                        g.DrawString(str, font,
                        Brushes.Chocolate, i * size.Width + 3, rand.Next(2, 10));
                        font.Dispose();
                    }
                    // Lưu captcha vào session
                    Session["captcha"] = strCaptcha.ToString();

                    // Ghi ảnh trực tiếp ra luồng xuất theo định dạng gif
                    Response.ContentType = "image/GIF";
                    bmp.Save(Response.OutputStream, System.Drawing.Imaging.ImageFormat.Gif);
                }
            }
        }
    }
}
