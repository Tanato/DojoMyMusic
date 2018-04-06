using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DojoMyMusic.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Distributed;
using Newtonsoft.Json;

namespace DojoMyMusic.Controllers
{
    [Route("api/[controller]")]
    public class MusicaController : Controller
    {
        private readonly ApplicationDbContext db;
        private readonly IDistributedCache cache;
        public MusicaController(ApplicationDbContext db,
           IDistributedCache cache)
        {
            this.db = db;
            this.cache = cache;
        }

        [HttpGet("{filtro}")]
        [ProducesResponseType(typeof(IEnumerable<Musica>), 200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(204)]
        public async Task<IActionResult> Get(string filtro)
        {
            if (filtro.Length < 3)
                return BadRequest("Favor invormar mais de 3 caracteres!");

            var result = await db.Musicas.Include(x => x.Artista)
                        .Where(x => x.Nome.Contains(filtro) || x.Artista.Nome.Contains(filtro))
                        .OrderBy(x => x.Artista.Nome).ThenBy(x => x.Nome)
                        .ToListAsync();

            if (result == null)
                return NoContent();
            else
                return Ok(result);
        }

        // Método com cache no Redis
        [HttpGet("Cached/{filtro}")]
        [ProducesResponseType(typeof(IEnumerable<Musica>), 200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(204)]
        public async Task<IActionResult> GetCached(string filtro)
        {
            if (filtro.Length < 3)
                return BadRequest("Favor invormar mais de 3 caracteres!");

            IEnumerable<Musica> result;

            var value = await this.cache.GetAsync(filtro);
            if (value != null)
            {
                result = JsonConvert.DeserializeObject<IEnumerable<Musica>>(Encoding.ASCII.GetString(value));
            }
            else
            {
                result = await db.Musicas.Include(x => x.Artista)
                .Where(x => x.Nome.Contains(filtro) || x.Artista.Nome.Contains(filtro))
                .OrderBy(x => x.Artista.Nome).ThenBy(x => x.Nome)
                .ToListAsync();

                var cacheEntryOptions = new DistributedCacheEntryOptions().SetSlidingExpiration(TimeSpan.FromMinutes(10));
                await this.cache.SetAsync(filtro, Encoding.ASCII.GetBytes(JsonConvert.SerializeObject(result)), cacheEntryOptions);
            }

            if (result == null)
                return NoContent();
            else
                return Ok(result);
        }
    }
}
