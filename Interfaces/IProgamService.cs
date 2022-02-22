using Exercicio1.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Exercicio1.Interfaces
{
    public interface IProgamService
    {
        Task<string> ConvertFromBase64(string arquivo);

        Task <List<CSVViewModel>> ReadCSV(string filePath);
    }
}
