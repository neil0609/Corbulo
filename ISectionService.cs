using Sabio.Web.Domain;
using Sabio.Web.Models.Requests;
using Sabio.Web.Models.Requests.Sections;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sabio.Web.Services.Interfaces
{
    public interface ISectionService
    {
        void Update(int sectionId, SectionsUpdateRequest model);

        int Create(string userId, SectionsAddRequest model);

        List<Section> GetAll();

        void UpdateInfo(int id, SectionDescriptionUpdate model);

        void PutLocation(int id, SectionLocation model);

        void DeleteInstructor(int instrutorId, int Id);

        List<SectionInstructors> GetChosenInstructors(int id);

        void addSectionInstructors(int instrutorId,int Id);

        List<SectionEnrollment> Get(int id);

        void InsertSectionStudent(string id, int sectionId);

        void DeleteSectionSudent(string userId, int id);
       
        Section GetSection(int id);

        List<Section> GetSections();

        void Delete(int id);

        int AddUserSection(UserSectionAddRequest model, string userId);

        List<Campus> GetCampuses();
       
        Section GetSectionDetailsByUserId(string UserId);

        List<Section> GetSectionsByCourseId(int id);

        List<UserSection> GetSectionsByUserId(string UserId);
       
    }
}
