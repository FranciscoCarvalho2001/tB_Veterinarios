using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Vets.Data;
using Vets.Models;

namespace Vets.Controllers
{
    public class VeterinariosController : Controller
    {
        private readonly ApplicationDbContext _context;

        private readonly IWebHostEnvironment _webHostEnvironment;

       
        public VeterinariosController(ApplicationDbContext context, IWebHostEnvironment webHostEnvironment)
        {
            _context = context;
            _webHostEnvironment = webHostEnvironment;
        }

        // GET: Veterinarios
        public async Task<IActionResult> Index()
        {
            /*acesso a base de dados
             * SELECT*
             * From Veterinarios
             * 
             * 
             * e, depois estamos a mandar os dados para a view
             */
            return View(await _context.Veterinarios.ToListAsync());
        }

        // GET: Veterinarios/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var veterinarios = await _context.Veterinarios
                .FirstOrDefaultAsync(m => m.Id == id);
            if (veterinarios == null)
            {
                return NotFound();
            }

            return View(veterinarios);
        }

        // GET: Veterinarios/Create
        /// <summary>
        /// usado para o primeiro acesso á view 'create', em modo HTTP GET
        /// </summary>
        /// <returns></returns>
        public IActionResult Create()
        {
            return View();
        }

        // POST: Veterinarios/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        /// <summary>
        /// este modulo é usado para recuperar os dados enviados pelos utilizadores
        /// </summary>
        /// <param name="veterinario"></param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(
            [Bind("Id,Nome,NumCedulaProf,Fotografia")] Veterinarios veterinario,
            IFormFile fotoVet){
            /*
             * Algoritmo para o processar a imagem
             * 
             * se ficheiro imagem nulo
             *   atribuir uma imagem generica ao veteriñário
             * else
             *   será que o ficheiro é uma imagem?
             *   se não for
             *      criar mensagem de erro
             *      devolver o controlo da app á view
             *   else
             *      -definir o nome a atribuir á imagem
             *      -atribuir aos dados do novo vet, o nome do ficheiro da imagem
             *      -guardar a imagem no disco rigido do servidor
             * 
             */

            if(fotoVet == null){
                veterinario.Fotografia = "noVet.png";
            }
            else {
                if(!(fotoVet.ContentType=="image/png" || fotoVet.ContentType == "image/jpg")) {
                    //criar mensagem de erro
                    ModelState.AddModelError("", "Por favor, adicione um ficheiro .png ou .jpg");
                    //devolver o controlo da app á view
                    //fornecendo-lhes os dados que o o utilizador ja tinha preenchido no formulário
                    return View(veterinario);
                }
                else
                {
                    //temos ficheiro e é uma imagem....
                    //+++++++++++++++++++++++++++++++++
                    //defenir nome da foto
                    Guid g =Guid.NewGuid();
                    string nomeFoto=veterinario.NumCedulaProf+ g.ToString();
                    string extensaoFoto=Path.GetExtension(fotoVet.FileName);
                    nomeFoto += extensaoFoto;
                    //atribuir ao vet o nome da sua foto
                    veterinario.Fotografia=nomeFoto;
                }
            }

            if (ModelState.IsValid)
            {
                try
                {
                    //adicionar dados a bd
                    _context.Add(veterinario);
                    //consolidar esses dados
                    await _context.SaveChangesAsync();
                }
                catch (Exception ex)
                {
                    // é da nossa responsabilidade!!! tratarmos da exceção

                    //registar no disco rigido do servidor todos os dados da operação
                    //      -data+hora
                    //      -nome do utilizador
                    //      -nome do controller + método
                    //      -dados do erro (ex)
                    //      -outros dados considerados úteis

                    //eventualmente, tentar guardar na (numa) base de dados os dados do erro

                    //eventualment, notificar o administrador da app do erro

                    // no nosso caso 
                    //criar uma msg do erro
                    ModelState.AddModelError("", "Ocorre um erro com  guardar os dados do veterinario (" + veterinario.Nome + ")");
                    return View(veterinario);
                }
                //+++++++++++++++++++++++++++++++++
                //concretizar a ação de guardar o ficheiro da foto
                //+++++++++++++++++++++++++++++++++
                if (fotoVet != null)
                {
                    //onde o ficheiro vai ser guardado?
                    string nomeLocalizacaoFicheiro = _webHostEnvironment.WebRootPath;
                    nomeLocalizacaoFicheiro = Path.Combine(nomeLocalizacaoFicheiro, "Fotos");
                    //avaliar se a pasta "Fotos" existe
                    if (Directory.Exists(nomeLocalizacaoFicheiro))
                    {
                        Directory.CreateDirectory(nomeLocalizacaoFicheiro);
                    }
                    //nome do documento a guardar
                    string nomeDaFoto = Path.Combine(nomeLocalizacaoFicheiro, veterinario.Fotografia);
                    //criar o objeto que vai manipular o ficheiro
                    using var stream = new FileStream(nomeDaFoto, FileMode.Create);
                    //guadar no disco rigido
                    await fotoVet.CopyToAsync(stream);

                    //devolver o controlo da app á view
                    return RedirectToAction(nameof(Index));
                }
            }
            return View(veterinario);
        }

        // GET: Veterinarios/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var veterinarios = await _context.Veterinarios.FindAsync(id);
            if (veterinarios == null)
            {
                return NotFound();
            }
            return View(veterinarios);
        }

        // POST: Veterinarios/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Nome,NumCedulaProf,Fotografia")] Veterinarios veterinarios)
        {
            if (id != veterinarios.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(veterinarios);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!VeterinariosExists(veterinarios.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(veterinarios);
        }

        // GET: Veterinarios/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var veterinarios = await _context.Veterinarios
                .FirstOrDefaultAsync(m => m.Id == id);
            if (veterinarios == null)
            {
                return NotFound();
            }

            return View(veterinarios);
        }

        // POST: Veterinarios/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult>DeleteConfirmed(int id)
        {
            try { 
                var veterinarios = await _context.Veterinarios.FindAsync(id);
                _context.Veterinarios.Remove(veterinarios);
                await _context.SaveChangesAsync();
                
                //remover o ficheiro com a foto do veterinário

            }
            catch (Exception)
            {
                //throw;
                //não esquecer, tratar da exceção
            }
            return RedirectToAction(nameof(Index));

        }

        private bool VeterinariosExists(int id)
        {
            return _context.Veterinarios.Any(e => e.Id == id);
        }
    }
}
