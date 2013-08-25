/*
 * Created by SharpDevelop.
 * User: ekr
 * Date: 25/08/2013
 * Time: 12:13
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.IO;
using Microsoft.Deployment.WindowsInstaller;
using Microsoft.Deployment.WindowsInstaller.Linq;
using Microsoft.Deployment.WindowsInstaller.Linq.Entities;

namespace DXInstaller
{
	/// <summary>
	/// MSIInstaller is a class for representing the information found in an MSI installer package
	/// </summary>
	public class MSIInstaller :  IInstaller
	{
		#region Properties
		private FileInfo _file;
		public FileInfo File {
			get { return _file; }
		}

//		private QDatabase _msi;
//		public QDatabase MSI {
//			get {
//				if (_msi == null || _msi.IsClosed) {
//					_msi = new QDatabase(this.File.FullName);
//				}
//				return _msi;
//			}
//		}
		private List<Property_> properties;
		public IEnumerable<Property_> Properties {
			get {
				if (IsInstaller) {
					using(QDatabase msi = new QDatabase(this.File.FullName))
					{
						var props = msi.Properties.Select(xx => xx);
						properties = new List<Property_>(props);
					}
				}
				return properties;
			}
		}
		private Boolean _isInstaller;
		public Boolean IsInstaller {
			get { return _isInstaller; }
		}

		private String _subject;
		public String Subject {
			get {
				return _subject;
			}
		}

		private String _comments;
		public String Comments {
			get {
				return _comments;
			}
		}

		private String _creator;
		public String Creator {
			get {
				return _creator;
			}
		}

		private String _company;
		public String Company {
			get {
				return _company;
			}
		}

		private String _version;
		public String Version {
			get {
				return _version;
			}
		}

		private String _guid;
		public String Guid {
			get {
				return _guid;
			}
		}

		private DateTime _created;
		public DateTime Created {
			get {
				return _created;
			}
		}
		#endregion

		public MSIInstaller(string file) : this(new FileInfo(file))
		{

		}
		public MSIInstaller(FileInfo file)
		{
			this._file = file;
			try {
				using(QDatabase  msi = new QDatabase(file.FullName))
				{
					_company =msi.SummaryInfo.Author;
					_created = msi.SummaryInfo.CreateTime;
					_guid =msi.SummaryInfo.RevisionNumber;
					_comments =msi.SummaryInfo.Comments;
					_subject = msi.SummaryInfo.Subject;
					
					var props = msi.Properties.Select(xx => xx);
					properties = new List<Property_>(props);
					
					var p = properties.Where(xx => xx.Property.Equals("ProductVersion")).FirstOrDefault();
					if (p != null) {
						_version = p.Value;
					}
					p = properties.Where(xx => xx.Property == "Manufacturer").FirstOrDefault();
					if (p != null) {
						_creator = p.Value;
					}
					this._isInstaller = true;
				}
			} catch (InstallerException ex) {
				this._isInstaller = false;
				Console.WriteLine(ex.Message);
			}
		}



//		public void Dispose()
//		{
//			try {
//				if (this._msi != null) {
//					if (!this._msi.IsClosed) {
//						this._msi.Close();
//					}
//				}
//			} catch (Exception ex) {
//
//			} finally {
//				this.MSI.Dispose();
//			}
//		}
	}
}
