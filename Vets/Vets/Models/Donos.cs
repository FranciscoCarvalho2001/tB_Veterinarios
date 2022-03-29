﻿namespace Vets.Models
{
    public class Donos
    {
        public Donos()
        {
            ListaAnimais= new HashSet<Animais>();
        }
        /// <summary>
        /// PK para a tabela do Dono
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// Nome do dono do animal
        /// </summary>
        public string Nome { get; set; }
        /// <summary>
        /// NIF do Dono
        /// </summary>
        public string NIF { get; set; }
        /// <summary>
        /// sexo do dono
        /// F-feminino e M-masculino
        /// </summary>

        public string Sexo { get; set; }

        public ICollection<Animais>ListaAnimais { get; set; }
    }
}