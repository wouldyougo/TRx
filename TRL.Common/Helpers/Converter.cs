using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.Common;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Security;
using System.Text;
using System.Threading;
using System.Windows.Media;
using System.Xml;
using System.Xml.Linq;
namespace Ecng.Common
{
	public static class Converter
	{
		private static readonly Dictionary<Type, DbType> _dbTypes;
		private static readonly Dictionary<string, Type> _aliases;
		private static readonly Dictionary<Type, List<string>> _aliasesByValue;
		private static readonly Dictionary<string, Type> _typeCache;
		static Converter()
		{
			Converter._dbTypes = new Dictionary<Type, DbType>();
			Converter._aliases = new Dictionary<string, Type>();
			Converter._aliasesByValue = new Dictionary<Type, List<string>>();
			Converter._typeCache = new Dictionary<string, Type>();
			Converter._dbTypes.Add(typeof(string), DbType.String);
			Converter._dbTypes.Add(typeof(char), DbType.String);
			Converter._dbTypes.Add(typeof(short), DbType.Int16);
			Converter._dbTypes.Add(typeof(int), DbType.Int32);
			Converter._dbTypes.Add(typeof(long), DbType.Int64);
			Converter._dbTypes.Add(typeof(ushort), DbType.UInt16);
			Converter._dbTypes.Add(typeof(uint), DbType.UInt32);
			Converter._dbTypes.Add(typeof(ulong), DbType.UInt64);
			Converter._dbTypes.Add(typeof(float), DbType.Single);
			Converter._dbTypes.Add(typeof(double), DbType.Double);
			Converter._dbTypes.Add(typeof(decimal), DbType.Decimal);
			Converter._dbTypes.Add(typeof(DateTime), DbType.DateTime);
			Converter._dbTypes.Add(typeof(DateTimeOffset), DbType.DateTimeOffset);
			Converter._dbTypes.Add(typeof(TimeSpan), DbType.Time);
			Converter._dbTypes.Add(typeof(Guid), DbType.Guid);
			Converter._dbTypes.Add(typeof(byte[]), DbType.Binary);
			Converter._dbTypes.Add(typeof(byte), DbType.Byte);
			Converter._dbTypes.Add(typeof(sbyte), DbType.SByte);
			Converter._dbTypes.Add(typeof(bool), DbType.Boolean);
			Converter._dbTypes.Add(typeof(object), DbType.Object);
			Converter.AddAlias(typeof(object), "object");
			Converter.AddAlias(typeof(bool), "bool");
			Converter.AddAlias(typeof(bool), "boolean");
			Converter.AddAlias(typeof(byte), "byte");
			Converter.AddAlias(typeof(sbyte), "sbyte");
			Converter.AddAlias(typeof(char), "char");
			Converter.AddAlias(typeof(char), "character");
			Converter.AddAlias(typeof(decimal), "decimal");
			Converter.AddAlias(typeof(decimal), "money");
			Converter.AddAlias(typeof(double), "double");
			Converter.AddAlias(typeof(float), "float");
			Converter.AddAlias(typeof(float), "single");
			Converter.AddAlias(typeof(float), "real");
			Converter.AddAlias(typeof(int), "int");
			Converter.AddAlias(typeof(uint), "uint");
			Converter.AddAlias(typeof(long), "long");
			Converter.AddAlias(typeof(ulong), "ulong");
			Converter.AddAlias(typeof(short), "short");
			Converter.AddAlias(typeof(ushort), "ushort");
			Converter.AddAlias(typeof(string), "string");
			Converter.AddAlias(typeof(DateTime), "date");
			Converter.AddAlias(typeof(DateTime), "datetime");
			Converter.AddAlias(typeof(TimeSpan), "time");
			Converter.AddAlias(typeof(TimeSpan), "timespan");
			Converter.AddAlias(typeof(IntPtr), "ptr");
			Converter.AddAlias(typeof(IntPtr), "intptr");
			Converter.AddAlias(typeof(UIntPtr), "uptr");
			Converter.AddAlias(typeof(UIntPtr), "uintptr");
			Converter.AddAlias(typeof(void), "void");
			Converter.AddAlias(typeof(Guid), "guid");
		}
		public static string GetHost(this EndPoint endPoint)
		{
			if (endPoint == null)
			{
				throw new ArgumentNullException("endPoint");
			}
			if (endPoint is IPEndPoint)
			{
				return ((IPEndPoint)endPoint).Address.ToString();
			}
			if (endPoint is DnsEndPoint)
			{
				return ((DnsEndPoint)endPoint).Host;
			}
			throw new InvalidOperationException("Неизвестная информация об адресе.");
		}
		public static int GetPort(this EndPoint endPoint)
		{
			if (endPoint == null)
			{
				throw new ArgumentNullException("endPoint");
			}
			if (endPoint is IPEndPoint)
			{
				return ((IPEndPoint)endPoint).Port;
			}
			if (endPoint is DnsEndPoint)
			{
				return ((DnsEndPoint)endPoint).Port;
			}
			throw new InvalidOperationException("Неизвестная информация об адресе.");
		}
		public static object To(this object value, Type destinationType)
		{
			if (destinationType == null)
			{
				throw new ArgumentNullException("destinationType");
			}
			object result;
			try
			{
				if (value == null)
				{
					if ((destinationType.IsValueType || destinationType.IsEnum()) && !destinationType.IsNullable())
					{
						throw new ArgumentNullException("value");
					}
					result = null;
				}
				else
				{
					Type type = value.GetType();
					object obj;
					if (destinationType.IsAssignableFrom(type))
					{
						obj = value;
					}
					else
					{
						if (value is Type && destinationType == typeof(DbType))
						{
							Type type2 = (Type)value;
							if (type2.IsNullable())
							{
								type2 = type2.GetGenericArguments()[0];
							}
							if (type2.IsEnum())
							{
								type2 = type2.GetEnumBaseType();
							}
							DbType dbType;
							if (!Converter._dbTypes.TryGetValue(type2, out dbType))
							{
								throw new ArgumentException(".NET type {0} doesn't have associated db type.".Put(new object[]
								{
									type2
								}));
							}
							obj = dbType;
						}
						else
						{
							if (value is DbType && destinationType == typeof(Type))
							{
								obj = Converter._dbTypes.Values.First((DbType arg) => (DbType)value == arg);
							}
							else
							{
								if (value is string && destinationType == typeof(byte[]))
								{
									obj = Encoding.Unicode.GetBytes((string)value);
								}
								else
								{
									if (value is byte[] && destinationType == typeof(string))
									{
										obj = Encoding.Unicode.GetString((byte[])value);
									}
									else
									{
										if (value is bool[] && destinationType == typeof(BitArray))
										{
											obj = new BitArray((bool[])value);
										}
										else
										{
											if (value is BitArray && destinationType == typeof(bool[]))
											{
												BitArray bitArray = (BitArray)value;
												bool[] array = new bool[bitArray.Length];
												bitArray.CopyTo(array, 0);
												obj = array;
											}
											else
											{
												if (value is byte[] && destinationType == typeof(BitArray))
												{
													obj = new BitArray((byte[])value);
												}
												else
												{
													if (value is BitArray && destinationType == typeof(byte[]))
													{
														BitArray bitArray2 = (BitArray)value;
														byte[] array2 = new byte[(int)((double)bitArray2.Length / 8.0).Ceiling()];
														bitArray2.CopyTo(array2, 0);
														obj = array2;
													}
													else
													{
														if (value is IPAddress)
														{
															IPAddress iPAddress = (IPAddress)value;
															if (destinationType == typeof(string))
															{
																obj = iPAddress.ToString();
															}
															else
															{
																if (destinationType == typeof(byte[]))
																{
																	obj = iPAddress.GetAddressBytes();
																}
																else
																{
																	if (!(destinationType == typeof(long)))
																	{
																		throw new ArgumentException("Can't convert IPAddress to type '{0}'.".Put(new object[]
																		{
																			destinationType
																		}), "value");
																	}
																	AddressFamily addressFamily = iPAddress.AddressFamily;
																	if (addressFamily != AddressFamily.InterNetwork)
																	{
																		if (addressFamily != AddressFamily.InterNetworkV6)
																		{
																			throw new ArgumentException("Can't convert IPAddress to long.", "value");
																		}
																		obj = iPAddress.ScopeId;
																	}
																	else
																	{
																		byte[] addressBytes = iPAddress.GetAddressBytes();
																		obj = ((long)((int)addressBytes[3] << 24 | (int)addressBytes[2] << 16 | (int)addressBytes[1] << 8 | (int)addressBytes[0]) & (long)((ulong)-1));
																	}
																}
															}
														}
														else
														{
															if (destinationType == typeof(IPAddress))
															{
																if (value is string)
																{
																	obj = IPAddress.Parse((string)value);
																}
																else
																{
																	if (value is byte[])
																	{
																		obj = new IPAddress((byte[])value);
																	}
																	else
																	{
																		if (!(value is long))
																		{
																			throw new ArgumentException("Can't convert type '{0}' to IPAddress.".Put(new object[]
																			{
																				destinationType
																			}), "value");
																		}
																		obj = new IPAddress((long)value);
																	}
																}
															}
															else
															{
																if (value is string && typeof(EndPoint).IsAssignableFrom(destinationType))
																{
																	string text = (string)value;
																	int num = text.LastIndexOf(':');
																	if (num != -1)
																	{
																		string text2 = text.Substring(0, num);
																		int port = text.Substring(num + 1).To<int>();
																		IPAddress address;
																		if (destinationType == typeof(IPEndPoint))
																		{
																			address = text2.To<IPAddress>();
																		}
																		else
																		{
																			if (!IPAddress.TryParse(text2, out address))
																			{
																				result = new DnsEndPoint(text2, port);
																				return result;
																			}
																		}
																		result = new IPEndPoint(address, port);
																		return result;
																	}
																	throw new FormatException("Invalid endpoint format.");
																}
																else
																{
																	if (destinationType == typeof(string) && value is EndPoint)
																	{
																		EndPoint endPoint = (EndPoint)value;
																		obj = endPoint.GetHost() + ":" + endPoint.GetPort();
																	}
																	else
																	{
																		if (destinationType.IsEnum() && (value is string || type.IsPrimitive))
																		{
																			if (value is string)
																			{
																				obj = Enum.Parse(destinationType, (string)value, true);
																			}
																			else
																			{
																				obj = Enum.ToObject(destinationType, value);
																			}
																		}
																		else
																		{
																			if (value is string && destinationType == typeof(Type))
																			{
																				string text3 = (string)value;
																				string key = text3.ToLowerInvariant();
																				Type type3;
																				if (!Converter._aliases.TryGetValue(key, out type3) && !Converter._typeCache.TryGetValue(key, out type3))
																				{
																					lock (Converter._typeCache)
																					{
																						if (!Converter._typeCache.TryGetValue(key, out type3))
																						{
																							type3 = Type.GetType(text3, false, true);
																							if (type3 == null)
																							{
																								string[] parts = text3.Split(", ", true);
																								if (parts.Length == 2)
																								{
																									Assembly assembly = AppDomain.CurrentDomain.GetAssemblies().FirstOrDefault((Assembly a) => a.GetName().Name == parts[1]) ?? Assembly.LoadWithPartialName(parts[1]);
																									if (assembly != null)
																									{
																										type3 = assembly.GetType(parts[0]);
																									}
																								}
																							}
																							if (!(type3 != null))
																							{
																								throw new ArgumentException("Type {0} doesn't exists.".Put(new object[]
																								{
																									value
																								}), "value");
																							}
																							Converter._typeCache.Add(key, type3);
																						}
																					}
																				}
																				obj = type3;
																			}
																			else
																			{
																				if (value is Type && destinationType == typeof(string))
																				{
																					obj = ((Type)value).AssemblyQualifiedName;
																				}
																				else
																				{
																					if (value is string && destinationType == typeof(StringBuilder))
																					{
																						obj = new StringBuilder((string)value);
																					}
																					else
																					{
																						if (value is StringBuilder && destinationType == typeof(string))
																						{
																							obj = value.ToString();
																						}
																						else
																						{
																							if (value is string && destinationType == typeof(DbConnectionStringBuilder))
																							{
																								obj = new DbConnectionStringBuilder
																								{
																									ConnectionString = (string)value
																								};
																							}
																							else
																							{
																								if (value is DbConnectionStringBuilder && destinationType == typeof(string))
																								{
																									obj = value.ToString();
																								}
																								else
																								{
																									if (value is SecureString && destinationType == typeof(string))
																									{
																										IntPtr intPtr = Marshal.SecureStringToBSTR((SecureString)value);
																										using (intPtr.MakeDisposable(new Action<IntPtr>(Marshal.ZeroFreeBSTR)))
																										{
																											obj = Marshal.PtrToStringBSTR(intPtr);
																											goto IL_2351;
																										}
																									}
																									if (value is string && destinationType == typeof(SecureString))
																									{
																										obj = ((string)value).ToCharArray().To(destinationType);
																									}
																									else
																									{
																										if (value is byte[] && destinationType == typeof(SecureString))
																										{
																											byte[] array3 = (byte[])value;
																											char[] array4 = new char[array3.Length / 2];
																											int num2 = 0;
																											for (int i = 0; i < array3.Length; i += 2)
																											{
																												array4[num2++] = BitConverter.ToChar(new byte[]
																												{
																													array3[i],
																													array3[i + 1]
																												}, 0);
																											}
																											obj = array4.To(destinationType);
																										}
																										else
																										{
																											if (value is char[] && destinationType == typeof(SecureString))
																											{
																												SecureString secureString = new SecureString();
																												char[] array5 = (char[])value;
																												for (int j = 0; j < array5.Length; j++)
																												{
																													char c = array5[j];
																													secureString.AppendChar(c);
																												}
																												obj = secureString;
																											}
																											else
																											{
																												if (value is string && destinationType == typeof(DbProviderFactory))
																												{
																													obj = DbProviderFactories.GetFactory((string)value);
																												}
																												else
																												{
																													if (destinationType == typeof(Type[]))
																													{
																														if (!(value is IEnumerable<object>))
																														{
																															value = new object[]
																															{
																																value
																															};
																														}
																														obj = ((IEnumerable<object>)value).Select(delegate(object arg)
																														{
																															if (arg != null)
																															{
																																return arg.GetType();
																															}
																															return typeof(void);
																														}).ToArray<Type>();
																													}
																													else
																													{
																														if (value is Stream && destinationType == typeof(string))
																														{
																															obj = value.To<byte[]>().To<string>();
																														}
																														else
																														{
																															if (value is Stream && destinationType == typeof(byte[]))
																															{
																																Stream stream = (Stream)value;
																																MemoryStream memoryStream;
																																if (stream is MemoryStream)
																																{
																																	memoryStream = (MemoryStream)stream;
																																}
																																else
																																{
																																	memoryStream = new MemoryStream(4096);
																																	byte[] buffer = new byte[1024];
																																	while (true)
																																	{
																																		int num3 = stream.Read(buffer, 0, 1024);
																																		if (num3 == 0)
																																		{
																																			break;
																																		}
																																		memoryStream.Write(buffer, 0, num3);
																																	}
																																}
																																obj = memoryStream.ToArray();
																															}
																															else
																															{
																																if (value is byte[] && destinationType == typeof(Stream))
																																{
																																	MemoryStream memoryStream2 = new MemoryStream(((byte[])value).Length);
																																	memoryStream2.Write((byte[])value, 0, memoryStream2.Capacity);
																																	memoryStream2.Position = 0L;
																																	obj = memoryStream2;
																																}
																																else
																																{
																																	if (value is string && destinationType == typeof(Stream))
																																	{
																																		obj = value.To<byte[]>().To<Stream>();
																																	}
																																	else
																																	{
																																		if (destinationType == typeof(byte[]))
																																		{
																																			if (value is Enum)
																																			{
																																				value = value.To(type.GetEnumBaseType());
																																			}
																																			if (value is byte)
																																			{
																																				obj = new byte[]
																																				{
																																					(byte)value
																																				};
																																			}
																																			else
																																			{
																																				if (value is bool)
																																				{
																																					obj = BitConverter.GetBytes((bool)value);
																																				}
																																				else
																																				{
																																					if (value is char)
																																					{
																																						obj = BitConverter.GetBytes((char)value);
																																					}
																																					else
																																					{
																																						if (value is short)
																																						{
																																							obj = BitConverter.GetBytes((short)value);
																																						}
																																						else
																																						{
																																							if (value is int)
																																							{
																																								obj = BitConverter.GetBytes((int)value);
																																							}
																																							else
																																							{
																																								if (value is long)
																																								{
																																									obj = BitConverter.GetBytes((long)value);
																																								}
																																								else
																																								{
																																									if (value is ushort)
																																									{
																																										obj = BitConverter.GetBytes((ushort)value);
																																									}
																																									else
																																									{
																																										if (value is uint)
																																										{
																																											obj = BitConverter.GetBytes((uint)value);
																																										}
																																										else
																																										{
																																											if (value is ulong)
																																											{
																																												obj = BitConverter.GetBytes((ulong)value);
																																											}
																																											else
																																											{
																																												if (value is float)
																																												{
																																													obj = BitConverter.GetBytes((float)value);
																																												}
																																												else
																																												{
																																													if (value is double)
																																													{
																																														obj = BitConverter.GetBytes((double)value);
																																													}
																																													else
																																													{
																																														if (value is DateTime)
																																														{
																																															obj = BitConverter.GetBytes(((DateTime)value).Ticks);
																																														}
																																														else
																																														{
																																															if (value is DateTimeOffset)
																																															{
																																																obj = BitConverter.GetBytes(((DateTimeOffset)value).UtcTicks);
																																															}
																																															else
																																															{
																																																if (value is Guid)
																																																{
																																																	obj = ((Guid)value).ToByteArray();
																																																}
																																																else
																																																{
																																																	if (value is TimeSpan)
																																																	{
																																																		obj = BitConverter.GetBytes(((TimeSpan)value).Ticks);
																																																	}
																																																	else
																																																	{
																																																		if (!(value is decimal))
																																																		{
																																																			throw new ArgumentException("Can't convert type '{0}' to byte[].".Put(new object[]
																																																			{
																																																				type
																																																			}), "value");
																																																		}
																																																		int[] bits = decimal.GetBits((decimal)value);
																																																		int num4 = bits[0];
																																																		int num5 = bits[1];
																																																		int num6 = bits[2];
																																																		int num7 = bits[3];
																																																		obj = new byte[]
																																																		{
																																																			(byte)num4,
																																																			(byte)(num4 >> 8),
																																																			(byte)(num4 >> 16),
																																																			(byte)(num4 >> 24),
																																																			(byte)num5,
																																																			(byte)(num5 >> 8),
																																																			(byte)(num5 >> 16),
																																																			(byte)(num5 >> 24),
																																																			(byte)num6,
																																																			(byte)(num6 >> 8),
																																																			(byte)(num6 >> 16),
																																																			(byte)(num6 >> 24),
																																																			(byte)num7,
																																																			(byte)(num7 >> 8),
																																																			(byte)(num7 >> 16),
																																																			(byte)(num7 >> 24)
																																																		};
																																																	}
																																																}
																																															}
																																														}
																																													}
																																												}
																																											}
																																										}
																																									}
																																								}
																																							}
																																						}
																																					}
																																				}
																																			}
																																		}
																																		else
																																		{
																																			if (value is byte[])
																																			{
																																				Type type4;
																																				if (destinationType.IsEnum())
																																				{
																																					type4 = destinationType;
																																					destinationType = destinationType.GetEnumBaseType();
																																				}
																																				else
																																				{
																																					type4 = null;
																																				}
																																				if (destinationType == typeof(byte))
																																				{
																																					obj = ((byte[])value)[0];
																																				}
																																				else
																																				{
																																					if (destinationType == typeof(bool))
																																					{
																																						obj = BitConverter.ToBoolean((byte[])value, 0);
																																					}
																																					else
																																					{
																																						if (destinationType == typeof(char))
																																						{
																																							obj = BitConverter.ToChar((byte[])value, 0);
																																						}
																																						else
																																						{
																																							if (destinationType == typeof(short))
																																							{
																																								obj = BitConverter.ToInt16((byte[])value, 0);
																																							}
																																							else
																																							{
																																								if (destinationType == typeof(int))
																																								{
																																									obj = BitConverter.ToInt32((byte[])value, 0);
																																								}
																																								else
																																								{
																																									if (destinationType == typeof(long))
																																									{
																																										obj = BitConverter.ToInt64((byte[])value, 0);
																																									}
																																									else
																																									{
																																										if (destinationType == typeof(ushort))
																																										{
																																											obj = BitConverter.ToUInt16((byte[])value, 0);
																																										}
																																										else
																																										{
																																											if (destinationType == typeof(uint))
																																											{
																																												obj = BitConverter.ToUInt32((byte[])value, 0);
																																											}
																																											else
																																											{
																																												if (destinationType == typeof(ulong))
																																												{
																																													obj = BitConverter.ToUInt64((byte[])value, 0);
																																												}
																																												else
																																												{
																																													if (destinationType == typeof(float))
																																													{
																																														obj = BitConverter.ToSingle((byte[])value, 0);
																																													}
																																													else
																																													{
																																														if (destinationType == typeof(double))
																																														{
																																															obj = BitConverter.ToDouble((byte[])value, 0);
																																														}
																																														else
																																														{
																																															if (destinationType == typeof(DateTime))
																																															{
																																																obj = new DateTime(BitConverter.ToInt64((byte[])value, 0));
																																															}
																																															else
																																															{
																																																if (destinationType == typeof(DateTimeOffset))
																																																{
																																																	obj = new DateTimeOffset(BitConverter.ToInt64((byte[])value, 0), TimeSpan.Zero);
																																																}
																																																else
																																																{
																																																	if (destinationType == typeof(Guid))
																																																	{
																																																		obj = new Guid((byte[])value);
																																																	}
																																																	else
																																																	{
																																																		if (destinationType == typeof(TimeSpan))
																																																		{
																																																			obj = new TimeSpan(BitConverter.ToInt64((byte[])value, 0));
																																																		}
																																																		else
																																																		{
																																																			if (destinationType == typeof(decimal))
																																																			{
																																																				byte[] array6 = (byte[])value;
																																																				int[] bits2 = new int[]
																																																				{
																																																					(int)array6[0] | (int)array6[1] << 8 | (int)array6[2] << 16 | (int)array6[3] << 24,
																																																					(int)array6[4] | (int)array6[5] << 8 | (int)array6[6] << 16 | (int)array6[7] << 24,
																																																					(int)array6[8] | (int)array6[9] << 8 | (int)array6[10] << 16 | (int)array6[11] << 24,
																																																					(int)array6[12] | (int)array6[13] << 8 | (int)array6[14] << 16 | (int)array6[15] << 24
																																																				};
																																																				result = new decimal(bits2);
																																																				return result;
																																																			}
																																																			throw new ArgumentException("Can't convert byte[] to type '{0}'.".Put(new object[]
																																																			{
																																																				destinationType
																																																			}), "value");
																																																		}
																																																	}
																																																}
																																															}
																																														}
																																													}
																																												}
																																											}
																																										}
																																									}
																																								}
																																							}
																																						}
																																					}
																																				}
																																				if (type4 != null)
																																				{
																																					obj = Enum.ToObject(type4, obj);
																																				}
																																			}
																																			else
																																			{
																																				if (value is TimeSpan && destinationType == typeof(long))
																																				{
																																					obj = ((TimeSpan)value).Ticks;
																																				}
																																				else
																																				{
																																					if (value is long && destinationType == typeof(TimeSpan))
																																					{
																																						obj = new TimeSpan((long)value);
																																					}
																																					else
																																					{
																																						if (value is DateTime && destinationType == typeof(long))
																																						{
																																							obj = ((DateTime)value).Ticks;
																																						}
																																						else
																																						{
																																							if (value is long && destinationType == typeof(DateTime))
																																							{
																																								obj = new DateTime((long)value);
																																							}
																																							else
																																							{
																																								if (value is DateTimeOffset && destinationType == typeof(long))
																																								{
																																									obj = ((DateTimeOffset)value).UtcTicks;
																																								}
																																								else
																																								{
																																									if (value is long && destinationType == typeof(DateTimeOffset))
																																									{
																																										obj = new DateTimeOffset((long)value, TimeSpan.Zero);
																																									}
																																									else
																																									{
																																										if (value is DateTime && destinationType == typeof(double))
																																										{
																																											obj = ((DateTime)value).ToOADate();
																																										}
																																										else
																																										{
																																											if (value is double && destinationType == typeof(DateTime))
																																											{
																																												obj = DateTime.FromOADate((double)value);
																																											}
																																											else
																																											{
																																												if (value is DateTime && destinationType == typeof(DateTimeOffset))
																																												{
																																													DateTime dateTime = (DateTime)value;
																																													if (dateTime == DateTime.MinValue)
																																													{
																																														obj = DateTimeOffset.MinValue;
																																													}
																																													else
																																													{
																																														if (dateTime == DateTime.MaxValue)
																																														{
																																															obj = DateTimeOffset.MaxValue;
																																														}
																																														else
																																														{
																																															obj = new DateTimeOffset(dateTime);
																																														}
																																													}
																																												}
																																												else
																																												{
																																													if (value is System.Drawing.Color && destinationType == typeof(int))
																																													{
																																														obj = ((System.Drawing.Color)value).ToArgb();
																																													}
																																													else
																																													{
																																														if (value is int && destinationType == typeof(System.Drawing.Color))
																																														{
																																															int num8 = (int)value;
																																															obj = System.Drawing.Color.FromArgb((int)((byte)((long)(num8 >> 24) & 255L)), (int)((byte)((long)(num8 >> 16) & 255L)), (int)((byte)((long)(num8 >> 8) & 255L)), (int)((byte)((long)num8 & 255L)));
																																														}
																																														else
																																														{
																																															if (value is System.Drawing.Color && destinationType == typeof(string))
																																															{
																																																obj = ColorTranslator.ToHtml((System.Drawing.Color)value);
																																															}
																																															else
																																															{
																																																if (value is string && destinationType == typeof(System.Drawing.Color))
																																																{
																																																	obj = ColorTranslator.FromHtml((string)value);
																																																}
																																																else
																																																{
																																																	if (value is System.Windows.Media.Color && destinationType == typeof(int))
																																																	{
																																																		System.Windows.Media.Color color = (System.Windows.Media.Color)value;
																																																		obj = ((int)color.A << 24 | (int)color.R << 16 | (int)color.G << 8 | (int)color.B);
																																																	}
																																																	else
																																																	{
																																																		if (value is int && destinationType == typeof(System.Windows.Media.Color))
																																																		{
																																																			int num9 = (int)value;
																																																			obj = System.Windows.Media.Color.FromArgb((byte)(num9 >> 24), (byte)(num9 >> 16), (byte)(num9 >> 8), (byte)num9);
																																																		}
																																																		else
																																																		{
																																																			if (value is System.Windows.Media.Color && destinationType == typeof(string))
																																																			{
																																																				obj = ((System.Windows.Media.Color)value).ToString();
																																																			}
																																																			else
																																																			{
																																																				if (value is string && destinationType == typeof(System.Windows.Media.Color))
																																																				{
																																																					obj = System.Windows.Media.ColorConverter.ConvertFromString((string)value);
																																																				}
																																																				else
																																																				{
																																																					if (value is Uri && destinationType == typeof(string))
																																																					{
																																																						obj = value.ToString();
																																																					}
																																																					else
																																																					{
																																																						if (value is string && destinationType == typeof(Uri))
																																																						{
																																																							obj = new Uri((string)value);
																																																						}
																																																						else
																																																						{
																																																							if (value is Version && destinationType == typeof(string))
																																																							{
																																																								obj = value.ToString();
																																																							}
																																																							else
																																																							{
																																																								if (value is string && destinationType == typeof(Version))
																																																								{
																																																									obj = new Version((string)value);
																																																								}
																																																								else
																																																								{
																																																									if (value is int && destinationType == typeof(IntPtr))
																																																									{
																																																										obj = new IntPtr((int)value);
																																																									}
																																																									else
																																																									{
																																																										if (value is long && destinationType == typeof(IntPtr))
																																																										{
																																																											obj = new IntPtr((long)value);
																																																										}
																																																										else
																																																										{
																																																											if (value is uint && destinationType == typeof(UIntPtr))
																																																											{
																																																												obj = new UIntPtr((uint)value);
																																																											}
																																																											else
																																																											{
																																																												if (value is ulong && destinationType == typeof(UIntPtr))
																																																												{
																																																													obj = new UIntPtr((ulong)value);
																																																												}
																																																												else
																																																												{
																																																													if (value is IntPtr && destinationType == typeof(int))
																																																													{
																																																														obj = ((IntPtr)value).ToInt32();
																																																													}
																																																													else
																																																													{
																																																														if (value is IntPtr && destinationType == typeof(long))
																																																														{
																																																															obj = ((IntPtr)value).ToInt64();
																																																														}
																																																														else
																																																														{
																																																															if (value is UIntPtr && destinationType == typeof(uint))
																																																															{
																																																																obj = ((UIntPtr)value).ToUInt32();
																																																															}
																																																															else
																																																															{
																																																																if (value is UIntPtr && destinationType == typeof(ulong))
																																																																{
																																																																	obj = ((UIntPtr)value).ToUInt64();
																																																																}
																																																																else
																																																																{
																																																																	if (value is CultureInfo && destinationType == typeof(int))
																																																																	{
																																																																		obj = ((CultureInfo)value).LCID;
																																																																	}
																																																																	else
																																																																	{
																																																																		if (value is int && destinationType == typeof(CultureInfo))
																																																																		{
																																																																			obj = new CultureInfo((int)value);
																																																																		}
																																																																		else
																																																																		{
																																																																			if (destinationType.GetUnderlyingType() != null)
																																																																			{
																																																																				if (value is string && (string)value == string.Empty)
																																																																				{
																																																																					obj = destinationType.CreateInstance(new object[0]);
																																																																				}
																																																																				else
																																																																				{
																																																																					obj = destinationType.CreateInstance(new object[]
																																																																					{
																																																																						value.To(destinationType.GetUnderlyingType())
																																																																					});
																																																																				}
																																																																			}
																																																																			else
																																																																			{
																																																																				if (value is string && destinationType == typeof(TimeSpan))
																																																																				{
																																																																					obj = TimeSpan.Parse((string)value);
																																																																				}
																																																																				else
																																																																				{
																																																																					if (value is TimeSpan && destinationType == typeof(string))
																																																																					{
																																																																						obj = value.ToString();
																																																																					}
																																																																					else
																																																																					{
																																																																						if (value is DateTime && destinationType == typeof(string))
																																																																						{
																																																																							DateTime dateTime2 = (DateTime)value;
																																																																							obj = ((dateTime2.Millisecond > 0) ? dateTime2.ToString("o") : value.ToString());
																																																																						}
																																																																						else
																																																																						{
																																																																							if (value is DateTimeOffset && destinationType == typeof(string))
																																																																							{
																																																																								DateTimeOffset dateTimeOffset = (DateTimeOffset)value;
																																																																								obj = ((dateTimeOffset.Millisecond > 0) ? dateTimeOffset.ToString("o") : value.ToString());
																																																																							}
																																																																							else
																																																																							{
																																																																								if (value is string && destinationType == typeof(DateTimeOffset))
																																																																								{
																																																																									obj = DateTimeOffset.Parse((string)value);
																																																																								}
																																																																								else
																																																																								{
																																																																									if (value is string && destinationType == typeof(TimeZoneInfo))
																																																																									{
																																																																										obj = TimeZoneInfo.FromSerializedString((string)value);
																																																																									}
																																																																									else
																																																																									{
																																																																										if (value is TimeZoneInfo && destinationType == typeof(string))
																																																																										{
																																																																											obj = ((TimeZoneInfo)value).ToSerializedString();
																																																																										}
																																																																										else
																																																																										{
																																																																											if (value is string && destinationType == typeof(Guid))
																																																																											{
																																																																												obj = new Guid((string)value);
																																																																											}
																																																																											else
																																																																											{
																																																																												if (value is Guid && destinationType == typeof(string))
																																																																												{
																																																																													obj = value.ToString();
																																																																												}
																																																																												else
																																																																												{
																																																																													if (value is string && destinationType == typeof(XDocument))
																																																																													{
																																																																														obj = XDocument.Parse((string)value);
																																																																													}
																																																																													else
																																																																													{
																																																																														if (value is string && destinationType == typeof(XElement))
																																																																														{
																																																																															obj = XElement.Parse((string)value);
																																																																														}
																																																																														else
																																																																														{
																																																																															if (value is XNode && destinationType == typeof(string))
																																																																															{
																																																																																obj = value.ToString();
																																																																															}
																																																																															else
																																																																															{
																																																																																if (value is string && destinationType == typeof(XmlDocument))
																																																																																{
																																																																																	XmlDocument xmlDocument = new XmlDocument();
																																																																																	xmlDocument.LoadXml((string)value);
																																																																																	obj = xmlDocument;
																																																																																}
																																																																																else
																																																																																{
																																																																																	if (value is XmlNode && destinationType == typeof(string))
																																																																																	{
																																																																																		obj = ((XmlNode)value).OuterXml;
																																																																																	}
																																																																																	else
																																																																																	{
																																																																																		if (value is string && destinationType == typeof(decimal))
																																																																																		{
																																																																																			obj = decimal.Parse((string)value, NumberStyles.Any, null);
																																																																																		}
																																																																																		else
																																																																																		{
																																																																																			TypeConverterAttribute attribute = destinationType.GetAttribute(true);
																																																																																			if (attribute != null)
																																																																																			{
																																																																																				ConstructorInfo[] constructors = attribute.ConverterTypeName.To<Type>().GetConstructors();
																																																																																				if (constructors.Length == 1)
																																																																																				{
																																																																																					ConstructorInfo constructorInfo = constructors[0];
																																																																																					TypeConverter typeConverter = (TypeConverter)((constructorInfo.GetParameters().Length == 0) ? constructorInfo.Invoke(null) : constructorInfo.Invoke(new object[]
																																																																																					{
																																																																																						destinationType
																																																																																					}));
																																																																																					if (typeConverter.CanConvertFrom(type))
																																																																																					{
																																																																																						result = typeConverter.ConvertFrom(value);
																																																																																						return result;
																																																																																					}
																																																																																				}
																																																																																			}
																																																																																			try
																																																																																			{
																																																																																				obj = Convert.ChangeType(value, destinationType, null);
																																																																																			}
																																																																																			catch
																																																																																			{
																																																																																				MethodInfo[] methods = type.GetMethods(BindingFlags.Static | BindingFlags.Public);
																																																																																				MethodInfo methodInfo = methods.FirstOrDefault((MethodInfo mi) => (mi.Name == "op_Implicit" || mi.Name == "op_Explicit") && mi.ReturnType == destinationType);
																																																																																				if (methodInfo == null)
																																																																																				{
																																																																																					throw;
																																																																																				}
																																																																																				obj = methodInfo.Invoke(null, new object[]
																																																																																				{
																																																																																					value
																																																																																				});
																																																																																			}
																																																																																		}
																																																																																	}
																																																																																}
																																																																															}
																																																																														}
																																																																													}
																																																																												}
																																																																											}
																																																																										}
																																																																									}
																																																																								}
																																																																							}
																																																																						}
																																																																					}
																																																																				}
																																																																			}
																																																																		}
																																																																	}
																																																																}
																																																															}
																																																														}
																																																													}
																																																												}
																																																											}
																																																										}
																																																									}
																																																								}
																																																							}
																																																						}
																																																					}
																																																				}
																																																			}
																																																		}
																																																	}
																																																}
																																															}
																																														}
																																													}
																																												}
																																											}
																																										}
																																									}
																																								}
																																							}
																																						}
																																					}
																																				}
																																			}
																																		}
																																	}
																																}
																															}
																														}
																													}
																												}
																											}
																										}
																									}
																								}
																							}
																						}
																					}
																				}
																			}
																		}
																	}
																}
															}
														}
													}
												}
											}
										}
									}
								}
							}
						}
					}
					IL_2351:
					result = obj;
				}
			}
			catch (Exception innerException)
			{
				throw new InvalidCastException("Cannot convert {0} to type {1}.".Put(new object[]
				{
					value,
					destinationType
				}), innerException);
			}
			return result;
		}
		public static T To<T>(this object value)
		{
			return (T)((object)value.To(typeof(T)));
		}
		public static void AddAlias(Type type, string name)
		{
			if (type == null)
			{
				throw new ArgumentNullException("type");
			}
			if (name.IsEmpty())
			{
				throw new ArgumentNullException("name");
			}
			Converter._aliases.Add(name, type);
			List<string> list;
			if (!Converter._aliasesByValue.TryGetValue(type, out list))
			{
				list = new List<string>();
				Converter._aliasesByValue.Add(type, list);
			}
			list.Add(name);
		}
		public static string GetAlias(Type type)
		{
			List<string> source;
			if (!Converter._aliasesByValue.TryGetValue(type, out source))
			{
				return null;
			}
			return source.FirstOrDefault<string>();
		}
		public static T DoInCulture<T>(this CultureInfo cultureInfo, Func<T> func)
		{
			if (cultureInfo == null)
			{
				throw new ArgumentNullException("cultureInfo");
			}
			if (func == null)
			{
				throw new ArgumentNullException("func");
			}
			CultureInfo currentCulture = Thread.CurrentThread.CurrentCulture;
			Thread.CurrentThread.CurrentCulture = cultureInfo;
			T result;
			try
			{
				result = func();
			}
			finally
			{
				Thread.CurrentThread.CurrentCulture = currentCulture;
			}
			return result;
		}
		public static void DoInCulture(this CultureInfo cultureInfo, Action action)
		{
			if (action == null)
			{
				throw new ArgumentNullException("action");
			}
			cultureInfo.DoInCulture(delegate
			{
				action();
				return null;
			});
		}
		public static string ToRadix(this long decimalNumber, int radix)
		{
			if (radix < 2 || radix > "0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZ".Length)
			{
				throw new ArgumentOutOfRangeException("radix", radix, "The radix must be >= 2 and <= {0}.".Put(new object[]
				{
					"0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZ".Length
				}));
			}
			if (decimalNumber == 0L)
			{
				return "0";
			}
			int num = 63;
			long num2 = Math.Abs(decimalNumber);
			char[] array = new char[64];
			while (num2 != 0L)
			{
				int index = (int)(num2 % (long)radix);
				array[num--] = "0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZ"[index];
				num2 /= (long)radix;
			}
			string text = new string(array, num + 1, 64 - num - 1);
			if (decimalNumber < 0L)
			{
				text = "-" + text;
			}
			return text;
		}
	}
}
