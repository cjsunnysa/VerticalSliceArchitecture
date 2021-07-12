using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace VerticalSliceArchitecture.Api.Features.Courses
{
    [Route("[controller]")]
    [ApiController]
    public class CoursesController : ControllerBase
    {
        private readonly IMediator _mediator;

        public CoursesController(IMediator mediator) => _mediator = mediator;

        [HttpGet]
        public async Task<GetAll.Response> GetAll(CancellationToken cancellationToken) => await _mediator.Send(new GetAll.Query(), cancellationToken);

        [HttpGet("{id}")]
        public async Task<GetById.Response> GetById(int Id, CancellationToken cancellationToken) => await _mediator.Send(new GetById.Query { Id = Id }, cancellationToken);
    }
}
