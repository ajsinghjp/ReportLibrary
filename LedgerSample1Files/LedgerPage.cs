using System;
using System.Collections.Generic;
using iTextSharp.text;

namespace InsTech.ForeSight.SalesPortal.Annuity.FIA.iText
{
    /// <summary>
    /// 
    /// </summary>
    public sealed class LedgerPage : LedgerReportBase
    {

        #region Constructor

        /// <summary>
        /// constructor, used to Set Document object with Main Report and XML File Name
        /// </summary>
        public LedgerPage()
            : base()
        {

        }

        #endregion

      
        /// <summary>
        /// Initialize ledger report
        /// </summary>
        /// <param name="ledgerReports">List of ledger report</param>
        protected override void InitializeLedgerReports(List<SalesPortal.LedgerReport> ledgerReports)
        {          
            LedgerReport ledger = new LedgerReport((Int32)LedgerReportId.Guranteed);

            ledger.AddResultSet(InputSupplement.AnnualResultSets[model]);

            // SetLedgerProperties(ledger);
            //ledger.YearsPerPage = 16;
            ////Set height of a row for the normal lines
            //ledger.RowHeight = 12.00f;
            ////Set height of a row for the normal lines. TotalRowHeight = RowHeight if not specified.
            //ledger.TotalRowHeight = 12.00f;
            //To have a border line at the bottom of the table
            ledger.BottomRowHeight = 2.0f;

            ledger.ConditionFields = CurrentInitialData.ConditionFields;

            //The below settings are to keep all the total line, normal lines and footnote section together in the one page.
            ledger.KeepTogetherLedgerTableAndEndnote = true;
            ledger.RowsTogether = 2;

            //if (InputSupplement.CurrentIllust.OutputOptions.YearsToIllustrate == (Int32)FS_DurationTotals.FiveYears)
            //{
            //    ledger.TotalSummaryDuration = TotalSummaryType.FiveYearsAndPageSummary;
            //    //ledger.TotalSummaryDuration = TotalSummaryType.FiveYears;
            //}
            //else if (InputSupplement.CurrentIllust.OutputOptions.YearsToIllustrate == (Int32)FS_DurationTotals.TenYears)
            //{
            //    ledger.TotalSummaryDuration = TotalSummaryType.TenYearsAndPageSummary;
            //    //ledger.TotalSummaryDuration = TotalSummaryType.TenYears;
            //}
            //else if (InputSupplement.CurrentIllust.OutputOptions.YearsToIllustrate == (Int32)FS_DurationTotals.PageTotals)
            //{
            //    ledger.TotalSummaryDuration = TotalSummaryType.PageSummary;
            //    //ledger.TotalSummaryDuration = TotalSummaryType.None;
            //}
            ////ledger.TotalSummaryDuration = TotalSummaryType.PageSummary;
            ledger.TotalSummaryDuration = TotalSummaryType.None;

            ledger.CaptionForSummary = "Sub-Totals";
            ledger.CaptionForTotal = "Totals";
            //To have a border line at the bottom of the table
            ledger.BottomRowHeight = 0f;
            ledger.TitleSectionLeading = Leading;



            ledger.FormatReportID = 1;
            //For Alternate row in different format
            ledger.SetHighlightRow(2, true, "DefaultStyle_HighlightPF");
            //For Add title or Preface section above the ledger
            ledger.AddTitle("Example: Guaranteed Values", "TitleFormat");              

            ledgerReports.Add(ledger);
        }
            
        

        /// <summary>
        /// Sets the columns
        /// </summary>
        /// <param name="ledger"></param>
        protected override void SetColumns(LedgerReport ledger)
        {
			Int32 rsIdx = ledger.AnchorResultSetIndex;
			Single restColumnsWidth = 8f;

            ledger.AddColumn("NoFormatCentered", restColumnsWidth, true, "Year", rsIdx, "Contract<br/>Year");
            ledger.AddColumn("NoFormatCentered", restColumnsWidth, true, "Age", rsIdx, "Age");
            
            ledger.AddColumn(7, rsIdx, GetSubSupScriptPhrase("Net Premiums", FontANeoC9PeacockBlue, "1", FontANeoC6PeacockBlue, 3), "Premiums", "CumulativePremiums");
           
            ledger.AddBlankColumn(0.1f, "LeftBorder");
                      
            ledger.AddColumn("PercentColumn", restColumnsWidth, true, "CreditedInterestRates", rsIdx, GetSubSupScriptPhrase("End of Year\nCredited Interest Rate", FontANeoC9PeacockBlue, "2", FontANeoC6PeacockBlue, 3));

            ledger.AddColumn(restColumnsWidth , rsIdx, "End of Year<br/>Accumulation Value", "AccountValues");
            ledger.AddColumn(restColumnsWidth, rsIdx, GetSubSupScriptPhrase("End of Year\nPre-MVA Cash Surrender Value", FontANeoC9PeacockBlue, "4", FontANeoC6PeacockBlue, 3), "SurrenderValues");

            ledger.AddColumn(restColumnsWidth , rsIdx, GetSubSupScriptPhrase("End of Year\nGuaranteed Minimum Value", FontANeoC9PeacockBlue, "3", FontANeoC6PeacockBlue, 3), "GuaranteedMinimumValue");
                           
            ledger.AddColumn("PercentColumn", restColumnsWidth+2f, true, "WithdrawalBenefitWithdrawalPcts", rsIdx, "Lifetime Withdrawal Percent");
            ledger.AddColumn(restColumnsWidth, rsIdx, "360 Benefit Annual Income Withdrawal", "IncomeBenefitAmounts", "CumulativeIncomeBenefitAmounts");
            ledger.AddColumn(restColumnsWidth, rsIdx, "Death Benefit", "DeathBenefit");              
        }


        /// <summary>
        /// Add a cell to a ledger table for the columns designated as special column
        /// </summary>
        /// <param name="table">The ledger table</param>
        /// <param name="richRowIndex">The RichResultSet row index</param>
        /// <param name="rsRowIndex">The ResultSet row index</param>
        /// <param name="lineDisplayFormat">Line display format type</param>
        /// <param name="column">The column that the cell will be added to</param>
        /// <param name="ledger">The LedgerReport holding the ledger table</param>
        /// <returns></returns>
        protected override bool AddSpeicalCell(ITPdfPTable table, int richRowIndex, int rsRowIndex, LineDisplayFormat lineDisplayFormat, LedgerTableColumn column, LedgerReport ledger)
        {
            Boolean cellAdded = false;
            String tempText = String.Empty;
            CellFormatProvider cfp = null;
            if (lineDisplayFormat == LineDisplayFormat.NormalLine && richRowIndex == 0)
            {
                cfp = (CellFormatProvider)ledger.GetCellFormatProvider(column, lineDisplayFormat).Clone();
                if (column.ColumnName != "Year" && column.ColumnName != "Date" && column.ColumnName != "Age" && (Double)InputSupplement.AnnualResultSets[model].Fields[column.ColumnName].Values.GetValue(richRowIndex) == 0)
                {
                    table.AddCell(String.Empty, cfp);
                    cellAdded = true;
                }
            }
            return cellAdded;
        }
      
    }
}

