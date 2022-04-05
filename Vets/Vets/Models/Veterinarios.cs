using System.ComponentModel.DataAnnotations;

namespace Vets.Models
{
    public class Veterinarios
    {
        /// <summary>
        /// modulo que interage com os dados dos veterinarios
        /// </summary>
        public Veterinarios()
        {
            ListaConsultas = new HashSet<Consultas>();
        }
        /// <summary>
        /// PK para cada um dos registos da tabela
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// Nome do veterinário
        /// </summary>
        public string Nome { get; set; }
        /// <summary>
        /// nº da cédula profissional
        /// </summary>
        public string NumCedulaProf { get; set; }
        /// <summary>
        /// nome do ficheiro
        /// </summary>
        public string Fotografia { get; set; }
        /// <summary>
        /// lista de consultas feitas pelo veterinário
        /// </summary>
        public ICollection<Consultas>ListaConsultas { get; set; }
    }
}
