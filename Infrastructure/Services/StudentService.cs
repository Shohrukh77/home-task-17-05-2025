using System.Net;
using AutoMapper;
using Domain.DTOs.Student;
using Domain.Entities;
using Domain.Filters;
using Domain.Responces;
using Domain.Responses;
using Infrastructure.Data;
using Infrastructure.Interfaces;

namespace Infrastructure.Services;

public class StudentService(IBaseRepository<Student, int> studservice, DataContext context, IMapper mapper)
{
    public async Task<Response<GetStudentDto>> CreateAsync(CreateStudentDto request)
    {
        var student = mapper.Map<Student>(request);
        var result = await studservice.AddAsync(student);
        var getStud = mapper.Map<GetStudentDto>(student);
        return result == 0
            ? new Response<GetStudentDto>(HttpStatusCode.BadRequest, "Student not added!")
            : new Response<GetStudentDto>(getStud);
    }

    public async Task<Response<string>> DeleteAsync(int Id)
    {
        var stud = await studservice.GetByIdAsync(Id);
        if (stud == null)
        {
            return new Response<string>(HttpStatusCode.BadRequest, "Student not found");
        }

        var res = await studservice.DeleteAsync(stud);
        return res == 0
            ? new Response<string>(HttpStatusCode.BadRequest, "Student not deleted!")
            : new Response<string>("Student  deleted!");
    }

    public async Task<Response<List<GetStudentDto>>> GetAllAsync(StudentFilter filter)
    {

        try
        {
            var validFilter = new ValidFilter(filter.PagesNumber, filter.PageSize);

            var students = await studservice.GetAllAsync();

            if (filter.Name != null)
            {
                students = students.Where(s => s.Name.ToLower().Contains(filter.Name.ToLower()));
            }

            var maped = mapper.Map<List<GetStudentDto>>(students);

            var totalRecords = maped.Count;

            var data = maped
                .Skip((validFilter.PageNumber - 1) * validFilter.PageSize)
                .Take(validFilter.PageSize)
                .ToList();

            return new PagedResponse<List<GetStudentDto>>(data, validFilter.PageNumber, validFilter.PageSize,
                totalRecords);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }

    public async Task<Response<GetStudentDto>> GetTaskAsync(int Id)
    {

        var exist = await studservice.GetByIdAsync(Id);
        if (exist == null)
        {
            return new Response<GetStudentDto>(HttpStatusCode.NotFound, "student not found!");
        }

        var studentDto = mapper.Map<GetStudentDto>(exist);
        return new Response<GetStudentDto>(studentDto);
    }

    public async Task<Response<GetStudentDto>> UpDateAsync(int Id, UpdateStudentDto request)
    {
        var exist = await studservice.GetByIdAsync(Id);
        if (exist == null)
        {
            return new Response<GetStudentDto>(HttpStatusCode.BadRequest, "Student not found");
        }

        exist.Name = request.Name;
        exist.Email = request.Email;
        var result = await studservice.UpdateAsync(exist);
        var student = mapper.Map<GetStudentDto>(exist);
        return result == 0
            ? new Response<GetStudentDto>(HttpStatusCode.BadRequest, "Student not updated!")
            : new Response<GetStudentDto>(student);

    }
}
