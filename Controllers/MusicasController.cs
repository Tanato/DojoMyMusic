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
    public class MusicasController : Controller
    {
        private readonly ApplicationDbContext db;
        private readonly IDistributedCache cache;
        public MusicasController(ApplicationDbContext db,
           IDistributedCache cache)
        {
            this.db = db;
            this.cache = cache;
        }

        [HttpGet("{filter}")]
        [ProducesResponseType(typeof(IEnumerable<Musica>), 200)]
        [ProducesResponseType(400)]
        public async Task<IActionResult> Get(string filter)
        {
            if (filter.Length < 3)
                return BadRequest("Favor invormar mais de 3 caracteres!");

            return Ok(await db.Musicas.Include(x => x.Artista)
            .Where(x => x.Nome.Contains(filter) || x.Artista.Nome.Contains(filter))
            .OrderBy(x => x.Artista.Nome).ThenBy(x => x.Nome)
            .ToListAsync());
        }

        // Método com cache no Redis
        [HttpGet("Cached/{filter}")]
        [ProducesResponseType(typeof(IEnumerable<Musica>), 200)]
        [ProducesResponseType(400)]
        public async Task<IActionResult> GetCached(string filter)
        {
            if (filter.Length < 3)
                return BadRequest("Favor invormar mais de 3 caracteres!");

            IEnumerable<Musica> result;

            var value = await this.cache.GetAsync(filter);
            if (value != null)
            {
                result = JsonConvert.DeserializeObject<IEnumerable<Musica>>(Encoding.ASCII.GetString(value));
            }
            else
            {
                result = await db.Musicas.Include(x => x.Artista)
                .Where(x => x.Nome.Contains(filter) || x.Artista.Nome.Contains(filter))
                .OrderBy(x => x.Artista.Nome).ThenBy(x => x.Nome)
                .ToListAsync();

                var cacheEntryOptions = new DistributedCacheEntryOptions().SetSlidingExpiration(TimeSpan.FromMinutes(10));
                await this.cache.SetAsync(filter, Encoding.ASCII.GetBytes(JsonConvert.SerializeObject(result)), cacheEntryOptions);
            }

            return Ok(result);
        }
    }
}
