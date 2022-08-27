using iText.Kernel.Colors;
using System.IO;

namespace FlatsAPI.Settings;

public class DocumentSettings
{
    public static readonly string CurrentDirectory = Directory.GetCurrentDirectory();
    public static readonly string LiberationSansFontPath = CurrentDirectory + @"\Static\Fonts\LiberationSans.ttf";
    public static readonly Color TableHeaderCellColor = new DeviceRgb(220, 220, 220);
}
