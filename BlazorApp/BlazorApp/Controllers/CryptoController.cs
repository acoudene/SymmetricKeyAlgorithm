using BlazorApp.Client.Dtos;
using CryptographyProvider;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;
using System.Net.Mime;

namespace BlazorApp.Controllers;

[Route("api/[controller]")]
[ApiController]
public class CryptoController : ControllerBase
{
  private readonly IMongoCollection<CryptoDataDto> _collection;
  private readonly IStatefulCryptographyProvider _cryptographyProvider;

  public CryptoController(
    IMongoCollection<CryptoDataDto> collection, 
    IStatefulCryptographyProvider cryptographyProvider)
  {
    _collection = collection;
    _cryptographyProvider = cryptographyProvider;
  }
  
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
      var encryptedDto = await _collection
        .Find(_ => true)
        .FirstOrDefaultAsync(cancellationToken);

      if (encryptedDto is null)
        return NotFound();

      return new CryptoDataDto 
      {
        Text = _cryptographyProvider.Decrypt(encryptedDto.Text)
      };
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

      var encryptedDto = new CryptoDataDto
      {
        Text = _cryptographyProvider.Encrypt(newOrToUpdateDto.Text)
      };

      var filter = Builders<CryptoDataDto>.Filter.Empty;
      await _collection.ReplaceOneAsync(filter, encryptedDto, new ReplaceOptions { IsUpsert = true });

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
