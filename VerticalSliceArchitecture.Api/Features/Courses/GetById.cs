using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Threading;
using System.Threading.Tasks;
using VerticalSliceArchitecture.Api.Data;

namespace VerticalSliceArchitecture.Api.Features.Courses
{
    public class GetById
    {
        public record Query : IRequest<Response>
        {
            public int Id { get; init; }
        }

        public record Response
        {
            public Course Item { get; init; }

            public record Course
            {
                public int CourseId { get; init; }
                public string Title { get; init; }
                public int Credits { get; init; }
                public int DepartmentId { get; init; }
                public string DepartmentName { get; init; }
            }
        }

        public class MappingProfile : Profile
        {
            public MappingProfile() => CreateMap<Course, Response.Course>();
        }

        public class Handler : IRequestHandler<Query, Response>
        {
            private readonly UniversityContext _dbContext;
            private readonly IConfigurationProvider _configurationProvider;

            public Handler(UniversityContext dbContext, IConfigurationProvider configurationProvider)
            {
                _dbContext = dbContext;
                _configurationProvider = configurationProvider;
            }

            public async Task<Response> Handle(Query request, CancellationToken cancellationToken)
            {
                var course = await
                    _dbContext
                        .Courses
                        .ProjectTo<Response.Course>(_configurationProvider)
                        .FirstOrDefaultAsync(c => c.CourseId == request.Id, cancellationToken);

                return new Response
                {
                    Item = course
                };
            }
        }
    }
}
