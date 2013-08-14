/*
 * Created by SharpDevelop.
 * User: ekr
 * Date: 08/10/2013
 * Time: 13:01
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Xaml;

using SQLite;

namespace DXUnionPacket.DataModel
{
	/// <summary>
	///  A data access layer for code Samples.
	/// </summary>
	public class Sample
	{
		private DXUnionPacket.DataModel.Database _db
		{
			get{
				return StructureMap.ObjectFactory.GetInstance<DXUnionPacket.DataModel.Database>();
			}
		}
		
		[PrimaryKey]
		public Guid Id {get;set;}
		
		public String Text{get;set;}
		[ Unique(Name="uk_sample_name")]
		public String Name{get;set;}
		public String Description{get;set;}
		[Indexed]
		[ Unique(Name="uk_sample_name")]
		public String Parent{get;set;}
		public DateTime DateCreated{get;set;}
		public DateTime DateModified{get;set;}
		
		public Sample()
		{
			this.Id = Guid.NewGuid();
			this.DateCreated = DateTime.Now;
		}
		public Sample GetParent()
		{
			return _db.QueryParent(this);
		}
		public IEnumerable<Sample> GetChildren()
		{
			return _db.QueryChildren(this);
		}
		
		public bool Update(bool tryInsert = true)
		{
			this.DateModified = DateTime.Now;
			int r = _db.Update(this);
			if(r > 0){
				return true;
			}else if(tryInsert){
				return this.Insert();
			}else{
				return false;
			}
			
		}
		public bool Insert()
		{
			this.DateModified = DateTime.Now;
			int r = _db.Insert(this);
			if(r > 0)
			{
				return true;
			}else{
				return false;
			}
		}
		public bool Delete()
		{
			int r = _db.Delete(this);
			if(r > 0)
			{
				return true;
			}else{
				return false;
			}
		}
		
		public int AddChildren(IEnumerable<Sample> children)
		{
			int r = 0;
			foreach(Sample s in children)
			{
				if(this.AddChild(s))
				{
					r++;
				}
			}
			return r;
		}
		public bool AddChild(Sample child)
		{
			child.Parent = this.Parent + "." +  this.Name;
			return child.Update();
		}
		
		public int RemoveChildren(IEnumerable<Sample> children)
		{
			int r = 0;
			foreach(Sample s in children)
			{
				if(this.RemoveChild(s))
				{
					r++;
				}
			}
			return r;
		}
		public bool RemoveChild(Sample child)
		{
			child.Parent = "";
			return child.Update();
		}
		
		public override string ToString ()
		{
			return string.Format ("{0}", Name);
		}
	}
	public class Database : SQLiteConnection
	{
		public Database () : base(System.IO.Path.Combine(System.IO.Path.GetDirectoryName(typeof(Database).Assembly.Location),@"db\samples.db"))
		{
			CreateTable<Sample> ();
			
		}
		public  IEnumerable<Sample>  QuerySamples ( string name  = "", string description = "", string text = "", string parent = "")
		{
			return this.QuerySamples(name, description, text, parent, DateTime.MinValue, DateTime.Now, DateTime.MinValue, DateTime.Now);
		}
		public  IEnumerable<Sample>  QuerySamples ( string name  = "", string description = "", string text = "", string parent = "",  DateTime? createdAfter = null, DateTime?  createdBefore = null, DateTime?  modifiedAfter = null, DateTime?  modifiedBefore = null)
		{
			if (createdAfter == null)
				createdAfter = DateTime.MinValue;
			if(createdBefore == null)
				createdBefore = DateTime.Now;
			if( modifiedAfter == null)
				modifiedAfter = DateTime.MinValue;
			if(modifiedBefore == null)
				modifiedBefore = DateTime.Now;
			try{
				var rr = Table<Sample>().Where<Sample>( xxx => xxx.Name !=null);
				foreach(Sample ss in rr)
				{
					ss.ToString();
					try{
						if(ss.Name.ToLower().Contains("".ToLower()))
						{
							ss.ToString();
						}
					}catch(Exception ex){
						ex.StackTrace.ToLower();
					}
				}
				var r =  Table<Sample>().Where<Sample>(xx =>
				                                       xx.Name != null && 
				                                       (
				                                       	name != null &&  xx.Name != null && xx.Name.ToLower().Contains(name.ToLower())
				                                       	||
				                                       	String.IsNullOrEmpty(name) && xx.Name == null
				                                       )  && (
				                                       	description != null  &&  xx.Description != null  &&  xx.Description.ToLower().Contains(description.ToLower())
				                                       	||
				                                       	String.IsNullOrEmpty(description) && xx.Description == null
				                                       )  && (
				                                       	text != null  && xx.Text != null &&  xx.Text.ToLower().Contains(text.ToLower())
				                                       	||
				                                       	String.IsNullOrEmpty(text)  && xx.Text == null
				                                       )&& (
				                                       	parent != null  && xx.Parent != null &&  xx.Parent.ToLower().Contains(parent.ToLower())
				                                       	||
				                                       	String.IsNullOrEmpty(parent)  && xx.Parent == null
				                                       )
				                                       && (
				                                       	xx.DateCreated.Ticks >= createdAfter.Value.Ticks && xx.DateCreated.Ticks <= createdBefore.Value.Ticks
				                                       )&& (
				                                       	xx.DateModified.Ticks >= modifiedAfter.Value.Ticks && xx.DateModified.Ticks <= modifiedBefore.Value.Ticks
				                                       )
				                                      );
				
				return r.ToList<Sample>();
			}catch(Exception ex)
			{
				ex.Message.ToLower();
				ex.StackTrace.ToString();
			}
			return null;
		}
		
		public Sample QueryParent(Sample parent){
			return this.QueryParent(parent.Parent);
		}
		public Sample QueryParent(String parent){
			var t = this.Table<Sample>();
			if (t != null) {
				var ss = t.Where<Sample>(  s =>  s.Parent.Equals(parent)   );
				if(ss != null && ss.Count() > 0){
					return ss.FirstOrDefault();
				}else{
					return null;
				}
			}else{
				return null;
			}
		}
		
		
		public IEnumerable<Sample> QueryChildren(Sample parent){
			return QueryChildren(parent.Parent);
		}
		public IEnumerable<Sample> QueryChildren(String parent){
			var t = this.Table<Sample>();
			if (t != null) {
				var ss = t.Where<Sample>(  s =>  s.Name.Equals(parent)   );
				return ss;
			}else{
				return null;
			}
		}
		public Sample QuerySample(String name){
			var t = this.Table<Sample>();
			if (t != null) {
				var ss = t.Where<Sample>(  s =>  s.Name.Equals(name.ToString())       );
				if(ss != null && ss.Count() > 0){
					return ss.FirstOrDefault();
				}else{
					return null;
				}
			}else{
				return null;
			}
		}
		public Sample QuerySample(Guid id){
			var t = this.Table<Sample>();
			if (t != null) {
				var ss = t.Where<Sample>(  s =>  s.Id == id || s.Id.ToString().Equals(id.ToString())       );
				if(ss != null && ss.Count() > 0){
					return ss.FirstOrDefault();
				}else{
					return null;
				}
			}else{
				return null;
			}
		}
	}
}
