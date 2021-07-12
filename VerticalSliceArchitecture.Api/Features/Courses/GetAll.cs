using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using VerticalSliceArchitecture.Api.Data;

namespace VerticalSliceArchitecture.Api.Features.Courses
{
    public class GetAll
    {
        public record Query : IRequest<Response>
        { }

        public record Response
        {
            public IEnumerable<Course> Courses { get; init; }

            public record Course
            {
                public int CourseId { get; init; }
                public string Title { get; init; }
                public int Credits { get; init; }
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
                var courses = await
                    _dbContext
                        .Courses
                        .ProjectTo<Response.Course>(_configurationProvider)
                        .ToListAsync(cancellationToken);

                return new Response
                {
                    Courses = courses
                };
            }
        }
    }
}
