using System;
using System.Collections.Generic;
using RDotNet;
using System.Data.SqlClient;
using System.Configuration;

namespace RNetTest
{
	class ChartGenerator
	{
		//class is used for holding data from SQL
		public class DataHolder
		{
			public DataHolder()
			{
				year = new List<Int32>();
				userCount = new List<Int32>();
			}
			public List<Int32> year { get; set; }
			public List<Int32> userCount { get; set; }
		}
		public static DataHolder holder = new DataHolder();

		static void Main(string[] args)
		{
			//intializing
			setPath();
			loadData();
			using (REngine rengine = REngine.GetInstance())
			{
				rengine.Initialize();
				/*
				String[] colNames = { "year", "userCount" };
				IEnumerable[] col = new IEnumerable[2];
				col[0] = h.year.ToArray();
				col[1] = h.userCount.ToArray();
				*/
				//Importing libraries
				rengine.Evaluate("library(ggplot2)");
				rengine.Evaluate("library(ggthemes)");
				rengine.Evaluate("library(dplyr)");
				//Creating the dataframe
				rengine.Evaluate("df <- setNames(data.frame(matrix(ncol=2, nrow=0)), c(\"year\",\"userCount\"))").AsDataFrame();
				//Storing data into the dataframe
				for(int i = 0; i < holder.year.Count; i++)
				{
					rengine.Evaluate("df[nrow(df)+1,] <- c(" + holder.year[i] + ", " + holder.userCount[i] + ")");
				}
				//Creating the time series chart
				rengine.Evaluate("chart <- df %>% ggplot(aes(x=year, y=userCount)) + geom_smooth(size=2, aes(colour=\"red\", fill=\"black\"), show.legend=FALSE) + labs(x=\"Year\",y=\"User Count\") + theme_classic()");
				//Saving the graph to a png file
				string fileName = "C:/Users/{insert username here}/Desktop/plot0.png";
				CharacterVector fileVector = rengine.CreateCharacterVector(new[] { fileName });
				rengine.SetSymbol("fileName", fileVector);
				rengine.Evaluate("png(filename=fileName, width=6, height=6, units='in',res=100)");
				rengine.Evaluate("print(chart)");
				//Cleaning up
				rengine.Evaluate("dev.off()");
				rengine.Dispose();
			}
		}
		//Method for reading data from SQL into the class object data members
		protected static void loadData()
		{
			string connection = ConfigurationManager.ConnectionStrings["Practice"].ConnectionString;
			using(SqlConnection con = new SqlConnection(connection))
			{
				con.Open();
				string select = "SELECT Year, UserCount FROM PracticeTable";
				using(SqlCommand selectCommand = new SqlCommand(select, con))
				{
					using(SqlDataReader reader = selectCommand.ExecuteReader())
					{
						while (reader.Read())
						{
							holder.year.Add(((Int16)reader["Year"]));
							holder.userCount.Add(((Int32)reader["UserCount"]));
						}
					}
				}
				con.Close();
			}
		}
		//Sets the path to R since RDotNet needs to know where to forward the method calls
		protected static void setPath() //not my method, credit goes to agstudy on this one
		{
			//https://stackoverflow.com/questions/19725649/i-am-trying-to-make-rdotnet-work-with-c-and-i-am-encountering-problems
			Microsoft.Win32.RegistryKey registryKey = Microsoft.Win32.Registry.LocalMachine.OpenSubKey(@"SOFTWARE\R-core\R");
			string rPath = (string)registryKey.GetValue("InstallPath");
			string rVersion = (string)registryKey.GetValue("Current Version");
			registryKey.Dispose();
		}
	}
}
