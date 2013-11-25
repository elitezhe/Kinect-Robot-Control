<?xml version='1.0' encoding='UTF-8'?>
<Project Type="Project" LVVersion="13008000">
	<Item Name="我的电脑" Type="My Computer">
		<Property Name="server.app.propertiesEnabled" Type="Bool">true</Property>
		<Property Name="server.control.propertiesEnabled" Type="Bool">true</Property>
		<Property Name="server.tcp.enabled" Type="Bool">false</Property>
		<Property Name="server.tcp.port" Type="Int">0</Property>
		<Property Name="server.tcp.serviceName" Type="Str">我的电脑/VI服务器</Property>
		<Property Name="server.tcp.serviceName.default" Type="Str">我的电脑/VI服务器</Property>
		<Property Name="server.vi.callsEnabled" Type="Bool">true</Property>
		<Property Name="server.vi.propertiesEnabled" Type="Bool">true</Property>
		<Property Name="specify.custom.address" Type="Bool">false</Property>
		<Item Name="Load Robot VIs" Type="Folder">
			<Item Name="load-leg-zuohou.vi" Type="VI" URL="../load-leg-zuohou.vi"/>
			<Item Name="load-leg-zuoqian.vi" Type="VI" URL="../load-leg-zuoqian.vi"/>
		</Item>
		<Item Name="Test Demo" Type="Folder">
			<Item Name="test-wrl read.vi" Type="VI" URL="../test-wrl read.vi"/>
		</Item>
		<Item Name="Transform Sub VIs" Type="Folder"/>
		<Item Name="WRL Files" Type="Folder">
			<Property Name="NI.SortType" Type="Int">3</Property>
			<Item Name="原点位置解释.png" Type="Document" URL="../原点位置解释.png"/>
			<Item Name="躯体.wrl" Type="Document" URL="../躯体.wrl"/>
			<Item Name="右臂1.wrl" Type="Document" URL="../右臂1.wrl"/>
			<Item Name="右臂2.wrl" Type="Document" URL="../右臂2.wrl"/>
			<Item Name="右臂3.wrl" Type="Document" URL="../右臂3.wrl"/>
			<Item Name="右腿1.wrl" Type="Document" URL="../右腿1.wrl"/>
			<Item Name="右腿2.wrl" Type="Document" URL="../右腿2.wrl"/>
			<Item Name="右腿3.wrl" Type="Document" URL="../右腿3.wrl"/>
			<Item Name="左臂1.wrl" Type="Document" URL="../左臂1.wrl"/>
			<Item Name="左臂2.wrl" Type="Document" URL="../左臂2.wrl"/>
			<Item Name="左臂3.wrl" Type="Document" URL="../左臂3.wrl"/>
			<Item Name="左腿1.wrl" Type="Document" URL="../左腿1.wrl"/>
			<Item Name="左腿2.wrl" Type="Document" URL="../左腿2.wrl"/>
			<Item Name="左腿3.wrl" Type="Document" URL="../左腿3.wrl"/>
		</Item>
		<Item Name="4-leg-robot-vr.vi" Type="VI" URL="../4-leg-robot-vr.vi"/>
		<Item Name="依赖关系" Type="Dependencies">
			<Item Name="vi.lib" Type="Folder">
				<Item Name="LVBoundsTypeDef.ctl" Type="VI" URL="/&lt;vilib&gt;/Utility/miscctls.llb/LVBoundsTypeDef.ctl"/>
				<Item Name="LVSceneTextAlignment.ctl" Type="VI" URL="/&lt;vilib&gt;/Utility/miscctls.llb/LVSceneTextAlignment.ctl"/>
				<Item Name="NI_3D Picture Control.lvlib" Type="Library" URL="/&lt;vilib&gt;/picture/3D Picture Control/NI_3D Picture Control.lvlib"/>
			</Item>
		</Item>
		<Item Name="程序生成规范" Type="Build"/>
	</Item>
</Project>
