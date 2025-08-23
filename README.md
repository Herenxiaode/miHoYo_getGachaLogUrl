# miHoYo_getGachaLogUrl
通过PowerShell与C#获取Windows下的原神,铁道,绝区零抽卡API




使用方法是将getUrl.cs文件和getUrl.ps1放在一起,然后运行getUrl.ps1.




问:提示ps1脚本被禁止.

解:管理员打开PowerShell输入`Set-ExecutionPolicy RemoteSigned -Scope CurrentUser`以接触当前用户限制.



问:为什么不做单文件.

答:因为尝试ps1的内嵌语法感觉不能完美兼容,比如不支持中文,需要额外编码,不想ps1脚本上多花时间维护...
