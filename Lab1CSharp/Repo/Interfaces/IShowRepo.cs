using System;
using System.Collections.Generic;
using Lab1CSharp.Domain;

namespace Lab1CSharp.Repo.Interfaces
{
    interface IShowRepo: IRepository<long, Show>
    {
        IList<Show> FilterByArtist(long artist);
        IList<Show> FilterByDate(DateTime dateTime);
    }
}