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
using FlatsAPI.Settings;
using System.Globalization;

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
        private Table GenerateRentsTable(ICollection<Rent> rents)
        {
            var table = new Table(6, false);

            table.SetHorizontalAlignment(HorizontalAlignment.RIGHT);
            table.SetWidth(UnitValue.CreatePercentValue(50));

            var tableHeader1 = new Cell(1, 1)
                .SetTextAlignment(TextAlignment.CENTER)
                .Add(new Paragraph("in."));

            var tableHeader2 = new Cell(1, 1)
                .SetTextAlignment(TextAlignment.CENTER)
                .Add(new Paragraph("Service/product name"));

            var tableHeader3 = new Cell(1, 1)
                .SetTextAlignment(TextAlignment.CENTER)
                .Add(new Paragraph("Netto price"));
            
            var tableHeader4 = new Cell(1, 1)
                .SetTextAlignment(TextAlignment.CENTER)
                .Add(new Paragraph("VAT"));

            var tableHeader5 = new Cell(1, 1)
                .SetTextAlignment(TextAlignment.CENTER)
                .Add(new Paragraph("VAT rate"));
            
            var tableHeader6 = new Cell(1, 1)
                .SetTextAlignment(TextAlignment.CENTER)
                .Add(new Paragraph("Price"));

            table.AddCell(tableHeader1);
            table.AddCell(tableHeader2);
            table.AddCell(tableHeader3);
            table.AddCell(tableHeader4);
            table.AddCell(tableHeader5);
            table.AddCell(tableHeader6);

            var index = 1;

            foreach (var rent in rents)
            {
                // Index number add
                table.AddCell($"{index++}.");

                // Product/service name add
                var property = rent.PropertyType == PropertyTypes.BlockOfFlats ? "Block of Flats" : "Flat";

                if (rent.PropertyType == PropertyTypes.BlockOfFlats)
                {
                    var blockOfFlats = _dbContext.BlockOfFlats.FirstOrDefault(b => b.Id == rent.PropertyId);

                    var address = blockOfFlats.Address;

                    table.AddCell($"{property} on {address}");
                }
                else
                {
                    var flat = _dbContext.Flats.FirstOrDefault(f => f.Id == rent.PropertyId);

                    var address = flat.BlockOfFlats.Address;

                    var number = flat.Number;

                    table.AddCell($"{property} on {address} no. {number}");
                }

                // Netto price add
                var formattedNettoPrice = rent.Price.ToString("C", CultureInfo.CurrentCulture);
                table.AddCell(formattedNettoPrice);

                // VAT percentage add
                var percentage = (PaymentSettings.TAX - 1) * 100;
                var percentageCurrencyString = percentage.ToString("C");
                table.AddCell($"{percentageCurrencyString}%");

                // VAT rate add
                var rate = (rent.Price * percentage).ToString("C", CultureInfo.CurrentCulture);
                table.AddCell(rate);

                // Brutto price add
                var bruttoPrice = rent.PriceWithTax.ToString("C", CultureInfo.CurrentCulture);
                table.AddCell(bruttoPrice);
            }

            return table;
        }
    }
}
