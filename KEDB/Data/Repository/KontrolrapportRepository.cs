using KEDB.Data.Interface;
using KEDB.Dto;
using KEDB.Model;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KEDB.Data.Repository
{
    public class KontrolrapportRepository : IKontrolrapportRepository
    {
        private readonly KEDBContext _context;

        double OprindelseslandRubrikNummer = 34;
        double ProcedurekodeRubrikNummer = 37;

        public KontrolrapportRepository(KEDBContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Kontrolrapport>> GetAll(KontrolrapportSearchFilterDto search)
        {
            var kontrolrapporter = _context.Kontrolrapporter
               .Include(kr => kr.Rubrikker
                   .Where(r => r.RubrikType.Nummer.Equals(OprindelseslandRubrikNummer) || r.RubrikType.Nummer.Equals(ProcedurekodeRubrikNummer)))
               .ThenInclude(r => r.RubrikType)
               .AsQueryable();

            kontrolrapporter = SearchReferencenummer(search, kontrolrapporter);
            kontrolrapporter = SearchVareModtagerCVR(search, kontrolrapporter);
            kontrolrapporter = SearchKlarererCVR(search, kontrolrapporter);
            kontrolrapporter = SearchToldsted(search, kontrolrapporter);
            kontrolrapporter = SearchAntagetTilDato(search, kontrolrapporter);
            kontrolrapporter = SearchAntagetFraDato(search, kontrolrapporter);
            kontrolrapporter = SearchProfilnummer(search, kontrolrapporter);
            kontrolrapporter = SearchBranchekode(search, kontrolrapporter);
            kontrolrapporter = SearchWorkzoneJournalnummer(search, kontrolrapporter);
            kontrolrapporter = SearchOprindelsesland(search, kontrolrapporter);
            kontrolrapporter = SearchAfrapporteretTilDato(search, kontrolrapporter);
            kontrolrapporter = SearchAfrapporteretFraDato(search, kontrolrapporter);
            kontrolrapporter = SearchProcedurekode(search, kontrolrapporter);
            kontrolrapporter = SearchSagsbehandler(search, kontrolrapporter);

            return await kontrolrapporter.ToListAsync();
        }

        public IEnumerable<Kontrolrapport> GetAllByYear(int year)
        {
            var result = _context.Kontrolrapporter
                    .Include(k => k.Rubrikker)
                    .ThenInclude(r => r.RubrikValgteFejl)
                    .Include(kr => kr.AndreUregelmaessigheder)
                    .Where(k => k.AntagetDato.Year.Equals(year)).AsSplitQuery();
            //.Where(k => k.AfrapporteretDato != null);

            return result;
        }

        public async Task<Kontrolrapport> GetById(int id)
        {
            var kontrolrapporter = _context.Kontrolrapporter;

            string kontrolrapportProfilNummer = kontrolrapporter.Select(af => af.Profilnummer)
                .FirstOrDefault();

            //henter Kontrolrapport og alle tilhørende rubrikker
            var kontrolrapport = kontrolrapporter
                .Include(kr => kr.AndreUregelmaessigheder)
                .Include(kr => kr.Toldrapport.ToldrapportKommunikation)
                .Include(kr => kr.Toldrapport.ToldrapportOvertraedelsesAktoer)
                .Include(kr => kr.Toldrapport.ToldrapportOpdagendeAktoer)
                .Include(kr => kr.Toldrapport.ToldrapportFejlKategori)
                .Include(kr => kr.Toldrapport.ToldrapportTransportmiddel)
                .Include(kr => kr.Rubrikker.OrderBy(r => r.RubrikType.Nummer))
                    .ThenInclude(r => r.RubrikType)
                    .ThenInclude(rt => rt.RubrikMuligeFejl.OrderBy(rmf => rmf.Fejltekst.Tekst)
                        .Where(rmf => rmf.Profil.ProfilNummer.Equals(kontrolrapportProfilNummer)))
                    .ThenInclude(rmf => rmf.Fejltekst)
                .Include(af => af.Rubrikker)
                    .ThenInclude(r => r.RubrikValgteFejl)
                    .ThenInclude(rf => rf.Fejltekst)
                .Where(af => af.Id == id)
                .FirstOrDefaultAsync();

            return await kontrolrapport;
        }

        public async Task<List<int>> GetToldsteder()
        {
            var kontrolrapporter = _context.Kontrolrapporter;

            var toldsteder = kontrolrapporter
                .Where(kr => !kr.AfrapporteretDato.HasValue)
                .Select(af => af.Toldsted)
                .Distinct();

            return await toldsteder.ToListAsync();

        }

        public async Task<Kontrolrapport> Add(Kontrolrapport kontrolrapport)
        {
            kontrolrapport.IndlaestDate = DateTime.Today;

            await _context.Kontrolrapporter.AddAsync(kontrolrapport);
            await _context.SaveChangesAsync();

            return kontrolrapport;
        }

        public async Task<Kontrolrapport> Update(Kontrolrapport kontrolrapport)
        {
            if (kontrolrapport.AfrapporteretDato.Equals(null))
            {
                kontrolrapport.AfrapporteretDato = DateTime.Today;
            }
            kontrolrapport.RedigeretDato = DateTime.Today;

            _context.Entry(kontrolrapport).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            return kontrolrapport;
        }

        private static IQueryable<Kontrolrapport> SearchReferencenummer(KontrolrapportSearchFilterDto searchFilter, IQueryable<Kontrolrapport> kontrolrapporter)
        {
            if (!string.IsNullOrEmpty(searchFilter.Referencenummer))
            {
                kontrolrapporter = kontrolrapporter.Where(af => af.Referencenummer.Equals(searchFilter.Referencenummer));
            }

            return kontrolrapporter;
        }

        private static IQueryable<Kontrolrapport> SearchVareModtagerCVR(KontrolrapportSearchFilterDto searchFilter, IQueryable<Kontrolrapport> kontrolrapporter)
        {
            if (searchFilter.VaremodtagerCVR.HasValue)
            {
                kontrolrapporter = kontrolrapporter.Where(af => af.VaremodtagerCVR == searchFilter.VaremodtagerCVR);
            }

            return kontrolrapporter;
        }

        private static IQueryable<Kontrolrapport> SearchKlarererCVR(KontrolrapportSearchFilterDto search, IQueryable<Kontrolrapport> kontrolrapporter)
        {
            if (search.KlarererCVR.HasValue)
            {
                kontrolrapporter = kontrolrapporter.Where(af => af.KlarererCVR == search.KlarererCVR);
            }

            return kontrolrapporter;
        }

        private static IQueryable<Kontrolrapport> SearchToldsted(KontrolrapportSearchFilterDto search, IQueryable<Kontrolrapport> kontrolrapporter)
        {
            if (search.Toldsted.HasValue)
            {
                kontrolrapporter = kontrolrapporter.Where(af => af.Toldsted == search.Toldsted);
            }

            return kontrolrapporter;
        }

        private static IQueryable<Kontrolrapport> SearchAntagetTilDato(KontrolrapportSearchFilterDto search, IQueryable<Kontrolrapport> kontrolrapporter)
        {
            if (search.AntagetTilDato.HasValue)
            {
                kontrolrapporter = kontrolrapporter.Where(af => af.AntagetDato <= search.AntagetTilDato);
            }

            return kontrolrapporter;
        }

        private static IQueryable<Kontrolrapport> SearchAntagetFraDato(KontrolrapportSearchFilterDto search, IQueryable<Kontrolrapport> kontrolrapporter)
        {
            if (search.AntagetFraDato.HasValue)
            {
                kontrolrapporter = kontrolrapporter.Where(af => af.AntagetDato >= search.AntagetFraDato);
            }

            return kontrolrapporter;
        }

        private static IQueryable<Kontrolrapport> SearchProfilnummer(KontrolrapportSearchFilterDto search, IQueryable<Kontrolrapport> kontrolrapporter)
        {
            if (!string.IsNullOrEmpty(search.Profilnummer))
            {
                kontrolrapporter = kontrolrapporter.Where(af => af.Profilnummer.Equals(search.Profilnummer));
            }

            return kontrolrapporter;
        }

        private static IQueryable<Kontrolrapport> SearchBranchekode(KontrolrapportSearchFilterDto search, IQueryable<Kontrolrapport> kontrolrapporter)
        {
            if (search.Branchekode.HasValue)
            {
                kontrolrapporter = kontrolrapporter.Where(af => af.Branchekode == search.Branchekode);
            }

            return kontrolrapporter;
        }

        private static IQueryable<Kontrolrapport> SearchWorkzoneJournalnummer(KontrolrapportSearchFilterDto search, IQueryable<Kontrolrapport> kontrolrapporter)
        {
            if (!string.IsNullOrEmpty(search.WorkzoneJournalnummer))
            {
                kontrolrapporter = kontrolrapporter.Where(af => af.WorkzoneJournalnummer.Equals(search.WorkzoneJournalnummer));
            }

            return kontrolrapporter;
        }

        private IQueryable<Kontrolrapport> SearchOprindelsesland(KontrolrapportSearchFilterDto search, IQueryable<Kontrolrapport> kontrolrapporter)
        {
            if (!string.IsNullOrEmpty(search.Oprindelsesland))
            {
                kontrolrapporter = kontrolrapporter
                    .Where(af => af.Rubrikker
                        .Where(r => r.RubrikType.Nummer.Equals(OprindelseslandRubrikNummer))
                        .Any(r => r.OriginalVaerdi.Equals(search.Oprindelsesland))
                    );
            }

            return kontrolrapporter;
        }

        private static IQueryable<Kontrolrapport> SearchAfrapporteretTilDato(KontrolrapportSearchFilterDto search, IQueryable<Kontrolrapport> kontrolrapporter)
        {
            if (search.AfrapporteretTilDato.HasValue)
            {
                kontrolrapporter = kontrolrapporter.Where(af => af.AfrapporteretDato <= search.AfrapporteretTilDato);
            }

            return kontrolrapporter;
        }

        private static IQueryable<Kontrolrapport> SearchAfrapporteretFraDato(KontrolrapportSearchFilterDto search, IQueryable<Kontrolrapport> kontrolrapporter)
        {
            if (search.AfrapporteretFraDato.HasValue)
            {
                kontrolrapporter = kontrolrapporter.Where(af => af.AfrapporteretDato >= search.AfrapporteretFraDato);
            }

            return kontrolrapporter;
        }

        private IQueryable<Kontrolrapport> SearchProcedurekode(KontrolrapportSearchFilterDto search, IQueryable<Kontrolrapport> kontrolrapporter)
        {
            if (!string.IsNullOrEmpty(search.Procedurekode))
            {
                kontrolrapporter = kontrolrapporter
                    .Where(af => af.Rubrikker
                        .Where(r => r.RubrikType.Nummer.Equals(ProcedurekodeRubrikNummer))
                        .Any(r => r.OriginalVaerdi.Equals(search.Procedurekode))
                    );
            }

            return kontrolrapporter;
        }

        private IQueryable<Kontrolrapport> SearchSagsbehandler(KontrolrapportSearchFilterDto search, IQueryable<Kontrolrapport> kontrolrapporter)
        {
            if (!string.IsNullOrEmpty(search.Sagsbehandler))
            {
                var sagsbehandlerId1 = Guid.Parse(search.Sagsbehandler).ToString();
                var sagsbehandlerId2 = Guid.Parse(search.Sagsbehandler).ToString("N");

                kontrolrapporter = kontrolrapporter
                    .Where(
                    kontrolrapport =>
                    kontrolrapport.Sagsbehandler.StartsWith(sagsbehandlerId1) || kontrolrapport.Sagsbehandler.StartsWith(sagsbehandlerId2));
            }

            return kontrolrapporter;
        }
    }
}