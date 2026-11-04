using Microsoft.Extensions.DependencyInjection;
using MyWebApp.Models;
using System;
using System.Linq;

namespace MyWebApp.Data
{
    public static class DbInitializer
    {
        public static void Initialize(IServiceProvider serviceProvider)
        {
            using (var context = serviceProvider.GetRequiredService<SchoolContext>())
            {
                Initialize(context);
            }
        }

        public static void Initialize(SchoolContext context)
        {
            context.Database.EnsureCreated();

            if (context.Majors.Any()) return; // Đã có dữ liệu thì không seed lại

            var majors = new Major[]
            {
                new Major { MajorName = "IT" },
                new Major { MajorName = "Economics" },
                new Major { MajorName = "Mathematics" },
                new Major { MajorName = "Electronics" }
            };
            foreach (var m in majors) context.Majors.Add(m);
            context.SaveChanges();

            var itId = majors[0].MajorID;
            var ecoId = majors[1].MajorID;
            var mathId = majors[2].MajorID;
            var eeId = majors[3].MajorID;

            var learners = new Learner[]
            {
                new Learner { FirstMidName = "Carson", LastName = "Alexander", EnrollmentDate = DateTime.Parse("2024-09-01"), MajorID = itId },
                new Learner { FirstMidName = "Meredith", LastName = "Alonso", EnrollmentDate = DateTime.Parse("2023-09-01"), MajorID = ecoId },
                new Learner { FirstMidName = "Arturo", LastName = "Anand", EnrollmentDate = DateTime.Parse("2024-09-01"), MajorID = mathId },
                new Learner { FirstMidName = "Gytis", LastName = "Barzdukas", EnrollmentDate = DateTime.Parse("2023-09-01"), MajorID = eeId },
                new Learner { FirstMidName = "Yan", LastName = "Li", EnrollmentDate = DateTime.Parse("2024-09-01"), MajorID = itId },
                new Learner { FirstMidName = "Peggy", LastName = "Justice", EnrollmentDate = DateTime.Parse("2023-09-01"), MajorID = ecoId },
                new Learner { FirstMidName = "Laura", LastName = "Norman", EnrollmentDate = DateTime.Parse("2024-09-01"), MajorID = mathId },
                new Learner { FirstMidName = "Nino", LastName = "Olivetto", EnrollmentDate = DateTime.Parse("2024-09-01"), MajorID = itId },
                new Learner { FirstMidName = "Hải Nam", LastName = "Nguyễn", EnrollmentDate = DateTime.Parse("2024-09-01"), MajorID = itId },
                new Learner { FirstMidName = "Minh Tú", LastName = "Trần", EnrollmentDate = DateTime.Parse("2023-09-01"), MajorID = ecoId },
                new Learner { FirstMidName = "Hoàng Phong", LastName = "Lê", EnrollmentDate = DateTime.Parse("2024-09-01"), MajorID = mathId },
                new Learner { FirstMidName = "Xuân Mai", LastName = "Vũ", EnrollmentDate = DateTime.Parse("2024-09-01"), MajorID = eeId }
            };
            foreach (var l in learners) context.Learners.Add(l);
            context.SaveChanges();

            var courses = new Course[]
            {
                new Course { CourseID = 1050, Title = "Chemistry", Credits = 3 },
                new Course { CourseID = 4022, Title = "Microeconomics", Credits = 3 },
                new Course { CourseID = 4041, Title = "Macroeconomics", Credits = 3 }
            };
            foreach (var c in courses) context.Courses.Add(c);
            context.SaveChanges();

            var enrollments = new Enrollment[]
            {
                new Enrollment { LearnerID = learners[0].LearnerID, CourseID = 1050, Grade = 5.5f },
                new Enrollment { LearnerID = learners[0].LearnerID, CourseID = 4022, Grade = 7.5f },
                new Enrollment { LearnerID = learners[1].LearnerID, CourseID = 1050, Grade = 3.5f }
            };
            foreach (var e in enrollments) context.Enrollments.Add(e);
            context.SaveChanges();
        }
    }
}
