using System.ComponentModel.DataAnnotations;

namespace Vets.Models
{
    /// <summary>
    /// Representa os dados do Dono de um Animal
    /// </summary>
    public class Donos
    {
        public Donos()
        {
            ListaAnimais = new HashSet<Animais>();
        }

        /// <summary>
        /// PK para a tabela dos Donos
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// Nome do Dono do animal
        /// </summary>
        [Required(ErrorMessage = "O {0} é de preenchimento obrigatório")]
        [StringLength(30, ErrorMessage = "O {0} não pode ter mais de {1} caracteres.")]
        [RegularExpression("[A-ZÂÓa-záéíóúàèìòùâêîôûäëïöü]+",ErrorMessage ="No {0} são só aceites letras")]
        public string Nome { get; set; }
        /// <summary>
        /// NIF do Dono do animal
        /// </summary>
        [Required(ErrorMessage = "O {0} é de preenchimento obrigatório")]
        [StringLength(9, MinimumLength = 9, ErrorMessage = "O {0} deve ter {1} caracteres")]
        [RegularExpression("[1234578]+[0-9][9]",ErrorMessage ="O {0} deve começar por 1, 2, 3")]
        public string NIF { get; set; }
        /// <summary>
        /// sexo do dono
        /// Ff - feminino; Mm - masculino
        /// </summary>
        [StringLength(1, ErrorMessage = "O {0} só aceita um carácter")]
        [RegularExpression("[FfMm]", ErrorMessage = "Apenas letras F ou M")]
        public string Sexo { get; set; }
        /// <summary>
        /// Lista de animais de quem o Dono é dono
        /// </summary>
        [EmailAddress(ErrorMessage ="Introduza um emailcorreto por favor")]
        public string Email { get; set; }
        /// <summary>
        /// Email
        /// </summary>
        public ICollection<Animais> ListaAnimais { get; set; }
    }
}
