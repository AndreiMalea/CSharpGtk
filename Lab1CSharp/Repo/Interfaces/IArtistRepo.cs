using System;
using System.Collections.Generic;
using Lab1CSharp.Domain;

namespace Lab1CSharp.Repo.Interfaces
{
    interface IArtistRepo: IRepository<long, Artist>
    {
        IList<Artist> FilterByName(String name);
        IList<Artist> FilterByGenre(String genre);
    }
}