using iText.IO.Font;
using iText.IO.Font.Constants;
using iText.Kernel.Font;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlatsAPI.Settings
{
    public class DocumentSettings
    {
        public PdfFont PrimaryFont { get; } = PdfFontFactory.CreateFont(StandardFonts.HELVETICA, PdfEncodings.UTF8);
        public PdfFont SecondaryFont { get; } = PdfFontFactory.CreateFont(StandardFonts.HELVETICA_OBLIQUE, PdfEncodings.UTF8);
    }
}
