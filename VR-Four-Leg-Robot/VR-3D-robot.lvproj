<?xml version='1.0' encoding='UTF-8'?>
<Project Type="Project" LVVersion="13008000">
	<Item Name="我的电脑" Type="My Computer">
		<Property Name="NI.SortType" Type="Int">3</Property>
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
			<Property Name="NI.SortType" Type="Int">3</Property>
			<Item Name="load-robot-vr.vi" Type="VI" URL="../load-robot-vr.vi"/>
			<Item Name="load-leg-zuoqian.vi" Type="VI" URL="../load-leg-zuoqian.vi"/>
			<Item Name="load-leg-youqian.vi" Type="VI" URL="../load-leg-youqian.vi"/>
			<Item Name="load-leg-zuohou.vi" Type="VI" URL="../load-leg-zuohou.vi"/>
			<Item Name="load-leg-youhou.vi" Type="VI" URL="../load-leg-youhou.vi"/>
		</Item>
		<Item Name="Sub VIs" Type="Folder">
			<Item Name="vr-states.ctl" Type="VI" URL="../vr-states.ctl"/>
			<Item Name="global-refs.vi" Type="VI" URL="../global-refs.vi"/>
			<Item Name="process-percent.vi" Type="VI" URL="../process-percent.vi"/>
			<Item Name="init-vr-scene-window.vi" Type="VI" URL="../init-vr-scene-window.vi"/>
		</Item>
		<Item Name="Test Demo" Type="Folder">
			<Item Name="test-main.vi" Type="VI" URL="../test-main.vi"/>
			<Item Name="test-scene-window.vi" Type="VI" URL="../test-scene-window.vi"/>
			<Item Name="test-wrl read.vi" Type="VI" URL="../test-wrl read.vi"/>
			<Item Name="test-nisastevent.vi" Type="VI" URL="../test-nisastevent.vi"/>
			<Item Name="vr-main-ctl.vi" Type="VI" URL="../vr-main-ctl.vi"/>
		</Item>
		<Item Name="Transform Sub VIs" Type="Folder">
			<Item Name="rotate-body.vi" Type="VI" URL="../rotate-body.vi"/>
			<Item Name="rotate-part1.vi" Type="VI" URL="../rotate-part1.vi"/>
			<Item Name="rotate-part2.vi" Type="VI" URL="../rotate-part2.vi"/>
			<Item Name="rotate-part3.vi" Type="VI" URL="../rotate-part3.vi"/>
			<Item Name="set-angle-to-z-axis-cord.vi" Type="VI" URL="../set-angle-to-z-axis-cord.vi"/>
			<Item Name="global-body-transv.vi" Type="VI" URL="../global-body-transv.vi"/>
		</Item>
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
		<Item Name="vr-main-window.vi" Type="VI" URL="../vr-main-window.vi"/>
		<Item Name="NISAST Event.lvclass" Type="LVClass" URL="/&lt;resource&gt;/importtools/Common/Event/NISAST Event.lvclass"/>
		<Item Name="依赖关系" Type="Dependencies">
			<Item Name="vi.lib" Type="Folder">
				<Item Name="LVBoundsTypeDef.ctl" Type="VI" URL="/&lt;vilib&gt;/Utility/miscctls.llb/LVBoundsTypeDef.ctl"/>
				<Item Name="LVRectTypeDef.ctl" Type="VI" URL="/&lt;vilib&gt;/Utility/miscctls.llb/LVRectTypeDef.ctl"/>
				<Item Name="LVSceneTextAlignment.ctl" Type="VI" URL="/&lt;vilib&gt;/Utility/miscctls.llb/LVSceneTextAlignment.ctl"/>
				<Item Name="NI_3D Picture Control.lvlib" Type="Library" URL="/&lt;vilib&gt;/picture/3D Picture Control/NI_3D Picture Control.lvlib"/>
			</Item>
		</Item>
		<Item Name="程序生成规范" Type="Build"/>
	</Item>
</Project>
