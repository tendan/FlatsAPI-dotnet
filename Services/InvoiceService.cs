using FlatsAPI.Entities;
using iText.Kernel.Geom;
using iText.Kernel.Pdf;
using iText.Layout;
using iText.Layout.Properties;
using iText.Layout.Element;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace FlatsAPI.Services
{
    public interface IInvoiceService
    {
        byte[] GetInvoiceForSpecifiedAccount(int accountId);
    }
    public class InvoiceService : IInvoiceService
    {
        private readonly FlatsDbContext _dbContext;

        public InvoiceService(FlatsDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public byte[] GetInvoiceForSpecifiedAccount(int accountId)
        {
            throw new NotImplementedException();
        }
        private void GeneratePdf(int accountId)
        {
            var stream = new MemoryStream();
            var writer = new PdfWriter(stream);
            var pdf = new PdfDocument(writer);
            var document = new Document(pdf);
        }
        private Table GenerateRentsTable()
        {
            var table = new Table(4, false);

            table.SetHorizontalAlignment(HorizontalAlignment.RIGHT);
            table.SetWidth(UnitValue.CreatePercentValue(50));

            var tableHeader1 = new Cell(1, 1)
                .SetTextAlignment(TextAlignment.LEFT)
                .Add(new Paragraph("Stawka VAT"));

            var tableHeader2 = new Cell(1, 1)
                .SetTextAlignment(TextAlignment.LEFT)
                .Add(new Paragraph("Netto"));

            var tableHeader3 = new Cell(1, 1)
                .SetTextAlignment(TextAlignment.LEFT)
                .Add(new Paragraph("VAT"));

            var tableHeader4 = new Cell(1, 1)
                .SetTextAlignment(TextAlignment.LEFT)
                .Add(new Paragraph("Brutto"));

            return table;
        }
    }
}
