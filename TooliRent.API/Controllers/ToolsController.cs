using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TooliRent.Application.Dto.Tool;
using TooliRent.Application.Services.Interfaces;

namespace TooliRent_project.Controllers
{
    //[Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class ToolsController : ControllerBase
    {
        private readonly IToolService _service;

        public ToolsController(IToolService service)
        {
            _service = service;
        }

        // GET: api/tools
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ToolDto>>> GetAllTools()
        {
            var tools = await _service.GetAllToolAsync();
            return Ok(tools);
        }

        // GET: api/tools/5
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(ToolDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<ToolDto>> GetToolById(int id)
        {
            var tool = await _service.GetToolByIdAsync(id);
            if (tool == null)
            {
                return NotFound(new { Message = $"Tool med IDt {id} existerar inte." });
            }
            return Ok(tool);
        }

        // GET: api/tools/available
        [HttpGet("available")]
        [ProducesResponseType(typeof(ToolDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<IEnumerable<ToolDto>>> GetAvailableTools()
        {
            var tools = await _service.GetAvailableToolAsync();
            return Ok(tools);
        }

        // GET: api/tools/category/1
        [HttpGet("category/{categoryId}")]
        [ProducesResponseType(typeof(ToolDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<IEnumerable<ToolDto>>> GetToolsByCategory(int categoryId)
        {
            var tools = await _service.GetToolByCategoryAsync(categoryId);
            if (!tools.Any())
            {
                return NotFound($"Inga verktyg hittades för kategori med IDt {categoryId}.");
            }
            return Ok(tools);
        }

        // GET: api/tools/filter?status=Available&categoryId=1
        [HttpGet("filter")]
        public async Task<ActionResult<IEnumerable<ToolDto>>> FilterTool([FromQuery] string status, [FromQuery] int? categoryId)
        {
            var tools = await _service.FilterToolAsync(status, categoryId);
            if (!tools.Any())
            {
                return NotFound($"Inga verktyg hittades för det filter villkoret.");
            }
            return Ok(tools);
        }

        // POST: api/tools
        [HttpPost]
        //[Authorize(Roles = "Admin")]
        [ProducesResponseType(typeof(ToolDto), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<ToolDto>> CreateTool([FromBody] CreateToolDto dto)
        {
            var tool = await _service.CreateToolAsync(dto);
            return CreatedAtAction(nameof(GetToolById), new { id = tool.Id }, tool);
        }

        // PUT: api/tools/5
        [HttpPut("{id}")]
        //[Authorize(Roles = "Admin")]
        [ProducesResponseType(typeof(ToolDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<ToolDto>> UpdateTool(int id, [FromBody] UpdateToolDto dto)
        {
            var tool = await _service.UpdateToolAsync(id, dto);
            return Ok(tool);
        }

        // DELETE: api/tools/5
        [HttpDelete("{id}")]
        //[Authorize(Roles = "Admin")]
        public async Task<ActionResult> DeleteTool(int id)
        {
            var deleted = await _service.DeleteToolAsync(id);
            if(!deleted)
            {
                return NotFound(new { Message = $"Tool med IDt {id} existerar inte." });
            }
            return NoContent();
        }
    }
}
