using Microsoft.AspNetCore.Mvc;
using test_new.Models;
using test_new.Data;
using test_new.Filter;
using test_new.Entities;
using test_new.Authorization;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.JsonPatch;
using System.Linq.Expressions;

namespace test_new.Controllers
{
    /// <summary>
    /// Controller responsible for managing goodsreturnworkflowstep-related operations in the API.
    /// </summary>
    /// <remarks>
    /// This controller provides endpoints for adding, retrieving, updating, and deleting goodsreturnworkflowstep information.
    /// </remarks>
    [Route("api/goodsreturnworkflowstep")]
    [Authorize]
    public class GoodsReturnWorkFlowStepController : ControllerBase
    {
        private readonly test_newContext _context;

        public GoodsReturnWorkFlowStepController(test_newContext context)
        {
            _context = context;
        }

        /// <summary>Adds a new goodsreturnworkflowstep to the database</summary>
        /// <param name="model">The goodsreturnworkflowstep data to be added</param>
        /// <returns>The result of the operation</returns>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [Produces("application/json")]
        [UserAuthorize("GoodsReturnWorkFlowStep",Entitlements.Create)]
        public IActionResult Post([FromBody] GoodsReturnWorkFlowStep model)
        {
            _context.GoodsReturnWorkFlowStep.Add(model);
            this._context.SaveChanges();
            return Ok(new { model.Id });
        }

        /// <summary>Retrieves a list of goodsreturnworkflowsteps based on specified filters</summary>
        /// <param name="filters">The filter criteria in JSON format. Use the following format: [{"PropertyName": "PropertyName", "Operator": "Equal", "Value": "FilterValue"}] </param>
        /// <param name="searchTerm">To searching data.</param>
        /// <param name="pageNumber">The page number.</param>
        /// <param name="pageSize">The page size.</param>
        /// <param name="sortField">The entity's field name to sort.</param>
        /// <param name="sortOrder">The sort order asc or desc.</param>
        /// <returns>The filtered list of goodsreturnworkflowsteps</returns>
        [HttpGet]
        [UserAuthorize("GoodsReturnWorkFlowStep",Entitlements.Read)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [Produces("application/json")]
        public IActionResult Get([FromQuery] string filters, string searchTerm, int pageNumber = 1, int pageSize = 10, string sortField = null, string sortOrder = "asc")
        {
            List<FilterCriteria> filterCriteria = null;
            if (pageSize < 1)
            {
                return BadRequest("Page size invalid.");
            }

            if (pageNumber < 1)
            {
                return BadRequest("Page mumber invalid.");
            }

            if (!string.IsNullOrEmpty(filters))
            {
                filterCriteria = JsonHelper.Deserialize<List<FilterCriteria>>(filters);
            }

            var query = _context.GoodsReturnWorkFlowStep.IncludeRelated().AsQueryable();
            int skip = (pageNumber - 1) * pageSize;
            var result = FilterService<GoodsReturnWorkFlowStep>.ApplyFilter(query, filterCriteria, searchTerm);
            if (!string.IsNullOrEmpty(sortField))
            {
                var parameter = Expression.Parameter(typeof(GoodsReturnWorkFlowStep), "b");
                var property = Expression.Property(parameter, sortField);
                var lambda = Expression.Lambda<Func<GoodsReturnWorkFlowStep, object>>(Expression.Convert(property, typeof(object)), parameter);
                if (sortOrder.Equals("asc", StringComparison.OrdinalIgnoreCase))
                {
                    result = result.OrderBy(lambda);
                }
                else if (sortOrder.Equals("desc", StringComparison.OrdinalIgnoreCase))
                {
                    result = result.OrderByDescending(lambda);
                }
                else
                {
                    return BadRequest("Invalid sort order. Use 'asc' or 'desc'.");
                }
            }

            var paginatedResult = result.Skip(skip).Take(pageSize).ToList();
            return Ok(paginatedResult);
        }

        /// <summary>Retrieves a specific goodsreturnworkflowstep by its primary key</summary>
        /// <param name="id">The primary key of the goodsreturnworkflowstep</param>
        /// <returns>The goodsreturnworkflowstep data</returns>
        [HttpGet]
        [Route("{id:Guid}")]
        [UserAuthorize("GoodsReturnWorkFlowStep",Entitlements.Read)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [Produces("application/json")]
        public IActionResult GetById([FromRoute] Guid id)
        {
            var entityData = _context.GoodsReturnWorkFlowStep.IncludeRelated().FirstOrDefault(entity => entity.Id == id);
            return Ok(entityData);
        }

        /// <summary>Deletes a specific goodsreturnworkflowstep by its primary key</summary>
        /// <param name="id">The primary key of the goodsreturnworkflowstep</param>
        /// <returns>The result of the operation</returns>
        [HttpDelete]
        [UserAuthorize("GoodsReturnWorkFlowStep",Entitlements.Delete)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [Produces("application/json")]
        [Route("{id:Guid}")]
        public IActionResult DeleteById([FromRoute] Guid id)
        {
            var entityData = _context.GoodsReturnWorkFlowStep.IncludeRelated().FirstOrDefault(entity => entity.Id == id);
            if (entityData == null)
            {
                return NotFound();
            }

            _context.GoodsReturnWorkFlowStep.Remove(entityData);
            var status = this._context.SaveChanges();
            return Ok(new { status });
        }

        /// <summary>Updates a specific goodsreturnworkflowstep by its primary key</summary>
        /// <param name="id">The primary key of the goodsreturnworkflowstep</param>
        /// <param name="updatedEntity">The goodsreturnworkflowstep data to be updated</param>
        /// <returns>The result of the operation</returns>
        [HttpPut]
        [UserAuthorize("GoodsReturnWorkFlowStep",Entitlements.Update)]
        [Route("{id:Guid}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [Produces("application/json")]
        public IActionResult UpdateById(Guid id, [FromBody] GoodsReturnWorkFlowStep updatedEntity)
        {
            if (id != updatedEntity.Id)
            {
                return BadRequest("Mismatched Id");
            }

            this._context.GoodsReturnWorkFlowStep.Update(updatedEntity);
            var status = this._context.SaveChanges();
            return Ok(new { status });
        }

        /// <summary>Updates a specific goodsreturnworkflowstep by its primary key</summary>
        /// <param name="id">The primary key of the goodsreturnworkflowstep</param>
        /// <param name="updatedEntity">The goodsreturnworkflowstep data to be updated</param>
        /// <returns>The result of the operation</returns>
        [HttpPatch]
        [UserAuthorize("GoodsReturnWorkFlowStep",Entitlements.Update)]
        [Route("{id:Guid}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [Produces("application/json")]
        public IActionResult UpdateById(Guid id, [FromBody] JsonPatchDocument<GoodsReturnWorkFlowStep> updatedEntity)
        {
            if (updatedEntity == null)
                return BadRequest("Patch document is missing.");
            var existingEntity = this._context.GoodsReturnWorkFlowStep.FirstOrDefault(t => t.Id == id);
            if (existingEntity == null)
                return NotFound();
            updatedEntity.ApplyTo(existingEntity, ModelState);
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            this._context.GoodsReturnWorkFlowStep.Update(existingEntity);
            var status = this._context.SaveChanges();
            return Ok(new { status });
        }
    }
}