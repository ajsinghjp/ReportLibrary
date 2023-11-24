using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Reflection;

using iTextSharp;
using iTextSharp.text;
using iTextSharp.text.pdf;

namespace InsTech.ForeSight.SalesPortal.Life.WL.iText
{
    /// <summary>
    /// Basic ledger
    /// </summary>
    public class LedgerBasic : LedgerReportBase
    {
        /// <summary>
        /// Initialize ledger report
        /// </summary>
        /// <param name="ledgerReports">List of ledger report</param>
        protected override void InitializeLedgerReports(List<SalesPortal.LedgerReport> ledgerReports)
        {
            LedgerReport ledger = new LedgerReport((Int32)LedgerReportId.Default);			
            ledger.FormatReportID = 2;

			//To have blank rows with horizontal line every 5 rows.
			ledger.RowsTogether = 5;
			ledger.SetHighlightRow(ledger.RowsTogether, true, "DefaultStyle_Highlight");

			ledger.AddTitle(GetTitleTable(), "TitleFormat");

            //Add the proper result set
            ledger.AddResultSet(InputSupplement.GuaranteedResultSet);
            ledger.AddResultSet(InputSupplement.HasDividendReduction ? InputSupplement.AssumedResultSet : InputSupplement.PrimaryResultSet);
			
            ledgerReports.Add(ledger);
        }
		
        #region Detail Section

        /// <summary>
        /// Sets the columns
        /// </summary>
        /// <param name="ledger"></param>
        protected override void SetColumns(LedgerReport ledger)
        {
            Int32 rsDefIdx = ledger.AnchorResultSetIndex;
            Int32 guarRsIdx = 0;
            Int32 currRsIdx = 1;
            Single borderColWidth = 1.25f;

			ledger.AddColumnGroup(0, String.Empty, (Int32)Group.Blank1, "BlankGroupFormat");
			ledger.AddColumnGroup(0, "GUARANTEED", (Int32)Group.Guar, "DefaultGroupFormat");
			ledger.AddColumnGroup(0, String.Empty, (Int32)Group.Blank2, "BlankGroupFormat");
			ledger.AddColumnGroup(0, "NON-GUARANTEED", (Int32)Group.NonGuar, "DefaultGroupFormat");

			ledger.AddColumn(3.25f, rsDefIdx, "AGE", "Age", (Int32)Group.Blank1);
			ledger.AddYearColumn(2.50f, rsDefIdx, "YR", "Years", (Int32)Group.Blank1);
			ledger.AddBlankColumn(borderColWidth, "LeftBorder", (Int32)Group.Blank1);

			ledger.AddColumn(6.75f, guarRsIdx, "POLICY PREMIUM", "TotalPremium", (Int32)Group.Guar);
			ledger.AddColumn(6.75f, guarRsIdx, "PREMIUM OUTLAY", "NetOutlay", (Int32)Group.Guar);
			ledger.AddColumn(7.25f, guarRsIdx, "CASH<br/>VALUE", "NetCashValue", (Int32)Group.Guar);
			ledger.AddColumn(7.25f, guarRsIdx, "DEATH BENEFIT", "NetDeathBenefits", (Int32)Group.Guar);
			ledger.AddColumn(7.25f, guarRsIdx, "MAXIMUM PAID-UP INSURANCE", "MaxPaidUpInsurance", (Int32)Group.Guar);
			
			ledger.AddBlankColumn(borderColWidth, "LeftBorder", (Int32)Group.Blank2);

			ledger.AddColumn(6.75f, currRsIdx, "POLICY PREMIUM", "TotalPremium", (Int32)Group.NonGuar);
			ledger.AddColumn(6.75f, currRsIdx, "DIVIDEND", "TotalDividends", (Int32)Group.NonGuar);
			ledger.AddColumn(4.25f, currRsIdx, "DIV OPT", "DividendOptionAbbr", (Int32)Group.NonGuar);
			ledger.AddColumn(6.75f, currRsIdx, "PREMIUM OUTLAY", "NetOutlay", (Int32)Group.NonGuar);
			ledger.AddColumn(7.25f, currRsIdx, "CASH<br/>VALUE", "NetCashValue", (Int32)Group.NonGuar);
			ledger.AddColumn(7.25f, currRsIdx, "DEATH BENEFIT", "NetDeathBenefits", (Int32)Group.NonGuar);
			ledger.AddColumn(7.25f, currRsIdx, "MAXIMUM PAID-UP INSURANCE", "MaxPaidUpInsurance", (Int32)Group.NonGuar);
        }

        #endregion

		#region Helper methods
		private ITPdfPTable GetTitleTable()
		{
			TextReportBuilder textBuilder = GetSummaryTextBuilder();
			
			SetSummaryTabeHeader(textBuilder, "TABULAR DETAIL");

			textBuilder.AddLine(ReportStyles.ThinBorder, true, true);
			textBuilder.AddSubhead(String.Format("{0}\n{1}", InputSupplement.ProductName, InputSupplement.PlanType));
			textBuilder.NextColumn();
			textBuilder.SpaceBetween = 1.5f;
			textBuilder.AddItemXML("CT_WL_LB_Title_Para1");
			//if (InputSupplement.APIDumpInAmount > 0 && InputSupplement.ExchangeAmount > 0 && InputSupplement.API1035Amount > 0 && InputSupplement.NonAPI1035Amount > 0)
			//{
			//	textBuilder.AddItemXML("CT_WL_LB_Title_Para2_APIAndExchGtAPI1035");
			//}
			//else if (InputSupplement.APIDumpInAmount > 0 && InputSupplement.ExchangeAmount > 0)
			//{
			//	textBuilder.AddItemXML("CT_WL_LB_Title_Para2_APIAndExch");
			//}
			//else if (InputSupplement.ExchangeAmount > 0 && InputSupplement.API1035Amount > 0 && InputSupplement.NonAPI1035Amount > 0)
			//{
			//	textBuilder.AddItemXML("CT_WL_LB_Title_Para2_ExchGtAPI1035");
			//}
			//else if (InputSupplement.ExchangeAmount > 0)
			//{
			//	textBuilder.AddItemXML("CT_WL_LB_Title_Para2_Exch");
			//}
			//else if (InputSupplement.APIDumpInAmount > 0)
			//{
				textBuilder.AddItemXML("CT_WL_LB_Title_Para2_API");
			//}

			textBuilder.AddItemXML("CT_WL_LB_Title_Para3");
		
			textBuilder.AddBlankSpace(12.75f);

			return textBuilder.GetTable();
		}
		#endregion Helper methods

	}
}
