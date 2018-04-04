using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace DojoMyMusic.Controllers
{
    [Route("api/[controller]")]
    public class MusicasController : Controller
    {
        [HttpGet("{filter}")]
        public IEnumerable<Musica> Get(string filter)
        {
            var artistaGuid = Guid.NewGuid();

            var result = new List<Musica>();
            result.Add(new Musica
            {
                Id = Guid.NewGuid(),
                Nome = "Two Minutes to Midnigth",
                ArtistaId = artistaGuid,
                Artista = new Artista { Id = artistaGuid, Nome = "Iron Maiden" }
            });
            result.Add(new Musica
            {
                Id = Guid.NewGuid(),
                Nome = "Aces High",
                ArtistaId = artistaGuid,
                Artista = new Artista { Id = artistaGuid, Nome = "Iron Maiden" }
            });
            result.Add(new Musica
            {
                Id = Guid.NewGuid(),
                Nome = "Fear of the Dark",
                ArtistaId = artistaGuid,
                Artista = new Artista { Id = artistaGuid, Nome = "Iron Maiden" }
            });
            return result.Where(x => x.Nome.Contains(filter) || x.Artista.Nome.Contains(filter));
        }
    }
}
