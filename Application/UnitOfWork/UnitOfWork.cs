using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Application.Repository;
using Domain.Interfaces;
using Persistence.Data;

namespace Application.UnitOfWork
{
    public class UnitOfWork : IUnitOfWork, IDisposable
    {
        private readonly ApiContext _context;
        private PeliculaRepository _peliculas;

        public UnitOfWork(ApiContext context)
        {
            _context = context;
        }

        public IPeliculaRepository Peliculas
        {
            get
            {
                if (_peliculas == null)
                {
                    _peliculas = new PeliculaRepository(_context);
                }
                return _peliculas;
            }
        }
        public async Task<int> SaveAsync()
        {
            return await _context.SaveChangesAsync();
        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }
}