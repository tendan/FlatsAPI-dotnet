using iText.IO.Font;
using iText.IO.Font.Constants;
using iText.Kernel.Colors;
using iText.Kernel.Font;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlatsAPI.Settings
{
    public class DocumentSettings
    {
        private static string CurrentDirectory = Directory.GetCurrentDirectory();
        private static string LiberationSansFontPath = CurrentDirectory + @"\Static\Fonts\LiberationSans.ttf";
        public static PdfFont PrimaryFont { get; } = PdfFontFactory.CreateFont(LiberationSansFontPath, PdfEncodings.IDENTITY_H);
        public static PdfFont SecondaryFont { get; } = PdfFontFactory.CreateFont(LiberationSansFontPath, PdfEncodings.IDENTITY_H);
        public static Color TableHeaderCellColor { get; } = new DeviceRgb(220, 220, 220);
    }
}
