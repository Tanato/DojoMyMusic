using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DojoMyMusic.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DojoMyMusic.Controllers
{
    [Route("api/[controller]")]
    public class MusicasController : Controller
    {
        private readonly ApplicationDbContext db;
        public MusicasController(ApplicationDbContext db)
        {
            this.db = db;
        }

        [HttpGet("{filter}")]
        public IEnumerable<Musica> Get(string filter)
        {
            var result = db.Musicas.Include(x => x.Artista)
                .Where(x => x.Nome.Contains(filter) || x.Artista.Nome.Contains(filter))
                .OrderBy(x => x.Artista.Nome).ThenBy(x => x.Nome)
                .ToList();

            return result;
        }
    }
}
