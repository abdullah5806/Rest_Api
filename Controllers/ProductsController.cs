using Microsoft.AspNetCore.Mvc;
using PMS.Data;
using PMS.Models;

namespace PMS.Controllers;

[ApiController]
[Route("api/[controller]")]
public sealed class ProductsController(ProductStore store) : ControllerBase
{
    [HttpGet]
    [ProducesResponseType<IReadOnlyList<Product>>(StatusCodes.Status200OK)]
    public ActionResult<IReadOnlyList<Product>> GetAll()
    {
        return Ok(store.GetAll());
    }

    [HttpGet("{id:int}")]
    [ProducesResponseType<Product>(StatusCodes.Status200OK)]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status404NotFound)]
    public ActionResult<Product> GetById(int id)
    {
        var product = store.GetById(id);
        return product is null
            ? Problem(
                statusCode: StatusCodes.Status404NotFound,
                title: "Product not found",
                detail: $"No product with ID {id} exists.")
            : Ok(product);
    }

    [HttpPost]
    [ProducesResponseType<Product>(StatusCodes.Status201Created)]
    [ProducesResponseType<ValidationProblemDetails>(StatusCodes.Status400BadRequest)]
    public ActionResult<Product> Create(ProductRequest request)
    {
        var product = store.Create(request);
        return CreatedAtAction(nameof(GetById), new { id = product.Id }, product);
    }

    [HttpPut("{id:int}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType<ValidationProblemDetails>(StatusCodes.Status400BadRequest)]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status404NotFound)]
    public IActionResult Update(int id, ProductRequest request)
    {
        if (!store.Update(id, request))
        {
            return Problem(
                statusCode: StatusCodes.Status404NotFound,
                title: "Product not found",
                detail: $"No product with ID {id} exists.");
        }

        return NoContent();
    }

    [HttpDelete("{id:int}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status404NotFound)]
    public IActionResult Delete(int id)
    {
        if (!store.Delete(id))
        {
            return Problem(
                statusCode: StatusCodes.Status404NotFound,
                title: "Product not found",
                detail: $"No product with ID {id} exists.");
        }

        return NoContent();
    }
}
