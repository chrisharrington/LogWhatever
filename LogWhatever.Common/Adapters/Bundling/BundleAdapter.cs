using System;
using System.Web.Optimization;

namespace LogWhatever.Common.Adapters.Bundling
{
	public class BundleAdapter : IBundle
	{
		#region Data Members
		private readonly Bundle _bundle;
		#endregion

		#region Properties
		public Bundle InternalBundle
		{
			get { return _bundle; }
		}
		#endregion

		#region Constructors
		public BundleAdapter(Bundle bundle)
		{
			if (bundle == null)
				throw new ArgumentNullException("bundle");

			_bundle = bundle;
		}
		#endregion

		#region Public Methods
		public IBundle IncludeDirectory(string directory, string pattern, bool subdirectories = true)
		{
			return new BundleAdapter(_bundle.IncludeDirectory(directory, pattern, subdirectories));
		}
		#endregion
	}
}