using Academico.Common.Interfaces;
using Academico.Data;
using Academico.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Academico.Repository
{
    public class CursoRepository : IGenericRepository<Curso>
    {
        private readonly ApplicationDbContext _context;

        public CursoRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Curso> CrearAsync(Curso entity)
        {
            try
            {
                await _context.Cursos.AddAsync(entity);
                await _context.SaveChangesAsync();
                return entity;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<Curso> ActualizarAsync(Curso entity)
        {
            try
            {
                var curso = await ObtenerPorIdAsync(entity.Id);
                if (curso == null) return null;

                curso.Nombre = entity.Nombre;
                curso.Descripcion = entity.Descripcion;
                curso.CupoMaximo = entity.CupoMaximo;

                _context.Cursos.Update(curso);
                await _context.SaveChangesAsync();
                return curso;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<bool> EliminarAsync(int id)
        {
            try
            {
                var curso = await ObtenerPorIdAsync(id);
                if (curso == null) return false;

                //borrado logico
                curso.Estado = false;
                _context.Cursos.Update(curso);
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<Curso> ObtenerPorIdAsync(int id)
        {
            try
            {
                return await _context.Cursos
                    .AsNoTracking()
                    .Include(c => c.Matriculas)
                    .FirstOrDefaultAsync(c => c.Id == id && c.Estado == true);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<List<Curso>> ObtenerTodosAsync()
        {
            try
            {
                return await _context.Cursos
                    .AsNoTracking()
                    .Include(c => c.Matriculas)
                    .Where(c => c.Estado == true)
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        //metodos agregados
        public async Task<bool> ExisteNombreAsync(string nombre, int? idExcluir = null)
        {
            try
            {
                var query = _context.Cursos
                    .Where(c => c.Nombre.ToLower() == nombre.ToLower() && c.Estado == true);

                if (idExcluir.HasValue)
                    query = query.Where(c => c.Id != idExcluir.Value);

                return await query.AnyAsync();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<int> ObtenerCantidadMatriculadosAsync(int cursoId)
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