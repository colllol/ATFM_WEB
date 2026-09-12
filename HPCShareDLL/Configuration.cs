using System;
using System.Collections;
using System.Collections.Specialized;
using System.Xml;
using System.Configuration;
using System.Web.Configuration;

namespace HPCShareDLL
{
	/// <summary>
	/// QUYNX - Configuration.
	/// </summary>
	#region class Configuration
	public class Configuration
	{
		Hashtable providers = new Hashtable();
		Hashtable ftp		= new Hashtable();
		string defaultProvider;
		string defaultFtp;
		string defaultLanguage;	
		string defaultSkin;
		string filesPath;
		string message;
		string defaultTitle;
		int	pagesize;
		string uploadPath;
		string pathfileupload;
		string pathfilemanager;
		string data_documents;
		string address_documents;
		string path_IP;
		
        #region Properties Configuration
		
		public string DefaultLanguage { get { return defaultLanguage; } }
		public string DefaultProvider { get { return defaultProvider; } }
		public string DefaultFtp	{get {return defaultFtp;}}
		public string DefaultSkin { get { return defaultSkin; } }
		public string FilesPath { get { return filesPath; } }
		public string Message { get { return message; } }
		public string Title { get { return defaultTitle; }}
		public int	  Pagesize { get {return pagesize;}}
		public string UploadPath{ get {return uploadPath;}}
		public string PathFileUpload{ get {return pathfileupload;}}
		public string PathFileManager{ get {return pathfilemanager;}}
		public string Data_Documents{ get {return data_documents;}}
		public string Address_Documents { get { return address_documents;}}
		public string Path_IP{get { return path_IP;}}
		
		public Hashtable Providers { get { return providers; } } 
		public Hashtable Ftp { get { return ftp;}}

        #endregion

		public static Configuration GetConfig() 
		{
			return (Configuration) System.Configuration.ConfigurationManager.GetSection("HPC/infor");
		}
		// *********************************************************************
		//  LoadValuesConfigurationXml
		//
		/// <summary>
		/// Loads the forums configuration values.
		/// </summary>
		/// <param name="node">XmlNode of the configuration section to parse.</param>
		/// 
		// ***********************************************************************/
		internal void LoadValuesConfigurationXml(XmlNode node) 
		{
			XmlAttributeCollection attributeCollection = node.Attributes;
            
			// Get the default provider
			//
			defaultProvider = attributeCollection["defaultProvider"].Value;


			// Get the default defaultFtp

			defaultFtp		= attributeCollection["defaultFtp"].Value;


			// Get the default language
			//
			defaultLanguage = attributeCollection["defaultLanguage"].Value;

			//Get themes
			//
			defaultSkin = attributeCollection["defaultSkin"].Value;

			//Get filesPath
			//
			filesPath = attributeCollection["filesPath"].Value;

			//Get path message
			//
			message = attributeCollection["message"].Value;
			
			//Get title
			//
			defaultTitle = attributeCollection["title"].Value;

			//Get pagesize
			//
			pagesize = Convert.ToInt32(attributeCollection["pagesize"].Value);
			//
			uploadPath=attributeCollection["uploadPath"].Value;
			//
			pathfileupload=attributeCollection["pathfileupload"].Value;

			//PathFileManager
			pathfilemanager=attributeCollection["pathfilemanager"].Value;
			
			data_documents =attributeCollection["data_documents"].Value;
			
			//
			path_IP = attributeCollection["path_IP"].Value;
			//Data Documents
			address_documents = attributeCollection["address_documents"].Value;

			// Read child nodes
			//
			foreach (XmlNode child in node.ChildNodes) 
			{

				if (child.Name == "providers")
					GetProviders(child);
			}

			foreach (XmlNode child in node.ChildNodes)
			{
				if(child.Name == "ftp")
					GetFtpInfor(child);
			}
		}

		// *********************************************************************
		//  GetProviders
		//
		/// <summary>
		/// Internal class used to populate the available providers.
		/// </summary>
		/// <param name="node">XmlNode of the providers to add/remove/clear.</param>
		/// 
		// ***********************************************************************/
		internal void GetProviders(XmlNode node) 
		{

			foreach (XmlNode provider in node.ChildNodes) 
			{

				switch (provider.Name) 
				{
					case "add" :
						providers.Add(provider.Attributes["name"].Value, new Provider(provider.Attributes) );
						break;

					case "remove" :
						providers.Remove(provider.Attributes["name"].Value);
						break;

					case "clear" :
						providers.Clear();
						break;

				}

			}

		}		
		// ***********************************************************************/
		internal void GetFtpInfor(XmlNode node) 
		{

			foreach (XmlNode _ftpinfor in node.ChildNodes) 
			{

				switch (_ftpinfor.Name) 
				{
					case "add" :
						ftp.Add(_ftpinfor.Attributes["name"].Value, new Ftp(_ftpinfor.Attributes) );
						break;

					case "remove" :
						ftp.Remove(_ftpinfor.Attributes["name"].Value);
						break;

					case "clear" :
						ftp.Clear();
						break;

				}

			}

		}		

	}

	#endregion

	#region class ftp
	public class Ftp
	{
		string name;
		NameValueCollection  ftpAttributes = new NameValueCollection();

		public Ftp (XmlAttributeCollection attributes)
		{
			name = attributes["name"].Value;
			foreach (XmlAttribute attribute in attributes) 
			{
				if ( attribute.Name != "name" )
					ftpAttributes.Add(attribute.Name,attribute.Value);
			}

		}
		public string Name 
		{
			get 
			{
				return name;
			}
		}		
		public NameValueCollection Attributes 
		{
			get 
			{
				return ftpAttributes;
			}
		}
	}

	#endregion
	
	#region class provider
	
	public class Provider 
	{
		string name;
		string providerType;
		NameValueCollection providerAttributes = new NameValueCollection();

		public Provider (XmlAttributeCollection attributes) 
		{

			// Set the name of the provider
			//
			name = attributes["name"].Value;

			providerType = attributes["type"].Value;
			// Store all the attributes in the attributes bucket
			//
			foreach (XmlAttribute attribute in attributes) 
			{

				if ( attribute.Name != "name" )
					providerAttributes.Add(attribute.Name,attribute.Value);

			}

		}


		public string Name 
		{
			get 
			{
				return name;
			}
		}
		public string Type 
		{
			get 
			{
				return providerType;
			}
		}
		public NameValueCollection Attributes 
		{
			get 
			{
				return providerAttributes;
			}
		}

	}
	#endregion
	internal class ConfigurationHandler : IConfigurationSectionHandler 
	{

		public virtual object Create(Object parent, Object context, XmlNode node) 
		{
			Configuration config = new Configuration();
			config.LoadValuesConfigurationXml(node);
			return config;
		}

	}
}
