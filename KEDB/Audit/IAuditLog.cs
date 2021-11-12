using System.Threading.Tasks;

namespace KEDB.Audit
{
    public interface IAuditLog
    {
        Task Log(UserAction userAction);
    }
}
