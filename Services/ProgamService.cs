using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Exercicio1.Interfaces;
using Exercicio1.Models;
using MMLib.Extensions;

namespace Exercicio1.Services
{
    public class ProgamService : IProgamService
    {
        public async Task<string> ConvertFromBase64(string base64EncodedData)
        {
            var base64EncodedBytes = System.Convert.FromBase64String(base64EncodedData);
            return System.Text.Encoding.UTF8.GetString(base64EncodedBytes);
        }

        public async Task<List<CSVViewModel>> ReadCSV(string filePath)
        {
            string[] linhasArquivo = System.IO.File.ReadAllLines(filePath);
            var dados = new List<CSVViewModel> {};
            if (linhasArquivo.Any())
            {
                var users = new List<User>();
                foreach (string linha in linhasArquivo)
                {
                    string[] props = linha.Split(',');
                    var user = new User 
                    {
                        Name = props[0],
                        Age = Convert.ToDecimal(props[2]),
                        State = props[1].RemoveDiacritics().ToLower()
                    };
                    users.Add(user);
                }
                var userGroupedByState = users.GroupBy(user => user.State);

                foreach (var group in userGroupedByState)
                {
                    decimal totalAge = 0;
                    foreach (var item in group)
                    {
                        totalAge =  item.Age + totalAge;
                    }
                    var dado = new CSVViewModel
                    {
                        media = Math.Round(totalAge / group.Count(), 2),
                        cidade = group.Key.ToString()
                    };
                    dados.Add(dado);

                }
            }
            return dados;
        }
    }
}
