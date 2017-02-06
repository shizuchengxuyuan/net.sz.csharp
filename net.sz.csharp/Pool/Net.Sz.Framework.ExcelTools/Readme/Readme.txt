

excel 列名  ID
            =P         表示ID这列是唯一字段
            =S_300     表示这列类型是 字符串(String) 类型， 长度控制300   
            =D         表示这列类型是 带小数点的(double)
            =FF_ddddd  表示关联的excel文件 ddddd 这个文件 无须后缀名，到处XML使用
            =L         表示这列类型是 长整型(long)
            =FL        表示这列类型是 带小数的float类型
            =B         表示这列类型是 真假类型(Boolean)
            =I         表示这列类型是 整型(int)  不填写任何类型的默认类型
            =HL        表示忽略这列 不读取
            =CL      标识字段的归属 ALL(可以不填写)=客户端和服务器通用 AC=表示客户端使用 AS=表示服务使用

如：aa.xls 文件
    ID=P
    name=S_300
    sex=I


数据库连接字符串  配置在 dbconfig.xml 文件里面

<?xml version="1.0"?>
<DBConfigs xmlns:xsd="http://www.w3.org/2001/XMLSchema" xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance">
  基础保存路径
  <SavePath>C:/ExcelSource</SavePath>
  jpa文件保存路径
  <SaveJPAPath>D:\worker\zc\ServerCode\newGame\net.sz.game.model\src\main\java\net\sz\game\po\data</SaveJPAPath>
  java protobuf message 保存路径
  <SaveJavaMessagePath>C:/ExcelSource</SaveJavaMessagePath>  
  csharp protobuf message 保存路径
  <SaveCsharpMessagePath>C:/ExcelSource</SaveCsharpMessagePath>
  是否默认过滤空白字符
  <IsNullEmpty>false</IsNullEmpty>
  名字空间
  <NamespaceStr>com.game.po.data</NamespaceStr>
  <Configs>    
    <DBConfig>
       连接地址
      <DBPath>192.168.1.198</DBPath> 
       连接端口
      <DBPart>3306</DBPart>
       连接用户名
      <DBUser>root</DBUser>
       连接数据库
      <DBBase>local_gamesr_data</DBBase>
       密码
      <DBPwd>fuckdaohaode1314</DBPwd>
    </DBConfig>
  </Configs>
</DBConfigs>