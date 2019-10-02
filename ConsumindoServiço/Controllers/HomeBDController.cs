using ConsumindoServiço.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace ConsumindoServiço.Controllers
{
    public class HomeBDController : Controller
    {  
        ConectaApi connection = new ConectaApi("api/repositorio/");    
        List<Cliente> clientes = new List<Cliente>();
        Cliente cliente = new Cliente();
                       
        public async Task<ActionResult> Index()
        {
            List<Cliente> clientes = new List<Cliente>();

            try
            {
                clientes = await connection.getClientes();
            }

            catch (Exception e)
            {
                Console.WriteLine("{0} Exception caught.", e);
            }
            return View(clientes);
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";
            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";
            return View();
        }

        //GET: Incluir
        public ActionResult Incluir(string nome, string email)
        {
            ViewBag.Message = "Incluir um novo registro";

            Cliente cliente = new Cliente();
            return View(cliente);
        }

        //POST: incluir
        [HttpPost]
        public ActionResult Incluir(Cliente cliente)
         {
            if (ModelState.IsValid)
            {
                connection.RespostaPost(cliente);

                if (connection.response.IsSuccessStatusCode)

                {
                    return RedirectToAction("Index");
                }
            }

            else
            {
                return View(cliente);
            }

            return View();
        }

        //GET: edit
        public async Task<ActionResult> Edit(int id)
        {
            ViewBag.Message = "Altere os dados do cliente";

            if (ModelState.IsValid)
            {
                cliente = await connection.getCliente(id);
                return View(cliente);
            }
            return View();
        }
        
        //PUT: edit
        [HttpPost]
        public ActionResult Edit(Cliente cliente)
        {
            if (ModelState.IsValid)
            {
                connection.RespostaPut(cliente);
                return RedirectToAction("Index");
            }
            else
            {
                return View(cliente);
            }
        }
        
        //GET: deletar
        public async Task<ActionResult> Deletar(int id)
        {
            ViewBag.Message = "Delete um registro";

            if (ModelState.IsValid)
            {
                cliente = await connection.getCliente(id);
            }

            return View(cliente);
        }
        
        //POST: deletar
        [HttpPost]
        public ActionResult Deletar(Cliente cliente)
        {
            if (ModelState.IsValid)
            {
                connection.RespostaDelete(cliente);
                return RedirectToAction("Index");
            }

            else
            {
                return View(cliente);
            }
        }
    }
}