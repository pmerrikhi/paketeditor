/*
 * Created by SharpDevelop.
 * User: ekr
 * Date: 04/09/2013
 * Time: 20:14
 * 
 */
using System;
using System.Collections.Generic;
using System.IO;

using Simple.Settings;

namespace DXUnionPacket
{
	/// <summary>
	/// Description of Settings.
	/// </summary>
	public class DXSettings: Simple.Settings.SimpleSettingsReader, Simple.Settings.ISimpleSettingsClient
	{
		
		public static readonly string SETTING_MSI_ROOT_DIR = "installer.dir";
		public DXSettings():base()
		{
			
			SetDefaults();
			foreach(string default_setting in this.Defaults.Keys)
			{
				if(!base.Settings.ContainsKey(default_setting))
				{
					base.Settings.Add(default_setting, this.Defaults[default_setting]);
				}
			}
		}
		
		public Simple.Settings.SimpleSettingsReader SettingsReader{
			get{
				return this;
			}
		}
		private IDictionary<string,string> _defaults;
		public IDictionary<String, String> Defaults {
			get{
				if(_defaults == null)
				{
					_defaults = new Dictionary<string,string>();
				}
				return _defaults;
			}
		}
		public void SetDefaults()
		{
			this.Defaults.Add(SETTING_MSI_ROOT_DIR,  System.Environment.GetFolderPath(System.Environment.SpecialFolder.UserProfile));
		}
		public bool PrintDefaultIni(String out_path)
		{
			bool r = false;
			try{
				using(FileStream fs = new FileStream(out_path, FileMode.OpenOrCreate))
				{
					
				}
				SimpleSettingsReader sr = new SimpleSettingsReader();
				sr.SettingsINI = out_path;
				sr.AddSettings(this.Defaults);
				r = sr.SaveSettings();
				sr.Settings.Clear();
				
				sr = null;
			}catch(Exception ex)
			{
				
			}
			return r;
		}
	}
}
