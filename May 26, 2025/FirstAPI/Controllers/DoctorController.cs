using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("/api/[controller]")]
public class DoctorController : ControllerBase
{
    static List<Doctor> doctors = new List<Doctor>
    {
        new Doctor{Id=101,Name="Ramu", Specialization="Cardiology"},
        new Doctor{Id=102,Name="Somu", Specialization="Neurology"},
    };
    [HttpGet]
    public ActionResult<IEnumerable<Doctor>> GetDoctors()
    {
        return Ok(doctors);
    }
    [HttpPost]
    public ActionResult<Doctor> PostDoctor([FromBody] Doctor doctor)
    {
        doctors.Add(doctor);
        return Created("", doctor);
    }
    [HttpPut]
    public ActionResult<Doctor> PutDoctor([FromBody] Doctor doctor)
    {
        var existingDoctor = doctors.FirstOrDefault(d => d.Id == doctor.Id);
        if (existingDoctor == null)
        {
            return NotFound();
        }
        existingDoctor.Name = doctor.Name;
        existingDoctor.Specialization = doctor.Specialization;
        if (string.IsNullOrEmpty(existingDoctor.Name) || string.IsNullOrEmpty(existingDoctor.Specialization))
        {
            return BadRequest("Doctor name and specialization cannot be empty.");
        }
        return Ok(existingDoctor);
    }
    [HttpDelete]
    public ActionResult DeleteDoctor(int id)
    {
        var doctor = doctors.FirstOrDefault(d => d.Id == id);
        if (doctor == null)
        {
            return NotFound();
        }
        doctors.Remove(doctor);
        return Ok("Doctor deleted successfully");
    }
}