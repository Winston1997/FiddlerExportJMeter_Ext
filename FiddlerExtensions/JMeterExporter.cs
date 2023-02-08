using Fiddler;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
namespace FiddlerExtensions
{
	[ProfferFormat("JMeter_Epoint性能", "JMeter .jmx Format")]
	public class JMeterExporter : ISessionExporter, IDisposable
	{
		//fiddler配置信息导出模块
		public bool ExportSessions(string sFormat, Session[] oSessions, Dictionary<string, object> dictOptions, EventHandler<ProgressCallbackEventArgs> evtProgressNotifications)
		{
			bool result = true;
			string text = Utilities.ObtainSaveFilename("Export As " + sFormat, "JMeter Files (*.jmx)|*.jmx");
			if (string.IsNullOrEmpty(text))
			{
				return false;
			}
			if (!Path.HasExtension(text))
			{
				text += ".jmx";
			}
			try
			{
				Encoding encoding = new UTF8Encoding(true);
				JMeterTestPlan jMeterTestPlan = new JMeterTestPlan(oSessions, text);
				StreamWriter streamWriter = new StreamWriter(text, false, encoding);
				streamWriter.Write(jMeterTestPlan.Jmx);
				streamWriter.Close();
				FiddlerApplication.Log.LogString("成功导出jmx格式，信息如下：");
				FiddlerApplication.Log.LogString("Successfully exported sessions to JMeter Test Plan");
				FiddlerApplication.Log.LogString(string.Format("\t{0}", text));
			}
			catch (Exception ex)
			{
				FiddlerApplication.Log.LogString("出现异常，信息如下：");
				FiddlerApplication.Log.LogString(ex.Message);
				FiddlerApplication.Log.LogString(ex.StackTrace);
				result = false;
			}
			return result;
		}
		public void Dispose()
		{
		}
	}
}
