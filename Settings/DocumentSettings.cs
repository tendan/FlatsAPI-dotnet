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
        public static readonly string CurrentDirectory = Directory.GetCurrentDirectory();
        public static readonly string LiberationSansFontPath = CurrentDirectory + @"\Static\Fonts\LiberationSans.ttf";
        public static readonly Color TableHeaderCellColor = new DeviceRgb(220, 220, 220);
    }
}
