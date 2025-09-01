using CatsApi.DTOs;
using CatsApi.FilterModels;
using CatsApi.Services.Cat;
using CsvHelper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Globalization;

namespace CatsApi.Controllers;

/// <summary>
/// Controller for Cats.
/// </summary>
[Route("api/[controller]")]
[ApiController]
public class CatsController : ControllerBase
{
    private readonly ICatService _cats;

    public CatsController(ICatService cats)
    {
        _cats = cats;
    }

    /// <summary>
    /// Creates a Cat.
    /// </summary>
    /// <param name="DTO">A DTO for creating a cat.</param>
    /// <returns> The newly created cat.</returns>
    /// <response code="201"> Returns the Newly Created Cat. </response>
    /// <response code="401"> If the User is Unauthorized. </response>
    [HttpPost]
    [Authorize]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<ActionResult> Create(CatCreateDTO DTO)
    {
        var cat = await _cats.CreateAsync(DTO);

        return CreatedAtAction(nameof(Get), new { id = cat.Id}, cat);
    }

    /// <summary>
    /// Creates Cats from an uploaded CSV file.
    /// </summary>
    /// <param name="csv"> The CSV file from which cats will be created. </param>
    /// <returns> An Ok Status Code. </returns>
    /// <response code="200"> Returns Ok. </response>
    /// <response code="401"> If the User is Unauthorized. </response>
    [HttpPost("upload-csv")]
    [Authorize]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<ActionResult> UploadCSV(IFormFile csv)
    {
        var csvStream = csv.OpenReadStream();
        using (var reader = new StreamReader(csvStream))
        using (var csvReader = new CsvReader(reader, CultureInfo.InvariantCulture))
        {
            var cats = csvReader.GetRecords<CatCreateDTO>().ToList();

            await _cats.CreateFromListAsync(cats);
        }

        return Ok();
    }

    /// <summary>
    /// Gets a List of Cats.
    /// </summary>
    /// <param name="filter"> A class that will be used to filter the list of cats. </param>
    /// <returns> A List of Cats. </returns>
    /// <response code="200"> Returns a List of Cats </response>
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<CatDTO>>> GetAll([FromQuery]CatFilter filter)
    {
        var cats = await _cats.GetAllAsync(filter);
        return Ok(cats);
    }

    /// <summary>
    /// Gets a Specific Cat.
    /// </summary>
    /// <param name="id"> The Id of the Specific Cat. </param>
    /// <returns> A cat with an Ok Status Code or a Not Found Status Code. </returns>
    /// <response code="200"> Returns the specific cat. </response>
    /// <response code="404"> If the Cat is Not Found. </response>
    [HttpGet("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<CatDTO>> Get(int id)
    {
        var cat = await _cats.GetAsync(id);

        if (cat is null) return NotFound();

        return Ok(cat);
    }

    /// <summary>
    /// Updates a Specific Cat.
    /// </summary>
    /// <param name="id"> The Id of the Cat to Update. </param>
    /// <param name="DTO"> The DTO containing the new values of the cat. </param>
    /// <returns> Status Code of No Content if Successful or a Status Code of Not Found if not. </returns>
    /// <response code="204"> Returns No Content as the Cat was updated successfully. </response>
    /// <response code="401"> If the User is Unauthorized. </response>
    /// <response code="404"> If the Cat is Not Found. </response>
    [HttpPut("{id}")]
    [Authorize]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<CatDTO>> Put(int id, CatCreateDTO DTO)
    {
        var success = await _cats.UpdateAsync(id, DTO);

        if (!success) return NotFound();

        return NoContent();
    }

    /// <summary>
    /// Deletes a Specific Cat.
    /// </summary>
    /// <param name="id"> The Id of Cat to Delete. </param>
    /// <returns> Status Code of No Content if Successful or a Status Code of Not Found if not. </returns>
    /// <response code="204"> Returns No Content as the Cat was deleted successfully. </response>
    /// <response code="401"> If the User is Unauthorized. </response>
    /// <response code="404"> If the Cat is Not Found. </response>
    [HttpDelete("{id}")]
    [Authorize]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult> Delete(int id)
    {
        var success = await _cats.DeleteAsync(id);

        if (!success) return NotFound();

        return NoContent();
    }
}
