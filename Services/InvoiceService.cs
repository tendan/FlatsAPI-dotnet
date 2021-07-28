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
using FlatsAPI.Models;
using iText.Layout.Borders;

namespace FlatsAPI.Services
{
    public interface IInvoiceService
    {
        byte[] GetInvoiceForSpecifiedAccount(int accountId);
    }
    public class InvoiceService : IInvoiceService
    {
        private const string DateFormat = "yyyy-MM-dd";
        private readonly FlatsDbContext _dbContext;

        public InvoiceService(FlatsDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public byte[] GetInvoiceForSpecifiedAccount(int accountId)
        {
            throw new NotImplementedException();
        }
        private Document GeneratePdf(Account account)
        {
            var accountRents = account.Rents;

            using var stream = new MemoryStream();
            using var writer = new PdfWriter(stream);
            using var pdf = new PdfDocument(writer);
            using var document = new Document(pdf);

            var beginning = GenerateBeginning();
            var buyerSellerChapter = GenerateBuyerSellerChapter(account);
            var rentsTable = GenerateRentsTable(accountRents);

            var nettoAndVat = CalculateSummaryOfNettoAndVat(accountRents);

            var nettoSummary = nettoAndVat.NettoSummary;
            var vatSummary = nettoAndVat.VatSummary;

            var summaryTable = GenerateSummaryTable(nettoSummary, vatSummary);

            document
                .Add(beginning)
                .Add(buyerSellerChapter)
                .Add(rentsTable)
                .Add(summaryTable);

            return document;
        }
        private Div GenerateBeginning()
        {
            var header = new Div();

            var currentDate = DateTime.Now;

            var paragraphBase = new Paragraph()
                .SetHorizontalAlignment(HorizontalAlignment.RIGHT);

            var placeParagraph = paragraphBase.Add("Kielce");

            var invoiceDate = currentDate.ToString(DateFormat);

            var invoiceDateParagraph = paragraphBase.Add(invoiceDate);

            string dueDate = currentDate.AddDays(20).ToString(DateFormat);

            var dueDateParagraph = paragraphBase.Add(dueDate);

            header.Add(placeParagraph);
            header.Add(invoiceDateParagraph);
            header.Add(dueDateParagraph);

            return header;
        }
        private Table GenerateBuyerSellerChapter(Account buyer)
        {
            var table = new Table(2)
                .SetHorizontalAlignment(HorizontalAlignment.LEFT)
                .SetWidth(UnitValue.CreatePercentValue(66));

            var headerCellBase = new Cell(1, 1)
                .SetBorder(Border.NO_BORDER)
                .SetPadding(5);

            var sellerCell = headerCellBase.Add(new Paragraph("Seller"));
            var buyerCell = headerCellBase.Add(new Paragraph("Buyer"));

            table
                .AddHeaderCell(sellerCell)
                .AddHeaderCell(buyerCell);

            return table;
            
        }
        private Table GenerateRentsTable(ICollection<Rent> rents)
        {
            var table = new Table(6, false)
                .SetHorizontalAlignment(HorizontalAlignment.RIGHT)
                .SetWidth(UnitValue.CreatePercentValue(50));

            var tableHeaderBase = new Cell(1, 1)
                .SetTextAlignment(TextAlignment.CENTER);

            var tableHeader1 = tableHeaderBase.Add(new Paragraph("In."));

            var tableHeader2 = tableHeaderBase.Add(new Paragraph("Service/product name"));

            var tableHeader3 = tableHeaderBase.Add(new Paragraph("Netto price"));

            var tableHeader4 = tableHeaderBase.Add(new Paragraph("VAT"));

            var tableHeader5 = tableHeaderBase.Add(new Paragraph("VAT rate"));

            var tableHeader6 = tableHeaderBase.Add(new Paragraph("Price"));

            table
                .AddHeaderCell(tableHeader1)
                .AddHeaderCell(tableHeader2)
                .AddHeaderCell(tableHeader3)
                .AddHeaderCell(tableHeader4)
                .AddHeaderCell(tableHeader5)
                .AddHeaderCell(tableHeader6);

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
                var percentageCurrencyString = percentage.ToString();
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
        private Table GenerateSummaryTable(float nettoSummary, float vatSummary)
        {
            var table = new Table(4, false)
                .SetHorizontalAlignment(HorizontalAlignment.RIGHT)
                .SetWidth(UnitValue.CreatePercentValue(50));

            var tableHeaderBase = new Cell(1, 1)
                .SetTextAlignment(TextAlignment.CENTER);

            var tableHeader1 = tableHeaderBase.Add(new Paragraph("Stawka VAT"));

            var tableHeader2 = tableHeaderBase.Add(new Paragraph("Netto"));

            var tableHeader3 = tableHeaderBase.Add(new Paragraph("VAT"));

            var tableHeader4 = tableHeaderBase.Add(new Paragraph("Brutto"));

            table
                .AddHeaderCell(tableHeader1)
                .AddHeaderCell(tableHeader2)
                .AddHeaderCell(tableHeader3)
                .AddHeaderCell(tableHeader4);

            // VAT add
            var percentage = (PaymentSettings.TAX - 1) * 100;
            var percentageCurrencyString = percentage.ToString();
            table.AddCell($"{percentageCurrencyString}%");

            // Netto summary add
            var nettoSummaryCurrencyString = nettoSummary.ToString("C", CultureInfo.CurrentCulture);
            table.AddCell(nettoSummaryCurrencyString);

            // VAT summary add
            var vatSummaryCurrencyString = vatSummary.ToString("C", CultureInfo.CurrentCulture);
            table.AddCell(vatSummaryCurrencyString);

            // Brutto add
            var brutto = (nettoSummary + vatSummary).ToString("C", CultureInfo.CurrentCulture);
            table.AddCell(brutto);

            return table;
        }
        private NettoAndVat CalculateSummaryOfNettoAndVat(ICollection<Rent> rents)
        {
            var nettoSummary = 0f;
            var vatSummary = 0f;

            foreach (var rent in rents)
            {
                nettoSummary += rent.Price;
                vatSummary += rent.Price * (PaymentSettings.TAX - 1);
            }

            NettoAndVat nettoAndVat = new() { 
                NettoSummary = nettoSummary, 
                VatSummary = vatSummary 
            };

            return nettoAndVat;
        }
    }
}
