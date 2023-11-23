using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.IRepositories
{
    public interface IExerciceRepository
    {
        Task<List<Chapitre>> GetListChapitres();
    }
}
