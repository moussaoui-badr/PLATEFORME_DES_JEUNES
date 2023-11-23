using Domain.Entities;
using Repository.Data;
using Repository.IRepositories;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.Repositories
{
    public class ExerciceRepository : IExerciceRepository
    {

        private readonly ApplicationDbContext _dbContext;

        public ExerciceRepository(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<List<Chapitre>> GetListChapitres()
        {
            var result = _dbContext.Chapitres.AsQueryable();
            
            return await result.ToListAsync();
        }
    }
}
