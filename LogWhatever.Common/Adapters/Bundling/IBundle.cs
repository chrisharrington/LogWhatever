using System.Web.Optimization;

namespace LogWhatever.Common.Adapters.Bundling
{
	public interface IBundle
	{
		#region Properties
		Bundle InternalBundle { get; }
		#endregion

		#region Public Methods
		IBundle IncludeDirectory(string directory, string pattern, bool subdirectories = true);
		#endregion
	}
}