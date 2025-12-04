using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using API.Infrastructure.Interface;
using Dapper;
using DAL.ModelView;
using DAL.ViewModels.ClaimDTO;
using OfficeOpenXml;

namespace API.Infrastructure.Application.Clients
{
    internal partial class ClientClaims
    {
        private const string InsertCustomerDevicePurchasesTempSql = @"
INSERT INTO [dbo].[CustomerDevicePurchases_Temp]
(
    [PhoneNumber],
    [CustomerName],
    [SecondaryContactName],
    [SecondaryContact],
    [IdNumber],
    [DateOfBirth],
    [PhoneModel],
    [ImeiNumber],
    [SerialNumber],
    [PhoneCost],
    [MpesaRef],
    [ModeOfPurchase],
    [LoanRefNumber],
    [RepaymentTerms],
    [LoanAmount],
    [InterestRate],
    [PremiumPaid],
    [UploadReference],
    [UploadFailed],
    [Error],
    [RowNumber],
    [CreatedAtUtc],
    [PurchaseDate]
)
VALUES
(
    @PhoneNumber,
    @CustomerName,
    @SecondaryContactName,
    @SecondaryContact,
    @IdNumber,
    @DateOfBirth,
    @PhoneModel,
    @ImeiNumber,
    @SerialNumber,
    @PhoneCost,
    @MpesaRef,
    @ModeOfPurchase,
    @LoanRefNumber,
    @RepaymentTerms,
    @LoanAmount,
    @InterestRate,
    @PremiumPaid,
    @UploadReference,
    @UploadFailed,
    @Error,
    @RowNumber,
    @CreatedAtUtc,
    @purchaseDate
);";

        public async Task<ResponseDTO<PolicyUploadResult>> UploadPolicyPurchasesAsync(PolicyUploadRequest request)
        {
            var response = new ResponseDTO<PolicyUploadResult>
            {
                Result = new PolicyUploadResult(),
                Success = false,
                ErrorMsg = "Unable to process policy upload."
            };

            if (request?.fileDetails == null || string.IsNullOrWhiteSpace(request.fileDetails.data))
            {
                response.ErrorMsg = "A valid excel file is required.";
                return response;
            }

            try
            {

                // Create "Policyupload" directory if not exists
                var uploadDir = Path.Combine(Directory.GetCurrentDirectory(), "Policyupload");
                if (!Directory.Exists(uploadDir))
                {
                    Directory.CreateDirectory(uploadDir);
                }

                // Save file to "Policyupload" folder
                var fileName = $"PolicyUpload_{DateTime.UtcNow:yyyyMMddHHmmssfff}.xlsx";
                var filePath = Path.Combine(uploadDir, fileName);

                File.WriteAllBytes(filePath, DecodeFile(request.fileDetails.data));

                // Prepare Insert into UploadSummary
                var summaryParams = new DynamicParameters();
                summaryParams.Add("@FileName", fileName);
                summaryParams.Add("@FilePath", filePath);
                summaryParams.Add("@WithFailure", false); // set to true if failures found, adjust later if needed
                summaryParams.Add("@FailureCount", 0);    // adjust after processing
                summaryParams.Add("@SuccessCount", 0);    // adjust after processing
                summaryParams.Add("@Processed", false);   // set to true after processing
                summaryParams.Add("@Deleted", false);

                // Insert to UploadSummary (before row processing, can update after)
                await _db.Connection.ExecuteAsync(@"
INSERT INTO [dbo].[UploadSummary]
    ([FileName],[FilePath],[WithFailure],[FailureCount],[SuccessCount],[Processed],[Deleted])
VALUES
    (@FileName, @FilePath, @WithFailure, @FailureCount, @SuccessCount, @Processed, @Deleted)
", summaryParams);

                var fileBytes = DecodeFile(request.fileDetails.data);
                ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

                using var package = new ExcelPackage(new MemoryStream(fileBytes));
                var worksheet = package.Workbook.Worksheets.FirstOrDefault();

                if (worksheet?.Dimension == null)
                {
                    response.ErrorMsg = "Uploaded file does not contain any worksheet data.";
                    return response;
                }

                var imeiNumbers = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
                for (var row = 2; row <= worksheet.Dimension.End.Row; row++)
                {
                    var imeiValue = worksheet.Cells[row, 8]?.Text?.Trim();
                    if (!string.IsNullOrWhiteSpace(imeiValue))
                    {
                        imeiNumbers.Add(imeiValue);
                    }
                }

                if (_db.Connection.State != ConnectionState.Open)
                {
                    _db.Connection.Open();
                }

                var existingImeis = imeiNumbers.Count > 0
                    ? new HashSet<string>(
                        await _db.Connection.QueryAsync<string>(
                            "SELECT ImeiNumber FROM PhoneInsuranceRequest WHERE ImeiNumber IN @Imeis",
                            new { Imeis = imeiNumbers.ToArray() }),
                        StringComparer.OrdinalIgnoreCase)
                    : new HashSet<string>(StringComparer.OrdinalIgnoreCase);

                var uploadReference = $"POL{DateTime.UtcNow:yyyyMMddHHmmssfff}";
                var imeiTracker = new Dictionary<string, int>(StringComparer.OrdinalIgnoreCase);
                var failures = new List<PolicyUploadFailure>();
                var successCount = 0;
                var processedRows = 0;
                var utcNow = DateTime.UtcNow;
                if(_db.Connection.State != ConnectionState.Open)
                {
                     _db.Connection.Open();
                }
                using var transaction = _db.Connection.BeginTransaction();
                try
                {

                    for (var row = 2; row <= worksheet.Dimension.End.Row; row++)
                    {
                        var record = ReadPolicyRow(worksheet, row);
                        if (record.IsEmpty)
                        {
                            continue;
                        }

                        processedRows++;

                        var errors = ValidatePolicyRow(record, imeiTracker, row, existingImeis);
                        var hasErrors = errors.Count > 0;

                        var parameters = BuildInsertParameters(record, uploadReference, utcNow, hasErrors, errors, row);
                        await _db.Connection.ExecuteAsync(InsertCustomerDevicePurchasesTempSql, parameters, transaction);

                        if (hasErrors)
                        {
                            failures.Add(new PolicyUploadFailure(record, row, string.Join("; ", errors)));
                        }
                        else
                        {
                            successCount++;
                        }
                    }

                    if (processedRows == 0)
                    {
                        transaction.Rollback();
                        response.ErrorMsg = "No records were found in the uploaded file.";
                        return response;
                    }

                    transaction.Commit();
                }
                catch(Exception e)
                {
                     _settings.LogRequests(e.Message, nameof(UploadPolicyPurchasesAsync), RequestType.Error);
                    transaction.Rollback();
                    throw;
                }

                var failureReport = failures.Count > 0
                    ? $"data:application/vnd.openxmlformats-officedocument.spreadsheetml.sheet;base64,{GenerateFailureWorkbook(failures)}"
                    : string.Empty;

                response.Result = new PolicyUploadResult
                {
                    UploadReference = uploadReference,
                    SuccessCount = successCount,
                    FailureCount = failures.Count,
                    FailureReport = failureReport
                };

                response.Success = true;
                response.ErrorMsg = string.Empty;
            }
            catch (Exception ex)
            {
                _settings.LogRequests(ex.Message, nameof(UploadPolicyPurchasesAsync), RequestType.Error);
            }

            return response;
        }

        private async Task EnsureConnectionIsOpenAsync()
        {
            if (_db.Connection.State != ConnectionState.Open)
            {
                
            }
        }

        private static byte[] DecodeFile(string payload)
        {
            var data = payload;
            var commaIndex = payload.IndexOf(',');
            if (commaIndex >= 0)
            {
                data = payload[(commaIndex + 1)..];
            }

            return Convert.FromBase64String(data);
        }

        private static PolicyUploadRecord ReadPolicyRow(ExcelWorksheet worksheet, int row)
        {
            return new PolicyUploadRecord
            {
                PhoneNumber = GetCellValue(worksheet, row, 1),
                CustomerName = GetCellValue(worksheet, row, 2),
                SecondaryContactName = GetCellValue(worksheet, row, 3),
                SecondaryContact = GetCellValue(worksheet, row, 4),
              
                IdNumber = GetCellValue(worksheet, row, 5),
                DateOfBirth = GetCellValue(worksheet, row, 6),
                PhoneModel = GetCellValue(worksheet, row, 7),
                ImeiNumber = GetCellValue(worksheet, row, 8),
                  purchaseDate = GetCellValue(worksheet, row, 9),
                SerialNumber = GetCellValue(worksheet, row, 10),
                PhoneCostRaw = GetCellValue(worksheet, row, 11),
                MpesaRef = GetCellValue(worksheet, row, 12),
                ModeOfPurchaseRaw = GetCellValue(worksheet, row, 13),
                LoanRefNumber = GetCellValue(worksheet, row, 14),
                RepaymentTerms = GetCellValue(worksheet, row, 15),
                LoanAmountRaw = GetCellValue(worksheet, row, 16),
                InterestRateRaw = GetCellValue(worksheet, row, 17),
                PremiumPaidRaw = GetCellValue(worksheet, row, 18)
            };
        }

        private static string GetCellValue(ExcelWorksheet worksheet, int row, int column) =>
            worksheet.Cells[row, column]?.Text?.Trim() ?? string.Empty;

        private static List<string> ValidatePolicyRow(
            PolicyUploadRecord record,
            Dictionary<string, int> imeiTracker,
            int rowNumber,
            HashSet<string> existingImeis)
        {
            var errors = new List<string>();

            RequireField(record.PhoneNumber, "Phone number", errors);
            RequireField(record.CustomerName, "Customer name", errors);
            RequireField(record.IdNumber, "ID number", errors);
            RequireField(record.PhoneModel, "Phone model", errors);

            if (string.IsNullOrWhiteSpace(record.ImeiNumber))
            {
                errors.Add("IMEI number is required.");
            }
            else
            {
                var normalizedImei = record.ImeiNumber.Trim();
                if (imeiTracker.TryGetValue(normalizedImei, out var duplicateRow))
                {
                    errors.Add($"Duplicate IMEI detected. Already captured on row {duplicateRow}.");
                }
                else
                {
                    imeiTracker[normalizedImei] = rowNumber;
                    if (existingImeis != null && existingImeis.Contains(normalizedImei))
                    {
                        errors.Add("IMEI number already exists in PhoneInsuranceRequest.");
                    }
                }
            }

            if (string.IsNullOrWhiteSpace(record.PhoneCostRaw))
            {
                errors.Add("Phone cost is required.");
                record.PhoneCost = 0;
            }
            else if (!TryParseDecimal(record.PhoneCostRaw, out var phoneCost))
            {
                errors.Add("Phone cost must be numeric.");
                record.PhoneCost = 0;
            }
            else
            {
                record.PhoneCost = phoneCost;
            }

            if (!string.IsNullOrWhiteSpace(record.LoanAmountRaw))
            {
                if (TryParseDecimal(record.LoanAmountRaw, out var loanAmount))
                {
                    record.LoanAmount = loanAmount;
                }
                else
                {
                    errors.Add("Loan amount must be numeric.");
                }
            }

            if (!string.IsNullOrWhiteSpace(record.InterestRateRaw))
            {
                if (TryParseDecimal(record.InterestRateRaw, out var interestRate))
                {
                    record.InterestRate = interestRate;
                }
                else
                {
                    errors.Add("Interest rate must be numeric.");
                }
            }

            if (!string.IsNullOrWhiteSpace(record.PremiumPaidRaw))
            {
                if (TryParseDecimal(record.PremiumPaidRaw, out var premiumPaid))
                {
                    record.PremiumPaid = premiumPaid;
                }
                else
                {
                    errors.Add("Premium paid must be numeric.");
                }
            }

            if (!string.IsNullOrWhiteSpace(record.ModeOfPurchaseRaw))
            {
                if (Enum.TryParse<ModeOfPurchase>(record.ModeOfPurchaseRaw, true, out var mode))
                {
                    record.ModeOfPurchase = mode;
                }
                else
                {
                    errors.Add("Mode of purchase must be either 'cash' or 'credit'.");
                }
            }

            return errors;
        }

        private static bool TryParseDecimal(string value, out decimal result) =>
            decimal.TryParse(value, NumberStyles.Number, CultureInfo.InvariantCulture, out result);

        private static DynamicParameters BuildInsertParameters(
            PolicyUploadRecord record,
            string uploadReference,
            DateTime createdAtUtc,
            bool hasErrors,
            List<string> errors,
            int rowNumber)
        {
            var parameters = new DynamicParameters();

            parameters.Add("@PhoneNumber", record.PhoneNumber);
            parameters.Add("@CustomerName", record.CustomerName);
            parameters.Add("@SecondaryContactName", record.SecondaryContactName);
            parameters.Add("@SecondaryContact", record.SecondaryContact);
            parameters.Add("@IdNumber", record.IdNumber);
            parameters.Add("@DateOfBirth", record.DateOfBirth);
            parameters.Add("@PhoneModel", record.PhoneModel);
            parameters.Add("@ImeiNumber", record.ImeiNumber);
            parameters.Add("@SerialNumber", record.SerialNumber);
            parameters.Add("@PhoneCost", record.PhoneCost);
            parameters.Add("@MpesaRef", record.MpesaRef);
            parameters.Add("@ModeOfPurchase", record.ModeOfPurchase.HasValue ? (int)record.ModeOfPurchase.Value : (int?)null);
            parameters.Add("@LoanRefNumber", record.LoanRefNumber);
            parameters.Add("@RepaymentTerms", record.RepaymentTerms);
            parameters.Add("@LoanAmount", record.LoanAmount);
            parameters.Add("@InterestRate", record.InterestRate);
            parameters.Add("@PremiumPaid", record.PremiumPaid);
            parameters.Add("@UploadReference", uploadReference);
            parameters.Add("@UploadFailed", hasErrors);
            parameters.Add("@Error", hasErrors ? string.Join("; ", errors) : null);
            parameters.Add("@RowNumber", rowNumber);
            parameters.Add("@CreatedAtUtc", createdAtUtc);
          parameters.Add("@PurchaseDate", record.purchaseDate);
            return parameters;
        }

        private static string GenerateFailureWorkbook(IEnumerable<PolicyUploadFailure> failures)
        {
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
            using var package = new ExcelPackage();
            var worksheet = package.Workbook.Worksheets.Add("FailedRecords");

            var headers = new[]
            {
                "RowNumber",
                "PhoneNumber",
                "CustomerName",
                "SecondaryContactName",
                "SecondaryContact",
                "IdNumber",
                "DateOfBirth",
                "PhoneModel",
                "ImeiNumber",
                "SerialNumber",
                "PhoneCost",
                "MpesaRef",
                "ModeOfPurchase",
                "LoanRefNumber",
                "RepaymentTerms",
                "LoanAmount",
                "InterestRate",
                "PremiumPaid",
                "Error"
            };

            for (var i = 0; i < headers.Length; i++)
            {
                worksheet.Cells[1, i + 1].Value = headers[i];
            }

            var currentRow = 2;
            foreach (var failure in failures)
            {
                var record = failure.Record;
                worksheet.Cells[currentRow, 1].Value = failure.RowNumber;
                worksheet.Cells[currentRow, 2].Value = record.PhoneNumber;
                worksheet.Cells[currentRow, 3].Value = record.CustomerName;
                worksheet.Cells[currentRow, 4].Value = record.SecondaryContactName;
                worksheet.Cells[currentRow, 5].Value = record.SecondaryContact;
                worksheet.Cells[currentRow, 6].Value = record.IdNumber;
                worksheet.Cells[currentRow, 7].Value = record.DateOfBirth;
                worksheet.Cells[currentRow, 8].Value = record.PhoneModel;
                worksheet.Cells[currentRow, 9].Value = record.ImeiNumber;
                worksheet.Cells[currentRow, 10].Value = record.SerialNumber;
                worksheet.Cells[currentRow, 11].Value = record.PhoneCost?.ToString("F2");
                worksheet.Cells[currentRow, 12].Value = record.MpesaRef;
                worksheet.Cells[currentRow, 13].Value = record.ModeOfPurchase?.ToString();
                worksheet.Cells[currentRow, 14].Value = record.LoanRefNumber;
                worksheet.Cells[currentRow, 15].Value = record.RepaymentTerms;
                worksheet.Cells[currentRow, 16].Value = record.LoanAmount?.ToString("F2");
                worksheet.Cells[currentRow, 17].Value = record.InterestRate?.ToString("F4");
                worksheet.Cells[currentRow, 18].Value = record.PremiumPaid?.ToString("F2");
                worksheet.Cells[currentRow, 19].Value = failure.Error;
                currentRow++;
            }

            worksheet.Cells.AutoFitColumns();

            return Convert.ToBase64String(package.GetAsByteArray());
        }

        private sealed class PolicyUploadRecord
        {
            public string? PhoneNumber { get; init; }
            public string? CustomerName { get; init; }
            public string? SecondaryContactName { get; init; }
            public string? SecondaryContact { get; init; }
            public string? IdNumber { get; init; }
            public string? DateOfBirth { get; init; }
            public string? PhoneModel { get; init; }
            public string? ImeiNumber { get; init; }
            public string? SerialNumber { get; init; }
            public string? PhoneCostRaw { get; init; }
            public string? purchaseDate { get; init; }
            public string? MpesaRef { get; init; }
            public string? ModeOfPurchaseRaw { get; init; }
            public string? LoanRefNumber { get; init; }
            public string? RepaymentTerms { get; init; }
            public string? LoanAmountRaw { get; init; }
            public string? InterestRateRaw { get; init; }
            public string? PremiumPaidRaw { get; init; }

            public decimal? PhoneCost { get; set; }
            public decimal? LoanAmount { get; set; }
            public decimal? InterestRate { get; set; }
            public decimal? PremiumPaid { get; set; }
            public ModeOfPurchase? ModeOfPurchase { get; set; }

            public bool IsEmpty =>
                string.IsNullOrWhiteSpace(PhoneNumber) &&
                string.IsNullOrWhiteSpace(CustomerName) &&
                string.IsNullOrWhiteSpace(SecondaryContactName) &&
                string.IsNullOrWhiteSpace(SecondaryContact) &&
                string.IsNullOrWhiteSpace(IdNumber) &&
                string.IsNullOrWhiteSpace(DateOfBirth) &&
                string.IsNullOrWhiteSpace(PhoneModel) &&
                string.IsNullOrWhiteSpace(ImeiNumber) &&
                string.IsNullOrWhiteSpace(SerialNumber) &&
                string.IsNullOrWhiteSpace(PhoneCostRaw) &&
                string.IsNullOrWhiteSpace(MpesaRef) &&
                string.IsNullOrWhiteSpace(ModeOfPurchaseRaw) &&
                string.IsNullOrWhiteSpace(LoanRefNumber) &&
                string.IsNullOrWhiteSpace(RepaymentTerms) &&
                string.IsNullOrWhiteSpace(LoanAmountRaw) &&
                string.IsNullOrWhiteSpace(InterestRateRaw) &&
                string.IsNullOrWhiteSpace(PremiumPaidRaw);
        }

        private sealed record PolicyUploadFailure(PolicyUploadRecord Record, int RowNumber, string Error);

        private static void RequireField(string? value, string fieldName, ICollection<string> errors)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                errors.Add($"{fieldName} is required.");
            }
        }
    }
}

