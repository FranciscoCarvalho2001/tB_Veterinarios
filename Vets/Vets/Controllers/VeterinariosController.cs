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

        public VeterinariosController(ApplicationDbContext context)
        {
            _context = context;
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
                    //temos ficheiro e é uma imagem
                }
            }

            if (ModelState.IsValid)
            {
                _context.Add(veterinario);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
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
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var veterinarios = await _context.Veterinarios.FindAsync(id);
            _context.Veterinarios.Remove(veterinarios);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool VeterinariosExists(int id)
        {
            return _context.Veterinarios.Any(e => e.Id == id);
        }
    }
}
