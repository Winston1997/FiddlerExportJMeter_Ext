using Fiddler;
using System;
using System.Net;
using System.Text;
namespace FiddlerExtensions
{

	public class HTTPSamplerProxy
	{
		private Session session;

        public string Xml
		{
			get
			{
				      
                //给线程组下的请求命名 格式为： id+“-”+ip和端口+path   例如：25-180.100.213.74/TPBidder/rest/memberLoginAction/page_load?isCommondto=true
                StringBuilder stringBuilder = new StringBuilder();
				string host = this.session.host;
				string url = this.Path;
				string oldValue = "://" + this.Port;
				host.Replace(oldValue, "");
				string arg = string.Concat(new object[]
				{
					this.session.id,
					"-",
                    this.session.oRequest.headers.UriScheme,
                    "://",
                    host,
					url.Substring(0, url.IndexOf("?"))
                });
				bool flag = this.session.oRequest.headers.Exists("Content-Type");
				bool flag2 = this.session.oFlags.ContainsKey("ui-comments");
				if (flag2)  //判断是否存在键“ui-comments”，给请求命名为 id+"-"+comment(fiddler中你给请求的备注)  例如：25-登录
				{
					string text2 = this.session.oFlags["ui-comments"].Trim();  
					if (!text2.Contains("[#") && text2 != null && !(text2 == ""))
					{
                        //arg = this.session.id + "-" + text2;//
                        arg = text2 + "-" + this.session.oRequest.headers.UriScheme + "://"+ this.session.host + url.Substring(0, url.IndexOf("?"));
                    }
                }

				//根据参数生成xml格式字段
				stringBuilder.Append(string.Format("<HTTPSamplerProxy guiclass=\"HttpTestSampleGui\" testclass=\"HTTPSamplerProxy\" testname=\"{0}\" enabled=\"true\">", arg));    //转取样器  名称，arg是获取到的 fiddler中每个请求你添加的Comment
				stringBuilder.Append("<boolProp name=\"HTTPSampler.postBodyRaw\">true</boolProp>");
				stringBuilder.Append("<elementProp name=\"HTTPsampler.Arguments\" elementType=\"Arguments\">");
				stringBuilder.Append("<collectionProp name=\"Arguments.arguments\">");
				stringBuilder.Append("<elementProp name=\"\" elementType=\"HTTPArgument\">");
				stringBuilder.Append("<boolProp name=\"HTTPArgument.always_encode\">false</boolProp>");

				if (flag)    //判断fiddler请求的headers里是否存在Content-Type
				{
					if (this.session.oRequest["Content-Type"].Contains("multipart/form-data; boundary"))     //判断请求的Content-Type中是否包含multipart/form-data; boundary 字段来判断是否是上传请求
					{
						stringBuilder.Append(string.Format("<stringProp name=\"Argument.value\">{0}</stringProp>", "错误：这是上传请求，请手动编辑取样器参数"));   //如果是上传请求，在body中指出报错
					}
					else
					{
						stringBuilder.Append(string.Format("<stringProp name=\"Argument.value\">{0}</stringProp>", this.RequestBody));   //如果不是上传请求，则写入fiddler请求的body部分
					}
				}
				else     
				{
					stringBuilder.Append(string.Format("<stringProp name=\"Argument.value\">{0}</stringProp>", this.RequestBody));   //如果没有Content-Type，则直接写入请求的body部分
				}

				//以下为转换请求为xml的过程
				stringBuilder.Append("<stringProp name=\"Argument.metadata\">=</stringProp>");
				stringBuilder.Append("</elementProp>");
				stringBuilder.Append("</collectionProp>");
				stringBuilder.Append("</elementProp>");
				stringBuilder.Append(string.Format("<stringProp name=\"HTTPSampler.domain\">{0}</stringProp>", this.session.hostname)); //转域名
				stringBuilder.Append(string.Format("<stringProp name=\"HTTPSampler.port\">{0}</stringProp>", this.Port));//转端口
                stringBuilder.Append("<stringProp name=\"HTTPSampler.connect_timeout\"></stringProp>");
                stringBuilder.Append("<stringProp name=\"HTTPSampler.response_timeout\"></stringProp>");
				stringBuilder.Append(string.Format("<stringProp name=\"HTTPSampler.protocol\">{0}</stringProp>", this.session.oRequest.headers.UriScheme));//转协议
                stringBuilder.Append("<stringProp name=\"HTTPSampler.contentEncoding\">utf-8</stringProp>");
				stringBuilder.Append(string.Format("<stringProp name=\"HTTPSampler.path\">{0}</stringProp>", this.Path));//转地址
                stringBuilder.Append(string.Format("<stringProp name=\"HTTPSampler.method\">{0}</stringProp>", this.session.oRequest.headers.HTTPMethod.ToUpper()));//转请求方式
                stringBuilder.Append("<boolProp name=\"HTTPSampler.follow_redirects\">true</boolProp>");
				stringBuilder.Append("<boolProp name=\"HTTPSampler.auto_redirects\">false</boolProp>");
				stringBuilder.Append("<boolProp name=\"HTTPSampler.use_keepalive\">true</boolProp>");
				stringBuilder.Append("<boolProp name=\"HTTPSampler.DO_MULTIPART_POST\">false</boolProp>");
				stringBuilder.Append("<boolProp name=\"HTTPSampler.monitor\">false</boolProp>");
				stringBuilder.Append("<stringProp name=\"HTTPSampler.embedded_url_re\"></stringProp>");
				string arg3 = this.session.oRequest.headers.UriScheme + "://" + this.session.host + this.Path;
                stringBuilder.Append(string.Format("<stringProp name=\"TestPlan.comments\">{0}</stringProp>",arg3));
                stringBuilder.Append("</HTTPSamplerProxy>");

				//以下为对线程组所进行的处理
				bool flag3 = true;
				if (this.MyReferer == null || this.MyReferer.Length < 1)
				{
					flag3 = false;
				}
				bool flag4 = this.session.oRequest.headers.Exists("X-Requested-With");
				bool flag5 = flag || flag3 || flag4;
				if (flag5)  //当请求中referer为空时或请求的headers包含字段“X-Requested-With”或者“Content-Type”时，分三种情况添加xml语句——判断该请求是否要为他添加HTTP信息头管理器
				{
                    //添加一个信息头管理器
                    stringBuilder.Append("" +
						"<hashTree>" +
						"<HeaderManager guiclass=\"HeaderPanel\" testclass=\"HeaderManager\" testname=\"HTTP信息头管理器\" enabled=\"true\">" +
						"<collectionProp name=\"HeaderManager.headers\">");   
					if (flag3)   //referer  不为空时
					{
						stringBuilder.Append(string.Format("" +
							"<elementProp name=\"\" elementType=\"Header\">" +
							"<stringProp name=\"Header.name\">Referer</stringProp>" +
							"<stringProp name=\"Header.value\">{0}</stringProp>" +
							"</elementProp>", this.MyReferer));	//转referer参数
					}
					if (flag4)  //当请求的headers包含字段“X-Requested-With”时
					{
						stringBuilder.Append(string.Format("" +
							"<elementProp name=\"\" elementType=\"Header\">" +
							"<stringProp name=\"Header.name\">X-Requested-With</stringProp>" +
							"<stringProp name=\"Header.value\">{0}</stringProp>" +
							"</elementProp>", this.session.oRequest["X-Requested-With"])); //转X-Requested-With参数
                    }
					if (flag)   //当请求的headers包含字段“Content-Type”时
					{
						stringBuilder.Append(string.Format("" +
							"<elementProp name=\"\" elementType=\"Header\">" +
							"<stringProp name=\"Header.name\">Content-Type</stringProp>" +
							"<stringProp name=\"Header.value\">{0}</stringProp>" +
							"</elementProp>", this.session.oRequest["Content-Type"]));  //转Content-Type参数
                    }
					stringBuilder.Append("</collectionProp>" +
					"</HeaderManager>");
				}
				stringBuilder.Append("<hashTree/>");
				if (flag5)  //当请求中referer为空时或请求的headers包含字段“X-Requested-With”或者“Content-Type”时
				{
					stringBuilder.Append("</hashTree>");  //添加结束语句
				}
				return stringBuilder.ToString();
			}
			//一个请求Xml转换完毕
		}

		private string Path		//获取地址
		{
			get
			{
				return WebUtility.HtmlEncode(this.session.PathAndQuery);
			}
		}
		private string Port		//获取端口
		{
			get
			{
				return this.getPort();
			}
		}
		private string RequestBody		//获取请求body
		{
			get
			{
				return WebUtility.HtmlEncode(this.session.GetRequestBodyAsString());
			}
		}
		private string MyReferer		//获取referer
		{
			get
			{
				return WebUtility.HtmlEncode(this.session.RequestHeaders["Referer"]);
			}
		}
		public HTTPSamplerProxy(Session session)
		{
			this.session = session;
		}
		private string getPort()   //获取端口
		{
			int port = this.session.port;    
			string uriScheme = this.session.oRequest.headers.UriScheme;
			if (uriScheme.ToLower() == "https" && port == 443)		//判断uri是HTTPS 还是HTTP，端口是443还是80
			{
				return "";
			}
			if (uriScheme.ToLower() == "http" && port == 80)
			{
				return "";
			}
			return port.ToString();
		}
	}
}
