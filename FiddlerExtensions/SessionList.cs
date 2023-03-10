using Fiddler;
using System;
using System.Text;
using System.Xml.Linq;

namespace FiddlerExtensions
{
	public class SessionList
	{
		private Session[] sessions;
		public string Xml
		{
			get
			{
				//创建列表

				StringBuilder stringBuilder = new StringBuilder();
				if (this.sessions.Length > 0)
				{
                    //创建测试计划组件
                    stringBuilder.Append("" +
						"<hashTree>" +
						"<TestPlan guiclass=\"TestPlanGui\" testclass=\"TestPlan\" testname=\"测试计划\" enabled=\"true\">" +
						"<stringProp name=\"TestPlan.comments\"></stringProp>" +
						"<boolProp name=\"TestPlan.functional_mode\">false</boolProp>" +
						"<boolProp name=\"TestPlan.serialize_threadgroups\">true</boolProp>" +
						"<elementProp name=\"TestPlan.user_defined_variables\" elementType=\"Arguments\" guiclass=\"ArgumentsPanel\" testclass=\"Arguments\" testname=\"用户定义的变量\" enabled=\"true\">" +
						"<collectionProp name=\"Arguments.arguments\"/>" +
						"</elementProp>" +
						"<stringProp name=\"TestPlan.user_define_classpath\"></stringProp>" +
						"</TestPlan>");
					//创建请求默认值组件
					stringBuilder.Append("" +
						"<hashTree>" +
						"<ConfigTestElement guiclass =\"HttpDefaultsGui\" testclass=\"ConfigTestElement\" testname=\"HTTP请求默认值(已配置127.0.0.1:8888)\" enabled=\"false\">" +
						"<elementProp name =\"HTTPsampler.Arguments\" elementType=\"Arguments\" guiclass=\"HTTPArgumentsPanel\" testclass=\"Arguments\" testname=\"用户定义的变量\" enabled=\"true\">" +
						"<collectionProp name =\"Arguments.arguments\"/>" +
						"</elementProp>" +
						"<stringProp name =\"HTTPSampler.domain\"></stringProp>" +
						"<stringProp name =\"HTTPSampler.port\"></stringProp>" +
						"<stringProp name =\"HTTPSampler.protocol\"></stringProp>" +
						"<stringProp name =\"HTTPSampler.contentEncoding\"></stringProp>" +
						"<stringProp name =\"HTTPSampler.path\"></stringProp>" +
						"<stringProp name =\"HTTPSampler.concurrentPool\">6</stringProp>" +
						"<stringProp name =\"HTTPSampler.proxyHost\">127.0.0.1</stringProp>" +
						"<stringProp name =\"HTTPSampler.proxyPort\">8888</stringProp>" +
						"<stringProp name =\"HTTPSampler.connect_timeout\"></stringProp>" +
						"<stringProp name =\"HTTPSampler.response_timeout\"></stringProp>" +
						"</ConfigTestElement>");
                    //创建HTTP缓存管理器
                    stringBuilder.Append("" +
                        "<hashTree/>" +
						"<CacheManager guiclass=\"CacheManagerGui\" testclass=\"CacheManager\" testname=\"HTTP缓存管理器\" enabled=\"true\">" +
						"<boolProp name=\"clearEachIteration\">false</boolProp>" +
						"<boolProp name=\"useExpires\">true</boolProp>" +
						"<boolProp name=\"CacheManager.controlledByThread\">false</boolProp>" +
						"</CacheManager>" +
						"<hashTree/>");
                    //创建HTTP Cookie 管理器
                    stringBuilder.Append("" +
						"<CookieManager guiclass=\"CookiePanel\" testclass=\"CookieManager\" testname=\"HTTP Cookie 管理器\" enabled=\"true\">" +
						"<collectionProp name=\"CookieManager.cookies\"/>" +
						"<boolProp name=\"CookieManager.clearEachIteration\">false</boolProp>" +
						"<stringProp name=\"CookieManager.policy\">standard</stringProp>" +
						"<stringProp name=\"CookieManager.implementation\">org.apache.jmeter.protocol.http.control.HC4CookieHandler</stringProp>" +
						"</CookieManager>" +
						"<hashTree/>");
                    //创建用户定义的变量“InfluxDB配置参数”
                    stringBuilder.Append("" +
						"<Arguments guiclass=\"ArgumentsPanel\" testclass=\"Arguments\" testname=\"InfluxDB配置参数\" enabled=\"true\">" +
						"<collectionProp name=\"Arguments.arguments\">" +
						"<elementProp name=\"InfluxDB_Host\" elementType=\"Argument\">" +
						"<stringProp name=\"Argument.name\">InfluxDB_Host</stringProp>" +
						"<stringProp name=\"Argument.value\">192.168.200.219</stringProp>" +
						"<stringProp name=\"Argument.metadata\">=</stringProp>" +
                        "<stringProp name=\"Argument.desc\">【后端监听器参数】默认值为192.168.200.219（项目组远程压测机本地部署为127.0.0.1）</stringProp>" +
						"</elementProp>" +
						"<elementProp name=\"DataBase\" elementType=\"Argument\">" +
						"<stringProp name=\"Argument.name\">DataBase</stringProp>" +
						"<stringProp name=\"Argument.value\">jmeter</stringProp>" +
						"<stringProp name=\"Argument.metadata\">=</stringProp>" +
                        "<stringProp name=\"Argument.desc\">【后端监听器参数】修改为你的数据库库名（项目组远程压测机本地部署为jmeter）</stringProp>" +
						"</elementProp>" +
						"<elementProp name=\"ProjectName\" elementType=\"Argument\">" +
						"<stringProp name=\"Argument.name\">ProjectName</stringProp>" +
						"<stringProp name=\"Argument.value\">项目名称</stringProp>" +
						"<stringProp name=\"Argument.metadata\">=</stringProp>" +
						"<stringProp name=\"Argument.desc\">【后端监听器参数】修改为项目名称</stringProp>" +
						"</elementProp>" +
						"<elementProp name=\"PlanName\" elementType=\"Argument\">" +
						"<stringProp name=\"Argument.name\">PlanName</stringProp>" +
						"<stringProp name=\"Argument.value\">计划名称</stringProp>" +
						"<stringProp name=\"Argument.desc\">【后端监听器参数】修改为计划名称</stringProp>" +
						"<stringProp name=\"Argument.metadata\">=</stringProp>" +
						"</elementProp>" +
						"</collectionProp>" +
						"</Arguments>" +
						"<hashTree/>");
                    //创建线程组
                    stringBuilder.Append("" +
						"<ThreadGroup guiclass=\"ThreadGroupGui\" testclass=\"ThreadGroup\" testname=\"线程组\" enabled=\"true\">" +
						"<stringProp name=\"ThreadGroup.on_sample_error\">continue</stringProp>" +
						"<elementProp name=\"ThreadGroup.main_controller\" elementType=\"LoopController\" guiclass=\"LoopControlPanel\" testclass=\"LoopController\" testname=\"循环控制器\" enabled=\"true\">" +
						"<boolProp name=\"LoopController.continue_forever\">false</boolProp>" +
						"<stringProp name=\"LoopController.loops\">-1</stringProp>" +
						"</elementProp>" +
						"<stringProp name=\"ThreadGroup.num_threads\">1</stringProp>" +
						"<stringProp name=\"ThreadGroup.ramp_time\">1</stringProp>" +
						"<longProp name=\"ThreadGroup.start_time\">1464706407000</longProp>" +
						"<longProp name=\"ThreadGroup.end_time\">1464706407000</longProp>" +
						"<boolProp name=\"ThreadGroup.scheduler\">true</boolProp>" +
						"<stringProp name=\"ThreadGroup.duration\">600</stringProp>" +
						"<stringProp name=\"ThreadGroup.delay\"></stringProp>" +
						"<boolProp name=\"ThreadGroup.same_user_on_next_iteration\">true</boolProp>" +
                        "</ThreadGroup>" +
						"<hashTree>");
					
					//写入fiddler抓包数据
                    Session[] array = this.sessions;
                    for (int i = 0; i < array.Length; i++)
					{
						Session session = array[i];
						HTTPSamplerProxy hTTPSamplerProxy = new HTTPSamplerProxy(session);
						stringBuilder.Append(hTTPSamplerProxy.Xml);
					}
					

					//添加查看结果树组件
					stringBuilder.Append("" +
						"</hashTree>" +
						"<ResultCollector guiclass=\"ViewResultsFullVisualizer\" testclass=\"ResultCollector\" testname=\"察看结果树\" enabled=\"true\">" +
						"<boolProp name=\"ResultCollector.error_logging\">false</boolProp>" +
						"<objProp>" +
						"<name>saveConfig</name>" +
						"<value class=\"SampleSaveConfiguration\">" +
						"<time>true</time>" +
						"<latency>true</latency>" +
						"<timestamp>true</timestamp>" +
						"<success>true</success>" +
						"<label>true</label>" +
						"<code>true</code>" +
						"<message>true</message>" +
						"<threadName>true</threadName>" +
						"<dataType>true</dataType>" +
						"<encoding>false</encoding>" +
						"<assertions>true</assertions>" +
						"<subresults>true</subresults>" +
						"<responseData>false</responseData>" +
						"<samplerData>false</samplerData>" +
						"<xml>false</xml>" +
						"<fieldNames>false</fieldNames>" +
						"<responseHeaders>false</responseHeaders>" +
						"<requestHeaders>false</requestHeaders>" +
						"<responseDataOnError>false</responseDataOnError>" +
						"<saveAssertionResultsFailureMessage>false</saveAssertionResultsFailureMessage>" +
						"<assertionsResultsToSave>0</assertionsResultsToSave>" +
						"<bytes>true</bytes>" +
						"<threadCounts>true</threadCounts>" +
						"</value>" +
						"</objProp>" +
						"<stringProp name=\"filename\"></stringProp>" +
						"</ResultCollector>" +
						"<hashTree/>" +
						"<BackendListener guiclass=\"BackendListenerGui\" testclass=\"BackendListener\" testname=\"后端监听器\" enabled=\"true\">" +
						"<elementProp name=\"arguments\" elementType=\"Arguments\" guiclass=\"ArgumentsPanel\" testclass=\"Arguments\" enabled=\"true\">" +
						"<collectionProp name=\"Arguments.arguments\">" +
						"<elementProp name=\"influxdbMetricsSender\" elementType=\"Argument\">" +
						"<stringProp name=\"Argument.name\">influxdbMetricsSender</stringProp>" +
						"<stringProp name=\"Argument.value\">org.apache.jmeter.visualizers.backend.influxdb.HttpMetricsSender</stringProp>" +
						"<stringProp name=\"Argument.metadata\">=</stringProp>" +
						"</elementProp>" +
						"<elementProp name=\"influxdbUrl\" elementType=\"Argument\">" +
						"<stringProp name=\"Argument.name\">influxdbUrl</stringProp>" +
						"<stringProp name=\"Argument.value\">http://${InfluxDB_Host}:8086/write?db=${DataBase}</stringProp>" +
						"<stringProp name=\"Argument.metadata\">=</stringProp>" +
						"</elementProp>" +
						"<elementProp name=\"application\" elementType=\"Argument\">" +
						"<stringProp name=\"Argument.name\">application</stringProp>" +
						"<stringProp name=\"Argument.value\">${PlanName}</stringProp>" +
						"<stringProp name=\"Argument.metadata\">=</stringProp>" +
						"</elementProp>" +
						"<elementProp name=\"measurement\" elementType=\"Argument\">" +
						"<stringProp name=\"Argument.name\">measurement</stringProp>" +
						"<stringProp name=\"Argument.value\">jmeter</stringProp>" +
						"<stringProp name=\"Argument.metadata\">=</stringProp>" +
						"</elementProp>" +
						"<elementProp name=\"summaryOnly\" elementType=\"Argument\">" +
						"<stringProp name=\"Argument.name\">summaryOnly</stringProp>" +
						"<stringProp name=\"Argument.value\">false</stringProp>" +
						"<stringProp name=\"Argument.metadata\">=</stringProp>" +
						"</elementProp>" +
						"<elementProp name=\"samplersRegex\" elementType=\"Argument\">" +
						"<stringProp name=\"Argument.name\">samplersRegex</stringProp>" +
						"<stringProp name=\"Argument.value\">.*</stringProp>" +
						"<stringProp name=\"Argument.metadata\">=</stringProp>" +
						"</elementProp>" +
						"<elementProp name=\"percentiles\" elementType=\"Argument\">" +
						"<stringProp name=\"Argument.name\">percentiles</stringProp>" +
						"<stringProp name=\"Argument.value\">90;95;99</stringProp>" +
						"<stringProp name=\"Argument.metadata\">=</stringProp> " +
						"</elementProp>" +
						"<elementProp name=\"testTitle\" elementType=\"Argument\">" +
						"<stringProp name=\"Argument.name\">testTitle</stringProp>" +
						"<stringProp name=\"Argument.value\">${ProjectName}</stringProp>" +
						"<stringProp name=\"Argument.metadata\">=</stringProp>" +
						"</elementProp>" +
						"<elementProp name=\"eventTags\" elementType=\"Argument\">" +
						"<stringProp name=\"Argument.name\">eventTags</stringProp>" +
						"<stringProp name=\"Argument.value\"></stringProp>" +
						"<stringProp name=\"Argument.metadata\">=</stringProp>" +
						"</elementProp>" +
						"</collectionProp>" +
						"</elementProp>" +
						"<stringProp name=\"classname\">org.apache.jmeter.visualizers.backend.influxdb.InfluxdbBackendListenerClient</stringProp>" +
						"</BackendListener>" +
                        "<hashTree/>" +
						"</hashTree>" +
						"</hashTree> ");
				}
				return stringBuilder.ToString();
			}
		}
		public SessionList()
		{
			this.sessions = new Session[0];
		}
		public SessionList(Session[] oSessions)
		{
			this.sessions = oSessions;
		}
	}
}
