namespace LogWhatever.Common.Adapters.Bundling
{
	public interface IBundleFactory
	{
		#region Public Methods
		IBundle CreateJavascriptBundle(string path);
		IBundle CreateCssBundle(string path);
		#endregion
	}
}