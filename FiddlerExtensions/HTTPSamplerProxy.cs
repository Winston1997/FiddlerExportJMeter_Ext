using Fiddler;
using System;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using System.Security;
namespace FiddlerExtensions
{

	public class HTTPSamplerProxy
	{
		private Session session;

        public string Xml
		{
			get
			{
				StringBuilder stringBuilder = new StringBuilder();
				string HTTPMethod = this.session.oRequest.headers.HTTPMethod; //定义请求方式
                string host = this.session.host; //定义Host
                string hostname = this.session.hostname;//定义域名
				string Port = this.Port;//定义端口
                string id = this.session.id.ToString();//定义请求序号
                string UriScheme = this.session.oRequest.headers.UriScheme;//定义访问方式
                string bodydecoded = WebUtility.UrlDecode(this.RequestBody);
				if(bodydecoded.Contains("&")) 
				{
					bodydecoded = this.RequestBody;
				}//定义请求体
				string urlFullPath = this.Path;//定义全路径
				string url = this.Path;//定义接口地址
				string b64url = this.Path;
				string b64body = this.RequestBody;
				string pathdecode = "";
                string reqbodydecode = "";

				if (b64url.Contains("frameUrlSecretParam="))
				{
					string[] b64url0 = b64url.Split(new[] { "frameUrlSecretParam=" }, StringSplitOptions.None);
					string b64url_0 = b64url0[0];
					string b64url_1 = b64url0[1];
					string b64url_2 = WebUtility.UrlDecode(b64url_1);
					string b64url_3 = Encoding.UTF8.GetString(Convert.FromBase64String(b64url_2));
					pathdecode = b64url_3;
					urlFullPath = b64url_0 + "frameUrlSecretParam=${path_"+id+"}";
				}

				if (b64body.Contains("frameBodySecretParam="))
				{
					string[] b64body0 = b64body.Split(new[] { "SecretParam=" }, StringSplitOptions.None);
					string b64body_1 = b64body0[1];
					string b64body_2 = WebUtility.UrlDecode(b64body_1);
					string b64body_3 = WebUtility.UrlDecode(b64body_2);
					string b64body_4 = Encoding.UTF8.GetString(Convert.FromBase64String(b64body_3));
					string reqbodydecode_0 = WebUtility.UrlDecode(b64body_4);
					string reqbodydecode_1 = reqbodydecode_0.Replace("\\", "\\\\");
					string reqbodydecode_2 = reqbodydecode_1.Replace("\"", "\\\"");
                    reqbodydecode = SecurityElement.Escape(reqbodydecode_2);
                    bodydecoded = "frameBodySecretParam=${body_" + id + "}";
				}

				if (url.Contains("?"))  //判断地址中是否包含参数包含参数则去除
                {
					url = url.Substring(0, url.IndexOf("?"));
				}

				string requestName = id + "-" + UriScheme + "://" + host + url;
                string requestComment = UriScheme + "://" + host + this.Path; 
				                
				bool flag1 = this.session.oFlags.ContainsKey("ui-comments");
				if (flag1)  //判断是否已给请求命名
				{
					string text2 = this.session.oFlags["ui-comments"].Trim();  
					if (!text2.Contains("[#") && text2 != null && !(text2 == ""))
					{
                        requestName = id + "-" + text2 + "-" + UriScheme + "://"+ host + url;
                    }
                }

				//根据参数生成xml格式字段
				stringBuilder.Append(string.Format("<HTTPSamplerProxy guiclass=\"HttpTestSampleGui\" testclass=\"HTTPSamplerProxy\" testname=\"{0}\" enabled=\"true\">", requestName));    //转取样器名称
				stringBuilder.Append("<boolProp name=\"HTTPSampler.postBodyRaw\">true</boolProp>");
				stringBuilder.Append("<elementProp name=\"HTTPsampler.Arguments\" elementType=\"Arguments\">");
				stringBuilder.Append("<collectionProp name=\"Arguments.arguments\">");
				stringBuilder.Append("<elementProp name=\"\" elementType=\"HTTPArgument\">");
				stringBuilder.Append("<boolProp name=\"HTTPArgument.always_encode\">false</boolProp>");

                bool flag2 = this.session.oRequest.headers.Exists("Content-Type");
                if (flag2)    //判断fiddler请求的headers里是否存在Content-Type
				{
					if (this.session.oRequest["Content-Type"].Contains("multipart/form-data; boundary"))     //判断请求的Content-Type中是否包含multipart/form-data; boundary 字段来判断是否是上传请求
					{
						stringBuilder.Append(string.Format("<stringProp name=\"Argument.value\">{0}</stringProp>", "错误：上传请求无法进行转换，请手动编辑"));   //如果是上传请求，在body中指出报错
					}
					else
					{
                        stringBuilder.Append(string.Format("<stringProp name=\"Argument.value\">{0}</stringProp>", bodydecoded));   //如果不是上传请求，则写入fiddler请求的body部分
                    }
				}
				else     
				{
					stringBuilder.Append(string.Format("<stringProp name=\"Argument.value\">{0}</stringProp>", bodydecoded));   //如果没有Content-Type，则直接写入请求的body部分
				}

				//以下为转换请求为xml的过程
				stringBuilder.Append("<stringProp name=\"Argument.metadata\">=</stringProp>");
				stringBuilder.Append("</elementProp>");
				stringBuilder.Append("</collectionProp>");
				stringBuilder.Append("</elementProp>");
				stringBuilder.Append(string.Format("<stringProp name=\"HTTPSampler.domain\">{0}</stringProp>", hostname)); //转域名
				stringBuilder.Append(string.Format("<stringProp name=\"HTTPSampler.port\">{0}</stringProp>", Port));//转端口
                stringBuilder.Append("<stringProp name=\"HTTPSampler.connect_timeout\"></stringProp>");
                stringBuilder.Append("<stringProp name=\"HTTPSampler.response_timeout\"></stringProp>");
				stringBuilder.Append(string.Format("<stringProp name=\"HTTPSampler.protocol\">{0}</stringProp>", UriScheme));//转协议
                stringBuilder.Append("<stringProp name=\"HTTPSampler.contentEncoding\">utf-8</stringProp>");
				stringBuilder.Append(string.Format("<stringProp name=\"HTTPSampler.path\">{0}</stringProp>", urlFullPath));//转地址
                stringBuilder.Append(string.Format("<stringProp name=\"HTTPSampler.method\">{0}</stringProp>", HTTPMethod.ToUpper()));//转请求方式
                stringBuilder.Append("<boolProp name=\"HTTPSampler.follow_redirects\">true</boolProp>");
				stringBuilder.Append("<boolProp name=\"HTTPSampler.auto_redirects\">false</boolProp>");
				stringBuilder.Append("<boolProp name=\"HTTPSampler.use_keepalive\">true</boolProp>");
				stringBuilder.Append("<boolProp name=\"HTTPSampler.DO_MULTIPART_POST\">false</boolProp>");
				stringBuilder.Append("<boolProp name=\"HTTPSampler.monitor\">false</boolProp>");
				stringBuilder.Append("<stringProp name=\"HTTPSampler.embedded_url_re\"></stringProp>");
				stringBuilder.Append(string.Format("<stringProp name=\"TestPlan.comments\">{0}</stringProp>", requestComment));//转注释
                stringBuilder.Append("</HTTPSamplerProxy>");

				//以下为对线程组所进行的处理
				bool flag3 = true;//判断是否存在Referer
				if (this.MyReferer == null || this.MyReferer.Length < 1)
				{
					flag3 = false;
				}
				bool flag4 = this.session.oRequest.headers.Exists("X-Requested-With");//判断是否存在X-Request-With
                bool flag6 = this.session.oRequest.headers.Exists("Authorization");
                bool flag7 = this.session.oRequest.headers.Exists("clientid");
                bool flag5 = flag1 || flag3 || flag4 || flag6 || flag7;
                if (flag5)  //当请求中referer为空时或请求的headers包含字段“X-Requested-With”或者“Referer”时，分三种情况添加xml语句——判断该请求是否要为他添加HTTP信息头管理器
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
					if (flag1)   //当请求的headers包含字段“Content-Type”时
					{
						stringBuilder.Append(string.Format("" +
							"<elementProp name=\"\" elementType=\"Header\">" +
							"<stringProp name=\"Header.name\">Content-Type</stringProp>" +
							"<stringProp name=\"Header.value\">{0}</stringProp>" +
							"</elementProp>", this.session.oRequest["Content-Type"]));  //转Content-Type参数
                    }
                    if (flag6)   //当请求的headers包含字段“Authorization”时
                    {
                        stringBuilder.Append(string.Format("" +
                            "<elementProp name=\"\" elementType=\"Header\">" +
                            "<stringProp name=\"Header.name\">Authorization</stringProp>" +
                            "<stringProp name=\"Header.value\">{0}</stringProp>" +
                            "</elementProp>", this.session.oRequest["Authorization"]));  //转Authorization参数
                    }
                    if (flag7)   //当请求的headers包含字段“clientid”时
                    {
                        stringBuilder.Append(string.Format("" +
                            "<elementProp name=\"\" elementType=\"Header\">" +
                            "<stringProp name=\"Header.name\">clientid</stringProp>" +
                            "<stringProp name=\"Header.value\">{0}</stringProp>" +
                            "</elementProp>", this.session.oRequest["clientid"]));  //转clientid参数
                    }
                    stringBuilder.Append("</collectionProp>" +
					"</HeaderManager>");
				}
				stringBuilder.Append("<hashTree/>");

				if (b64url.Contains("frameUrlSecretParam=")) //判断是否b64
				{
					stringBuilder.Append(string.Format("          <JSR223PreProcessor guiclass=\"TestBeanGUI\" testclass=\"JSR223PreProcessor\" testname=\"JSR223 预处理程序\" enabled=\"true\">\r\n            <stringProp name=\"cacheKey\">true</stringProp>\r\n            <stringProp name=\"filename\"></stringProp>\r\n            <stringProp name=\"parameters\"></stringProp>\r\n            <stringProp name=\"script\">//path参数\r\nString decodedPath =&quot;{0}&quot;;\r\n\r\n//path参数编码\r\nString encodePath1 =Base64.getEncoder().encodeToString(decodedPath.getBytes());\r\nString encodePath2 =URLEncoder.encode(encodePath1, &quot;UTF-8&quot;);\r\nvars.put(&quot;path_{2}&quot;,encodePath2);\r\n\r\n\r\n//body参数\r\nString decodedBody =&quot;{1}&quot;;\r\n\r\n//body参数编码 \r\nString encodeBody1 = URLEncoder.encode(decodedBody, &quot;UTF-8&quot;);\r\nString encodeBody2 = Base64.getEncoder().encodeToString(encodeBody1.getBytes());\r\nString encodeBody3 = URLEncoder.encode(encodeBody2, &quot;UTF-8&quot;);\r\nvars.put(&quot;body_{2}&quot;,encodeBody3);</stringProp>\r\n            <stringProp name=\"scriptLanguage\">java</stringProp>\r\n          </JSR223PreProcessor>\r\n          <hashTree/>", pathdecode, reqbodydecode, id));
				}

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
