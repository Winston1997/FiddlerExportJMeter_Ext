using Fiddler;
using System;
using System.Text;
using System.Xml;
using System.Xml.Linq;
namespace FiddlerExtensions
{
	public class JMeterTestPlan
	{
		private SessionList sessionList;
		private Session[] sessions;
		public string Jmx
		{
			get
			{
				//创建xml，下面为jmx的开头部分
				StringBuilder stringBuilder = new StringBuilder();
				stringBuilder.Append("<?xml version=\"1.0\" encoding=\"UTF-8\"?>\n");
				XDocument xDocument = XDocument.Parse(this.Xml.ToString());
				//XmlDocument xDocument = new XmlDocument();
				//xDocument.LoadXml(stringBuilder.ToString());
				stringBuilder.Append(xDocument.ToString());
				return stringBuilder.ToString();
			}
		}
		private string Xml
		{
			get
			{
				StringBuilder stringBuilder = new StringBuilder();
				stringBuilder.Append("<jmeterTestPlan version=\"1.2\" properties=\"5.0\">");   //jmx文件头
				stringBuilder.Append(this.sessionList.Xml);     //调用HTTPSamplerProxy向xml写入特定规则的组件
				stringBuilder.Append("</jmeterTestPlan>");		//jmx文件尾
				return stringBuilder.ToString();
			}
		}
		public JMeterTestPlan()
		{
			this.sessions = new Session[0];
			this.sessionList = new SessionList(this.sessions);
		}
		public JMeterTestPlan(Session[] oSessions, string outputFilename)
		{
			this.sessions = oSessions;
			this.sessionList = new SessionList(oSessions);
		}
	}
}
