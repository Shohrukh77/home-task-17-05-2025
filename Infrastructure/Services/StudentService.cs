using System.Net;
using AutoMapper;
using Domain.DTOs.Student;
using Domain.Entities;
using Domain.Filters;
using Domain.Responses;
using Infrastructure.Data;
using Infrastructure.Interfaces;
using Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Infrastructure.Services;

public class StudentService(IBaseRepository<Student, int> studentRepository, IMapper mapper, ILogger<StudentService> logger) : IStudentService
{
    public async Task<Response<List<GetStudentDto>>> GetAllAsync(StudentFilter filter)
    {
        try
        {
            logger.LogInformation("GetAllAsync started with filter: {@Filter}", filter);
            var validFilter = new ValidFilter(filter.PageNumber, filter.PageSize);

            var students = await studentRepository.GetAllAsync();

            if (filter.Name != null)
            {
                students = students.Where(s => string.Concat(s.FirstName, " ", s.LastName).ToLower().Contains(filter.Name.ToLower()));
            }

            if (filter.From != null) // f = 20 - 100
            {
                var year = DateTime.UtcNow.Year;
                students = students.Where(s => year - s.BirthDate.Year >= filter.From);
            }

            if (filter.To != null) // f = 20 , to = 45
            {
                var year = DateTime.UtcNow.Year;
                students = students.Where(s => year - s.BirthDate.Year <= filter.To);
            }

            var mapped = mapper.Map<List<GetStudentDto>>(await students.ToListAsync());

            var totalRecords = mapped.Count;

            var data = mapped
                .Skip((validFilter.PageNumber - 1) * validFilter.PageSize)
                .Take(validFilter.PageSize)
                .ToList();

            logger.LogInformation("GetAllAsync succeeded, total: {Count}", totalRecords);

            return new PagedResponse<List<GetStudentDto>>(data, validFilter.PageNumber, validFilter.PageSize,
                totalRecords);
        }
        catch (Exception e)
        {
            logger.LogError(e, "Error in GetAllAsync");
            Console.WriteLine(e);
            throw;
        }
    }
    public async Task<Response<GetStudentDto>> GetByIdAsync(int id)
    {
        logger.LogInformation("GetByIdAsync called with id: {Id}", id);
        var student = await studentRepository.GetByIdAsync(id);
        if (student == null)
        {
            logger.LogWarning("Student not found with id: {Id}", id);
            return new Response<GetStudentDto>(HttpStatusCode.BadRequest, "Student not found");
        }

        var getStudentDto = mapper.Map<GetStudentDto>(student);

        return new Response<GetStudentDto>(getStudentDto);
    }

    public async Task<Response<GetStudentDto>> CreateAsync(CreateStudentDto request)
    {
        try
        {
            logger.LogInformation("CreateAsync called with: {@Request}", request);
            var student = mapper.Map<Student>(request);

            student.BirthDate = request.BirthDate!.Value.ToUniversalTime();

            var result = await studentRepository.AddAsync(student);

            var getStudentDto = mapper.Map<GetStudentDto>(student);

            return result == 0
                ? new Response<GetStudentDto>(HttpStatusCode.BadRequest, "Student not created")
                : new Response<GetStudentDto>(getStudentDto);
        }
        catch (Exception e)
        {
            logger.LogError(e, "Error in CreateAsync");
            Console.WriteLine(e.InnerException.Message);
            throw;
        }
    }

    public async Task<Response<GetStudentDto>> UpdateAsync(int id, UpdateStudentDto request)
    {
        logger.LogInformation("Обновление студента с ID: {Id}, данные: {@Request}", id, request);
        var existingStudent = await studentRepository.GetByIdAsync(id);
        if (existingStudent == null)
        {
            logger.LogWarning("Студент с ID {Id} не найден для обновления", id);
            return new Response<GetStudentDto>(HttpStatusCode.BadRequest, "Student not found");
        }

        existingStudent.FirstName = request.FirstName;
        existingStudent.LastName = request.LastName;
        existingStudent.BirthDate = request.BirthDate!.Value.ToUniversalTime();

        var result = await studentRepository.UpdateAsync(existingStudent);


        var getStudentDto = mapper.Map<GetStudentDto>(existingStudent);

        return result == 0
            ? new Response<GetStudentDto>(HttpStatusCode.BadRequest, "Student not updated")
            : new Response<GetStudentDto>(getStudentDto);
    }

    public async Task<Response<string>> DeleteAsync(int id)
    {
        logger.LogInformation("Попытка удалить студента с ID: {Id}", id);
        var student = await studentRepository.GetByIdAsync(id);

        if (student == null)
        {
            logger.LogWarning("Студент с ID {Id} не найден для удаления", id);
            return new Response<string>(HttpStatusCode.NotFound, "Student not found");
        }

        var result = await studentRepository.DeleteAsync(student);
        return result == 0
            ? new Response<string>(HttpStatusCode.BadRequest, "Student not deleted")
            : new Response<string>("Student deleted successfully");
    }
}