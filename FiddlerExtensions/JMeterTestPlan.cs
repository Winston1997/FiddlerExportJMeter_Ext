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
				//����xml������Ϊjmx�Ŀ�ͷ����
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
				stringBuilder.Append("<jmeterTestPlan version=\"1.2\" properties=\"5.0\">");   //jmx�ļ�ͷ
				stringBuilder.Append(this.sessionList.Xml);     //����HTTPSamplerProxy��xmlд���ض���������
				stringBuilder.Append("</jmeterTestPlan>");		//jmx�ļ�β
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
