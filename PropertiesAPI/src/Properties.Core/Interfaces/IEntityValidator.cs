using System.Collections.Generic;
using System.Threading.Tasks;

namespace Properties.Core.Interfaces
{
    public interface IEntityValidator<T>
    {
        Task<IEnumerable<string>> ValidatePropertyAsync(T entity);
    }
}