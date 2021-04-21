using System.Collections.Generic;

namespace Lab1CSharp.Repo.Interfaces
{
    interface IRepository<TId, TE>
    {
        TE FindOne(TId id);
        IDictionary<TId, TE> FindAll();
        IList<TE> GetAllList();
        TE Save(TE entity);
        TE Delete(TId id);
        TE Update(TE entity);
    }
}