using System.Web;
using System.Web.Optimization;

namespace LogWhatever.Common.Adapters.Bundling
{
	public class BundleFactory : IBundleFactory
	{
		#region Properties
		internal bool IsDebuggingEnabled
		{
			get { return HttpContext.Current.IsDebuggingEnabled; }
		}
		#endregion

		#region Public Methods
		public IBundle CreateJavascriptBundle(string path)
		{
			var bundle = new Bundle(path);
			if (!IsDebuggingEnabled)
				bundle.Transforms.Add(new JsMinify());
			return new BundleAdapter(bundle);
		}

		public IBundle CreateCssBundle(string path)
		{
			var bundle = new Bundle("~/css");
			if (!IsDebuggingEnabled)
				bundle.Transforms.Add(new CssMinify());
			return new BundleAdapter(bundle);
		}
		#endregion
	}
}