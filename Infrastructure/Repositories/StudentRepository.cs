using Domain.Entities;
using Infrastructure.Data;
using Infrastructure.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories;

public class StudentRepository(DataContext context) : IBaseRepository<Student, int>
{
    public async Task<int> AddAsync(Student entity)
    {
        await context.Students.AddAsync(entity);
        var result = await context.SaveChangesAsync();
        return result;
    }

    public async Task<int> DeleteAsync(Student entity)
    {
        context.Remove(entity);
        var result = await context.SaveChangesAsync();
        return result;
    }

    public Task<IQueryable<Student>> GetAllAsync()
    {
        var students = context.Students.AsQueryable();
        return Task.FromResult(students);
    }

    public async Task<Student?> GetByIdAsync(int id)
    {
        var student = await context.Students.FindAsync(id);
        return student;
    }

    public async Task<int> UpdateAsync(Student entity)
    {
        context.Update(entity);
        var result = await context.SaveChangesAsync();
        return result;
    }
}