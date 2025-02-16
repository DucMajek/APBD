using Exercise3.Models;
using Exercise3.Models.DTOs;
using Exercise3.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace Exercise3.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StudentsController : ControllerBase
    {
        private readonly IStudentsRepository _studentsRepository;
        public StudentsController(IStudentsRepository studentsRepository)
        {
            _studentsRepository = studentsRepository;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            
            try
            {
                return Ok(_studentsRepository.GetStudents());
            }
            catch (Exception ex) 
            {
                return Problem();
            }

           
        }

        [HttpGet("{index}")]
        public async Task<IActionResult> Get(string index)
        {
                var student = _studentsRepository.GetStudents().FirstOrDefault(e => e.IndexNumber == index);
                if (student is null)
                {
                    return NotFound();
                }
                return Ok(student);
        }

        [HttpPut("{index}")]
        public async Task<IActionResult> Put(string index, StudentPUT newStudentData)
        {
            try
            {
                var studentExist = _studentsRepository.GetStudents().FirstOrDefault(e => e.IndexNumber == index);

                if (studentExist is null)
                {
                    return NotFound();
                }

                var UpdateStudentData = new Student
                {
                    FirstName = newStudentData.FirstName ?? studentExist.FirstName,
                    LastName = newStudentData.LastName ?? studentExist.LastName,
                    IndexNumber = studentExist.IndexNumber,
                    BirthDate = newStudentData.BirthDate ?? studentExist.BirthDate,
                    StudyName = newStudentData.StudyName ?? studentExist.StudyName,
                    StudyMode = newStudentData.StudyMode ?? studentExist.StudyMode,
                    Email = newStudentData.Email ?? studentExist.Email,
                    FathersName = newStudentData.FathersName ?? studentExist.FathersName,
                    MothersName = newStudentData.MothersName ?? studentExist.MothersName
                };

               

                await _studentsRepository.UpdateStudent(studentExist, UpdateStudentData);
                return Ok(UpdateStudentData);
            }
            catch (Exception ex) { return Problem(); }
   
        }

        [HttpPost]
        public async Task<IActionResult> Post(StudentPOST newStudent)
        {

            try
            {

                if (_studentsRepository.GetStudents().Any(s => s.IndexNumber == newStudent.IndexNumber))
                {
                    return Conflict();
                }

                Student NewStudent;
                await _studentsRepository.AddStudent(NewStudent = new Student()
                {
                    FirstName = newStudent.FirstName,
                    LastName = newStudent.LastName,
                    IndexNumber = newStudent.IndexNumber,
                    BirthDate = newStudent.BirthDate,
                    StudyName = newStudent.StudyName,
                    StudyMode = newStudent.StudyMode,
                    Email = newStudent.Email,
                    FathersName = newStudent.FathersName,
                    MothersName = newStudent.MothersName
                });



                return Created("Student created", NewStudent);

            }
            catch (Exception ex) { return Problem(); }
            
        }

        [HttpDelete("{index}")]
        public async Task<IActionResult> Delete(string index)
        {
            try
            {
                var student = _studentsRepository.GetStudents().Where(e => e.IndexNumber == index).FirstOrDefault();
                
                if (student is null)
                {
                    return NotFound();
                }

                await _studentsRepository.DeleteStudent(student);
                

                return Ok(student);
            }
            catch (Exception ex){ return Problem(); }
        }

    }
}
