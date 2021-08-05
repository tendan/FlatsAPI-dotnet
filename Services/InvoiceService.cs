using FlatsAPI.Entities;
using iText.Kernel.Pdf;
using iText.Layout;
using iText.Layout.Properties;
using iText.Layout.Element;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using FlatsAPI.Settings;
using System.Globalization;
using FlatsAPI.Models;
using iText.Layout.Borders;
using FlatsAPI.Exceptions;
using Microsoft.EntityFrameworkCore;
using iText.Kernel.Pdf.Annot;
using iText.Kernel.Pdf.Navigation;
using iText.Kernel.Pdf.Action;
using iText.Kernel.Font;
using iText.IO.Font;
using FlatsAPI.Settings.Permissions;

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
        private readonly IUserContextService _userContextService;

        public InvoiceService(FlatsDbContext dbContext, IUserContextService userContextService)
        {
            _dbContext = dbContext;
            _userContextService = userContextService;
        }

        public Invoice GetInvoiceForSpecifiedAccount(int accountId)
        {
            var account = _dbContext.Accounts.Include(a => a.Rents).FirstOrDefault(a => a.Id == accountId);

            if (account is null)
                throw new NotFoundException("Account not found");

            _userContextService.AuthorizeAccess(accountId, InvoicePermissions.ReadOthers);

            if (account.BillingAddress is null)
            {
                if (accountId == _userContextService.GetUserId)
                    throw new ForbiddenException("Billing address is missing");

                throw new BadRequestException("Account does not have billing address");
            }

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

            var primaryFont = PdfFontFactory.CreateFont(DocumentSettings.LiberationSansFontPath, PdfEncodings.IDENTITY_H);
            var secondaryFont = PdfFontFactory.CreateFont(DocumentSettings.LiberationSansFontPath, PdfEncodings.IDENTITY_H);

            using var stream = new MemoryStream();
            using var writer = new PdfWriter(stream);
            using var pdf = new PdfDocument(writer);
            using var document = new Document(pdf)
                .SetFont(secondaryFont)
                .SetFontSize(12);
            
            var beginning = GenerateBeginning();
            var buyerSellerChapter = GenerateBuyerSellerInvoiceChapter(account);
            
            var rentsTable = GenerateRentsTable(accountRents);

            var nettoAndVat = CalculateSummaryOfNettoAndVat(accountRents);

            var nettoSummary = nettoAndVat.NettoSummary;
            var vatSummary = nettoAndVat.VatSummary;

            var summaryTable = GenerateSummaryTable(nettoSummary, vatSummary);

            var footer = GenerateFooter(pdf.GetDefaultPageSize().GetHeight());

            document
                .Add(beginning)
                .Add(buyerSellerChapter)
                .Add(rentsTable)
                .Add(summaryTable)
                .Add(footer);

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

            var noBorderCell = new Cell(1, 1)
                .SetBorder(Border.NO_BORDER);

            var headerCellBase = noBorderCell
                .Clone(false)
                .SetBold();

            var sellerCell = headerCellBase
                .Clone(false)
                .Add(new Paragraph("Seller"));

            var buyerCell = headerCellBase
                .Clone(false)
                .Add(new Paragraph("Buyer"));

            table
                .AddHeaderCell(sellerCell)
                .AddHeaderCell(buyerCell);

            var sellerCredentials = noBorderCell
                .Clone(false)
                .Add(new Paragraph("Flats of Blocks Inc."))
                .Add(new Paragraph("Przeskok 12A"))
                .Add(new Paragraph("Kielce, Świętokrzyskie 25-813"));

            string billingAddress = buyer.BillingAddress;

            var buyerCredentials = noBorderCell
                .Clone(false)
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
                .SetMarginTop(10)
                .SetHorizontalAlignment(HorizontalAlignment.RIGHT)
                .SetWidth(UnitValue.CreatePercentValue(100));

            var tableHeaderBase = new Cell(1, 1)
                .SetBackgroundColor(DocumentSettings.TableHeaderCellColor)
                .SetTextAlignment(TextAlignment.CENTER);

            var indexNo = tableHeaderBase
                .Clone(false)
                .Add(new Paragraph("In.").SetBold());

            var serviceProductName = tableHeaderBase
                .Clone(false)
                .Add(new Paragraph("Service/product name").SetBold());

            var nettoPrice = tableHeaderBase
                .Clone(false)
                .Add(new Paragraph("Netto price").SetBold());

            var vat = tableHeaderBase
                .Clone(false)
                .Add(new Paragraph("VAT").SetBold());

            var vatRate = tableHeaderBase
                .Clone(false)
                .Add(new Paragraph("VAT rate").SetBold());

            var price = tableHeaderBase
                .Clone(false)
                .Add(new Paragraph("Price").SetBold());

            table
                .AddHeaderCell(indexNo)
                .AddHeaderCell(serviceProductName)
                .AddHeaderCell(nettoPrice)
                .AddHeaderCell(vat)
                .AddHeaderCell(vatRate)
                .AddHeaderCell(price);

            var index = 1;

            var alignedRightCell = new Cell(1, 1)
                .SetTextAlignment(TextAlignment.RIGHT);

            foreach (var rent in rents)
            {
                // Index number add
                var indexNumberCell = alignedRightCell
                    .Clone(false)
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
                var nettoCell = alignedRightCell
                    .Clone(false)
                    .Add(new Paragraph(formattedNettoPrice));
                table.AddCell(nettoCell);

                // VAT percentage add
                var percentage = Math.Round((PaymentSettings.TAX - 1) * 100);
                var percentageCurrencyString = percentage.ToString();
                var vatCell = alignedRightCell
                    .Clone(false)
                    .Add(new Paragraph($"{percentageCurrencyString}%"));
                table.AddCell(vatCell);

                // VAT rate add
                var rate = (rent.Price * (percentage / 100)).ToString("C", CultureInfo.CurrentCulture);
                var vatRateCell = alignedRightCell
                    .Clone(false)
                    .Add(new Paragraph(rate));
                table.AddCell(vatRateCell);

                // Brutto price add
                var bruttoPrice = rent.PriceWithTax.ToString("C", CultureInfo.CurrentCulture);
                var bruttoCell = alignedRightCell
                    .Clone(false)
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

            var tableHeader1 = tableHeaderBase
                .Clone(false)
                .Add(new Paragraph("Stawka VAT"));

            var tableHeader2 = tableHeaderBase
                .Clone(false)
                .Add(new Paragraph("Netto"));

            var tableHeader3 = tableHeaderBase
                .Clone(false)
                .Add(new Paragraph("VAT"));

            var tableHeader4 = new Cell(1, 1)
                .SetBackgroundColor(DocumentSettings.TableHeaderCellColor)
                .SetBold()
                .SetTextAlignment(TextAlignment.CENTER)
                .Add(new Paragraph("Brutto"));

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
                .SetTextAlignment(TextAlignment.RIGHT);

            // VAT summary add
            var vatSummaryCurrencyString = vatSummary.ToString("C", CultureInfo.CurrentCulture);
            table
                .AddCell(vatSummaryCurrencyString)
                .SetTextAlignment(TextAlignment.RIGHT);

            // Brutto add
            var brutto = (nettoSummary + vatSummary).ToString("C", CultureInfo.CurrentCulture);
            var bruttoCell = new Cell(1, 1)
                .SetBackgroundColor(DocumentSettings.TableHeaderCellColor)
                .SetBold()
                .Add(new Paragraph(brutto));

            table
                .AddCell(bruttoCell)
                .SetTextAlignment(TextAlignment.RIGHT);

            return table;
        }
        private Div GenerateFooter(float pageHeight)
        {
            var footer = new Div()
                .SetFixedPosition(0, 0, UnitValue.CreatePercentValue(80))
                .SetMargin(0)
                .SetPadding(0);

            var link = new Link(
                        "https://github.com/TenDan/FlatsAPI-dotnet",
                        PdfAction.CreateURI("https://github.com/TenDan/FlatsAPI-dotnet")
                    );

            var paragraph = new Paragraph(
                "This invoice is not real, it is a part of a Flats Of Blocks project on GitHub.\n"
                )
                .SetFontSize(14)
                .SetBold()
                .SetMargin(7)
                .SetPaddingBottom(0);

            paragraph.Add(link);

            footer.Add(paragraph);

            return footer;
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
