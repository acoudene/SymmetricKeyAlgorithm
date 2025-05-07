using BlazorApp.Client.Dtos;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;
using System.Net.Mime;

namespace BlazorApp.Controllers;

[Route("api/[controller]")]
[ApiController]
public class CryptoController : ControllerBase
{
  private readonly IMongoCollection<CryptoDataDto> _collection;

  public CryptoController(IMongoCollection<CryptoDataDto> collection) => _collection = collection;
  
  [HttpGet("raw")]
  [Produces(MediaTypeNames.Application.Json)]
  [ProducesResponseType(StatusCodes.Status200OK)]
  [ProducesResponseType(StatusCodes.Status400BadRequest)]
  [ProducesResponseType(StatusCodes.Status500InternalServerError)]
  public async Task<ActionResult<CryptoDataDto>> GetEncryptedTextAsync(CancellationToken cancellationToken = default)
  {
    try
    {
      return 
        await _collection
        .Find(_ => true)
        .FirstOrDefaultAsync(cancellationToken);
    }
    catch (ArgumentException)
    {
      return BadRequest();
    }
    catch (Exception)
    {
      return Problem();
    }
  }

  [HttpGet()]
  [Produces(MediaTypeNames.Application.Json)]
  [ProducesResponseType(StatusCodes.Status200OK)]
  [ProducesResponseType(StatusCodes.Status400BadRequest)]
  [ProducesResponseType(StatusCodes.Status500InternalServerError)]
  public async Task<ActionResult<CryptoDataDto>> GetClearTextAsync(CancellationToken cancellationToken = default)
  {
    try
    {
      return
        await _collection
        .Find(_ => true)
        .FirstOrDefaultAsync(cancellationToken);
    }
    catch (ArgumentException)
    {
      return BadRequest();
    }
    catch (Exception)
    {
      return Problem();
    }
  }

  [HttpPost()]
  [Consumes(MediaTypeNames.Application.Json)]
  [Produces(MediaTypeNames.Application.Json)]
  [ProducesResponseType(StatusCodes.Status204NoContent)]
  [ProducesResponseType(StatusCodes.Status201Created)]
  [ProducesResponseType(StatusCodes.Status400BadRequest)]
  [ProducesResponseType(StatusCodes.Status500InternalServerError)]
  public virtual async Task<ActionResult<CryptoDataDto>> CreateOrUpdateAsync(
     [FromBody] CryptoDataDto newOrToUpdateDto,
     CancellationToken cancellationToken = default)
  {
    try
    {
      if (newOrToUpdateDto is null)
        throw new ArgumentNullException(nameof(newOrToUpdateDto));

      var filter = Builders<CryptoDataDto>.Filter.Empty;
      await _collection.ReplaceOneAsync(filter, newOrToUpdateDto, new ReplaceOptions { IsUpsert = true });

      return await _collection
        .Find(_ => true)
        .FirstOrDefaultAsync(cancellationToken);

    }
    catch (ArgumentException)
    {     
      return BadRequest();
    }
    catch (Exception)
    {      
      return Problem();
    }
  }
}
