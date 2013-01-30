using System;
using System.Collections.Generic;
using System.Web.Http.Dependencies;

namespace LogWhatever.DataService
{
    public class AspAutofacDependencyResolver : IDependencyResolver
	{
		#region Data Members
	    internal readonly System.Web.Mvc.IDependencyResolver _resolver;
		#endregion

		#region Constructors
		public AspAutofacDependencyResolver(System.Web.Mvc.IDependencyResolver resolver)
        {
			if (resolver == null)
				throw new ArgumentNullException("resolver");

            _resolver = resolver;
        }
		#endregion

		#region Public Methods
		public IDependencyScope BeginScope()
        {
            return this;
        }

        public object GetService(Type serviceType)
        {
            return _resolver.GetService(serviceType);
        }

        public IEnumerable<object> GetServices(Type serviceType)
        {
            return _resolver.GetServices(serviceType);
        }

        public void Dispose()
        {

        }
		#endregion
    }
}