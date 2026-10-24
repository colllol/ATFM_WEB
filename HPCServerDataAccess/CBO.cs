using System; 
using System.Web ; 
using System.Collections;
using System.Web.Caching; 
using System.Reflection; 
using System.Xml; 
using System.Xml.Serialization; 
using System.Text; 
using System.IO; 
using System.Data;
using prjInfo;

namespace HPCServerDataAccess
{
 public class CBO
{
   
    public static ArrayList GetPropertyInfo(Type objType) 
		{ 
			ArrayList objProperties = ((ArrayList)(DataCache.GetCache(objType.FullName))); 
			if (objProperties == null) 
			{ 
				objProperties = new ArrayList(); 
				//PropertyInfo objProperty; 
				foreach(PropertyInfo objProperty in objType.GetProperties()) 
				{ 
					objProperties.Add(objProperty); 
				} 
				DataCache.SetCache(objType.FullName, objProperties); 
			} 
			return objProperties; 
		} 

		private static int[] GetOrdinals(ArrayList objProperties, IDataReader dr) 
		{ 
			int[] arrOrdinals = new int[objProperties.Count]; 
			//int intProperty; 
			if (!(dr == null)) 
			{ 
				for (int intProperty = 0; intProperty <= objProperties.Count - 1; intProperty++) 
				{ 
					arrOrdinals[intProperty] = -1; 
					try 
					{ 
						arrOrdinals[intProperty] = dr.GetOrdinal(((PropertyInfo)(objProperties[intProperty])).Name); 
					} 
					catch 
					{ 
					} 
				} 
			} 
			return arrOrdinals; 
		} 

		private static object CreateObject(Type objType, IDataReader dr, ArrayList objProperties, int[] arrOrdinals) 
		{ 
			object objObject = Activator.CreateInstance(objType); 
			//int intProperty; 
			for (int intProperty = 0; intProperty <= objProperties.Count - 1; intProperty++) 
			{ 
				if (((PropertyInfo)(objProperties[intProperty])).CanWrite) 
				{ 
					if (arrOrdinals[intProperty] != -1) 
					{ 
						if (Microsoft.VisualBasic.Information.IsDBNull(dr.GetValue(arrOrdinals[intProperty]))) 
						{ 
							((PropertyInfo)(objProperties[intProperty])).SetValue(objObject, Null.SetNull(((PropertyInfo)(objProperties[intProperty]))), null); 
						} 
						else 
						{ 
							try 
							{ 
								((PropertyInfo)(objProperties[intProperty])).SetValue(objObject, dr.GetValue(arrOrdinals[intProperty]), null); 
							} 
							catch 
							{ 
								try 
								{ 
									Type pType = ((PropertyInfo)(objProperties[intProperty])).PropertyType; 
									if (pType.BaseType.Equals(typeof(System.Enum))) 
									{ 
										((PropertyInfo)(objProperties[intProperty])).SetValue(objObject, System.Enum.ToObject(pType, dr.GetValue(arrOrdinals[intProperty])), null); 
									} 
									else 
									{ 
										((PropertyInfo)(objProperties[intProperty])).SetValue(objObject, Convert.ChangeType(dr.GetValue(arrOrdinals[intProperty]), pType), null); 
									} 
								} 
								catch 
								{ 
									((PropertyInfo)(objProperties[intProperty])).SetValue(objObject, Null.SetNull(((PropertyInfo)(objProperties[intProperty]))), null); 
								} 
							} 
						} 
					} 
					else 
					{ 
						((PropertyInfo)(objProperties[intProperty])).SetValue(objObject, Null.SetNull(((PropertyInfo)(objProperties[intProperty]))), null); 
					} 
				} 
			} 
			return objObject; 
		} 

		public static object FillObject(IDataReader dr, Type objType) 
		{ 
			object objFillObject; 
			//int intProperty; 
			ArrayList objProperties = GetPropertyInfo(objType); 
			int[] arrOrdinals = GetOrdinals(objProperties, dr); 
			if (dr.Read()) 
			{ 
				objFillObject = CreateObject(objType, dr, objProperties, arrOrdinals); 
			} 
			else 
			{ 
				objFillObject = null; 
			} 
			if (!(dr == null))
			{ 
				dr.Close(); 
			} 
			return objFillObject; 
		} 

		public static ArrayList FillCollection(IDataReader dr, Type objType) 
		{ 
			ArrayList objFillCollection = new ArrayList(); 
			object objFillObject; 
			//int intProperty; 
			ArrayList objProperties = GetPropertyInfo(objType); 
			int[] arrOrdinals = GetOrdinals(objProperties, dr); 
			while (dr.Read()) 
			{ 
				objFillObject = CreateObject(objType, dr, objProperties, arrOrdinals); 
				objFillCollection.Add(objFillObject); 
			} 
			if (!(dr == null)) 
			{ 
				dr.Close(); 
			} 
			return objFillCollection; 
		} 

		public static IList FillCollection(IDataReader dr, Type objType, ref IList objToFill) 
		{ 
			object objFillObject; 
			//int intProperty; 
			ArrayList objProperties = GetPropertyInfo(objType); 
			int[] arrOrdinals = GetOrdinals(objProperties, dr); 
			while (dr.Read()) 
			{ 
				objFillObject = CreateObject(objType, dr, objProperties, arrOrdinals); 
				objToFill.Add(objFillObject); 
			} 
			if (!(dr == null)) 
			{ 
				dr.Close(); 
			} 
			return objToFill; 
		} 

		public static object InitializeObject(object objObject, Type objType) 
		{ 
			//int intProperty; 
			ArrayList objProperties = GetPropertyInfo(objType); 
			for (int intProperty = 0; intProperty <= objProperties.Count - 1; intProperty++) 
			{ 
				if (((PropertyInfo)(objProperties[intProperty])).CanWrite) 
				{ 
					((PropertyInfo)(objProperties[intProperty])).SetValue(objObject, Null.SetNull(((PropertyInfo)(objProperties[intProperty]))), null); 
				} 
			} 
			return objObject; 
		} 

		public static XmlDocument Serialize(object objObject) 
		{ 
			XmlSerializer objXmlSerializer = new XmlSerializer(objObject.GetType()); 
			StringBuilder objStringBuilder = new StringBuilder(); 
			TextWriter objTextWriter = new StringWriter(objStringBuilder); 
			objXmlSerializer.Serialize(objTextWriter, objObject); 
			StringReader objStringReader = new StringReader(objTextWriter.ToString()); 
			DataSet objDataSet = new DataSet(); 
			objDataSet.ReadXml(objStringReader); 
			XmlDocument xmlSerializedObject = new XmlDocument(); 
			xmlSerializedObject.LoadXml(objDataSet.GetXml()); 
			return xmlSerializedObject; 
		} 
}
public class DataCache 
	{ 

		public static object GetCache(string CacheKey) 
		{ 
			System.Web.Caching.Cache objCache = HttpRuntime.Cache; 
			return objCache[CacheKey]; 
		} 

		public static void SetCache(string CacheKey, object objObject) 
		{ 
			System.Web.Caching.Cache objCache = HttpRuntime.Cache; 
			objCache.Insert(CacheKey, objObject); 
		} 

		public static void SetCache(string CacheKey, object objObject, System.Web.Caching.CacheDependency objDependency) 
		{ 
			System.Web.Caching.Cache objCache = HttpRuntime.Cache; 
			objCache.Insert(CacheKey, objObject, objDependency); 
		} 

		public static void SetCache(string CacheKey, object objObject, int SlidingExpiration) 
		{ 
			System.Web.Caching.Cache objCache = HttpRuntime.Cache; 
			objCache.Insert(CacheKey, objObject, null, DateTime.MaxValue, TimeSpan.FromSeconds(SlidingExpiration)); 
		} 

		public static void SetCache(string CacheKey, object objObject, System.DateTime AbsoluteExpiration) 
		{ 
			System.Web.Caching.Cache objCache = HttpRuntime.Cache; 
			objCache.Insert(CacheKey, objObject, null, AbsoluteExpiration, TimeSpan.Zero); 
		} 

		public static void RemoveCache(string CacheKey) 
		{ 
			System.Web.Caching.Cache objCache = HttpRuntime.Cache; 
			if (!(objCache[CacheKey] == null)) 
			{ 
				objCache.Remove(CacheKey); 
			} 
		} 
	} 
public class Null 
	{ 

		public static int NullInteger 
		{ 
			get 
			{ 
				return -1; 
			} 
		} 

		public static System.DateTime NullDate 
		{ 
			get 
			{ 
				return System.DateTime.MinValue; 
			} 
		} 

		public static string NullString 
		{ 
			get 
			{ 
				return ""; 
			} 
		} 
		
		public static Boolean IsNullOrEmpty(Object objCompare)
		{
			Boolean Kq;
			if((objCompare==null)||(objCompare.ToString()==""))
			{
				Kq=true;
			}
			else
			{
				Kq=false;
			}
			return Kq;
		}

		public static bool NullBoolean 
		{ 
			get 
			{ 
				return false; 
			} 
		} 

		public static Guid NullGuid 
		{ 
			get 
			{ 
				return Guid.Empty; 
			} 
		} 

		public static object SetNull(object objField) 
		{ 
			if (!(objField == null)) 
			{ 
				if (objField is int) 
				{ 
					return NullInteger; 
				} 
				else if (objField is float) 
				{ 
					return NullInteger; 
				} 
				else if (objField is double) 
				{ 
					return NullInteger; 
				} 
				else if (objField is decimal) 
				{ 
					return NullInteger; 
				} 
				else if (objField is System.DateTime) 
				{ 
					return NullDate; 
				} 
				else if (objField is string) 
				{ 
					return NullString; 
				} 
				else if (objField is bool) 
				{ 
					return NullBoolean; 
				} 
				else if (objField is Guid) 
				{ 
					return NullGuid; 
				} 
				else 
				{ 
					throw new NullReferenceException(); 
				} 
			} 
			else 
			{ 
				return NullString; 
			} 
		} 

		public static object SetNull(PropertyInfo objPropertyInfo) 
		{ 
			if (objPropertyInfo.PropertyType.ToString() == "System.Int16" || objPropertyInfo.PropertyType.ToString() == "System.Int32" || objPropertyInfo.PropertyType.ToString() == "System.Int64" || objPropertyInfo.PropertyType.ToString() == "System.Single" || objPropertyInfo.PropertyType.ToString() == "System.Double" || objPropertyInfo.PropertyType.ToString() == "System.Decimal") 
			{ 
				return NullInteger; 
			} 
			else if (objPropertyInfo.PropertyType.ToString() == "System.DateTime") 
			{ 
				return NullDate; 
			} 
			else if (objPropertyInfo.PropertyType.ToString() == "System.String" || objPropertyInfo.PropertyType.ToString() == "System.Char") 
			{ 
				return NullString; 
			} 
			else if (objPropertyInfo.PropertyType.ToString() == "System.Boolean") 
			{ 
				return NullBoolean; 
			} 
			else if (objPropertyInfo.PropertyType.ToString() == "System.Guid") 
			{ 
				return NullGuid; 
			} 
			else 
			{ 
				Type pType = objPropertyInfo.PropertyType; 
				if (pType.BaseType.Equals(typeof(System.Enum))) 
				{ 
					System.Array objEnumValues = System.Enum.GetValues(pType); 
					Array.Sort(objEnumValues); 
					return System.Enum.ToObject(pType, objEnumValues.GetValue(0)); 
				} 
				else 
				{ 
					throw new NullReferenceException(); 
				} 
			} 
		} 

		public static object GetNull(object objField, object objDBNull) 
		{ 
			Object GetNull;
			GetNull = objField; 
			if (objField == null) 
			{ 
				GetNull = objDBNull; 
			} 
			else if (objField is int) 
			{ 
				if (Convert.ToInt32(objField) == NullInteger) 
				{ 
					GetNull = objDBNull; 
				} 
			} 
			else if (objField is float) 
			{ 
				if (Convert.ToSingle(objField) == NullInteger) 
				{ 
					GetNull = objDBNull; 
				} 
			} 
			else if (objField is double) 
			{ 
				if (Convert.ToDouble(objField) == NullInteger) 
				{ 
					GetNull = objDBNull; 
				} 
			} 
			else if (objField is decimal) 
			{ 
				if (Convert.ToDecimal(objField) == NullInteger) 
				{ 
					GetNull = objDBNull; 
				} 
			} 
			else if (objField is System.DateTime) 
			{ 
				if (Convert.ToDateTime(objField) == NullDate) 
				{ 
					GetNull = objDBNull; 
				} 
			} 
			else if (objField is string) 
			{ 
				if (objField == null) 
				{ 
					GetNull = objDBNull; 
				} 
				else 
				{ 
					if (objField.ToString() == NullString) 
					{ 
						GetNull = objDBNull; 
					} 
				} 
			} 
			else if (objField is bool) 
			{ 
				if (Convert.ToBoolean(objField) == NullBoolean) 
				{ 
					GetNull = objDBNull; 
				} 
			} 
			else if (objField is Guid) 
			{ 
				if (((System.Guid)(objField)).Equals(NullGuid)) 
				{ 
					GetNull = objDBNull; 
				} 
			} 
			else 
			{ 
				throw new NullReferenceException(); 
			} 
			return GetNull;
		} 

		public static Boolean IsNull(object objField) 
		{ 
			if (objField.Equals(SetNull(objField))) 
			{ 
				return true; 
			} 
			else 
			{ 
				return false; 
			} 
		} 
	} 
}
