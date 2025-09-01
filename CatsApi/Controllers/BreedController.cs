using CatsApi.DTOs;
using CatsApi.FilterModels;
using CatsApi.Services.Breed;
using CsvHelper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Globalization;

namespace CatsApi.Controllers;

/// <summary>
/// Controller for Cat Breeds.
/// </summary>
[Route("api/[controller]")]
[ApiController]
public class BreedController : ControllerBase
{
    private readonly IBreedService _breeds;

    public BreedController(IBreedService breeds)
    {
        _breeds = breeds;
    }

    /// <summary>
    /// Creates a Cat Breed.
    /// </summary>
    /// <param name="DTO">A DTO for creating a cat breed.</param>
    /// <returns> The newly created cat breed.</returns>
    /// <response code="201"> Returns the Newly Created Cat Breed. </response>
    /// <response code="401"> If the User is Unauthorized. </response>
    [HttpPost]
    [Authorize]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<ActionResult> Post(BreedCreateDTO DTO)
    {
        var breed = await _breeds.CreateAsync(DTO);

        return CreatedAtAction(nameof(Get), new { id = breed.Id }, breed);
    }

    /// <summary>
    /// Creates Cat Breeds from an uploaded CSV file.
    /// </summary>
    /// <param name="csv"> The CSV file from which cat breeds will be created. </param>
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
            var breeds = csvReader.GetRecords<BreedCreateDTO>().ToList();

            await _breeds.CreateFromListAsync(breeds);
        }

        return Ok();
    }

    /// <summary>
    /// Gets a List of Cat Breeds.
    /// </summary>
    /// <param name="filter"> A class that will be used to filter the list of cat breeds. </param>
    /// <returns> A List of Cat Breeds. </returns>
    /// <response code="200"> Returns a List of Cat Breeds </response>
    [HttpGet]
    public async Task<ActionResult<IEnumerable<BreedDTO>>> GetAll([FromQuery]BreedFilter filter)
    {
        var breeds = await _breeds.GetAllAsync(filter);
        return Ok(breeds);
    }

    /// <summary>
    /// Gets a Specific Cat Breed.
    /// </summary>
    /// <param name="id"> The Id of the Specific Cat Breed. </param>
    /// <returns> A cat breed with an Ok Status Code or a Not Found Status Code. </returns>
    /// <response code="200"> Returns the specific cat breed. </response>
    /// <response code="404"> If the cat breed is Not Found. </response>
    [HttpGet("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<BreedDTO>> Get(int id)
    {
        var breed = await _breeds.GetAsync(id);

        if (breed is null) return NotFound();

        return Ok(breed);
    }

    /// <summary>
    /// Updates a Specific Cat Breed.
    /// </summary>
    /// <param name="id"> The Id of the Cat Breed to Update. </param>
    /// <param name="DTO"> The DTO containing the new values of the cat breed. </param>
    /// <returns> Status Code of No Content if Successful or a Status Code of Not Found if not. </returns>
    /// <response code="204"> Returns No Content as the Cat Breed was updated successfully. </response>
    /// <response code="404"> If the Cat Breed is Not Found. </response>
    /// <response code="401"> If the User is Unauthorized. </response>
    [HttpPut("{id}")]
    [Authorize]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<BreedDTO>> Put(int id, BreedCreateDTO DTO)
    {
        var success = await _breeds.UpdateAsync(id, DTO);

        if (!success) return NotFound();

        return NoContent();
    }

    /// <summary>
    /// Deletes a Specific Cat Breed.
    /// </summary>
    /// <param name="id"> The Id of Cat Breed to Delete. </param>
    /// <returns> Status Code of No Content if Successful or a Status Code of Not Found if not. </returns>
    /// <response code="204"> Returns No Content as the Cat Breed was deleted successfully. </response>
    /// <response code="404"> If the Cat Breed is Not Found. </response>
    /// <response code="401"> If the User is Unauthorized. </response>
    [HttpDelete("{id}")]
    [Authorize]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult> Delete(int id)
    {
        var success = await _breeds.DeleteAsync(id);

        if (!success) return NotFound();

        return NoContent();
    }
}
