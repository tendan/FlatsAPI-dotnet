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
using FlatsAPI.Exceptions;
using Microsoft.EntityFrameworkCore;

namespace FlatsAPI.Services
{
    public interface IInvoiceService
    {
        Invoice GetInvoiceForSpecifiedAccount(int accountId);
    }
    public class InvoiceService : IInvoiceService
    {
        private const string DateFormat = "yyyy-MM-dd";
        private readonly FlatsDbContext _dbContext;

        public InvoiceService(FlatsDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public Invoice GetInvoiceForSpecifiedAccount(int accountId)
        {
            var account = _dbContext.Accounts.Include(a => a.Rents).FirstOrDefault(a => a.Id == accountId);

            if (account is null)
                throw new NotFoundException("Account not found");

            if (account.BillingAddress is null)
                throw new ForbiddenException("Billing address is missing");

            if (!account.Rents.Any())
                throw new BadRequestException("No rents were found");

            var document = GeneratePdf(account, out string fileName);

            var fileContents = document.ToArray();

            Invoice invoice = new()
            {
                FileContents = fileContents,
                ContentType = "application/pdf",
                FileName = fileName
            };

            return invoice;
        }
        private MemoryStream GeneratePdf(Account account, out string fileName)
        {
            var accountRents = account.Rents;

            using var stream = new MemoryStream();
            using var writer = new PdfWriter(stream);
            using var pdf = new PdfDocument(writer);
            using var document = new Document(pdf)
                .SetFont(DocumentSettings.SecondaryFont)
                .SetFontSize(12);
            
            var beginning = GenerateBeginning();
            var buyerSellerChapter = GenerateBuyerSellerInvoiceChapter(account);
            
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

            var generatedId = new Random().Next();

            fileName = $"invoice_{generatedId}";

            return stream;
        }
        private Div GenerateBeginning()
        {
            var header = new Div();

            var currentDate = DateTime.Now;

            var title = new Paragraph()
                .SetFontSize(24)
                .Add("Flats of Blocks Inc.");

            var paragraphBase = new Paragraph();
            //var invoiceDate = currentDate.ToString(DateFormat);
            //string dueDate = currentDate.AddDays(20).ToString(DateFormat);

            //var credentials = paragraphBase
            //    .Add("Kielce\n")
            //    .Add($"{invoiceDate}\n")
            //    .Add($"{dueDate}");

            var credentials = paragraphBase
                .Add("Przeskok 12A\n")
                .Add("Kielce, Świętokrzyskie 25-813");

            header
                .Add(title)
                .Add(credentials);

            return header;
        }
        private Table GenerateBuyerSellerInvoiceChapter(Account buyer)
        {
            var table = new Table(2)
                .SetHorizontalAlignment(HorizontalAlignment.LEFT)
                .SetWidth(UnitValue.CreatePercentValue(66));

            var sellerParagraph = new Paragraph("Seller")
                .SetPadding(0)
                .SetBold();

            var buyerParagraph = new Paragraph("Buyer")
                .SetPadding(0)
                .SetBold();

            var sellerCell = new Cell(1, 1)
                .Add(sellerParagraph)
                .SetBorder(Border.NO_BORDER);
            var buyerCell = new Cell(1, 1)
                .Add(buyerParagraph)
                .SetBorder(Border.NO_BORDER);

            table
                .AddHeaderCell(sellerCell)
                .AddHeaderCell(buyerCell);

            var sellerCredentials = new Cell(1, 1)
                .SetBorder(Border.NO_BORDER)
                .Add(new Paragraph("Flats of Blocks Inc."))
                .Add(new Paragraph("Przeskok 12A"))
                .Add(new Paragraph("Kielce, Świętokrzyskie 25-813"));

            string billingAddress = buyer.BillingAddress;

            var buyerCredentials = new Cell(1, 1)
                .SetBorder(Border.NO_BORDER)
                .Add(new Paragraph($"{buyer.FirstName} {buyer.LastName}"))
                .Add(new Paragraph(billingAddress));

            table
                .AddCell(sellerCredentials)
                .AddCell(buyerCredentials);

            return table;
            
        }
        private Table GenerateRentsTable(ICollection<Rent> rents)
        {
            var table = new Table(6, false)
                .SetHorizontalAlignment(HorizontalAlignment.RIGHT)
                .SetWidth(UnitValue.CreatePercentValue(100));

            var tableHeaderBase = new Cell(1, 1)
                .SetTextAlignment(TextAlignment.CENTER);

            var indexNo = new Cell(1, 1)
                .SetTextAlignment(TextAlignment.CENTER)
                .Add(new Paragraph("In.").SetBold());

            var serviceProductName = new Cell(1, 1)
                .SetTextAlignment(TextAlignment.CENTER)
                .Add(new Paragraph("Service/product name").SetBold());

            var nettoPrice = new Cell(1, 1)
                .SetTextAlignment(TextAlignment.CENTER)
                .Add(new Paragraph("Netto price").SetBold());

            var vat = new Cell(1, 1)
                .SetTextAlignment(TextAlignment.CENTER)
                .Add(new Paragraph("VAT").SetBold());

            var vatRate = new Cell(1, 1)
                .SetTextAlignment(TextAlignment.CENTER)
                .Add(new Paragraph("VAT rate").SetBold());

            var price = new Cell(1, 1)
                .SetTextAlignment(TextAlignment.CENTER)
                .Add(new Paragraph("Price").SetBold());

            table
                .AddHeaderCell(indexNo)
                .AddHeaderCell(serviceProductName)
                .AddHeaderCell(nettoPrice)
                .AddHeaderCell(vat)
                .AddHeaderCell(vatRate)
                .AddHeaderCell(price);

            var index = 1;

            foreach (var rent in rents)
            {
                // Index number add
                var indexNumberCell = new Cell(1, 1).SetTextAlignment(TextAlignment.RIGHT)
                    .Add(new Paragraph($"{index++}."));
                table.AddCell(indexNumberCell);

                // Product/service name add
                var property = rent.PropertyType == PropertyTypes.BlockOfFlats ? "Block of Flats" : "Flat";
                var productServiceCell = new Cell(1, 1);
                if (rent.PropertyType == PropertyTypes.BlockOfFlats)
                {
                    var blockOfFlats = _dbContext.BlockOfFlats.FirstOrDefault(b => b.Id == rent.PropertyId);

                    var address = blockOfFlats.Address;

                    productServiceCell.Add(new Paragraph($"{property} on {address}"));
                    table.AddCell(productServiceCell);
                }
                else
                {
                    var flat = _dbContext.Flats.Include(f => f.BlockOfFlats).FirstOrDefault(f => f.Id == rent.PropertyId);

                    var address = flat.BlockOfFlats.Address;

                    var number = flat.Number;

                    productServiceCell.Add(new Paragraph($"{property} on {address} no. {number}"));
                    table.AddCell(productServiceCell);
                }

                // Netto price add
                var formattedNettoPrice = rent.Price.ToString("C", CultureInfo.CurrentCulture);
                var nettoCell = new Cell(1, 1)
                    .SetTextAlignment(TextAlignment.RIGHT)
                    .Add(new Paragraph(formattedNettoPrice));
                table.AddCell(nettoCell);

                // VAT percentage add
                var percentage = Math.Round((PaymentSettings.TAX - 1) * 100);
                var percentageCurrencyString = percentage.ToString();
                var vatCell = new Cell(1, 1)
                    .SetTextAlignment(TextAlignment.RIGHT)
                    .Add(new Paragraph($"{percentageCurrencyString}%"));
                table.AddCell(vatCell);

                // VAT rate add
                var rate = (rent.Price * (percentage / 100)).ToString("C", CultureInfo.CurrentCulture);
                var vatRateCell = new Cell(1, 1)
                    .SetTextAlignment(TextAlignment.RIGHT)
                    .Add(new Paragraph(rate));
                table.AddCell(vatRateCell);

                // Brutto price add
                var bruttoPrice = rent.PriceWithTax.ToString("C", CultureInfo.CurrentCulture);
                var bruttoCell = new Cell(1, 1)
                    .SetTextAlignment(TextAlignment.RIGHT)
                    .Add(new Paragraph(bruttoPrice));
                table.AddCell(bruttoCell);
            }

            return table;
        }
        private Table GenerateSummaryTable(float nettoSummary, float vatSummary)
        {
            var table = new Table(4, false)
                .SetHorizontalAlignment(HorizontalAlignment.RIGHT)
                .SetWidth(UnitValue.CreatePercentValue(66));

            var tableHeaderBase = new Cell(1, 1)
                .SetTextAlignment(TextAlignment.CENTER);

            var tableHeader1 = new Cell(1, 1).SetTextAlignment(TextAlignment.CENTER).Add(new Paragraph("Stawka VAT"));

            var tableHeader2 = new Cell(1, 1).SetTextAlignment(TextAlignment.CENTER).Add(new Paragraph("Netto"));

            var tableHeader3 = new Cell(1, 1).SetTextAlignment(TextAlignment.CENTER).Add(new Paragraph("VAT"));

            var tableHeader4 = new Cell(1, 1).SetTextAlignment(TextAlignment.CENTER).Add(new Paragraph("Brutto"));

            table
                .AddHeaderCell(tableHeader1)
                .AddHeaderCell(tableHeader2)
                .AddHeaderCell(tableHeader3)
                .AddHeaderCell(tableHeader4);

            // VAT add
            var percentage = Math.Round((PaymentSettings.TAX - 1) * 100);
            var percentageCurrencyString = percentage.ToString();
            table
                .AddCell($"{percentageCurrencyString}%")
                .SetTextAlignment(TextAlignment.RIGHT);

            // Netto summary add
            var nettoSummaryCurrencyString = nettoSummary.ToString("C", CultureInfo.CurrentCulture);
            table
                .AddCell(nettoSummaryCurrencyString)
                .SetTextAlignment(TextAlignment.RIGHT); ;

            // VAT summary add
            var vatSummaryCurrencyString = vatSummary.ToString("C", CultureInfo.CurrentCulture);
            table
                .AddCell(vatSummaryCurrencyString)
                .SetTextAlignment(TextAlignment.RIGHT); ;

            // Brutto add
            var brutto = (nettoSummary + vatSummary).ToString("C", CultureInfo.CurrentCulture);
            table
                .AddCell(brutto)
                .SetTextAlignment(TextAlignment.RIGHT); ;

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
