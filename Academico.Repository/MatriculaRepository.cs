using Academico.Common.Interfaces;
using Academico.Data;
using Academico.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Academico.Repository
{
    public class MatriculaRepository : IGenericRepository<Matricula>
    {
        private readonly ApplicationDbContext _context;

        public MatriculaRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Matricula> CrearAsync(Matricula entity)
        {
            try
            {
                await _context.Matriculas.AddAsync(entity);
                await _context.SaveChangesAsync();
                return entity;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        //no se usa porque no tiene DTO
        public async Task<Matricula> ActualizarAsync(Matricula entity)
        {
            throw new NotImplementedException();
        }

        //no se usa por requerimentos
        public async Task<bool> EliminarAsync(int id)
        {
            try
            {
                var matricula = await ObtenerPorIdAsync(id);
                if (matricula == null) return false;

                //borrado logico
                matricula.Estado = false;
                _context.Matriculas.Update(matricula);
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<Matricula> ObtenerPorIdAsync(int id)
        {
            try
            {
                return await _context.Matriculas
                    .AsNoTracking()
                    .Include(m => m.Estudiante)
                    .Include(m => m.Curso)
                    .FirstOrDefaultAsync(m => m.Id == id && m.Estado == true);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<List<Matricula>> ObtenerTodosAsync()
        {
            try
            {
                return await _context.Matriculas
                    .AsNoTracking()
                    .Include(m => m.Estudiante)
                    .Include(m => m.Curso)
                    .Where(m => m.Estado == true)
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        //metodos agregados para validaciones
        public async Task<bool> ExisteMatriculaAsync(int estudianteId, int cursoId)
        {
            try
            {
                return await _context.Matriculas
                    .AnyAsync
                    (m => m.EstudianteId == estudianteId &&
                     m.CursoId == cursoId &&
                     m.Estado == true);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<int> ObtenerCupoOcupadoAsync(int cursoId)
        {
            try
            {
                return await _context.Matriculas
                    .CountAsync(m => m.CursoId == cursoId && m.Estado == true);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}