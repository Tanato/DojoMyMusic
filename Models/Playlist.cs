using System;
using System.Collections.Generic;

namespace DojoMyMusic
{
    public class Playlist
    {

        public Guid Id { get; set; }
        public List<Musica> Musicas { get; set; }
        public Guid UsuarioId { get; set; }
    }
}