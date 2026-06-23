using Academico.Common.Interfaces;
using Academico.Data;
using Academico.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Academico.Repository
{
    public class EstudianteRepository : IGenericRepository<Estudiante>
    {
        private readonly ApplicationDbContext _context;

        public EstudianteRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Estudiante> CrearAsync(Estudiante entity)
        {
            try
            {
                await _context.Estudiantes.AddAsync(entity);
                await _context.SaveChangesAsync();
                return entity;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<Estudiante> ActualizarAsync(Estudiante entity)
        {
            try
            {
                var estudiante = await ObtenerPorIdAsync(entity.Id);
                if (estudiante == null) return null;

                estudiante.Cedula = entity.Cedula;
                estudiante.Nombre = entity.Nombre;
                estudiante.PrimerApellido = entity.PrimerApellido;
                estudiante.SegundoApellido = entity.SegundoApellido;
                estudiante.CorreoElectronico = entity.CorreoElectronico;
                estudiante.Telefono = entity.Telefono;

                _context.Estudiantes.Update(estudiante);
                await _context.SaveChangesAsync();
                return estudiante;
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
                var estudiante = await ObtenerPorIdAsync(id);
                if (estudiante == null) return false;

                //borrado logico
                estudiante.Estado = false;
                _context.Estudiantes.Update(estudiante);
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<Estudiante> ObtenerPorIdAsync(int id)
        {
            try
            {
                return await _context.Estudiantes
                    .AsNoTracking()
                    .Include(e => e.Matriculas)
                    .FirstOrDefaultAsync(e => e.Id == id && e.Estado == true);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<List<Estudiante>> ObtenerTodosAsync()
        {
            try
            {
                return await _context.Estudiantes
                    .AsNoTracking()
                    .Include(e => e.Matriculas)
                    .Where(e => e.Estado == true)
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        //metodos agregados para validaciones especificas
        public async Task<Estudiante> ObtenerPorCedulaAsync(string cedula)
        {
            try
            {
                return await _context.Estudiantes
                    .AsNoTracking()
                    .FirstOrDefaultAsync(e => e.Cedula == cedula && e.Estado == true);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<Estudiante> ObtenerPorCorreoAsync(string correo)
        {
            try
            {
                if (string.IsNullOrEmpty(correo)) return null;
                return await _context.Estudiantes
                    .AsNoTracking()
                    .FirstOrDefaultAsync(e => e.CorreoElectronico == correo && e.Estado == true);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<bool> ExisteCedulaAsync(string cedula, int? idExcluir = null)
        {
            try
            {
                var query = _context.Estudiantes
                    .Where(e => e.Cedula == cedula && e.Estado == true);

                if (idExcluir.HasValue)
                    query = query.Where(e => e.Id != idExcluir.Value);

                return await query.AnyAsync();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<bool> ExisteCorreoAsync(string correo, int? idExcluir = null)
        {
            try
            {
                if (string.IsNullOrEmpty(correo)) return false;

                var query = _context.Estudiantes
                    .Where(e => e.CorreoElectronico == correo && e.Estado == true);

                if (idExcluir.HasValue)
                    query = query.Where(e => e.Id != idExcluir.Value);

                return await query.AnyAsync();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}