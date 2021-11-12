using System.Collections.Generic;

namespace KEDB.Dto.Statistik
{
    public class Top10
    {
        public List<VirksomhedCount> Virksomheder { get; set; }

        public Top10()
        {
            Virksomheder = new List<VirksomhedCount>();
        }

        public void Add(VirksomhedCount vc)
        {
            Virksomheder.Add(vc);
        }
    }
}
