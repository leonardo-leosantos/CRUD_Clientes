using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ConsumindoServiço.Models
{
    public class Cliente
    {
        [Required(ErrorMessage = "O campo nome é obrigatório.")]
        public int ID { get; set; }
        public string Nome { get; set; }
        public string Email { get; set; }
        public bool Ativo { get; set; }

        //public Cliente()
        //{
        //    ID = 0;
        //}

        //public Cliente( int id)
        //{
        //    ID = id;
        //}

        //public Cliente(int id,string nome,string email,bool ativo)
        //{
        //    ID = id;
        //    Nome = nome;
        //    Email = email;
        //    Ativo = false;
        //}
    }
}