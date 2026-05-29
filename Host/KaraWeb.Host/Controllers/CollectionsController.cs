using KaraWeb.Core.Helpers;
using KaraWeb.Core.Models;
using KaraWeb.Host.Providers.Collections;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace KaraWeb.Host.Controllers
{
    [Route(Constants.ApiMainRoutePrefix + "collections")]
    public class CollectionsController : ControllerBase
    {
        private ICollectionsProvider _collectionsProvider;

        public CollectionsController(ICollectionsProvider collectionsProvider)
        {
            _collectionsProvider = collectionsProvider;
        }

        [HttpGet]
        public async Task<ActionResult<List<Collection>>> GetAllCollectionsAsync(CancellationToken cancellationToken = default)
        {
            return Ok(await _collectionsProvider.GetCollectionsAsync(cancellationToken).ToListAsync(cancellationToken));
        }
    }
}
