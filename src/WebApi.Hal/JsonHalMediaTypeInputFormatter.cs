using Newtonsoft.Json;
using System;
using Microsoft.AspNetCore.Mvc.Formatters;
using WebApi.Hal.JsonConverters;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.ObjectPool;
using System.Buffers;

namespace WebApi.Hal
{
    public class JsonHalMediaTypeInputFormatter : JsonInputFormatter
    {
        private readonly LinksConverter _linksConverter = new LinksConverter();

        private readonly ResourceListConverter _resourceListConverter = new ResourceListConverter();

        private readonly ResourceConverter _resourceConverter = new ResourceConverter();

        private readonly EmbeddedResourceConverter _embeddedResourceConverter = new EmbeddedResourceConverter();

        #region Constructors

        //https://github.com/aspnet/Mvc/issues/4765

        public JsonHalMediaTypeInputFormatter(ILogger logger, JsonSerializerSettings serializerSettings, ArrayPool<char> charPool, ObjectPoolProvider objectPoolProvider, IHypermediaResolver hypermediaResolver):base(logger, serializerSettings, charPool, objectPoolProvider)
        {
            if (hypermediaResolver == null)
            {
                throw new ArgumentNullException(nameof(hypermediaResolver));
            }

            _resourceConverter = new ResourceConverter(hypermediaResolver);
            Initialize();
        }

        public JsonHalMediaTypeInputFormatter(ILogger logger, JsonSerializerSettings serializerSettings, ArrayPool<char> charPool, ObjectPoolProvider objectPoolProvider) : base(logger, serializerSettings, charPool, objectPoolProvider)
        {
            Initialize();
        }


        #endregion

        #region Private methods

        private void Initialize()
        {
            SerializerSettings.Converters.Add(_linksConverter);
            SerializerSettings.Converters.Add(_resourceListConverter);
            SerializerSettings.Converters.Add(_resourceConverter);
            SerializerSettings.Converters.Add(_embeddedResourceConverter);
            SerializerSettings.NullValueHandling = NullValueHandling.Include;
        }

        #endregion
    }
}
