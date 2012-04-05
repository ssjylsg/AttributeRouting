using System;
using System.Linq;
using System.Reflection;
using System.Web.Routing;
using AttributeRouting.Mvc.Framework;
using AttributeRouting.Mvc.Framework.Factories;

namespace AttributeRouting.Mvc
{
    /// <summary>
    /// Extensions to the MVC RouteCollection.
    /// </summary>
    public static class RouteCollectionExtensions
    {
        /// <summary>
        /// Scans the calling assembly for all routes defined with AttributeRouting attributes,
        /// using the default conventions.
        /// </summary>
        public static void MapAttributeRoutes(this RouteCollection routes)
        {
            var configuration = new AttributeRoutingConfiguration();
            configuration.ScanAssembly(Assembly.GetCallingAssembly());

            routes.MapAttributeRoutesInternal(configuration);
        }

        /// <summary>
        /// Scans the specified assemblies for all routes defined with AttributeRouting attributes,
        /// and applies configuration options against the routes found.
        /// </summary>
        /// <param name="configurationAction">
        /// The initialization action that builds the configuration object.
        /// </param>
        public static void MapAttributeRoutes(this RouteCollection routes, Action<AttributeRoutingConfiguration> configurationAction)
        {
            var configuration = new AttributeRoutingConfiguration();
            configurationAction.Invoke(configuration);

            routes.MapAttributeRoutesInternal(configuration);
        }

        /// <summary>
        /// Scans the specified assemblies for all routes defined with AttributeRouting attributes,
        /// and applies configuration options against the routes found.
        /// </summary>
        /// <param name="configuration">
        /// The configuration object.
        /// </param>
        public static void MapAttributeRoutes(this RouteCollection routes, AttributeRoutingConfiguration configuration)
        {
            routes.MapAttributeRoutesInternal(configuration);
        }

        private static void MapAttributeRoutesInternal(this RouteCollection routes, AttributeRoutingConfiguration configuration)
        {
            var generatedRoutes = new RouteBuilder(
                configuration, new AttributeRouteFactory(), new ConstraintFactory(), new UrlParameterFactory()).BuildAllRoutes();

            generatedRoutes.ToList().ForEach(r => routes.Add(r.RouteName, r.Route));
        }
    }
}