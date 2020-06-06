using System;

namespace Codenation.Challenge
{
    public class SoccerTeam
    {
      long id { get; set; }
      public string name { get; set; }
      DateTime dataCriacao { get; set; }
      public string corUniformePrincipal { get; set; }
      public string corUniformeSecundario { get; set; }
      public long captainPlayerId { get; set; }

      public SoccerTeam(long id, string name, DateTime dataCriacao, string corUniformePrincipal, string corUniformeSecundario)
      {
        this.id = id;
        this.name = name;
        this.dataCriacao = dataCriacao;
        this.corUniformePrincipal = corUniformePrincipal;
        this.corUniformeSecundario = corUniformeSecundario;
      }
    }
}