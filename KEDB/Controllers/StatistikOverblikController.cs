using KEDB.Data.Interface;
using KEDB.Dto.Statistik;
using KEDB.Model;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;

namespace KEDB.Controllers
{
    [Route("api/[controller]")]
    // [Authorize(Roles = "kedb-super, kedb-read")]
    [ApiController]
    public class StatistikOverblikController : ControllerBase
    {
        private readonly IKontrolrapportRepository _kontrolrapportRepository;

        public StatistikOverblikController(IKontrolrapportRepository kontrolrapportRepository)
        {
            _kontrolrapportRepository = kontrolrapportRepository;
        }

        [HttpGet("{year}")]
        public ActionResult<StatistikOverblikViewModel> StatistikOverblik(int year)
        {
            IEnumerable<Kontrolrapport> afrapporteredeKontrolrapporter = _kontrolrapportRepository.GetAllByYear(year).ToList();


            if (afrapporteredeKontrolrapporter == null)
            {
                return NotFound();
            }

            IEnumerable<Kontrolrapport> kontrolrapporterMedFejl = getKontrolrapporterMedFejl(afrapporteredeKontrolrapporter);

            StatistikOverblikViewModel statistikOverblik = new StatistikOverblikViewModel();

            statistikOverblik.AfledteEffekter = udregnAfledteEffekter(afrapporteredeKontrolrapporter);
            statistikOverblik.Reguleringer = udregnReguleringer(afrapporteredeKontrolrapporter);
            statistikOverblik.AntalKontrollerOgFejl = udregnAntalKontrollerOgFejl(afrapporteredeKontrolrapporter);
            statistikOverblik.Top10VaremodtagereMedFejl = modtagereMedFejl(afrapporteredeKontrolrapporter, kontrolrapporterMedFejl);
            statistikOverblik.Top10KlarererMedFejl = klarererMedFejl(afrapporteredeKontrolrapporter, kontrolrapporterMedFejl);
            statistikOverblik.Top10VaremodtagereUdtaget = modtagereUdtaget(afrapporteredeKontrolrapporter, kontrolrapporterMedFejl);
            statistikOverblik.Top10KlarererUdtaget = klarererUdtaget(afrapporteredeKontrolrapporter, kontrolrapporterMedFejl);
            statistikOverblik.Reguleringer = reguleringerKalender(afrapporteredeKontrolrapporter, statistikOverblik.Reguleringer);

            return statistikOverblik;
        }

        private AfledteEffekter udregnAfledteEffekter(IEnumerable<Kontrolrapport> kontrolrapporter)
        {
            AfledteEffekter resultat = new AfledteEffekter();
            resultat.SendtTilAnalyseCount = kontrolrapporter.Where(kr => kr.OversendtTilAnalyse).Count();
            resultat.OversendtTilToldrapportCount = kontrolrapporter.Where(kr => kr.OversendtTilToldrapport).Count();

            var SendtTilAndreUregelmaessigheder = kontrolrapporter.Where(kr => kr.AndreUregelmaessigheder != null);
            resultat.SendtTilAndreUregelmaessighederCount = SendtTilAndreUregelmaessigheder.Where(kr => kr.AndreUregelmaessigheder.Andet || kr.AndreUregelmaessigheder.Doping || kr.AndreUregelmaessigheder.IPRvarer || kr.AndreUregelmaessigheder.Narkotika || kr.AndreUregelmaessigheder.Vaaben).Count();

            int antalKontrolRapporterUdtaget = kontrolrapporter.Count(); //total udtaget til kontrol

            if (antalKontrolRapporterUdtaget > 0)
            {
                resultat.SendtTilAnalysePercent = Math.Round(((double)resultat.SendtTilAnalyseCount / (double)antalKontrolRapporterUdtaget) * 100, 2);
                resultat.OversendtTilToldrapportPercent = (resultat.OversendtTilToldrapportCount / antalKontrolRapporterUdtaget) * 100;
                resultat.OversendtTilToldrapportPercent = Math.Round(((double)resultat.OversendtTilToldrapportCount / (double)antalKontrolRapporterUdtaget) * 100, 2);
                resultat.SendtTilAndreUregelmaessighederPercent = Math.Round(((double)resultat.SendtTilAndreUregelmaessighederCount / (double)antalKontrolRapporterUdtaget) * 100, 2);
            }

            return resultat;
        }

        private Reguleringer udregnReguleringer(IEnumerable<Kontrolrapport> kontrolrapporter)
        {
            Reguleringer reguleringer = new Reguleringer();
            reguleringer.ReguleringCount = kontrolrapporter.Where(kr => (kr.ToldmaessigAendringOpkraevning > 0) || (kr.ToldmaessigAendringTilbagebetaling > 0)).Count();
            reguleringer.ReguleringOpkreavet = kontrolrapporter.Where(kr => kr.ToldmaessigAendringOpkraevning > 0).Select(kr => kr.ToldmaessigAendringOpkraevning).Sum();
            reguleringer.ReguleringTilbagebetalt = kontrolrapporter.Where(kr => kr.ToldmaessigAendringTilbagebetaling > 0).Select(kr => kr.ToldmaessigAendringTilbagebetaling).Sum();

            return reguleringer;
        }

        private AntalKontrollerOgFejl udregnAntalKontrollerOgFejl(IEnumerable<Kontrolrapport> kontrolrapporter)
        {
            AntalKontrollerOgFejl resultat = new AntalKontrollerOgFejl();

            var kontrollerMedAndreFejl = kontrolrapporter
                .Where(
                        kr =>
                        kr.AndreUregelmaessigheder != null &&
                        (kr.ToldmaessigAendringOpkraevning > 0 ||
                        kr.ToldmaessigAendringTilbagebetaling > 0 ||
                        kr.OversendtTilAnalyse ||
                        kr.OversendtTilToldrapport)
                    )
                    .Where(
                        kr => kr.AndreUregelmaessigheder.Andet ||
                        kr.AndreUregelmaessigheder.Doping ||
                        kr.AndreUregelmaessigheder.IPRvarer ||
                        kr.AndreUregelmaessigheder.Narkotika ||
                        kr.AndreUregelmaessigheder.Vaaben
                    ); //Antal afrapporterede med Andre uregelmæssigheder

            IEnumerable<Rubrik> rubrikkerMedFejl = kontrolrapporter.SelectMany(ak => ak.Rubrikker).Where(r => r.RubrikValgteFejl.Count > 0).ToList();   //finder alle rubrikker hvor der er valgt fejl

            var result = kontrolrapporter.Where(rapport => rapport.Rubrikker.Any(rubrik => rubrik.RubrikValgteFejl.Any()));

            var kombineretListeMedFejl = result.Union(kontrollerMedAndreFejl);  //Laver en kombineret liste af kontrolrapporter med andre fejl og kotrolrapporter med fejl i en rubrik uden dublikater

            var totalMedFejlCount = kombineretListeMedFejl.Count();     //Total antal af kontrolrapporter med en eller flere fejl

            var afrapporteredeKontrolrapporterCount = kontrolrapporter.Where(kr => kr.AfrapporteretDato != null).Count(); //Antal af afrapporterede kontrolrapporter

            int totalUdtagetCount = kontrolrapporter.Count();   //total antal af vareposter udtaget til kontrol. Både afrapporterede og ikke afrapporterede

            double percentFejl = 0;     //procent af de afrapporterede kontrolrapporter med fejl

            if (afrapporteredeKontrolrapporterCount > 0)
            {
                percentFejl = Math.Round(((double)totalMedFejlCount / (double)afrapporteredeKontrolrapporterCount) * 100, 2);
            }

            resultat.ProcentMedFejl = percentFejl;
            resultat.TotalUdtagetTilKontrol = totalUdtagetCount;
            resultat.TotalAfrapporteretMedFejl = totalMedFejlCount;
            resultat.TotalAfrapporteretUdenFejl = afrapporteredeKontrolrapporterCount - totalMedFejlCount;
            resultat.TotalAfrapporteret = afrapporteredeKontrolrapporterCount;

            return resultat;
        }

        private Reguleringer reguleringerKalender(IEnumerable<Kontrolrapport> kontrolrapporter, Reguleringer reguleringer)
        {
            var beloebPerMaaned =
                kontrolrapporter
                .Where(kr => (kr.ToldmaessigAendringTilbagebetaling > 0) || (kr.ToldmaessigAendringOpkraevning > 0))
                .GroupBy(kr => kr.AntagetDato.Month)
                .OrderBy(g => g.Key)
                .Select(g => new
                {
                    Tilbagebetalt = g.Sum(rapport => rapport.ToldmaessigAendringTilbagebetaling),
                    Opkrævet = g.Sum(rapport => rapport.ToldmaessigAendringOpkraevning)
                });

            reguleringer.tilbagebetalt = beloebPerMaaned.Select(x => x.Tilbagebetalt).ToArray();
            reguleringer.opkravet = beloebPerMaaned.Select(x => x.Opkrævet).ToArray();

            return reguleringer;
        }

        private Top10 modtagereMedFejl(IEnumerable<Kontrolrapport> kontrolrapporter, IEnumerable<Kontrolrapport> kontrolrapporterMedFejl)
        {
            var modtagereUdtaget = kontrolrapporterMedFejl.GroupBy(i => new { i.VaremodtagerCVR, i.VaremodtagerNavn }).ToList()
           .Select(g => new
           {
               g.Key.VaremodtagerCVR,
               g.Key.VaremodtagerNavn,
               AntalMedFejl = g.Count(),

           }).OrderByDescending(a => a.AntalMedFejl).Take(10).ToList();

            var modtageresKontrolrapporter = kontrolrapporterMedFejl.Where(p => modtagereUdtaget.Any(p2 => p2.VaremodtagerCVR == p.VaremodtagerCVR)).ToList();  //finder kontrolrapporter der høre til modtagerne

            Top10 top10 = new Top10();

            foreach (var modtager in modtagereUdtaget)
            {
                VirksomhedCount v = new VirksomhedCount();
                v.ReguleringTilbagebetalt = modtageresKontrolrapporter.Where(kr => (kr.ToldmaessigAendringTilbagebetaling > 0) && (kr.VaremodtagerCVR.Equals(modtager.VaremodtagerCVR))).Sum(kr => kr.ToldmaessigAendringTilbagebetaling);
                v.ReguleringOpkreavet = modtageresKontrolrapporter.Where(kr => (kr.ToldmaessigAendringOpkraevning > 0) && (kr.VaremodtagerCVR.Equals(modtager.VaremodtagerCVR))).Sum(kr => kr.ToldmaessigAendringOpkraevning);
                v.AntalMedFejl = modtager.AntalMedFejl;
                v.Antal = kontrolrapporter.Where(kr => kr.VaremodtagerCVR.Equals(modtager.VaremodtagerCVR)).Count();
                v.AntalUdenFejl = v.Antal - v.AntalMedFejl;
                v.Navn = modtager.VaremodtagerNavn;
                v.CVR = modtager.VaremodtagerCVR;
                v.AndelAfSamlet = ((double)v.AntalMedFejl / (double)kontrolrapporter.Count()) * 100;
                top10.Virksomheder.Add(v);
            }

            return top10;
        }

        private IEnumerable<Kontrolrapport> getKontrolrapporterMedFejl(IEnumerable<Kontrolrapport> kontrolrapporter)
        {
            var kontrollerMedAndreFejl = kontrolrapporter
                .Where(
                        kr =>
                        kr.AndreUregelmaessigheder != null &&
                        (kr.ToldmaessigAendringOpkraevning > 0 ||
                        kr.ToldmaessigAendringTilbagebetaling > 0 ||
                        kr.OversendtTilAnalyse ||
                        kr.OversendtTilToldrapport)
                    )
                    .Where(
                        kr => kr.AndreUregelmaessigheder.Andet ||
                        kr.AndreUregelmaessigheder.Doping ||
                        kr.AndreUregelmaessigheder.IPRvarer ||
                        kr.AndreUregelmaessigheder.Narkotika ||
                        kr.AndreUregelmaessigheder.Vaaben
                    ).ToList(); //Antal afrapporterede med Andre uregelmæssigheder

            IEnumerable<Rubrik> rubrikkerMedFejl = kontrolrapporter.SelectMany(ak => ak.Rubrikker).Where(r => r.RubrikValgteFejl.Count > 0).ToList();   //finder alle rubrikker hvor der er valgt fejl

            var result = kontrolrapporter.Where(rapport => rapport.Rubrikker.Any(rubrik => rubrik.RubrikValgteFejl.Any()));

            var kombineretListeMedFejl = result.Union(kontrollerMedAndreFejl).ToList();  //Laver en kombineret liste af kontrolrapporter med andre fejl og kotrolrapporter med fejl i en rubrik uden dublikater

            return kombineretListeMedFejl;
        }

        private Top10 modtagereUdtaget(IEnumerable<Kontrolrapport> kontrolrapporter, IEnumerable<Kontrolrapport> kontrolrapporterMedFejl)
        {
            var modtagereUdtaget = kontrolrapporter.GroupBy(i => new { i.VaremodtagerCVR, i.VaremodtagerNavn }).ToList()
           .Select(g => new
           {
               g.Key.VaremodtagerCVR,
               g.Key.VaremodtagerNavn,
               Antal = g.Count(),

           }).OrderByDescending(a => a.Antal).Take(10).ToList();

            var modtageresKontrolrapporter = kontrolrapporter.Where(p => modtagereUdtaget.Any(p2 => p2.VaremodtagerCVR == p.VaremodtagerCVR)).ToList();  //finder kontrolrapporter der hører til modtagerne

            Top10 top10 = new Top10();

            foreach (var modtager in modtagereUdtaget)
            {
                VirksomhedCount v = new VirksomhedCount();
                var antalMedFejl = kontrolrapporterMedFejl.Where(kr => kr.VaremodtagerCVR.Equals(modtager.VaremodtagerCVR)).Count();

                v.ReguleringTilbagebetalt = modtageresKontrolrapporter.Where(kr => (kr.ToldmaessigAendringTilbagebetaling > 0) && (kr.VaremodtagerCVR.Equals(modtager.VaremodtagerCVR))).Sum(kr => kr.ToldmaessigAendringTilbagebetaling);
                v.ReguleringOpkreavet = modtageresKontrolrapporter.Where(kr => (kr.ToldmaessigAendringOpkraevning > 0) && (kr.VaremodtagerCVR.Equals(modtager.VaremodtagerCVR))).Sum(kr => kr.ToldmaessigAendringOpkraevning);
                v.AntalMedFejl = antalMedFejl;
                v.Antal = kontrolrapporter.Where(kr => kr.VaremodtagerCVR.Equals(modtager.VaremodtagerCVR)).Count();
                v.AntalUdenFejl = v.Antal - v.AntalMedFejl;
                v.Navn = modtager.VaremodtagerNavn;
                v.CVR = modtager.VaremodtagerCVR;
                v.AndelAfSamlet = ((double)v.AntalMedFejl / (double)kontrolrapporter.Count()) * 100;
                top10.Virksomheder.Add(v);
            }
            return top10;
        }

        private Top10 klarererUdtaget(IEnumerable<Kontrolrapport> kontrolrapporter, IEnumerable<Kontrolrapport> kontrolrapporterMedFejl)
        {
            //liste over 10 varemodtagere der optræder flest gange
            var klarererUdtaget = kontrolrapporter.GroupBy(i => new { i.KlarererCVR, i.KlarererNavn }).ToList()
                .Select(g => new
                {
                    g.Key.KlarererCVR,
                    g.Key.KlarererNavn,
                    Antal = g.Count(),
                }).OrderByDescending(a => a.Antal).Take(10).ToList();


            var modtageresKontrolrapporter = kontrolrapporter.Where(p => klarererUdtaget.Any(p2 => p2.KlarererCVR == p.VaremodtagerCVR)).ToList();  //finder kontrolrapporter der høre til modtagerne

            Top10 top10 = new Top10();

            foreach (var modtager in klarererUdtaget)
            {
                VirksomhedCount v = new VirksomhedCount();
                var antalMedFejl = kontrolrapporterMedFejl.Where(kr => kr.KlarererCVR.Equals(modtager.KlarererCVR)).Count();

                v.ReguleringTilbagebetalt = modtageresKontrolrapporter.Where(kr => (kr.ToldmaessigAendringTilbagebetaling > 0) && (kr.KlarererCVR.Equals(modtager.KlarererCVR))).Sum(kr => kr.ToldmaessigAendringTilbagebetaling);
                v.ReguleringOpkreavet = modtageresKontrolrapporter.Where(kr => (kr.ToldmaessigAendringOpkraevning > 0) && (kr.KlarererCVR.Equals(modtager.KlarererCVR))).Sum(kr => kr.ToldmaessigAendringOpkraevning);
                v.AntalMedFejl = antalMedFejl;
                v.Antal = kontrolrapporter.Where(kr => kr.KlarererCVR.Equals(modtager.KlarererCVR)).Count();
                v.AntalUdenFejl = v.Antal - v.AntalMedFejl;
                v.Navn = modtager.KlarererNavn;
                v.CVR = modtager.KlarererCVR;
                v.AndelAfSamlet = ((double)v.AntalMedFejl / (double)kontrolrapporter.Count()) * 100;
                top10.Virksomheder.Add(v);
            }
            return top10;
        }

        private Top10 klarererMedFejl(IEnumerable<Kontrolrapport> kontrolrapporter, IEnumerable<Kontrolrapport> kontrolrapporterMedFejl)
        {

            var klarererUdtaget = kontrolrapporterMedFejl.GroupBy(i => new { i.KlarererCVR, i.KlarererNavn }).ToList()
           .Select(g => new
           {
               g.Key.KlarererCVR,
               g.Key.KlarererNavn,
               AntalMedFejl = g.Count(),

           }).OrderByDescending(a => a.AntalMedFejl).Take(10).ToList();

            var klarererKontrolrapporter = kontrolrapporterMedFejl.Where(p => klarererUdtaget.Any(p2 => p2.KlarererCVR == p.KlarererCVR)).ToList();  //finder kontrolrapporter der høre til klarererne

            Top10 top10 = new Top10();

            foreach (var klarerer in klarererUdtaget)
            {
                VirksomhedCount v = new VirksomhedCount();
                v.ReguleringTilbagebetalt = klarererKontrolrapporter.Where(kr => (kr.ToldmaessigAendringTilbagebetaling > 0) && (kr.KlarererCVR.Equals(klarerer.KlarererCVR))).Sum(kr => kr.ToldmaessigAendringTilbagebetaling);
                v.ReguleringOpkreavet = klarererKontrolrapporter.Where(kr => (kr.ToldmaessigAendringOpkraevning > 0) && (kr.KlarererCVR.Equals(klarerer.KlarererCVR))).Sum(kr => kr.ToldmaessigAendringOpkraevning);
                v.AntalMedFejl = klarerer.AntalMedFejl;
                v.Antal = kontrolrapporter.Where(kr => kr.KlarererCVR.Equals(klarerer.KlarererCVR)).Count();
                v.AntalUdenFejl = v.Antal - v.AntalMedFejl;
                v.Navn = klarerer.KlarererNavn;
                v.CVR = klarerer.KlarererCVR;
                v.AndelAfSamlet = ((double)v.AntalMedFejl / (double)kontrolrapporter.Count()) * 100;
                top10.Virksomheder.Add(v);
            }
            return top10;
        }
    }
}