using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using TravelBookingClassLibrary;

[Route("api/[controller]")]
[ApiController]
public class PackageController : ControllerBase
{
    private readonly PackageRepository _repository;

    public PackageController()
    {
        _repository = new PackageRepository(new PackageContext());
    }

    [HttpGet]
    public async Task<ActionResult> GetPackages()
    {
        var packages = await _repository.GetAllPackagesAsync();
        return Ok(packages);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult> Get(int id)
    {
        var package = await _repository.GetPackageByPackageIdAsync(id);
        if (package == null)
        {
            return NotFound();
        }
        return Ok(package);
    }

    [HttpGet("search/title")]
    public async Task<ActionResult> GetPackageByTitleAsync([FromQuery] string title)
    {
        var packages = await _repository.GetPackageBytitleAsync(title);
        if (packages == null || !packages.Any())
        {
            return NotFound();
        }
        return Ok(packages);
    }

    [HttpGet("search")]
    public async Task<ActionResult> GetPackageByPriceDurationTitleAsync([FromQuery] long price, [FromQuery] int duration, [FromQuery] string title)
    {
        var packages = await _repository.GetPackageBypricedurationtitleAsync(price, duration, title);
        if (packages == null || !packages.Any())
        {
            return NotFound();
        }
        return Ok(packages);
    }

    [HttpGet("search/price-duration")]
    public async Task<ActionResult> GetPackageByPriceDurationAsync([FromQuery] long price, [FromQuery] int duration)
    {
        var packages = await _repository.GetPackageBypricedurationAsync(price, duration);
        if (packages == null || !packages.Any())
        {
            return NotFound();
        }
        return Ok(packages);
    }

    [HttpGet("search/duration")]
    public async Task<ActionResult> GetPackageByDurationAsync([FromQuery] int duration)
    {
        var packages = await _repository.GetPackageByDurationAsync(duration);
        if (packages == null || !packages.Any())
        {
            return NotFound();
        }
        return Ok(packages);
    }

    [HttpGet("search/description")]
    public async Task<ActionResult> GetPackageByDescriptionAsync([FromQuery] string description)
    {
        var packages = await _repository.GetPackageBydescriptionAsync(description);
        if (packages == null || !packages.Any())
        {
            return NotFound();
        }
        return Ok(packages);
    }
    [HttpGet("search/price-range")]
    public async Task<ActionResult> GetPackageByPriceRangeAsync([FromQuery] long minPrice, [FromQuery] long maxPrice)
    {
        var packages = await _repository.GetPackageByPriceRangeAsync(minPrice, maxPrice);
        if (packages == null || !packages.Any())
        {
            return NotFound();
        }
        return Ok(packages);
    }

    [HttpGet("search/includedservices")]
    public async Task<ActionResult> GetPackageByIncludedServicesAsync([FromQuery] string includedservices)
    {
        var packages = await _repository.GetPackageByincludedservicesAsync(includedservices);
        if (packages == null || !packages.Any())
        {
            return NotFound();
        }
        return Ok(packages);
    }

    [HttpGet("search/price")]
    public async Task<ActionResult> GetPackageByPriceAsync([FromQuery] long price)
    {
        var packages = await _repository.GetPackageByPriceAsync(price);
        if (packages == null || !packages.Any())
        {
            return NotFound();
        }
        return Ok(packages);
    }
    [HttpGet("search/Category")]
    public async Task<ActionResult> GetPackageByCategoryAsync([FromQuery] string category)
    {
        var packages = await _repository.GetPackageByCategoryAsync(category);
        if (packages == null || !packages.Any())
        {
            return NotFound();
        }
        return Ok(packages);
    }

    [HttpPost]
    public async Task<ActionResult> AddPackage([FromBody] Package package)
    {
        await _repository.AddPackagesAsync(package);
        return CreatedAtAction(nameof(Get), new { id = package.PackageID }, package);
    }

    [HttpPut("{id}")]
    public async Task<ActionResult> UpdatePackage(int id, [FromBody] Package package)
    {
        var existingPackage = await _repository.GetPackageByPackageIdAsync(id);
        if (existingPackage == null)
        {
            return NotFound();
        }
        await _repository.UpdatePackageAsync(id, package.Title, package.Description, package.Duration, package.Price, package.IncludedServices);
        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult> DeletePackage(int id)
    {
        var package = await _repository.GetPackageByPackageIdAsync(id);
        if (package == null)
        {
            return NotFound();
        }
        await _repository.DeletePackageAsync(id);
        return NoContent();
    }
}