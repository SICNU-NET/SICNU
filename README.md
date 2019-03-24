# SICNU
MapSuite9SamplesUpgrade
# Map Suite 9 Samples Upgrade

**第一阶段 —— 自动编译工具**

1. 项目背景

  MapSuite已更新为10.0，但目前很多sample还是基于MapSuite9.0。除此之外， ThinkGeo官网的主题也已被重新创建。基于此，我们需要修改这些sample的主题，并 重写使用MapSuite9.0版本的sample，最后还应使所有sample都能运行在Linux上。

2. 自动编译工具使用情景

-  查找使用了MapSuite9中不被MapSuite10支持或被其他东西取代的sample
-  更改sample后，统一验证更改结果
-  每周定时用自动编译工具下载所有sample，并用最新的dll去编译这些sample


 3. 使用步骤：

    1. 模糊编译：提供关键字，自动编译所有匹配的sample，适用于情景a和情景c

       -  将结果存放路径和搜索关键字总数以及搜索关键字写入WebClaw.exe.config文件。

           -   若是想搜索ThinkGeo目录下的所有sample，则输入all

       -  运行自动编译的批处理程序tool.bat。运行Compile.exe文件，编译日志存于Log目录下对应日期的目录中

    2. 精确编译：下载最新的dll，自动编译指定的sample，适用于场景b

        -  更改selectResult.txt文件内容为所需编译的项目名列表
       -  将项目放于该工具目录中的Project目录
       - 运行Compile.exe文件，编译日志存于Log目录下对应日期的目录中

 -  备注：

	1. 使用该工具需要联网
	2. 每个模块均可单独运行
	3. 该工具包最好存放在根目录下，且路径中无空格



 4. 使用工具

     1. 下载模块：Git（自行下载，下载地址：https://git-scm.com/downloads）
     2. 还原和更新项目引用包：NuGet.exe （已提供）
    3. 编译工具：MSBuild.exe （一般为安装VS自带，路径为：C:\Windows\Microsoft.NET\Framework\v4.0.30319\MSBuild.exe）

 5. 模块简介：

     1. 搜索模块

        1. 构思：使用爬虫程序根据设置的关键字在Github中的ThinkGeo库中查找匹配的sample名
        2. 模块运行示例：
		   - 在WebClaw.exe.config文件中设置关键字
![在这里插入图片描述](https://img-blog.csdnimg.cn/20190324180242594.png?x-oss-process=image/watermark,type_ZmFuZ3poZW5naGVpdGk,shadow_10,text_aHR0cHM6Ly9ibG9nLmNzZG4ubmV0L0Nhb3lhbmdfSGU=,size_16,color_FFFFFF,t_70)
其中all表示搜索所有Sample

				- 运行WebClaw.exe文件

 ![在这里插入图片描述](https://img-blog.csdnimg.cn/2019032418043270.png)
				     - 在selectResult.txt中查看结果
![在这里插入图片描述](https://img-blog.csdnimg.cn/20190324180613358.png)
 ![在这里插入图片描述](https://img-blog.csdnimg.cn/20190324180640407.png)

 

 1. 下载模块
		1. 构思：遍历搜索结果文件，根据搜索到的sample名使用Git下载对应的sample存放到对应的日志文件夹中。
		2. 模块运行示例：
				- 在DownLoadSample.exe.config文件中设置sample下载列表的路径
![在这里插入图片描述](https://img-blog.csdnimg.cn/20190324180722384.png)
				- 运行DownLoadSample.exe文件
![在这里插入图片描述](https://img-blog.csdnimg.cn/20190324180811743.png)
				- 下载的sample均存放在Project目录中
		![在这里插入图片描述](https://img-blog.csdnimg.cn/20190324180831820.png)

 

1. 编译模块

	1. 构思： 使用NuGet工具，还原并更新项目中引用的包，再使用Microsoft.NET的MSBuild平台和自定义的XML日志生成器编译项目，生成XML日志。
	2. 模块运行示例：

- 在Compile.exe.config文件中设置sample编译列表的路径

![在这里插入图片描述](https://img-blog.csdnimg.cn/20190324180906380.png)

- 运行Compile.exe文件
![在这里插入图片描述](https://img-blog.csdnimg.cn/201903241812582.png)
 

- 编译日志存于Log目录下对应日期的目录中
![在这里插入图片描述](https://img-blog.csdnimg.cn/20190324181305507.png?x-oss-process=image/watermark,type_ZmFuZ3poZW5naGVpdGk,shadow_10,text_aHR0cHM6Ly9ibG9nLmNzZG4ubmV0L0Nhb3lhbmdfSGU=,size_16,color_FFFFFF,t_70)
目录：
![在这里插入图片描述](https://img-blog.csdnimg.cn/20190324181320229.png?x-oss-process=image/watermark,type_ZmFuZ3poZW5naGVpdGk,shadow_10,text_aHR0cHM6Ly9ibG9nLmNzZG4ubmV0L0Nhb3lhbmdfSGU=,size_16,color_FFFFFF,t_70)
 
	4. 运行结果截图模块
		- 构思：遍历待运行sample列表，新建一个进程来启动对应的exe，获取exe的主界面句柄，而后调用系统自带的user32.dll进行截图，并存至指定目录
		- 模块运行示例：
			- 在ScreenShot.exe.config中设置待运行sample列表路径（selectResultPath）和截图存放路径（screentshotPath）
![在这里插入图片描述](https://img-blog.csdnimg.cn/20190324181515448.png?x-oss-process=image/watermark,type_ZmFuZ3poZW5naGVpdGk,shadow_10,text_aHR0cHM6Ly9ibG9nLmNzZG4ubmV0L0Nhb3lhbmdfSGU=,size_16,color_FFFFFF,t_70)
		- 运行ScreentShot.exe文件：
![在这里插入图片描述](https://img-blog.csdnimg.cn/20190324181522107.png)
		- 运行结果：
		截图目录：
		![在这里插入图片描述](https://img-blog.csdnimg.cn/20190324181539922.png)
		![在这里插入图片描述](https://img-blog.csdnimg.cn/20190324181548662.png?x-oss-process=image/watermark,type_ZmFuZ3poZW5naGVpdGk,shadow_10,text_aHR0cHM6Ly9ibG9nLmNzZG4ubmV0L0Nhb3lhbmdfSGU=,size_16,color_FFFFFF,t_70)
		5. 日志读取器模块
			- 用XmlReader.exe读取指定日志
			- 左边文本框加载指定日志文件
			- 右边第一个文本框加载指定日志文件夹
			- 右边第二个文本框搜索指定文件编译情况
			- 备注：指定日志文件和指定日志文件夹只用选定一个即可
			- 效果：  
			![在这里插入图片描述](https://img-blog.csdnimg.cn/20190324181652132.png?x-oss-process=image/watermark,type_ZmFuZ3poZW5naGVpdGk,shadow_10,text_aHR0cHM6Ly9ibG9nLmNzZG4ubmV0L0Nhb3lhbmdfSGU=,size_16,color_FFFFFF,t_70)

6. 涉及到的文件
	-	WebClaw.exe.config文件：存放结果存放路径和搜索关键字总数以及搜索关键字
![在这里插入图片描述](https://img-blog.csdnimg.cn/20190324181815114.png?x-oss-process=image/watermark,type_ZmFuZ3poZW5naGVpdGk,shadow_10,text_aHR0cHM6Ly9ibG9nLmNzZG4ubmV0L0Nhb3lhbmdfSGU=,size_16,color_FFFFFF,t_70)
		- resultPath表示搜索结果存放路径
		- selectKeysCount为搜索关键字总个数
		- selectKey0、selectKey1表示搜索的sample名

- DownLoadSample.exe.config文件：
![在这里插入图片描述](https://img-blog.csdnimg.cn/20190324181834887.png)
		- downSourceTxt表示需下载的sample列表

- Compile.exe.config文件：
![在这里插入图片描述](https://img-blog.csdnimg.cn/20190324181956661.png)
		- nuget.exe表示nuget.exe的路径
		- msbuild.exe表示msbuild.exe的路径
		- compileTxt表示待编译sample列表的路径
		- loggerClass表示日志生成器类名
		- loggerName表示日志生成器的路径

- ScreenShot.exe.config文件：
![在这里插入图片描述](https://img-blog.csdnimg.cn/20190324182025879.png?x-oss-process=image/watermark,type_ZmFuZ3poZW5naGVpdGk,shadow_10,text_aHR0cHM6Ly9ibG9nLmNzZG4ubmV0L0Nhb3lhbmdfSGU=,size_16,color_FFFFFF,t_70)
			- selectResultPath表示待运行sample列表路径
			- screenshotPath表示截图存放路径

- selectResult.txt：存放根据关键字搜索到的项目名
			示例:
![在这里插入图片描述](https://img-blog.csdnimg.cn/2019032418205390.png)
- 编译日志：
示例：
![在这里插入图片描述](https://img-blog.csdnimg.cn/20190324182104364.png?x-oss-process=image/watermark,type_ZmFuZ3poZW5naGVpdGk,shadow_10,text_aHR0cHM6Ly9ibG9nLmNzZG4ubmV0L0Nhb3lhbmdfSGU=,size_16,color_FFFFFF,t_70)
 其中：
		- 根节点HowDoI为HowDoISample-ForWinforms的解决方案名HowDoI.sln
		- 二级节点ProjectCompileResult为该解决方案下的项目编译结果，该sample中只有一个HowDoI项目，所以该部分只有一个HowDoI项目的编译状态，为编译成功
		- 二级结点ProjectCompileMessage为具体的编译结果
		- 三级结点HowDoI表示HowDoI项目中的warning和error信息
		- DataProvider为HowDoI下的文件夹名
		- LoadAHeatLayerc表示DataProvider下的文件
		- warningFile="true"表示该cs文件有warning，errorFile=”true”表示该cs文件有error
		- warning结点下的子节点location为warning或error出现的行和列的位置，infoCode为warning或error的编号，description为具体的描述

 		

