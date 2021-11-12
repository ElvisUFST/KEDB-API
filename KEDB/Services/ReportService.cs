using DinkToPdf;
using DinkToPdf.Contracts;
using KEDB.Model;
using Microsoft.AspNetCore.Mvc;
using System.Text;

namespace KEDB.Services
{
    public class ReportService : IReportService
    {
        private readonly IConverter _converter;
        public ReportService(IConverter converter)
        {
            _converter = converter;
        }
        public byte[] GeneratePdfReport(Kontrolrapport kontrolrapport, IUrlHelper url)
        {
            var template = new StringBuilder();

            var logoUri = url.Link("Logo", null);

            template.Append(@$"
            <html>
                <head>
                </head>
                <body>
                                            
                <table style='width: 100%;' border='0' cellpadding='4'>
                    <tbody>
                <tr>
                    <td rowspan='2' valign='top'><p class=' '><img src='{logoUri}' width='210px'></p></td>
                    <td colspan='3' valign='top'><h1 class=''>Kontrolrapport</h1></td>

                </tr>");

            template.AppendFormat(@"
                <tr>
                    <td><b>Referencenummer:</b> <p>{0}</p></td>
                    <td><b>Varepost nr:</b> <p>{1}</p></td>
                    <td><b>Journalnr:</b> <p>{2}</p></td>
                </tr>
                ", kontrolrapport.Referencenummer, kontrolrapport.Varepostnummer, kontrolrapport.WorkzoneJournalnummer);

            template.AppendFormat(@"
                <tr>
                <td></td>
                        <td><b>Antagelsesdato:</b> <p>{0}</p></td>
                        <td><b>Oprettelsesdato:</b> <p>{1}</p></td>
                        <td><b>Ændringsdato:</b> <p>{2}</p></td>

                </tr>
                ", kontrolrapport.AntagetDato.Date.ToString("dd/MM/yyyy"), kontrolrapport.IndlaestDate.Date.ToString("dd/MM/yyyy"), kontrolrapport.RedigeretDato);

            template.AppendFormat(@"
                <tr>

                <td></td>
                    <td><b>Sagsbehandler</b> <p>{0}</p></td>
                    <td><b>Branchekode</b> <p>{1}</p></td>
                    <td><b>Samlet toldmæssig regulering</b> <p>{2}</p></td>
                </tr>
                ",
            "ikke sat",
            kontrolrapport.Branchekode,
            "kr. " + (kontrolrapport.ToldmaessigAendringOpkraevning + kontrolrapport.ToldmaessigAendringTilbagebetaling));


            string oversendtTilAnalyse = "Nej";
            if (kontrolrapport.OversendtTilAnalyse)
                oversendtTilAnalyse = "Ja";

            string oversendtTilToldrapport = "Nej";
            if (kontrolrapport.OversendtTilToldrapport)
                oversendtTilToldrapport = "Ja";

            string AndreUregelmaessigheder = "Nej";
            if (kontrolrapport.AndreUregelmaessigheder != null)
            {
                if (kontrolrapport.AndreUregelmaessigheder.Doping)
                    AndreUregelmaessigheder = "Ja";
                if (kontrolrapport.AndreUregelmaessigheder.IPRvarer)
                    AndreUregelmaessigheder = "Ja";
                if (kontrolrapport.AndreUregelmaessigheder.Narkotika)
                    AndreUregelmaessigheder = "Ja";
                if (kontrolrapport.AndreUregelmaessigheder.Vaaben)
                    AndreUregelmaessigheder = "Ja";
            }

            template.AppendFormat(@"
                <tr>
                <td></td>
                </tr><tr>
                <td></td>
                    <td>
                        <b>Oversendt til analyse</b> 
                        <p>{0}</p>
                    </td>
                        
                    <td>
                        <b>Oversendt til toldrapport</b> 
                        <p>{1}</p>
                    </td>
                    <td>
                        <b>Fysisk kontrol</b> 
                        <p>{2}</p>
                    </td>
                </tr>",
            oversendtTilAnalyse,
            oversendtTilToldrapport,
            AndreUregelmaessigheder
            );

            template.Append(@"
               

                    </tbody>
                </table>


                <br/><br/>

                <table id='rubrik-table'   style='width:100%; border: 1px solid #cccccc'  cellspacing='0'>
                <thead >
                <tr>
                    <td style='border:none !important;'><div style='padding:10px; '><b>Rubrik</b></div></td>
                    <td style='border:none !important;'><div style='padding:10px; '><b>Værdi</b></div></td>
                    <td style='border:none !important;'><div style='padding:10px; '><b>Fejl</b></div></td>
                </tr>
                </thead>
                <tbody>");

            foreach (Rubrik rubrik in kontrolrapport.Rubrikker)
            {
                string valgteFejl = "";
                foreach (var item in rubrik.RubrikValgteFejl)
                {
                    valgteFejl += "\n" + item.Fejltekst.Tekst;
                }

                template.AppendFormat(@"
                        <tr style='border: 1px solid #cccccc;'>
                        <td class='rubrik-data' style='border: 1px solid #cccccc; '><div style='padding:10px; '>{0}</div></td>
                        <td class='rubrik-data' style='border: 1px solid #cccccc; '><div style='padding:10px; '>{1}</div></td>
                        <td class='rubrik-data' style='border: 1px solid #cccccc; '><div style='padding:10px; '>{2}</div></td>
                        
                        </tr>
                    ",
                rubrik.RubrikType.Nummer + " - " + rubrik.RubrikType.Navn,
                rubrik.OriginalVaerdi,
                valgteFejl
                );
            }

            template.AppendFormat(@"  </tbody>  </table>");


            var style = new StringBuilder();

            GlobalSettings globalSettings = new GlobalSettings();
            globalSettings.ColorMode = ColorMode.Color;
            globalSettings.Orientation = Orientation.Portrait;
            globalSettings.PaperSize = PaperKind.A4;
            globalSettings.Margins = new MarginSettings { Top = 25, Bottom = 25 };
            WebSettings webSettings = new WebSettings();
            webSettings.DefaultEncoding = "utf-8";
            webSettings.UserStyleSheet = "Static/Utils/PDFStyle.css";
            ObjectSettings objectSettings = new ObjectSettings();
            objectSettings.PagesCount = true;
            objectSettings.WebSettings = webSettings;
            objectSettings.HtmlContent = template.ToString();
            HtmlToPdfDocument htmlToPdfDocument = new HtmlToPdfDocument()
            {
                GlobalSettings = globalSettings,
                Objects = { objectSettings },
            };

            return _converter.Convert(htmlToPdfDocument);
        }
    }
}