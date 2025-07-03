using Microsoft.AspNetCore.Mvc;
using FirstAPI.Models;

namespace FirstAPI.Controllers
{
    [ApiController]
    [Route("/api/[controller]")]
    public class PatientController : ControllerBase
    {
        static List<Patient> patients = new List<Patient>
    {
        new Patient{Id=201,Name="Wani", Age=25},
        new Patient{Id=202,Name="Hari", Age=30},
    };
        [HttpGet]
        public ActionResult<IEnumerable<Patient>> GetPatients()
        {
            return Ok(patients);
        }
        [HttpPost]
        public ActionResult<Patient> PostPatient([FromBody] Patient patient)
        {
            patients.Add(patient);
            return Created("", patient);
        }
        [HttpPut]
        public ActionResult<Doctor> PutDoctor([FromBody] Patient patient)
        {
            var existingPatient = patients.FirstOrDefault(p => p.Id == patient.Id);
            if (existingPatient == null)
            {
                return NotFound();
            }
            existingPatient.Name = patient.Name;
            return Ok(existingPatient);
        }
        [HttpDelete]
        public ActionResult DeleteDoctor(int id)
        {
            var patient = patients.FirstOrDefault(p => p.Id == id);
            if (patient == null)
            {
                return NotFound();
            }
            patients.Remove(patient);
            return Ok("Patient deleted successfully");
        }
    }
}