#pragma warning disable 108 // new keyword hiding
#pragma warning disable 114 // new keyword hiding
namespace Windows.ApplicationModel.Contacts
{
	#if false || false || false || false || false || false || false
	[global::Uno.NotImplemented]
	#endif
	public  partial class ContactQueryOptions 
	{
		#if __ANDROID__ || __IOS__ || NET461 || __WASM__ || __SKIA__ || __NETSTD_REFERENCE__ || __MACOS__
		[global::Uno.NotImplemented("__ANDROID__", "__IOS__", "NET461", "__WASM__", "__SKIA__", "__NETSTD_REFERENCE__", "__MACOS__")]
		public  bool IncludeContactsFromHiddenLists
		{
			get
			{
				throw new global::System.NotImplementedException("The member bool ContactQueryOptions.IncludeContactsFromHiddenLists is not implemented in Uno.");
			}
			set
			{
				global::Windows.Foundation.Metadata.ApiInformation.TryRaiseNotImplemented("Windows.ApplicationModel.Contacts.ContactQueryOptions", "bool ContactQueryOptions.IncludeContactsFromHiddenLists");
			}
		}
		#endif
		#if __ANDROID__ || __IOS__ || NET461 || __WASM__ || __SKIA__ || __NETSTD_REFERENCE__ || __MACOS__
		[global::Uno.NotImplemented("__ANDROID__", "__IOS__", "NET461", "__WASM__", "__SKIA__", "__NETSTD_REFERENCE__", "__MACOS__")]
		public  global::Windows.ApplicationModel.Contacts.ContactAnnotationOperations DesiredOperations
		{
			get
			{
				throw new global::System.NotImplementedException("The member ContactAnnotationOperations ContactQueryOptions.DesiredOperations is not implemented in Uno.");
			}
			set
			{
				global::Windows.Foundation.Metadata.ApiInformation.TryRaiseNotImplemented("Windows.ApplicationModel.Contacts.ContactQueryOptions", "ContactAnnotationOperations ContactQueryOptions.DesiredOperations");
			}
		}
		#endif
		// Skipping already declared property DesiredFields
		#if __ANDROID__ || __IOS__ || NET461 || __WASM__ || __SKIA__ || __NETSTD_REFERENCE__ || __MACOS__
		[global::Uno.NotImplemented("__ANDROID__", "__IOS__", "NET461", "__WASM__", "__SKIA__", "__NETSTD_REFERENCE__", "__MACOS__")]
		public  global::System.Collections.Generic.IList<string> AnnotationListIds
		{
			get
			{
				throw new global::System.NotImplementedException("The member IList<string> ContactQueryOptions.AnnotationListIds is not implemented in Uno.");
			}
		}
		#endif
		#if __ANDROID__ || __IOS__ || NET461 || __WASM__ || __SKIA__ || __NETSTD_REFERENCE__ || __MACOS__
		[global::Uno.NotImplemented("__ANDROID__", "__IOS__", "NET461", "__WASM__", "__SKIA__", "__NETSTD_REFERENCE__", "__MACOS__")]
		public  global::System.Collections.Generic.IList<string> ContactListIds
		{
			get
			{
				throw new global::System.NotImplementedException("The member IList<string> ContactQueryOptions.ContactListIds is not implemented in Uno.");
			}
		}
		#endif
		#if __ANDROID__ || __IOS__ || NET461 || __WASM__ || __SKIA__ || __NETSTD_REFERENCE__ || __MACOS__
		[global::Uno.NotImplemented("__ANDROID__", "__IOS__", "NET461", "__WASM__", "__SKIA__", "__NETSTD_REFERENCE__", "__MACOS__")]
		public  global::Windows.ApplicationModel.Contacts.ContactQueryTextSearch TextSearch
		{
			get
			{
				throw new global::System.NotImplementedException("The member ContactQueryTextSearch ContactQueryOptions.TextSearch is not implemented in Uno.");
			}
		}
		#endif
		// Skipping already declared method Windows.ApplicationModel.Contacts.ContactQueryOptions.ContactQueryOptions(string)
		// Forced skipping of method Windows.ApplicationModel.Contacts.ContactQueryOptions.ContactQueryOptions(string)
		// Skipping already declared method Windows.ApplicationModel.Contacts.ContactQueryOptions.ContactQueryOptions(string, Windows.ApplicationModel.Contacts.ContactQuerySearchFields)
		// Forced skipping of method Windows.ApplicationModel.Contacts.ContactQueryOptions.ContactQueryOptions(string, Windows.ApplicationModel.Contacts.ContactQuerySearchFields)
		// Skipping already declared method Windows.ApplicationModel.Contacts.ContactQueryOptions.ContactQueryOptions()
		// Forced skipping of method Windows.ApplicationModel.Contacts.ContactQueryOptions.ContactQueryOptions()
		// Forced skipping of method Windows.ApplicationModel.Contacts.ContactQueryOptions.TextSearch.get
		// Forced skipping of method Windows.ApplicationModel.Contacts.ContactQueryOptions.ContactListIds.get
		// Forced skipping of method Windows.ApplicationModel.Contacts.ContactQueryOptions.IncludeContactsFromHiddenLists.get
		// Forced skipping of method Windows.ApplicationModel.Contacts.ContactQueryOptions.IncludeContactsFromHiddenLists.set
		// Forced skipping of method Windows.ApplicationModel.Contacts.ContactQueryOptions.DesiredFields.get
		// Forced skipping of method Windows.ApplicationModel.Contacts.ContactQueryOptions.DesiredFields.set
		// Forced skipping of method Windows.ApplicationModel.Contacts.ContactQueryOptions.DesiredOperations.get
		// Forced skipping of method Windows.ApplicationModel.Contacts.ContactQueryOptions.DesiredOperations.set
		// Forced skipping of method Windows.ApplicationModel.Contacts.ContactQueryOptions.AnnotationListIds.get
	}
}
