/*
 * Created by SharpDevelop.
 * User: ekr
 * Date: 08/12/2013
 * Time: 15:14
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using DXUnionPacket.DataModel;

namespace DXSampleImporter
{
	public class Importer
	{
		public static void Main(string[] args)
		{
			
			StructureMap.ObjectFactory.Initialize(  x  => {
			                                      	x.For<DXUnionPacket.DataModel.Database>().Singleton();
			                                      });
			Importer i = new Importer();
			i.ParseSamples2Dat();
		}
		
		private string sfile;
		public string sFile
		{
			get{
				if(file == null)
				{
					sfile = System.IO.Path.GetDirectoryName(this.GetType().Assembly.Location);
					sfile = System.IO.Path.Combine(sfile, "db");
					sfile = System.IO.Path.Combine(sfile, "samples2.dat");
				}
				return sfile;
			}
		}
		private FileInfo file;
		public FileInfo File
		{
			get{
				if(file == null)
				{
					file = new FileInfo(sFile);
				}
				return file;
			}
		}
		
		private List<Sample> samples;
		public List<Sample> Samples
		{
			get{
				if(samples == null)
				{
					this.samples = new List<Sample>();
					
				}
				return samples;
			}
		}
		
		public void ParseSamples2Dat()
		{
			byte[] bytes = new byte[0];
			using (FileStream fs = new FileStream(this.File.FullName, FileMode.Open, FileAccess.Read ))
			{
				using (BufferedStream bs = new BufferedStream(fs))
				{
					using(MemoryStream ms = new MemoryStream())
					{
						int bytesRead = default(int);
						byte[] buffer = new byte[512];
						while ((bytesRead = bs.Read(buffer, 0, buffer.Length)) > 0)
						{
							ms.Write(buffer, 0, bytesRead);
						}
						bytes = ms.ToArray();
					}
				}
			}
			int prev_null = 0;
			byte prev_byte = 0;
			byte[] new_line = new byte[]{ 13, 10 };
			List<byte> dyn_buff = new List<byte>();
			bool last_was_null = false;
			
			String f = String.Format("output_{0:yyyy-MM-ddThh}.vb", DateTime.Now) ;
			WriteToTemp(new byte[]{}, f, true);
			for(int i =0; i < bytes.Length ; i++)
			{
				byte b = bytes[i];
				
				if(b == 0)
				{
					if(prev_null < 1)
					{
						if(dyn_buff.Count > 0)
						{
							bool is_four_null = true;
							byte f_1 = b;
							byte f_2 = bytes[i + 1];
							byte f_3 = bytes[i + 2];
							byte f_4 = bytes[i + 3];
							if(f_1 == 0 && f_2 == 0 &&  f_3 == 0 &&  f_4 == 0)
							{
								
							}else if(f_1 == 0 && f_2 == 0  && f_3 != 0){
								if(dyn_buff.Count > 1)
								{
									dyn_buff.RemoveAt(dyn_buff.Count - 1);
									dyn_buff.RemoveAt(dyn_buff.Count - 1);
								}
							}else{
								dyn_buff.RemoveAt(dyn_buff.Count - 1);
							}
							WriteToTemp(dyn_buff.ToArray(), f );
							WriteToTemp(new byte[]{ 13, 10}, f );
							dyn_buff.Clear();
						}
					}
					prev_null ++;
				}else if(b == 255)
				{
					byte next_byte = bytes[i +3];
					if(next_byte == 39){
						i += 4;
						WriteToTemp(dyn_buff.ToArray(), f );
						dyn_buff.Clear();
						WriteToTemp(new_line, f );
						WriteToTemp(System.Text.ASCIIEncoding.ASCII.GetBytes("FF - 27"), f );
						WriteToTemp(new_line, f );
					}else{
						dyn_buff.Add(b);
					}
				}else if(b == 39)
				{
					byte next_byte = bytes[i + 1];
					if(last_was_null && next_byte == 32 ){
						int prev_byte_counter = 1;
						byte prev_byte_space = bytes[ i - prev_byte_counter ];
						while( prev_byte_space < 20 || prev_byte_space > 122){
							if(dyn_buff.Count > 0){
								dyn_buff.RemoveAt(dyn_buff.Count - 1);
							}
							prev_byte_counter ++;
							prev_byte_space = bytes[ i - prev_byte_counter];
						}
						WriteToTemp(dyn_buff.ToArray(), f );
						dyn_buff.Clear();
						WriteToTemp(new_line, f );
						WriteToTemp(System.Text.ASCIIEncoding.ASCII.GetBytes("AA - 27"), f );
						WriteToTemp(new_line, f );
						i += 1;
					}else{
						if(prev_null == 2)
						{
							last_was_null = true;
							WriteToTemp(new_line, f );
							WriteToTemp(System.Text.ASCIIEncoding.ASCII.GetBytes("2- "), f );
						}else if(prev_null == 3)
						{
							last_was_null = true;
							WriteToTemp(new_line, f );
							WriteToTemp(System.Text.ASCIIEncoding.ASCII.GetBytes("3- "), f );
						}else if(prev_null == 4)
						{
							last_was_null = true;
							WriteToTemp(new_line, f );
							WriteToTemp(System.Text.ASCIIEncoding.ASCII.GetBytes("4- "), f );
						}
						else{
							dyn_buff.Add(b);
							
							if(prev_byte == 13 && b == 10)
							{
								last_was_null = false;
								WriteToTemp(dyn_buff.ToArray(), f );
								dyn_buff.Clear();
							}
						}
						prev_null = 0;
					}
				}
				else
				{
					if(prev_null == 2)
					{
						last_was_null = true;
						WriteToTemp(new_line, f );
						WriteToTemp(System.Text.ASCIIEncoding.ASCII.GetBytes("2- "), f );
					}else if(prev_null == 3)
					{
						last_was_null = true;
						WriteToTemp(new_line, f );
						WriteToTemp(System.Text.ASCIIEncoding.ASCII.GetBytes("3- "), f );
					}else if(prev_null == 4)
					{
						last_was_null = true;
						WriteToTemp(new_line, f );
						WriteToTemp(System.Text.ASCIIEncoding.ASCII.GetBytes("4- "), f );
					}
					else{
						dyn_buff.Add(b);
						
						if(prev_byte == 13 && b == 10)
						{
							last_was_null = false;
							WriteToTemp(dyn_buff.ToArray(), f );
							dyn_buff.Clear();
						}
					}
					prev_null = 0;
				}
				prev_byte = b;
			}
			ParseTemp(f);
		}
		public void WriteToTemp(byte[] bytes, String filename, bool createNew = false)
		{
			if(createNew && System.IO.File.Exists(filename))
			{
				System.IO.File.Delete(filename);
			}
			using (FileStream fs = new FileStream(filename, FileMode.Append, FileAccess.Write ))
			{
				using (BufferedStream bs = new BufferedStream(fs))
				{
					foreach (byte b in bytes)
					{
						bs.WriteByte(b);
					}
				}
			}
		}
		

		public void ParseTemp(String filename)
		{
			LinkedList<string>  current_parent = new LinkedList<string>();
			int level = 0;
			Dictionary<string, List<string>> parents = new Dictionary<string, List<string>>();
			List<Sample> samples = new List<Sample>();
			Sample  s = null;
			bool new_level = false;
			using (FileStream fs = new FileStream(filename, FileMode.Open, FileAccess.Read ))
			{
				using (BufferedStream bs = new BufferedStream(fs))
				{
					using(StreamReader sr = new StreamReader(bs))
					{
						String line = sr.ReadLine();
						while(line != null){
							if(line.StartsWith("4-"))
							{
								if(s != null)
								{
									samples.Add(s);
								}
								s = new Sample();
								//Is Tree Parent

								line = sr.ReadLine();
								if(line.StartsWith("3-"))
								{
									line =  line.Replace("3- ", "");
									if(String.IsNullOrWhiteSpace(line))
									{
										//First of tree pivot
										line = sr.ReadLine();
										line =  line.Replace("3- ", "");
										s.Name = line;
										s.Text = line;
										s.Description = line;
										if(current_parent.Count > 0){
											//s.Parent = current_parent.First.Value;
											s.Parent = Parents( current_parent);
										}else{
											s.Parent = "";
										}
										samples.Add(s);
										s = null;
										current_parent.AddFirst(line);
									}else{
										current_parent.RemoveFirst();
										s.Name = line;
										s.Text = line;
										s.Description = line;
										//s.Parent = Parents( current_parent.First.Value;
										s.Parent = Parents( current_parent);
										samples.Add(s);
										s = null;
										current_parent.AddFirst(line);
									}
								}
							}
							else if(line.StartsWith("3-") )
							{
								if(s != null)
								{
									samples.Add(s);
								}
								s = new Sample();
								line =  line.Replace("3- ", "");
								if(String.IsNullOrWhiteSpace(line))
								{
									//First of tree pivot
									line = sr.ReadLine();
									while(!String.IsNullOrWhiteSpace(line))
									{
										if(line.StartsWith("3-"))
										{
											line = line.Replace("3-", "");
											break;
										}
										if(line.StartsWith("2-"))
										{
											line = line.Replace("2-", "");
											break;
										}
										line = sr.ReadLine();
									}
								}
								line = line.Trim();
								s.Name = line;
								s.Description = line;
								//s.Parent = current_parent.First.Value;
								s.Parent = Parents( current_parent);
								sr.ReadLine();
								sr.ReadLine();
								sr.ReadLine();
								sr.ReadLine();
							}
							else if(line.StartsWith("2-"))
							{
								if(s != null)
								{
									samples.Add(s);
								}
								s = new Sample();
								line =  line.Replace("2- ", "");
								if(String.IsNullOrWhiteSpace(line))
								{
									//First of tree pivot
									line = sr.ReadLine();
									while(!String.IsNullOrWhiteSpace(line))
									{
										if(line.StartsWith("3-"))
										{
											line = line.Replace("3-", "");
											break;
										}
										if(line.StartsWith("2-"))
										{
											line = line.Replace("2-", "");
											break;
										}
										line = sr.ReadLine();
									}
								}
								line = line.Trim();
								s.Name = line;
								s.Description = line;
								//s.Parent = current_parent.First.Value;
								s.Parent = Parents( current_parent);
								sr.ReadLine();
								sr.ReadLine();
								sr.ReadLine();
								sr.ReadLine();
								
							}else{
								if(s != null)
								{
									s.Text += line;
									s.Text += System.Environment.NewLine;
								}
							}
							line = sr.ReadLine();
						}
					}
				}
			}
			if(s != null)
			{
				samples.Add(s);
			}
			foreach(Sample ss in samples)
			{
				ss.Update(true);
			}
			parents.ToString();
		}
		private static string Parents(LinkedList<string> parents)
		{
			string r = "";
			foreach(string parent in parents.Reverse())
			{
				r += parent;
				r += ".";
			}
			r = r.TrimEnd(new char[]{'.'});
			return r;
		}
	}
}