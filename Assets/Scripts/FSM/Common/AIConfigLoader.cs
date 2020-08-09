using UnityEngine;
using System.Collections.Generic;
using System.IO;
using System;

/// <summary>
/// AI配置文件的加载工具
/// </summary>
public static class AIConfigLoader
{
    /// <summary>
    /// 配置文件地址
    /// </summary>
    private static string sourcePath;
    /// <summary>
    /// 文件加载对象
    /// </summary>
    private static WWW www = null;

    private static Dictionary<string, string> AICache = new Dictionary<string, string>();

    /// <summary>
    /// 加载配置文件，并形成字典结构
    /// </summary>
    /// <param name="aiConfigFile">配置文件名，配置文件必须放在StreamingAssets文件中</param>
    /// <returns>构建好的字典，字典中主键是状态类名，子键是转换条件Id，子值是输出状态Id</returns>
    public static Dictionary<string, Dictionary<string, string>> Load(string aiConfigFile)
    {
        if (AICache.ContainsKey(aiConfigFile))
            return BuildDic(AICache[aiConfigFile]);
        
        //sourcePath = Application.streamingAssetsPath + @"/" + aiConfigFile;
        sourcePath = Path.Combine(Application.streamingAssetsPath, aiConfigFile) ;
        if (Application.platform != RuntimePlatform.Android)
            sourcePath = "file://" + sourcePath;

        Debug.Log("sourcePath = " + sourcePath);
        www = new WWW(sourcePath);
        while (true)
        {
            if (www != null && www.isDone && string.IsNullOrEmpty(www.error))
            {
                AICache.Add(aiConfigFile, www.text);
                return BuildDic(www.text);

            }
            if (!string.IsNullOrEmpty(www.error))
            {
                Debug.Log(www.error);
                return null;
            }
        }
        return null;
    }

    /// <summary>
    /// 将加载的文本构建为字典
    /// </summary>
    /// <param name="text">配置文件中的内容</param>
    /// <returns></returns>
    private static Dictionary<string, Dictionary<string, string>> BuildDic(string text)
    {
       var dicConfig = new Dictionary<string, Dictionary<string, string>>();
        StringReader reader = new StringReader(text);

        string mainKey = null;
        string subKey = null;
        string subValue = null;
        string line = null;
        //遍历每一行数据
        while ((line = reader.ReadLine()) != null)
        {
            //处理方式 
            //先去除文本两端的空白
            line = line.Trim();
            //检查是否为null或""
            if (!string.IsNullOrEmpty(line))//如果不为空
            {
                //检查是否是"["开头
                if (line.StartsWith("["))
                {
                    //如果是 则在字典中加入主键，主键为"["与"]"之间的内容
                    mainKey = line.Substring(1, line.IndexOf("]") - 1);
                    Debug.Log("mainKey = " + mainKey);
                    //添加主键对应的值 
                    dicConfig.Add(mainKey, new Dictionary<string, string>());
                }
                else//不是"["开头 ,意味该行是配置项   配置键= 配置值
                {
                    //将"="左右两边的内容取出
                    var configKeyValue = line.Split(new char[] { '>' }, StringSplitOptions.RemoveEmptyEntries);
                    //左->子键，右->子值
                    subKey = configKeyValue[0].Trim();
                    subValue = configKeyValue[1].Trim();

                    //将子键与子值加入字典
                    dicConfig[mainKey].Add(subKey, subValue);
                }
            }
        }

        return dicConfig;

    }

}
