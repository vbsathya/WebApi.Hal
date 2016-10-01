using System;
using Newtonsoft.Json;
using WebApi.Hal.JsonConverters;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.Net.Http.Headers;
using System.Buffers;

namespace WebApi.Hal
{
    public class JsonHalMediaTypeOutputFormatter : JsonOutputFormatter
    {
        private const string _mediaTypeHeaderValueName = "application/hal+json";

        private readonly LinksConverter _linksConverter = new LinksConverter();

        private readonly ResourceListConverter _resourceListConverter = new ResourceListConverter();

        private readonly ResourceConverter _resourceConverter = new ResourceConverter();

        private readonly EmbeddedResourceConverter _embeddedResourceConverter = new EmbeddedResourceConverter();

        #region Constructors

        public JsonHalMediaTypeOutputFormatter(JsonSerializerSettings serializerSettings, ArrayPool<char> charPool, IHypermediaResolver hypermediaResolver):base(serializerSettings, charPool)
        {
            if (hypermediaResolver == null)
            {
                throw new ArgumentNullException(nameof(hypermediaResolver));
            }

            _resourceConverter = new ResourceConverter(hypermediaResolver);
            Initialize();
        }

        public JsonHalMediaTypeOutputFormatter(JsonSerializerSettings serializerSettings, ArrayPool<char> charPool) : base(serializerSettings, charPool)
        {
            Initialize();
        }

        #endregion

        #region Private methods

        private void Initialize()
        {
            SupportedMediaTypes.Add(new MediaTypeHeaderValue(_mediaTypeHeaderValueName));
            SerializerSettings.Converters.Add(_linksConverter);
            SerializerSettings.Converters.Add(_resourceListConverter);
            SerializerSettings.Converters.Add(_resourceConverter);
            SerializerSettings.Converters.Add(_embeddedResourceConverter);
            SerializerSettings.NullValueHandling = NullValueHandling.Include;
        }
        protected override JsonSerializer CreateJsonSerializer()
        {
            return JsonSerializer.Create(this.SerializerSettings);
        }
        #endregion
    }
}